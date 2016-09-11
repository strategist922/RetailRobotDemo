using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MSTC.Robot.Interactions.Utility
{
    internal class GenericTypeHelper
    {
        internal static T GetData<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return default(T);
            }
            if (typeof(T) == typeof(string))
            {
                var s = Encoding.Default.GetString(data);
                return (T)Convert.ChangeType(s, typeof(T));
            }
            else
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (var ms = new MemoryStream(data))
                {
                    T t = (T)binary.Deserialize(ms);
                    return t;

                }

            }
        }
    }
}
