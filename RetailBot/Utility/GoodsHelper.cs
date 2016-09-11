using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetailBot.Utility
{
    public class GoodsHelper
    {
        public static string GetLocation(string good)
        {
            return $"{good} 在右邊架上從上數下來第二排";
        }
        public static string GetPrice(string good)
        {
            return $"{good} 一隻40元";
        }
    }
}