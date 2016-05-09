using System.Collections.Generic;

namespace ReVersion.Models.Shared
{
    public class BranchPickerModel : BaseModel
    {
        public string SelectedBranch { get; set; }
        public List<string> Branches { get; set; }
    }
}
