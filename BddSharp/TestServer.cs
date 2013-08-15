﻿using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace BddSharp
{
    public class TestServer
    {
        public NameValueCollection EnvironmentVariables = new NameValueCollection();
        private readonly string PortNumber, ApplicationRoot;
        Process _iisProcess;


        public TestServer(string portNumber, string appRoot, NameValueCollection environmentVariables)
        {
            PortNumber = portNumber;
            ApplicationRoot = appRoot;
            EnvironmentVariables = environmentVariables;
        }

        public TestServer(string portNumber, string appRoot)
        {
            PortNumber = portNumber;
            ApplicationRoot = appRoot;
        }

        public TestServer()
        {
            PortNumber = ConfigurationManager.AppSettings["PortNumber"];
            ApplicationRoot = ConfigurationManager.AppSettings["AppRoot"];
        }

        private bool IsRunning
        {
            get { return _iisProcess != null && !_iisProcess.HasExited; }
        }

        public void Kill()
        {
            if (!IsRunning)
                return;

            _iisProcess.Kill();

        }

        public void Spawn()
        {
            if (IsRunning)
                return;

            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + @"\IIS Express\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = string.Format("/path:\"{0}\" /port:{1}", ApplicationRoot, PortNumber);

            _iisProcess.StartInfo.CreateNoWindow = true;
            //_iisProcess.StartInfo.UseShellExecute = false;


            if (EnvironmentVariables != null)
            {
                foreach (var key in EnvironmentVariables.AllKeys)
                {
                    _iisProcess.StartInfo.EnvironmentVariables.Add(key, EnvironmentVariables[key]);
                }
            }

            _iisProcess.Start();
        }
    }
}

/*
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace BddSharp
{
    public abstract class TestServer
    {
        private readonly string WebDevPath, PortNumber, ApplicationRoot, VirtualPath;
        public NameValueCollection EnvironmentVariables = new NameValueCollection();

        protected TestServer()
        {
            WebDevPath = ConfigurationManager.AppSettings["WebDevPath"];
            PortNumber = ConfigurationManager.AppSettings["PortNumber"];
            VirtualPath = ConfigurationManager.AppSettings["VirtualPath"];
            ApplicationRoot = ConfigurationManager.AppSettings["AppRoot"];
        }

        protected TestServer(string webDevPath, string portNumber, string virtualPath, string appRoot)
        {
            WebDevPath = webDevPath;
            PortNumber = portNumber;
            VirtualPath = virtualPath;
            ApplicationRoot = appRoot;
        }

        protected TestServer(string webDevPath, string portNumber, string applicationRoot, string virtualPath = "", NameValueCollection environmentVars = null)
        {
            WebDevPath = webDevPath;
            PortNumber = portNumber;
            ApplicationRoot = applicationRoot;
            VirtualPath = virtualPath;

            if (environmentVars != null)
                EnvironmentVariables.Add(environmentVars);
        }

        public bool IsRunning { get { return RunningServers.Length > 0; } }

        private Process[] RunningServers
        {
            get { return Process.GetProcessesByName("WebDev.WebServer40"); }
        } 

        public void Kill()
        {
            if (!IsRunning)
                return;

            BeforeKill();

            try
            {

                foreach (var serverProc in Process.GetProcesses())
                {
                    if (serverProc.ProcessName.Contains("WebDev.WebServer"))
                    {
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

            string LocalHostUrl = string.Format("http://localhost:{0}", PortNumber);

            Process process = new Process();

            process.StartInfo.FileName = WebDevPath;
            process.StartInfo.Arguments = string.Format("/port:{0} /path:\"{1}\" /virtual:\"{2}\"", PortNumber, ApplicationRoot, VirtualPath);

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

            AfterSpawn();
        }

        protected virtual void BeforeSpawn() { }
        protected virtual void AfterSpawn() { }
        protected virtual void BeforeKill() { }
        protected virtual void AfterKill() { }
    }
}
*/