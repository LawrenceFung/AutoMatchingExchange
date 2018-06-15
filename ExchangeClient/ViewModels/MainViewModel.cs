using ExchangeClient.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Disposables;
using MessageQueue;
using static MessageQueue.ChannelKeyUtil;
using System.Reactive.Linq;
using System.Threading;

namespace ExchangeClient.ViewModels
{
    public class MainViewModel: ReactiveObject, IDisposable
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly SynchronizationContext uiContext;

        private readonly ZmqContext zmqContext;
        private readonly ZmqRequester requester;
        private readonly ZmqSubscriber subscriber;

        private readonly CompositeDisposable disposableResources;

        private bool isLogged;
        private string clientSecret;
        private string clientId;

        public SettingViewModel SettingVM { get; private set; }
        public ReactiveCommand LoginCommand { get; private set; }

        public PlaceOrderViewModel PlaceOrderVM { get; private set; }
        public ReactiveCommand PlaceCommand { get; private set; }
        public ReactiveCommand CancelCommand { get; private set; }

        private string orderbookText;
        public string OrderbookText
        {
            get => orderbookText;
            set => this.RaiseAndSetIfChanged(ref orderbookText, value);
        }

        private string tradeText;
        public string TradeText
        {
            get => tradeText;
            set => this.RaiseAndSetIfChanged(ref tradeText, value);
        }

        private string logText;
        public string LogText
        {
            get => logText;
            set => this.RaiseAndSetIfChanged(ref logText, value);
        }

        public MainViewModel()
        {
            uiContext = SynchronizationContext.Current;
            zmqContext = new ZmqContext();
            requester = new ZmqRequester(zmqContext);
            subscriber = new ZmqSubscriber(zmqContext);
            isLogged = false;

            disposableResources = new CompositeDisposable();

            SettingVM = new SettingViewModel()
            {
                ClientId = Settings.Default.ClientId,
                ServerEndpoint = Settings.Default.ServerEndpoint,
                ChannelEndpoint = Settings.Default.ChannelEndpoint
            };
            LoginCommand = ReactiveCommand.CreateFromTask(() => Task.Run(() => Login()));

            PlaceOrderVM = new PlaceOrderViewModel()
            {
                OrderId = "",
                Side = SideOptions.Buy,
                Qty = 0,
                Price = 0
            };
            PlaceCommand = ReactiveCommand.CreateFromTask(() => Task.Run(() => PlaceOrder()));
            CancelCommand = ReactiveCommand.CreateFromTask(() => Task.Run(() => CancelOrder()));

            orderbookText = string.Empty;
            tradeText = string.Empty;
            logText = string.Empty;

            disposableResources.Add(
              Observable.FromEventPattern<string>(
                  h => subscriber.OnPublicOrderbookUpdated += h,
                  h => subscriber.OnPublicOrderbookUpdated -= h)
              .ObserveOn(SynchronizationContext.Current)
              .Subscribe(args => OrderbookText = args.EventArgs));

            disposableResources.Add(
              Observable.FromEventPattern<string>(
                  h => subscriber.OnPublicTradeUpdated += h,
                  h => subscriber.OnPublicTradeUpdated -= h)
              .ObserveOn(SynchronizationContext.Current)
              .Subscribe(args =>
              {
                  var msg = args.EventArgs + Environment.NewLine;
                  TradeText += msg;
              }));

            disposableResources.Add(
              Observable.FromEventPattern<string>(
                  h => subscriber.OnPrivateMessageUpdated += h,
                  h => subscriber.OnPrivateMessageUpdated -= h)
              .ObserveOn(SynchronizationContext.Current)
              .Subscribe(args =>
              {
                  var msg = string.Concat("Your order updated:", Environment.NewLine, args.EventArgs, Environment.NewLine);
                  LogText += msg;
              }));
        }



        private void Login()
        {
            if (!isLogged)
            {
                var timeout = Settings.Default.RequestTimeoutMilliSeconds;
                requester.Set(SettingVM.ServerEndpoint, timeout, timeout);
                requester.Start(RequesterErrorReceived);
                var clientId = SettingVM.ClientId;
                var message = requester.Login(clientId);
                var messageType = message[0].ReadInt32();
                var clientSecret = message[1].ReadString();
                if (!string.IsNullOrEmpty(clientSecret))
                {
                    this.clientSecret = clientSecret;
                    this.clientId = clientId;
                    subscriber.Setup(SettingVM.ChannelEndpoint, SubscriberErrorReceived);
                    subscriber.Start();
                    subscriber.Subscribe(PublicOrderbookKey);
                    subscriber.Subscribe(PublicTradeKey);
                    subscriber.Subscribe(GetPrivateOrderKey(clientSecret));
                    isLogged = true;
                    var msgToLog = $"Login succeeded. Client ID = {clientId}, Client Secret = {clientSecret}";
                    LogMessage(msgToLog);
                }
                else
                {
                    var msgToLog = $"Login failed. Input Client ID = {clientId}";
                    LogMessage(msgToLog);
                }
            }
        }

        private void PlaceOrder()
        {
            if (isLogged)
            {
                var orderId = PlaceOrderVM.OrderId;
                var isBid = (PlaceOrderVM.Side == SideOptions.Buy);
                decimal qty;
                if ((qty = PlaceOrderVM.Qty) <= 0)
                {
                    LogMessage($"Add/Update Order: Quantity = {PlaceOrderVM.Qty} must be positive.");
                    return;
                }
                decimal price;
                if ((price = PlaceOrderVM.Price) <= 0)
                {
                    LogMessage($"Add/Update Order: Limit Price = {PlaceOrderVM.Price} must be positive.");
                    return;
                }
                var message = requester.PlaceOrder(orderId, isBid, qty, price, DateTimeOffset.Now, clientId);
                var messageType = message[0].ReadInt32();
                var success = bool.Parse(message[1].ReadString());

                var msgToLog = success ?
                    $"Add/Update Order succeeded." : $"Add/Update Order failed.";
                LogMessage(msgToLog);
            }
            else
            {
                LogMessage("Please Login before Add/Update Order!");
            }
        }

        private void CancelOrder()
        {
            if (isLogged)
            {
                var orderId = PlaceOrderVM.OrderId;
                if (string.IsNullOrEmpty(orderId))
                {
                    LogMessage($"Cancel Order: Order ID must be filled.");
                    return;
                }

                var message = requester.CancelOrder(orderId, clientId);
                var messageType = message[0].ReadInt32();
                var success = bool.Parse(message[1].ReadString());

                var msgToLog = success ?
                    $"Cancel Order succeeded" : $"Cancel Order failed";
                LogMessage(msgToLog);
            }
            else
            {
                LogMessage("Please Login before Cancel Order!");
            }
        }

        private void LogMessage(string msgToLog)
        {
            logger.Info(msgToLog);
            msgToLog += Environment.NewLine;

            uiContext.Post((o) => 
            {
                LogText += msgToLog;

            }, null);
        }

        private void RequesterErrorReceived(Exception ex)
            => LogMessage($"RequesterError: " + ex.ToString());

        private void SubscriberErrorReceived(Exception ex)
            => LogMessage($"SubscriberError: " + ex.ToString());

        public void Dispose()
        {
            subscriber.Stop();
            subscriber.Dispose();
            requester.Stop();
            zmqContext.Stop();
            zmqContext.Dispose();
            disposableResources.Dispose();
        }
    }
}
