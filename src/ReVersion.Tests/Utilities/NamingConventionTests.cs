using FluentAssertions;
using ReVersion.Models.Settings;
using ReVersion.Utilities.Extensions;
using Xunit;

namespace ReVersion.Tests.Utilities
{
    public class NamingConventionTests
    {
        [Theory]
        [InlineData("example repo 1")]
        [InlineData("EXAMPLE_REPO_TWO")]
        [InlineData("example-repo-3")]
        [InlineData("example_repo_4")]
        [InlineData("example repo Five")]
        [InlineData("ExampleRepoSix")]
        [InlineData("Example Repo7")]
        public void Can_preserve_original_convention(string originalName)
        {
            originalName.ToConventionCase(SvnNamingConvention.PreserveOriginal)
                .Should().Be(originalName);
        }

        // Camel

        [Theory]
        [InlineData("example repo 1", "ExampleRepo1")]
        [InlineData("EXAMPLE_REPO_TWO", "ExampleRepoTwo")]
        [InlineData("example-repo-3", "ExampleRepo3")]
        [InlineData("example_repo_4", "ExampleRepo4")]
        [InlineData("example repo Five", "ExampleRepoFive")]
        [InlineData("ExampleRepoSix", "ExampleRepoSix")]
        [InlineData("Example Repo7", "ExampleRepo7")]
        public void Can_generate_Upper_Camel_Case(string originalName, string expectedName)
        {
            originalName.ToConventionCase(SvnNamingConvention.UpperCamelCase)
                .Should().Be(expectedName);
        }

        [Theory]
        [InlineData("example repo 1", "exampleRepo1")]
        [InlineData("EXAMPLE_REPO_TWO", "exampleRepoTwo")]
        [InlineData("example-repo-3", "exampleRepo3")]
        [InlineData("example_repo_4", "exampleRepo4")]
        [InlineData("example repo Five", "exampleRepoFive")]
        [InlineData("ExampleRepoSix", "exampleRepoSix")]
        [InlineData("Example Repo7", "exampleRepo7")]
        public void Can_generate_Lower_Camel_Case(string originalName, string expectedName)
        {
            originalName.ToConventionCase(SvnNamingConvention.LowerCamelCase)
                .Should().Be(expectedName);
        }

        // Hyphen

        [Theory]
        [InlineData("example repo 1", "Example-Repo-1")]
        [InlineData("EXAMPLE_REPO_TWO", "Example-Repo-Two")]
        [InlineData("example-repo-3", "Example-Repo-3")]
        [InlineData("example_repo-4", "Example-Repo-4")]
        [InlineData("example repo Five", "Example-Repo-Five")]
        [InlineData("ExampleRepoSix", "Example-Repo-Six")]
        [InlineData("Example Repo7", "Example-Repo7")] // Ideally this would be -7
        public void Can_generate_Upper_Hyphen_Case(string originalName, string expectedName)
        {
            originalName.ToConventionCase(SvnNamingConvention.UpperHyphenCase)
                .Should().Be(expectedName);
        }

        [Theory]
        [InlineData("example repo 1", "example-repo-1")]
        [InlineData("EXAMPLE_REPO_TWO", "example-repo-two")]
        [InlineData("example-repo-3", "example-repo-3")]
        [InlineData("example_repo_4", "example-repo-4")]
        [InlineData("example repo Five", "example-repo-five")]
        [InlineData("ExampleRepoSix", "example-repo-six")]
        [InlineData("Example Repo7", "example-repo7")]
        public void Can_generate_Lower_Hyphen_Case(string originalName, string expectedName)
        {
            originalName.ToConventionCase(SvnNamingConvention.LowerHyphenCase)
                .Should().Be(expectedName);
        }

        // Underscore

        [Theory]
        [InlineData("example repo 1", "Example_Repo_1")]
        [InlineData("EXAMPLE_REPO_TWO", "Example_Repo_Two")]
        [InlineData("example-repo-3", "Example_Repo_3")]
        [InlineData("example_repo-4", "Example_Repo_4")]
        [InlineData("example repo Five", "Example_Repo_Five")]
        [InlineData("ExampleRepoSix", "Example_Repo_Six")]
        [InlineData("Example Repo7", "Example_Repo7")] // Ideally this would be _7
        public void Can_generate_Upper_Underscore_Case(string originalName, string expectedName)
        {
            originalName.ToConventionCase(SvnNamingConvention.UpperUnderscoreCase)
                .Should().Be(expectedName);
        }

        [Theory]
        [InlineData("example repo 1", "example_repo_1")]
        [InlineData("EXAMPLE_REPO_TWO", "example_repo_two")]
        [InlineData("example-repo-3", "example_repo_3")]
        [InlineData("example_repo_4", "example_repo_4")]
        [InlineData("example repo Five", "example_repo_five")]
        [InlineData("ExampleRepoSix", "example_repo_six")]
        [InlineData("Example Repo7", "example_repo7")] // Ideally this would be _7
        public void Can_generate_Lower_Underscore_Case(string originalName, string expectedName)
        {
            originalName.ToConventionCase(SvnNamingConvention.LowerUnderscoreCase)
                .Should().Be(expectedName);
        }


    }
}
