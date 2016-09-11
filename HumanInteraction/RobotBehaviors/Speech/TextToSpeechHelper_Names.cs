using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MSTC.Robot.Interactions.RobotBehaviors.Speech
{
    public partial class TextToSpeechHelper
    {
        private Dictionary<string, Dictionary<string, string>> speakerNames = new Dictionary<string, Dictionary<string, string>>{
            {
                "ar-EG",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (ar-EG, Hoda)" }
                }
            },
            {
                "de-DE",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (de-DE, Hedda)" },
                        { "Male","Microsoft Server Speech Text to Speech Voice (de-DE, Stefan, Apollo)" }
                }
            },
            {
                "en-AU",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (en-AU, Catherine" }
                }
            },
            {
                "en-CA",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (en-CA, Linda)" }
                }
            },
            {
                "en-GB",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (en-GB, Susan, Apollo))" },
                        { "Male","Microsoft Server Speech Text to Speech Voice (en-GB, George, Apollo)" }
                }
            },
            {
                "en-IN",new Dictionary<string, string>{
                        { "Male","Microsoft Server Speech Text to Speech Voice (en-IN, Ravi, Apollo)" }
                }
            },
            {
                "en-US",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)" },
                        { "Male","Microsoft Server Speech Text to Speech Voice (en-US, Ravi, Apollo)" }
                }
            },
            {
                "es-ES",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (es-ES, Laura, Apollo)" },
                        { "Male","Microsoft Server Speech Text to Speech Voice (es-ES, Pablo, Apollo)" }
                }
            },
            {
                "es-MX	",new Dictionary<string, string>{
                        { "Male","Microsoft Server Speech Text to Speech Voice (es-MX, Raul, Apollo)" }
                }
            },
            {
                "fr-CA",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (fr-CA, Caroline)" }
                }
            },
            {
                "fr-FR",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (fr-FR, Julie, Apollo)" },
                        { "Male","Microsoft Server Speech Text to Speech Voice (fr-FR, Paul, Apollo)" }
                }
            },
            {
                "it-IT",new Dictionary<string, string>{
                        { "Male","Microsoft Server Speech Text to Speech Voice (it-IT, Cosimo, Apollo)" }
                }
            },
            {
                "ja-JP",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (ja-JP, Ayumi, Apollo)" },
                        { "Male","Microsoft Server Speech Text to Speech Voice (ja-JP, Ichiro, Apollo)" }
                }
            },
            {
                "pt-BR",new Dictionary<string, string>{
                        { "Male","Microsoft Server Speech Text to Speech Voice (pt-BR, Daniel, Apollo)" }
                }
            },
            {
                "ru-RU",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (pt-BR, Daniel, Apollo)" },
                        { "Male","Microsoft Server Speech Text to Speech Voice (ru-RU, Pavel, Apollo)" }
                }
            },
            {
                "zh-CN",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (zh-CN, HuihuiRUS)" },
                        //{ "Female","Microsoft Server Speech Text to Speech Voice (zh-CN, Yaoyao, Apollo)" },
                        { "Male", "Microsoft Server Speech Text to Speech Voice (zh-CN, Kangkang, Apollo)" }
                }
            },
            {
                "zh-HK",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (zh-HK, Tracy, Apollo)" },
                        { "Male","Microsoft Server Speech Text to Speech Voice (zh-HK, Danny, Apollo)" }
                }
            },
            {
                "zh-TW",new Dictionary<string, string>{
                        { "Female","Microsoft Server Speech Text to Speech Voice (zh-TW, Yating, Apollo)" },
                        { "Male","Microsoft Server Speech Text to Speech Voice (zh-TW, Zhiwei, Apollo)" }
                }
            }
        };
    }
}
