using System.IO;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Common
{
    public class FilesStorageService
    {
        private const string ContainerName = "constructionprogressfiles";
        private readonly CloudBlobContainer _container;
        private readonly StorageUri _blobStorageUri;

        public FilesStorageService(CloudStorageAccount storageAccount)
        {
            _blobStorageUri = storageAccount.BlobStorageUri;
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(ContainerName);
        }
        
        public async Task<string> UploadFile(string fileName, Stream file)
        {
            if (!_container.Exists())
            {
                _container.Create();
                _container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(file);
            return $"{_blobStorageUri.PrimaryUri}{ContainerName}/{fileName}";
        }
    }
}