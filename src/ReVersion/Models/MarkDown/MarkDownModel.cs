using System;

namespace ReVersion.Models.MarkDown
{
    internal class MarkDownModel : BaseModel
    {

        #region Properties
        
        private string _fileName;
        private Uri _browserSource;


        public string FileName
        {
            get { return _fileName; }
            set { SetField(ref _fileName, value); }
        }

        public Uri BrowserSource
        {
            get { return _browserSource; }
            set { SetField(ref _browserSource, value); }
        }
        
        #endregion
        
    }
}