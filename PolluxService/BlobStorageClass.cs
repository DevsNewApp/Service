using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Identity;
using Azure;



namespace PolluxService
{
    internal class BlobStorageClass
    {
        private string connectionString = "";
        private string _container = "";
        public string Container
        {
            get => _container;
            set => _container = value;
        }
        public BlobStorageClass(string container, ConfigClass conf)
        {
            this._container = container;
            this.connectionString = conf.ConnectionString;

        }
        public void deleteBlob(BlobItem elemento)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(this.connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_container);
                containerClient.CreateIfNotExists(PublicAccessType.Blob);
                BlockBlobClient blockBlob = containerClient.GetBlockBlobClient(elemento.Name);
                blockBlob.DeleteIfExists();


            }
            catch (Exception)
            {
                throw;
            }
        }
        public string FileSave(BlobItem elemento, FService fs)
        {
            string resultado = "";
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(this.connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_container);
                containerClient.CreateIfNotExists(PublicAccessType.Blob);
                BlockBlobClient blockBlob = containerClient.GetBlockBlobClient(elemento.Name);
                using (var fileStream = System.IO.File.OpenWrite(fs.getPath() + "\\" + elemento.Name))
                {
                    blockBlob.DownloadTo(fileStream);
                    resultado = "OK";
                }


            }
            catch (Exception err) {
                resultado = err.Message;
            }
            return resultado;
        }
        public Pageable<BlobItem> ListBlobs()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(this.connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_container);
            return containerClient.GetBlobs();
        }
        
        public string  BlobSave(string filetoup)
        {
            string retorno = "OK";
               
           
            try
            {
                string fileName = Path.GetFileName(filetoup);
                BlobClient blobClient = new BlobClient(
                                        connectionString: connectionString,
                                        blobContainerName: _container,
                                        blobName: fileName);

                blobClient.Upload(filetoup);
            }
            catch (AuthenticationFailedException err)
            {
                retorno = err.Message;
            }
           
            return retorno;
        }
    }

}
