using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SP = Microsoft.SharePoint.Client;

namespace BinMonitor.Common.Sharepoint
{
    public class SharepointConnectionInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            { handler(this, new PropertyChangedEventArgs(propertyName)); }
        }

        private string _Host;
        public string Host
        { 
            get {return _Host;}
            set 
            {
                if (string.Equals(_Host, value) == false)
                {
                    ClearHost();
                    _Host = value;
                    NotifyPropertyChanged("Host");
                }
            }
        }

        private string _Domain;
        public string Domain
        {
            get { return _Domain; }
            set
            {
                if (string.Equals(_Domain, value) == false)
                {
                    ClearDomain();
                    _Domain = value;
                    NotifyPropertyChanged("Domain");
                }
            }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set 
            { 
                if (string.Equals(_UserName, value) == false)
                {
                    ClearUserName();
                    _UserName = value;
                    NotifyPropertyChanged("UserName");
                }
                
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                if (string.Equals(_Password, value) == false)
                {
                    ClearPassword();
                    _Password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        private SP.ClientAuthenticationMode _ClientAuthenticationMode = SP.ClientAuthenticationMode.FormsAuthentication;
        public SP.ClientAuthenticationMode ClientAuthenticationMode 
        {
            get { return _ClientAuthenticationMode; }
            set { _ClientAuthenticationMode = value; }
        }

        void ClearPassword()
        { _Password = null; }

        void ClearUserName()
        {
            ClearPassword();
            _UserName = null; 
        }

        void ClearDomain()
        {
            ClearUserName();
            _Domain = null;
        }

        void ClearHost()
        {
            ClearDomain();
            _Host = null;
        }
        

    }
}
