using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BddSharp
{
    public abstract class TestServer
    {
        public string DevServerPath = @"C:\Program Files (x86)\Common Files\Microsoft Shared\DevServer\11.0\WebDev.WebServer40.exe";
//        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string PortNumber, PhysicalPath, VirtualPath;
        public NameValueCollection EnvironmentVariables = new NameValueCollection();

        protected TestServer(string portNumber, string physicalPathCompiledApp, string virtualPath = "", NameValueCollection environmentVars = null)
        {
            PortNumber = portNumber;
            PhysicalPath = physicalPathCompiledApp;
            VirtualPath = virtualPath;

            if (environmentVars != null)
                EnvironmentVariables.Add(environmentVars);

            IsRunning = false;
        }

        public bool IsRunning { get; private set; }

        public void Kill()
        {
            if (!IsRunning)
                return;

            BeforeKill();

//            logger.Debug("--- Attemping to kill test server ---");
            try
            {

                foreach (var serverProc in Process.GetProcesses())
                {
                    if (serverProc.ProcessName.Contains("WebDev.WebServer"))
                    {
//                        logger.Debug("Killing process: " + serverProc.ProcessName);
                        serverProc.Kill();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            AfterKill();
        }

        public void Spawn()
        {
            if (IsRunning)
                return;

            BeforeSpawn();

//            logger.Debug("--- Spawning Test Server for tests ---");

            string LocalHostUrl = string.Format("http://localhost:{0}", PortNumber);
            //            string PhysicalPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName).FullName + "\\ALG.Garage.Web"; //  the path of compiled web app

//            logger.Debug(String.Format("Test server: {0}", LocalHostUrl));

            Process process = new Process();

            process.StartInfo.FileName = DevServerPath;
            process.StartInfo.Arguments = string.Format("/port:{0} /path:\"{1}\" /virtual:\"{2}\"", PortNumber, PhysicalPath, VirtualPath);

            if (EnvironmentVariables != null)
            {
                foreach (var key in EnvironmentVariables.AllKeys)
                {
                    process.StartInfo.EnvironmentVariables.Add(key, EnvironmentVariables[key]);
                }
            }

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;

            process.Start();
            IsRunning = true;

            AfterSpawn();
        }

        protected virtual void BeforeSpawn() { }
        protected virtual void AfterSpawn() { }
        protected virtual void BeforeKill() { }
        protected virtual void AfterKill() { }
    }
}
