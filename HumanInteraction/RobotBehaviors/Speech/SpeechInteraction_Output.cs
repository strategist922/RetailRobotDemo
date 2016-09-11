using Microsoft.ProjectOxford.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSTC.Robot.Interactions.RobotBehaviors;
namespace MSTC.Robot.Interactions.RobotBehaviors.Speech
{
    public partial class SpeechInteration : IRobotInteraction
    {
        public async void StartOutput(OutputData data)
        {
            TextToSpeechHelper helper = new TextToSpeechHelper(
                                                SPEECHAPI_KEY1,
                                                SPEECHAPI_KEY2,
                                                Language
                                            );
            await helper.PostAsync(data.GetData<string>());
        }
    }
}
