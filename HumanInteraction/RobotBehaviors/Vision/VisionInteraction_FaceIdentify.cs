using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.ProjectOxford.Face;
using MSTC.Robot.Interactions.RobotBehaviors;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing.Imaging;

namespace MSTC.Robot.Interactions.RobotBehaviors.Vision
{
    partial class VisionInteraction : IRobotInteraction
    {
        private string UploadToCloud(Bitmap bitmap)
        {
            CloudStorageAccount csa = new CloudStorageAccount(new StorageCredentials(
                                                                    ConfigurationManager.AppSettings["StorageAccountName"],
                                                                     ConfigurationManager.AppSettings["StorageAccountKey"]), true);
            var bc = csa.CreateCloudBlobClient();
            var cr = bc.GetContainerReference("retailsdemo");
            cr.CreateIfNotExists(BlobContainerPublicAccessType.Container);
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms,ImageFormat.Bmp);
                var blob = cr.GetBlockBlobReference($"{Guid.NewGuid().ToString("N")}.bmp");
                ms.Seek(0, SeekOrigin.Begin);
                blob.UploadFromStream(ms, ms.Length);
                
                return blob.Uri.ToString();
            }
        }
        internal async Task CapturePhoto()
        {
            var capture = new VideoCapture(CaptureDevice.Any);
            int sleepTime = 33;
            string url = null;
            using (var window = new Window("capture"))
            {
                Mat image = new Mat();

                while (true)
                {
                    capture.Read(image);
                    if (image.Empty())
                        break;

                    window.ShowImage(image);
                    if (Cv2.WaitKey(sleepTime) != 0)
                    {
                        var bitmap = BitmapConverter.ToBitmap(image);
                        url = UploadToCloud(bitmap);

                        break;
                    }
                }
            }
            await IdentifyUsersAsync(url);

        }

        internal async Task IdentifyUsersAsync(string url)
        {
            VisionServiceClient VisionServiceClient = new VisionServiceClient(VISIONAPI_KEY);
            VisualFeature[] visualFeatures = new VisualFeature[] {
                                                        VisualFeature.Adult, VisualFeature .Categories,
                                                            VisualFeature.Color,VisualFeature.Description,
                                                                VisualFeature.Faces,VisualFeature.ImageType,
                                                                    VisualFeature.Tags};

            //url = "https://michistorageea.blob.core.windows.net/data/DSC01498.JPG";

            var result = await VisionServiceClient.AnalyzeImageAsync(url, visualFeatures);

            if (result.Faces != null && result.Faces.Count() > 0)
            {
                var personGroupId = "demo";
                var fsc = new FaceServiceClient(FACEAPI_KEY);
                var faces = await fsc.DetectAsync(url);
                var faceIds = faces.Select(f => f.FaceId).ToArray();
                var faceIdentifyResults = await fsc.IdentifyAsync(personGroupId, faceIds);
                List<string> users = new List<string>();
                foreach (var identifyResult in faceIdentifyResults)
                {
                    var user = identifyResult.Candidates.OrderByDescending(c => c.Confidence).FirstOrDefault();
                    var person = await fsc.GetPersonAsync(personGroupId, user.PersonId);
                    users.Add(person.Name);
                }

                if(_onOutputReceived != null)
                {
                    _onOutputReceived(new FinalOutputEvemt()
                    {
                        IsCompleted = true,
                        EventData = Encoding.UTF8.GetBytes(string.Join(",", users))
                    });
                }
            }
        }
    }
}

