using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RetailClientWinForm.RobotBehaviors
{
    public delegate void OnPartialOutputReceived(PartialOutputEvent e);
    public delegate void OnIntentReceived(IntentEvent e);
    public delegate void OnOutputReceived(FinalOutputEvemt e);

    public class IntentEvent
    {
        public string Result { get; set; }
    }
    public class FinalOutputEvemt : PartialOutputEvent { }
    public class PartialOutputEvent
    {
        
        public bool IsCompleted { get; set; }
        internal byte[] EventData { get; set; }
        internal void SetEventData(string data)
        {
            EventData = Encoding.Default.GetBytes(data);
        }
        public T GetEventData<T>()
        {
            if(EventData == null || EventData.Length == 0)
            {
                return default(T);
            }
            if(typeof(T) == typeof(string))
            {
                var s = Encoding.Default.GetString(EventData);
                return (T)Convert.ChangeType(s, typeof(T));
            }
            else
            {
                BinaryFormatter binary = new BinaryFormatter();
                T t = (T)binary.Deserialize(new MemoryStream(EventData));

                return t;
            }
        }
    }
    /// <summary>
    /// Define the interation between robot and user
    /// </summary>
    interface IRobotInteraction
    {
        /// <summary>
        /// Starts an input from user
        /// </summary>
        /// <param name="OnPartialOutputReceivedHandler">Triggered when partial interaction results were received, can be null if the interaction is not a continous interaction</param>
        /// <param name="OnFinalOutputReceivedHandler">Triggered when full action is returned</param>
        /// <param name="OnIntentReceivedHandler">If the backend is able to identify the intent of this input, this event will be triggered.</param>
        void StartInput(
            OnPartialOutputReceived OnPartialOutputReceivedHandler, 
            OnOutputReceived OnFinalOutputReceivedHandler,
            OnIntentReceived OnIntentReceivedHandler);
    }
}
