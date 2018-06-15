using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ExchangeClient.ViewModels
{
    public class SettingViewModel
    {
        private string clientId;
        [Category("Setting")]
        [DisplayName("Client ID")]
        public string ClientId
        {
            get => clientId;
            set => clientId = value; 
        }

        private string serverEndpoint;
        [Category("Setting")]
        [DisplayName("Server Endpoint")]
        [Description("Endpoint for connecting server.")]
        public string ServerEndpoint
        {
            get => serverEndpoint;
            set => serverEndpoint = value;
        }

        private string channelEndpoint;
        [Category("Setting")]
        [DisplayName("Channel Endpoint")]
        [Description("Endpoint for subscribing channel callbacks.")]
        public string ChannelEndpoint
        {
            get => channelEndpoint;
            set => channelEndpoint = value;
        }
    }
}
