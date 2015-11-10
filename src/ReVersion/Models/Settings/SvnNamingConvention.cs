using System.ComponentModel;

namespace ReVersion.Models.Settings
{
    public enum SvnNamingConvention
    {
        [Description("The name of all Repositories is persevered in the Original format")]
        PreserveOriginal,

        [Description("The name of all Repositories are converted to upperCamelCase")]
        UpperCamelCase,

        [Description("The name of all Repositories are converted to lowerCamelCase")]
        LowerCamelCase,

        [Description("The name of all Repositories are converted to lower-hyphen-case")]
        LowerHyphenCase,

        [Description("The name of all Repositories are converted to Upper-Hyphen-Case")]
        UpperHyphenCase,

        [Description("The name of all Repositories are converted to lower_underscore_case")]
        LowerUnderscoreCase,

        [Description("The name of all Repositories are converted to Upper_Underscore_Case")]
        UpperUnderscoreCase
    }
}
