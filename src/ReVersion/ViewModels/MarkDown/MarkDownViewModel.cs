using System;
using System.ComponentModel;
using ReVersion.Models.MarkDown;
using ReVersion.Utilities.Helpers;
using HeyRed.MarkdownSharp;

namespace ReVersion.ViewModels.MarkDown
{
    internal class MarkDownViewModel : BaseViewModel<MarkDownModel>
    {
        internal MarkDownViewModel(string markdownText)
        {
            Model = new MarkDownModel();

            SetMarkDown(markdownText);
        }
        
        public void SetMarkDown(string markdownText)
        {
            var markdownParser = new Markdown();
            var html = markdownParser.Transform(markdownText);

            // Add a reference to the Stylesheet
            var cssPath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\Markdown.css";
            html += $"<link href=\"{cssPath}\" type=\"text/css\" rel=\"stylesheet\"></link>";

            Model.FileName = Guid.NewGuid().ToString();

            var htmlFilePath = AppDataHelper.WriteFile(Model.FileName, "html", html, "readme");

            Model.BrowserSource = new Uri(htmlFilePath);
        }
        

        public void WindowClosing(object sender, CancelEventArgs e)
        {
            // remove the temporary file
            if (Model.BrowserSource != null)
            {
                AppDataHelper.RemoveFile(Model.FileName, "html", "readme");
            }
        }
    }
}
