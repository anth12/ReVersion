
namespace ReVersion.Models.Home
{
    public class WindowModel : BaseModel
    {
        
        private int _columnWidth;

        public int ColumnWidth
        {
            get => _columnWidth;
            set => SetField(ref _columnWidth, value);
        }
        
    }
}
