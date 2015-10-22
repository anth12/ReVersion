using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using ReVersion.Helpers;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient.Requests;

namespace ReVersion.Services.SvnClient
{
    public class SvnClientService
    {
        public void CheckoutRepository(CheckoutRepositoryRequest request)
        {
            var projectFolder = SettingsService.Current.CheckoutFolder;

            if (!projectFolder.EndsWith("\\"))
                projectFolder += "\\";

            //TODO naming convensions
            projectFolder += request.ProjectName + "\\trunk";
            //TODO make trunk optional

            if (!Directory.Exists(projectFolder))
            {
                Directory.CreateDirectory(projectFolder);
            }




            var process = new Process
            {
                EnableRaisingEvents = true,
                
                StartInfo =
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = projectFolder,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                    //CreateNoWindow = true
                }
            };

            // Set UseShellExecute to false for redirection.

            // Redirect the standard output of the sort command.  
            // This stream is read asynchronously using an event handler.
            var sortOutput = new StringBuilder("");

            // Set our event handler to asynchronously read the sort output.
            process.OutputDataReceived += (sender, args) =>
            {
                File.AppendAllLines("C:\\Temp\\output.txt", new[] { args.Data });

            };
            
            process.Exited += (sender, args) =>
            {

            };

            // Redirect standard input as well.  This stream
            // is used synchronously.
            //process.StartInfo.RedirectStandardInput = true;
            process.Start();

            // Use a stream writer to synchronously write the sort input.
            //StreamWriter sortStreamWriter = process.StandardInput;

            // Start the asynchronous read of the sort output stream.
            process.BeginOutputReadLine();

            //output = process.StandardOutput;
            StreamReader sr = process.StandardOutput;

            for(var i = 0; i< 100; i++)
            {
                // Trying to use synchronous reading here

                File.AppendAllLines("C:\\Temp\\output.txt", new[] { sr.ReadLine() });

            }

            //sortStreamWriter.WriteLine($"cd {projectFolder}");
            //sortStreamWriter.WriteLine($"svn checkout {request.SvnServerUrl}/{request.ProjectName}/trunk"); //TODO make trunk optional

            //var a = output.ReadLine();



            //    var p = new Process();
            //var info = new ProcessStartInfo
            //{
            //    FileName = "cmd.exe",
            //    RedirectStandardInput = true,
            //    UseShellExecute = false,

            //};

            //p.StartInfo = info;
            //p.Start();

            //using (var sw = p.StandardInput)
            //{
            //    if (sw.BaseStream.CanWrite)
            //    {
            //    }
            //}
        }

        private StringBuilder consoleOutput = new StringBuilder();
        private StreamReader output;

        private void SortOutputHandler(object sender, DataReceivedEventArgs e)
        {
            consoleOutput.AppendLine(e.Data);


            File.AppendAllLines("C:\\Temp\\output.txt", new [] { e.Data });
        }
    }
}
