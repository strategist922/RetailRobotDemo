//#define LUIS_INTEGRATION 
using RetailClientWinForm.Helpers;
using RetailClientWinForm.RobotBehaviors;
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

namespace RetailClientWinForm
{
    public partial class RobotMain : Form
    {
        private delegate void WriteLineHandler(string text);
        private delegate void SetButtonStatusHandler(bool enabled);
        private BotFxClient bot = null;
#if LUIS_INTEGRATION
        SpeechInteration speech = new SpeechInteration(
                                            language: "zh-TW",
                                            speechApiPrimaryKey: "ba7f8231a25d4f2b871f326213cac223",
                                            speechApiSecondaryKey: "fbc2ff88054f40099aef8d780b56b5c8",
                                            luisApiId: "d91dbfa4-dcff-49bb-8d33-5371a25cdadd",
                                            luisSubscriptionId: "6d35713e91ae40859618c38a6ceb95c5");
#else
        SpeechInteration speech = new SpeechInteration(
                                            language: "zh-TW",
                                            speechApiPrimaryKey: "ba7f8231a25d4f2b871f326213cac223",
                                            speechApiSecondaryKey: "fbc2ff88054f40099aef8d780b56b5c8");
#endif
        public RobotMain()
        {
            InitializeComponent();

            var key = ConfigurationManager.AppSettings["BotFxDirectLineSecret"];
            var url = ConfigurationManager.AppSettings["BotUrl"];
            bot = new BotFxClient(url,key);
        }
        private void WriteLine(string msg)
        {
            if (log.InvokeRequired)
            {
                log.Invoke(new WriteLineHandler((text)=> { log.Text = log.Text + System.Environment.NewLine + text; }), msg);
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
            }else
            {
                start.Enabled = state;
            }
        }
        private void start_Click(object sender, EventArgs e)
        {
            //TalkToBot("牙膏多少錢").Wait();
            //return;
            EnableButton(false);
#if LUIS_INTEGRATION
            speech.StartInput(
                (p) =>
                {
                    WriteLine(p.GetEventData<string>());

                },
                (f) =>
                {

                    WriteLine($"====== final result ====={System.Environment.NewLine} {f.GetEventData<string>()}");
                },
                (r) =>
                {
                    WriteLine(r.Result);
                    EnableButton(true);
                }
            );
#else
            speech.StartInput(
                (p) =>
                {
                    WriteLine($">>>{p.GetEventData<string>()}");

                },
                (f) =>
                {

                    WriteLine($"====== final result ====={System.Environment.NewLine} {f.GetEventData<string>()}");
                    EnableButton(true);

                    TalkToBot(f.GetEventData<string>());
                },
                null
            );
#endif
        }
        async Task TalkToBot(string msg)
        {
            var resp = await bot.TalkAsync(msg);

        }
    }
}
