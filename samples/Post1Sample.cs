using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace azurecopy.BlogSamples
{
    public class UsingBlobs
    {
        public static string connectionString = "DefaultEndpointsProtocol=https;AccountName=[your_account_name];AccountKey=[your_account_primary_key]";
        public static string containerName = "pick_any_name";

        public static string UploadPhoto(byte[] photobytes, string photoName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            container.CreateIfNotExists();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(containerPermissions);
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            photo.UploadFromByteArray(photobytes, 0, photobytes.Length);
            return photo.Uri.ToString();
        }

        public static byte[] DownloadPhoto(string photoName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            byte[] photobytes = null;
            photo.DownloadToByteArray(photobytes, 0);
            return photobytes;
        }
    }
}
