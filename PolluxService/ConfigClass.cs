using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace PolluxService
{
    internal class ConfigClass
    {
        public static string BLOB2FS = "blob2fs";
        public static string FS2BLOB = "fs2blob";
        private string _numberChannels = "2";
        private List<ChannelClass> _channels = new List<ChannelClass>();
        private string _pathOrigenChannel1 = "c:\\archivos";
        private string _pathDestinationChannel1 = "pdf";
        private string _typeChannel1 = FS2BLOB;
        private string _pathOrigenChannel2 = "pdf";
        private string _pathDestinationChannel2 = "c:\\resultados";
        private string _typeChannel2 = BLOB2FS;
        private string _pathOrigenChannel3 = "c:\\archivos";
        private string _pathDestinationChannel3 = "pdf";
        private string _typeChannel3 = FS2BLOB;
        private string _pathOrigenChannel4 = "c:\\resultados";
        private string _pathDestinationChannel4 = "dat";
        private string _typeChannel4 = BLOB2FS;
        private string _applicationName = "PolluxService";
        private string _logName = "FiPolluxLog";
        private string _logNamea = "StartLog";
        private string _connectionString = "";
        private System.Diagnostics.EventLog _eventLog = new System.Diagnostics.EventLog();
        public ConfigClass()
        {
            
            if (!System.Diagnostics.EventLog.SourceExists(this._applicationName))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    this._applicationName, this._logName);

            }
            _eventLog.Source = this._applicationName;
            _eventLog.Log = this._logName;
            this.RegistryInit();
        }
        private void RegistryInit()
        {

            try
            {
                _eventLog.WriteEntry("Al principio del Registry Init");
                RegistryKey keyreg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Vida");
                if (keyreg != null)
                {
                    _eventLog.WriteEntry("Si el keyreg no es igual a Null");
                    this._connectionString = keyreg.GetValue("ConnectionString") != null ? keyreg.GetValue("ConnectionString").ToString() : "";

                    this._applicationName = keyreg.GetValue("ApplicationName") !=null ? keyreg.GetValue("ApplicationName").ToString() : "";
                    this._logName = keyreg.GetValue("LogName").ToString() !=null ? keyreg.GetValue("LogName").ToString(): "";
                    this._numberChannels = keyreg.GetValue("NumberChannels").ToString() != null ? keyreg.GetValue("NumberChannels").ToString(): "";
                    int counter = int.Parse(this._numberChannels);
                    _eventLog.WriteEntry("Este es el numero de canales "+ this._numberChannels);
                    for (int num = 1; num <= counter; num++)
                    {
                        string nChl = num.ToString().Trim();
                        RegistryKey keyregSub = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Vida\" + nChl);
                        _eventLog.WriteEntry("Esta es la SubKey Abierta SOFTWARE/Vida/" + nChl);
                        if (keyregSub != null)
                        {

                            _eventLog.WriteEntry("Este es KeyRegSub interno ");
                            ChannelClass baseData = new ChannelClass();
                            baseData.pathSourceChannel = keyregSub.GetValue("PathSourceChannel").ToString() != null ? keyregSub.GetValue("PathSourceChannel").ToString() : "";
                            baseData.pathDestinationChannel = keyregSub.GetValue("PathTargetChannel").ToString() != null ? keyregSub.GetValue("PathTargetChannel").ToString() : "";
                            baseData.typeChannel = keyregSub.GetValue("TypeChannel").ToString() != null ? keyregSub.GetValue("TypeChannel").ToString() : "";
                            baseData.deleteFiles = keyregSub.GetValue("DeleteFiles").ToString() != null ? keyregSub.GetValue("DeleteFiles").ToString() : "";
                            baseData.backupFiles = keyregSub.GetValue("BackupFiles").ToString() != null ? keyregSub.GetValue("BackupFiles").ToString() : "";
                            this._channels.Add(baseData);
                            keyregSub.Close();
                        }
                        else {
                            this._numberChannels = null;

                            _eventLog.WriteEntry("Esto es que no encontro la clave " );
                        }
                    }
                    keyreg.Close();


                }
                else
                {
                    RegistryKey keycon = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Vida");
                    keycon.SetValue("ApplicationName", this._applicationName);
                    keycon.SetValue("LogName", this._logName);
                    keycon.SetValue("NumberChannels", this._numberChannels);
                    RegistryKey keyregSub = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Vida\1");
                    keyregSub.SetValue("PathSourceChannel", this._pathOrigenChannel1);
                    keyregSub.SetValue("PathTargetChannel", this._pathDestinationChannel1);
                    keyregSub.SetValue("TypeChannel", this._typeChannel1);
                    keyregSub.SetValue("DeleteFiles", "true");
                    keyregSub.SetValue("BackupFiles", "false");
                    ChannelClass baseData = new ChannelClass();
                    baseData.Init(this._pathOrigenChannel1, this._pathDestinationChannel1, this._typeChannel1, "true", "false");
                    _channels.Add(baseData);
                    keyregSub = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Vida\2");
                    keyregSub.SetValue("PathSourceChannel", this._pathOrigenChannel2);
                    keyregSub.SetValue("PathTargetChannel", this._pathDestinationChannel2);
                    keyregSub.SetValue("TypeChannel", this._typeChannel2);
                    keyregSub.SetValue("DeleteFiles", "true");
                    keyregSub.SetValue("BackupFiles", "false");
                    baseData = new ChannelClass();
                    baseData.Init(this._pathOrigenChannel2, this._pathDestinationChannel2, this._typeChannel2, "true", "false");
                    _channels.Add(baseData);
                    keycon.Close();
                    keyregSub.Close();
                }
            } catch (Exception g)
            {
                throw new Exception("Servicio falla en su inicio por configuracion errada: " + g.Message);
            }
        }
        public string PathOrigenChannel1
        {
            get => _pathOrigenChannel1;
            set => _pathOrigenChannel1 = value;
        }
        public string PathDestinationChannel1
        {
            get => _pathDestinationChannel1;
            set => _pathDestinationChannel1 = value;
        }
        public string TypeChannel1
        {
            get => _typeChannel1;
            set => _typeChannel1 = value;
        }
        public string PathOrigenChannel2
        {
            get => _pathOrigenChannel2;
            set => _pathOrigenChannel2 = value;
        }
        public string PathDestinationChannel2
        {
            get => _pathDestinationChannel2;
            set => _pathDestinationChannel2 = value;
        }
        public string TypeChannel2
        {
            get => _typeChannel2;
            set => _typeChannel2 = value;
        }
        public string PathOrigenChannel3
        {
            get => _pathOrigenChannel3;
            set => _pathOrigenChannel3 = value;
        }
        public string PathDestinationChannel3
        {
            get => _pathDestinationChannel3;
            set => _pathDestinationChannel3 = value;
        }
        public string TypeChannel3
        {
            get => _typeChannel3;
            set => _typeChannel3 = value;
        }
        public string PathOrigenChannel4
        {
            get => _pathOrigenChannel4;
            set => _pathOrigenChannel4 = value;
        }
        public string PathDestinationChannel4
        {
            get => _pathDestinationChannel4;
            set => _pathDestinationChannel4 = value;
        }
        public string TypeChannel4
        {
            get => _typeChannel4;
            set => _typeChannel4 = value;
        }
        public string NumberChannels
        {
            get => _numberChannels;
            set => _numberChannels = value;
        }
        public List<ChannelClass> Channels
        {
            get => _channels;
            set => _channels = value;
        }
        public string ApplicationName
        {
            get => _applicationName;
            set => _applicationName = value;
        }
        public string LogName
        {
            get => _logName;
            set => _logName = value;
        }
        public string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }
        public System.Diagnostics.EventLog EventLog
        {
            get => _eventLog;
            set => _eventLog = value;
        }

    }
}
