using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;

namespace PictureOptimizer
{
    public class Functions
    {
        // TODO ex3: Implement communication with Service Bus using WebJob SDK
        private const string QueueName = "resizepicturesqueue";

        // https://azure.microsoft.com/pl-pl/documentation/articles/websites-dotnet-webjobs-sdk-service-bus/
        public static void ProcessQueueMessage([ServiceBusTrigger(QueueName)] ResizePictureMessage message, TextWriter logger)
        {
            var azureStorageConnectionString = ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString;

            byte[] data;
            using (var client = new WebClient())
            {
                data = client.DownloadData(message.PictureReference);
            }

            var tumbnail = ImageResizer.CreateTumbnail(Image.FromStream(new MemoryStream(data)));

            var storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
            var filesStorageService = new FilesStorageService(storageAccount);
            
            var uploadedFileUrl = filesStorageService.UploadFile(Guid.NewGuid().ToString(), tumbnail).Result;

            using (var context = new ConstructionsProgressTrackerContext())
            {
                context.ProgressTrackingEntries.Single(e => e.Id == message.Id).TumbnailPictureReference = uploadedFileUrl;
                context.SaveChanges();
            }
            
            logger.WriteLine($"Tumbnail for entry:{message.Id} successfully created.");
        }
    }
}
