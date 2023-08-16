using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs.Models;

namespace PolluxService
{
    internal class ProcessService
    {
        ConfigClass config;
        public ProcessService(ConfigClass configure)
        {
            this.config = configure;

        }
        public List<string> ProcessStart()
        {
            List<string> response = new List<string>();
            List<ChannelClass> channels = config.Channels;
            response.Add("Procesando archivos " + DateTime.Now.ToString() + "numero de canales a procesar " + config.NumberChannels);
              foreach (ChannelClass channel in channels)
               {
                if (channel.typeChannel.Equals(ConfigClass.BLOB2FS))
                {
                    BlobStorageClass blob = new BlobStorageClass(channel.pathSourceChannel, this.config);
                    FService fs = new FService(channel.pathDestinationChannel);
                    string resulta = this.TransferBlob2Fs(blob, fs);
                    if (resulta.Length >5)
                    response.Add(channel.pathSourceChannel + " --> "+ resulta); ;
                }
                if (channel.typeChannel.Equals(ConfigClass.FS2BLOB))
                {
                    BlobStorageClass blob = new BlobStorageClass(channel.pathDestinationChannel, this.config);
                    FService fs = new FService(channel.pathSourceChannel);
                    string resulta = this.TransferFs2Blob(fs, blob);
                    if (resulta.Length > 5)
                    response.Add(channel.pathDestinationChannel + " --> "+ resulta);
                }
              } 
           
            return response;
        }
        private string TransferBlob2Fs(BlobStorageClass blob, FService fs )
        {
            string resultado = "";
            try
            {
                Pageable<BlobItem>listblobs= blob.ListBlobs();
               
                foreach( BlobItem blobItem in listblobs)
                {
                    string resulting =  blob.FileSave(blobItem, fs);
                    resultado = resultado + blobItem.Name+ " "+ resulting + "\n";
                    blob.deleteBlob(blobItem);
                }
            }
            catch(Exception err)
            {
                resultado = err.Message;
            }
            return resultado;
            
        }

        private string TransferFs2Blob(FService fs,BlobStorageClass blob)
        {
            string resultado = "";
            try{

                if (fs.getPath() != null)
                {
                    string response = "";
                    List<string> files = fs.ProcessDirectory();

                    foreach (string file in files)
                    {
                        
                        response = blob.BlobSave(file);
                        if (response == "OK") File.Delete(file);
                        resultado = resultado + file + " " + response + "\n";
                    }

                }
            }catch (Exception err)
            {
                resultado = err.Message;
            }
            return resultado;

        }
    }

}
