using System;
using System.Windows;

namespace ReVersion.Models.Home
{
    public class RepositoryModel : BaseModel
    {

        private bool checkedOut;
        private bool isChecked;
        private bool isEnabled;
        private Visibility visibility;
        private bool bulkCheckoutVisible;
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

        public Visibility Visibility
        {
            get { return visibility; }
            set { SetField(ref visibility, value); }
        }

        public bool BulkCheckoutVisible
        {
            get { return bulkCheckoutVisible; }
            set { SetField(ref bulkCheckoutVisible, value); }
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
