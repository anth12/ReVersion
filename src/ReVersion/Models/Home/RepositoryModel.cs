using System;
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
        private long _repositorySize;

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
                OnPropertyChanged(nameof(ProgressEnabled));
                OnPropertyChanged(nameof(ProgressPercentage));
            }
        }

        public long RepositorySize
        {
            get { return _repositorySize; }
            set
            {
                SetField(ref _repositorySize, value);
                OnPropertyChanged(nameof(ProgressText));
                OnPropertyChanged(nameof(ProgressEnabled));
                OnPropertyChanged(nameof(ProgressPercentage));
            }
        }


        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public string ProgressText
        {
            get
            {
                var currentSuffix = (int)Math.Max(0, Math.Log(_progress, 1024));
                var currentDownload = Math.Round(_progress / Math.Pow(1024, currentSuffix), 0);
                var result = $"{currentDownload} {SizeSuffixes[currentSuffix]}";

                if (RepositorySize > 0)
                {
                    var totalSuffix = (int)Math.Max(0, Math.Log(RepositorySize, 1024));
                    var totalDownload = Math.Round(RepositorySize / Math.Pow(1024, totalSuffix), 0);

                    result += $" of {totalDownload} {SizeSuffixes[totalSuffix]}";
                }

                return result;
            }
        }

        public bool ProgressEnabled
        {
            get { return !CheckoutEnabled && ProgressPercentage > 0 && ProgressPercentage < 100; }
            set { }
        }

        public double ProgressPercentage
        {
            get { return RepositorySize > 0 ? (((double)Progress/(double)RepositorySize)*100) : 0; }
            set { }
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
