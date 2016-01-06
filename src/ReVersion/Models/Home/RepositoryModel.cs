using System;
using System.Windows.Media;

namespace ReVersion.Models.Home
{
    internal class RepositoryModel : BaseModel
    {

        private bool _checkedOut;
        private bool _isChecked;
        private bool _isEnabled;
        private bool _checkoutEnabled;
        private bool _bulkCheckoutActive;
        private string _name;
        private Guid _svnServerId;
        private string _url;

        public bool CheckedOut
        {
            get { return _checkedOut; }
            set { SetField(ref _checkedOut, value); OnPropertyChanged(nameof(BackgroundBrush)); }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetField(ref _isChecked, value); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetField(ref _isEnabled, value); }
        }

        public bool CheckoutEnabled
        {
            get { return _checkoutEnabled; }
            set { SetField(ref _checkoutEnabled, value); }
        }

        public bool BulkCheckoutActive
        {
            get { return _bulkCheckoutActive; }
            set { SetField(ref _bulkCheckoutActive, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        public Guid SvnServerId
        {
            get { return _svnServerId; }
            set { SetField(ref _svnServerId, value); }
        }


        public string Url
        {
            get { return _url; }
            set { SetField(ref _url, value); }
        }

        public SolidColorBrush BackgroundBrush => CheckedOut
            ? new SolidColorBrush(Color.FromRgb(101, 84, 117))
            : new SolidColorBrush(Colors.Transparent);
    }
}
