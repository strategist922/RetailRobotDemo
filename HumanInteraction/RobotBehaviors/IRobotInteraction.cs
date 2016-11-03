using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MSTC.Robot.Interactions.RobotBehaviors
{
    public delegate void OnPartialOutputReceived(PartialOutputEvent e);
    public delegate void OnIntentReceived(IntentEvent e);
    public delegate void OnOutputReceived(FinalOutputEvent e);
    public delegate void OnError(ErrorEvent e);



    /// <summary>
    /// Define the interation between robot and user
    /// </summary>
    interface IRobotInteraction:IDisposable
    {
        OnPartialOutputReceived OnPartialOutputReceivedHandler { get; set; }
        OnOutputReceived OnFinalOutputReceivedHandler { get; set; }
        OnIntentReceived OnIntentReceivedHandler { get; set; }
        OnError OnErrorHandler { get; set; }
        /// <summary>
        /// Starts an input from user
        /// </summary>
        /// <param name="OnPartialOutputReceivedHandler">Triggered when partial interaction results were received, can be null if the interaction is not a continous interaction</param>
        /// <param name="OnFinalOutputReceivedHandler">Triggered when full action is returned</param>
        /// <param name="OnIntentReceivedHandler">If the backend is able to identify the intent of this input, this event will be triggered.</param>
        Task StartInputAsync();

        Task StartInputAsync(Stream stream);
        void StopInput();

        Task StopInputAsync(OutputData data);
    }
}
