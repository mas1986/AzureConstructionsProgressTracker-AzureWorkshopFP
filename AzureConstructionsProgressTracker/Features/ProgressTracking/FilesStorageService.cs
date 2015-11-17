using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;
using System.Web;

namespace AzureConstructionsProgressTracker.Features.ProgressTracking
{
    public class FilesStorageService
    {
        private const string ContainerName = "constructionprogressfiles";
        
        public async Task<string> UploadFile(string fileName, HttpPostedFileBase file)
        {
            // TODO: 
            // https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#programmatically-access-blob-storage
            // - Add proper nuget
            // - Connect to storage
            // - create container
            // - upload blob
            // - return URL

            string BlogconnectionString = "DefaultEndpointsProtocol=https;AccountName=masazurestorage2;AccountKey=CCAWxGDj7z8ItWqpZz9uOTkmi/EwsjwGuFmRKSXY+x6aSCMzx7xiAEMnVgenm+XwpvRcU4gAmf+kHLtBMPP2aQ==";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(BlogconnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            container.SetPermissions(new BlobContainerPermissions{     PublicAccess =    BlobContainerPublicAccessType.Blob    });

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(file.InputStream);

            return blockBlob.Uri.AbsoluteUri;
        }
    }
}