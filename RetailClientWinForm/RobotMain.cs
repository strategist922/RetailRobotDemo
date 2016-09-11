//#define LUIS_INTEGRATION 
using RetailClientWinForm.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;
using MSTC.Robot.Interactions.RobotBehaviors.Speech;
using MSTC.Robot.Interactions.RobotBehaviors.Vision;
using MSTC.Robot.Interactions.RobotBehaviors;

namespace RetailClientWinForm
{
    public partial class RobotMain : Form
    {
        private delegate void WriteLineHandler(string text);
        private delegate void SetButtonStatusHandler(bool enabled);
        private BotFxClient bot = null;
        private string UserName = null;
        
#if LUIS_INTEGRATION
        SpeechInteration speech = new SpeechInteration(
                                            language: ConfigurationManager.AppSettings["SpeechLanguage"],
                                            speechApiPrimaryKey: ConfigurationManager.AppSettings["SpeechApiPrimaryKey"],
                                            speechApiSecondaryKey: ConfigurationManager.AppSettings["SpeechApiSecondaryKey"]);
                                            luisApiId:ConfigurationManager.AppSettings["LuisAppId"],
                                            luisSubscriptionId: ConfigurationManager.AppSettings["LuisSubscriptionKey"],;
#else
        SpeechInteration speech = new SpeechInteration(
                                            language: ConfigurationManager.AppSettings["SpeechLanguage"],
                                            speechApiPrimaryKey: ConfigurationManager.AppSettings["SpeechApiPrimaryKey"],
                                            speechApiSecondaryKey: ConfigurationManager.AppSettings["SpeechApiSecondaryKey"]);
#endif
        public RobotMain()
        {
            InitializeComponent();

            var key = ConfigurationManager.AppSettings["BotFxDirectLineSecret"];
            var url = ConfigurationManager.AppSettings["BotUrl"];
            bot = new BotFxClient(url, key);
        }
        private void WriteLine(string msg)
        {
            if (log.InvokeRequired)
            {
                log.Invoke(new WriteLineHandler((text) => { log.Text = log.Text + System.Environment.NewLine + text; }), msg);
            }
            else
            {
                log.Text = log.Text + System.Environment.NewLine + msg;

            }
        }
        private void EnableButton(bool state)
        {
            if (start.InvokeRequired)
            {
                start.Invoke(new SetButtonStatusHandler(
                                            (enabled) => { start.Enabled = enabled; }), state);
            }
            else
            {
                start.Enabled = state;
            }
        }
        private async void start_Click(object sender, EventArgs e)
        {
            //TalkToBot("牙膏多少錢").Wait();
            //return;
            EnableButton(false);
#if LUIS_INTEGRATION
            speech.StartInput(
                (p) =>
                {
                    WriteLine($">>>{p.GetEventData<string>()}");

                },
                async (f) =>
                {

                    WriteLine($"====== final result ====={System.Environment.NewLine} {f.GetEventData<string>()}");
                    var msg = f.GetEventData<string>();
                    var resp = await TalkToBot(msg);
                    WriteLine(resp);

                    EnableButton(true);

                },
                (intent)=>{
                    WriteLine(intent);
                    EnableButton(true);
                },
                (err) =>
                {
                    WriteLine(err.ErrorMessage);
                    EnableButton(true);

                }
            );
#else
            speech.StartInput(
                (p) =>
                {
                    WriteLine($">>>{p.GetEventData<string>()}");

                },
                async (f) =>
                {

                    WriteLine($"====== final result ====={System.Environment.NewLine} {f.GetEventData<string>()}");
                    var msg = f.GetEventData<string>();
                    var resp = await TalkToBot(msg);
                    WriteLine(resp);

                    EnableButton(true);

                },
                null,
                (err) =>
                {
                    WriteLine(err.ErrorMessage);
                    EnableButton(true);

                }
            );
#endif
        }
        async Task<string> TalkToBot(string msg)
        {
            var resp = await bot.TalkAsync(msg);
            return resp;
        }
        private string UploadToCloud(Bitmap bitmap)
        {
            CloudStorageAccount csa = new CloudStorageAccount(new StorageCredentials(
                                                                    ConfigurationManager.AppSettings["StorageAccountName"],
                                                                     ConfigurationManager.AppSettings["StorageAccountKey"]), true);
            var bc = csa.CreateCloudBlobClient();
            var cr = bc.GetContainerReference("retails");
            cr.CreateIfNotExists();
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                var blob = cr.GetBlockBlobReference($"{Guid.NewGuid().ToString("B")}.bmp");
                blob.UploadFromStream(ms);

                return blob.Uri.ToString();
            }
        }
        private void recognize_Click(object sender, EventArgs e)
        {
            VisionInteraction vision = new VisionInteraction(
                                                ConfigurationManager.AppSettings["VisionApiKey"],
                                                ConfigurationManager.AppSettings["FaceApiKey"],
                                                ConfigurationManager.AppSettings["StorageAccountName"],
                                                ConfigurationManager.AppSettings["StorageAccountKey"]);
            vision.StartInput(
                null,
                (arg) => {
                            UserName = arg.GetEventData<string>();
                            vision.StopInput();

                            
                        },
                null,
                null);
        }
    }
}
