using System;

namespace ReVersion.Models.Home
{
    public class RepositoryModel : BaseModel
    {

        private bool _checkedOut;
        private bool _isChecked;
        private bool _isEnabled;
        private bool _bulkCheckoutActive;
        private string _name;
        private Guid _svnServerId;
        private string _url;

        public bool CheckedOut
        {
            get { return _checkedOut; }
            set { SetField(ref _checkedOut, value); }
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
    }
}
