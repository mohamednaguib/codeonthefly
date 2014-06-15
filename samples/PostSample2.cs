using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace azurecopy.BlogSamples
{
    class UsingBlobsSample2
    {
        // connection string for azure storage
        public static string connectionString = "DefaultEndpointsProtocol=https;AccountName=[your_account_name];AccountKey=[your_account_primary_key]";
        // container to be generated to upload files to
        public static string containerName = "pick_any_name";

        public static string UploadPhotoAync(byte[] photobytes, string photoName)
        {
            // define your azure cloud account
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            // intilaize a client for the created account
            CloudBlobClient client = account.CreateCloudBlobClient();
            // intialize the container
            CloudBlobContainer container = client.GetContainerReference(containerName);
            // create the container if doesn't exist
            container.CreateIfNotExists();
            // set public permission for your blob to be able to be used by everyone 
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(containerPermissions);
            // set the photo path ... note the photo name can be either "name.jpg" or it can has a virtual path like "pics/folder/name.jpg"
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            // start uploading
            photo.UploadFromByteArrayAsync(photobytes, 0, photobytes.Length);
            // return the url of the photo after uploading
            return photo.Uri.ToString();
        }

        public static string UploadPhotoAsyncWithCallBack(byte[] photobytes, string photoName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            container.CreateIfNotExists();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(containerPermissions);

            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("key", "value");
            // begin upload async and pass the callback function
            photo.BeginUploadFromByteArray(photobytes, 0, photobytes.Length, new AsyncCallback(HandleUploadCallBack), data);
            return photo.Uri.ToString();
        }
        // handling the call back function of uploading blobs
        private static void HandleUploadCallBack(IAsyncResult ar)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)ar.AsyncState;
            // some code that notify users by sending email or other service
        }


        public static byte[] DownloadPhotoAync(string photoName)
        {
            // define your azure cloud account
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            // intilaize a client for the created account
            CloudBlobClient client = account.CreateCloudBlobClient();
            // intialize the container
            CloudBlobContainer container = client.GetContainerReference(containerName);
            // create the container if doesn't exist
            container.CreateIfNotExists();
            // set public permission for your blob to be able to be used by everyone 
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(containerPermissions);
            // set the photo path ... note the photo name can be either "name.jpg" or it can has a virtual path like "pics/folder/name.jpg"
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            byte[] photobytes = null;
            // start uploading
            photo.DownloadToByteArrayAsync(photobytes, 0);
            // return the url of the photo after uploading
            return photobytes;
        }

        public static string DownloadPhotoAsyncWithCallBack(string photoName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            container.CreateIfNotExists();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(containerPermissions);

            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            byte[] photobytes = null;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("key", "value");
            // begin upload async and pass the callback function
            photo.BeginDownloadToByteArray(photobytes, 0, new AsyncCallback(HandleUploadCallBack), data);
            return photo.Uri.ToString();
        }
        // handling the call back function of uploading blobs
        private static void HandleDownloadCallBack(IAsyncResult ar)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)ar.AsyncState;
            // do any logic you want to
        }

    }
}
