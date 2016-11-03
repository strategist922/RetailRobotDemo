using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.ProjectOxford.Face;
using MSTC.Robot.Interactions.RobotBehaviors;
namespace MSTC.Robot.Interactions.RobotBehaviors.Vision
{
    public partial class VisionInteraction : IRobotInteraction
    {
        private string VISIONAPI_KEY = null;
        private string FACEAPI_KEY = null;
        private string STORAGE_NAME = null;
        private string STORAGE_KEY = null;
        public VisionInteraction(string visionKey, string faceKey, string storageName, string stroageKey)
        {
            VISIONAPI_KEY = visionKey;
            FACEAPI_KEY = faceKey;
            STORAGE_NAME = storageName;
            STORAGE_KEY = stroageKey;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public void StopInput()
        {

        }
        OnOutputReceived _onOutputReceived = null;
        public async Task StartInput(OnPartialOutputReceived OnPartialOutputReceivedHandler, 
                                OnOutputReceived OnFinalOutputReceivedHandler, 
                                OnIntentReceived OnIntentReceivedHandler, 
                                OnError OnErrorHandler)
        {
            _onOutputReceived = OnFinalOutputReceivedHandler;

            await CapturePhoto();
        }

        public async Task StopInputAsync(OutputData data)
        {
            //No vision output interaction is required
        }
    }
}