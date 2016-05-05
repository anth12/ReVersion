using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace ReVersion.Models.Home
{
    internal class RepositoryModel : BaseModel
    {

        private bool _checkedOut;
        private bool _showProgress;
        private bool _isChecked;
        private bool _isEnabled;
        private bool _checkoutEnabled;
        private bool _bulkCheckoutActive;
        private string _name;
        private Guid _svnServerId;
        private string _url;
        private long _progress;

        public bool CheckedOut
        {
            get { return _checkedOut; }
            set { SetField(ref _checkedOut, value); OnPropertyChanged(nameof(BackgroundBrush)); }
        }

        public bool ShowProgress
        {
            get { return _showProgress; }
            set { SetField(ref _showProgress, value); }
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

        public long Progress
        {
            get { return _progress; }
            set
            {
                SetField(ref _progress, value);
                OnPropertyChanged(nameof(ProgressText));
            }
        }


        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public string ProgressText
        {
            get
            {
                var mag = (int)Math.Max(0, Math.Log(_progress, 1024));
                var adjustedSize = Math.Round(_progress / Math.Pow(1024, mag), 0);
                return $"{adjustedSize} {SizeSuffixes[mag]}";
            }
        }

        public SolidColorBrush BackgroundBrush => CheckedOut
            ? new SolidColorBrush(Color.FromRgb(101, 84, 117))
            : new SolidColorBrush(Colors.Transparent);
    }

    internal class RepositoryAction
    {
        public string Title { get; set; }
        public ICommand Command { get; set; }
    }
}
