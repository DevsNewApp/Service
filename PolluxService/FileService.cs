using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PolluxService
{
    public partial class FileService : ServiceBase
    {

        Timer timer = new Timer(); // name space(using System.Timers;)
        ConfigClass config = new ConfigClass(); 
        public FileService()
        {
            InitializeComponent();
            eventLog1 = config.EventLog;
            
        }

        protected override void OnStart(string[] args)
        {
           
            eventLog1.WriteEntry("Al momento de Iniciar.");
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds
            timer.Enabled = true;

                
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Al momento de Parar el servicio.");
            WriteToFile("Service is stopped at " + DateTime.Now);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
        
            ProcessService processService = new ProcessService(config);
            if (config.NumberChannels != null)
            {
              
                List<string> files = processService.ProcessStart();
                string processinMsg = "";
                int lines = 0;
                foreach (string file in files)
                {
                    lines++;
                    processinMsg = processinMsg + file + "\n";
                }
               if (lines > 1) eventLog1.WriteEntry(processinMsg);
            }
            else
            {
                eventLog1.WriteEntry("ERROR DE CONFIGURACION");
            }
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
