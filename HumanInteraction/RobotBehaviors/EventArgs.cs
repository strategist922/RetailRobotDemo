using MSTC.Robot.Interactions.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MSTC.Robot.Interactions.RobotBehaviors
{
    public class ErrorEvent
    {
        public string ErrorMessage { get; set; }
        public Exception InnerException { get; set; }
    }
    public class IntentEvent
    {
        public string Result { get; set; }
    }
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
            return GenericTypeHelper.GetData<T>(EventData);
        }
    }
    public class FinalOutputEvent : PartialOutputEvent { }

    public class OutputData
    {
        private byte[] _rawData = null;
        public OutputData(byte [] data)
        {
            _rawData = data;
        }
        public T GetData<T>()
        {
            return GenericTypeHelper.GetData<T>(_rawData);
        }
    }
}
