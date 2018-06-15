using ExchangeClient.ViewModels;
using MessageQueue;
using ReactiveUI;
using System;
using System.Windows.Forms;

namespace ExchangeClient
{
    public partial class MainForm : Form, IViewFor<MainViewModel>
    {
        private MainViewModel vm;

        public MainForm()
        {
            InitializeComponent();

            vm = new MainViewModel();

            this.Bind(vm, x => x.SettingVM, x => x.pgSetting.SelectedObject);
            this.BindCommand(vm, x => x.LoginCommand, x => x.btnLogin);

            this.Bind(vm, x => x.PlaceOrderVM, x => x.pgOrder.SelectedObject);
            this.BindCommand(vm, x => x.PlaceCommand, x => x.btnPlaceOrder);
            this.BindCommand(vm, x => x.CancelCommand, x => x.bnCancelOrder);

            this.Bind(vm, x => x.OrderbookText, x => x.txtOrderbook.Text);
            this.Bind(vm, x => x.TradeText, x => x.txtTrade.Text);
            this.Bind(vm, x => x.LogText, x => x.txtLog.Text);

            this.FormClosing += (s, e) => Environment.Exit(0);
        }

        #region IVoewFor
        object IViewFor.ViewModel
        {
            get => vm; 
            set => vm = (MainViewModel)value; 
        }

        public MainViewModel ViewModel
        {
            get => vm;
            set => vm = value; 
        }
        #endregion
    }
}
