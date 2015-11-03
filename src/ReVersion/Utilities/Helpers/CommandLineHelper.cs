using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading;

namespace ReVersion.Utilities.Helpers
{
    [SecurityPermission(SecurityAction.LinkDemand,
        Unrestricted = true)]
    public static class CommandLineHelper
    {
        public static string Run(string fileName, string arguments, out string errorMessage)
        {
            errorMessage = "";
            var cmdLineProcess = new Process();
            using (cmdLineProcess)
            {
                cmdLineProcess.StartInfo.FileName = fileName;
                cmdLineProcess.StartInfo.Arguments = arguments;
                cmdLineProcess.StartInfo.UseShellExecute = false;
                cmdLineProcess.StartInfo.CreateNoWindow = true;
                cmdLineProcess.StartInfo.RedirectStandardOutput = true;
                cmdLineProcess.StartInfo.RedirectStandardError = true;

                if (cmdLineProcess.Start())
                {
                    return ReadProcessOutput(cmdLineProcess, ref errorMessage, fileName);
                }

                throw new Exception($"Could not start command line process: {fileName}");
                /* Note: arguments aren't also shown in the 
                     * exception as they might contain privileged 
                     * information (such as passwords).
                     */
            }
        }

        private static string ReadProcessOutput(Process cmdLineProcess, ref string errorMessage, string fileName)
        {
            StringDelegate outputStreamAsyncReader = cmdLineProcess.StandardOutput.ReadToEnd;
            StringDelegate errorStreamAsyncReader = cmdLineProcess.StandardError.ReadToEnd;

            var outAR = outputStreamAsyncReader.BeginInvoke(null, null);
            var errAR = errorStreamAsyncReader.BeginInvoke(null, null);

            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                /* WaitHandle.WaitAll fails on single-threaded 
                 * apartments. Poll for completion instead:
                 */
                while (!(outAR.IsCompleted && errAR.IsCompleted))
                {
                    /* Check again every 10 milliseconds: */
                    Thread.Sleep(10);
                }
            }
            else
            {
                var arWaitHandles = new WaitHandle[2];
                arWaitHandles[0] = outAR.AsyncWaitHandle;
                arWaitHandles[1] = errAR.AsyncWaitHandle;

                if (!WaitHandle.WaitAll(arWaitHandles))
                {
                    throw new Exception($"Command line aborted: {fileName}");
                    /* Note: arguments aren't also shown in the 
                     * exception as they might contain privileged 
                     * information (such as passwords).
                     */
                }
            }

            var results = outputStreamAsyncReader.EndInvoke(outAR);
            errorMessage = errorStreamAsyncReader.EndInvoke(errAR);

            /* At this point the process should surely have exited,
             * since both the error and output streams have been fully read.
             * To be paranoid, let's check anyway...
             */
            if (!cmdLineProcess.HasExited)
            {
                cmdLineProcess.WaitForExit();
            }

            return results;
        }

        public static string Run(string fileName, string arguments)
        {
            string errorMsg;

            var result = Run(fileName, arguments, out errorMsg);

            if (errorMsg.Length > 0)
                throw new Exception(errorMsg);

            return result;
        }

        public static string Run(string fileName)
        {
            return Run(fileName, "");
        }

        private delegate string StringDelegate();
    }
}