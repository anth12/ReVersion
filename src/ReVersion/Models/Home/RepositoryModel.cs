using System;

namespace ReVersion.Models.Home
{
    public class RepositoryModel : BaseModel
    {

        private bool checkedOut;
        private bool isChecked;
        private bool isEnabled;
        private bool bulkCheckoutActive;
        private string name;
        private Guid svnServerId;
        private string url;

        public bool CheckedOut
        {
            get { return checkedOut; }
            set { SetField(ref checkedOut, value); }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { SetField(ref isChecked, value); }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetField(ref isEnabled, value); }
        }
        
        public bool BulkCheckoutActive
        {
            get { return bulkCheckoutActive; }
            set { SetField(ref bulkCheckoutActive, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetField(ref name, value); }
        }

        public Guid SvnServerId
        {
            get { return svnServerId; }
            set { SetField(ref svnServerId, value); }
        }


        public string Url
        {
            get { return url; }
            set { SetField(ref url, value); }
        }
    }
}
