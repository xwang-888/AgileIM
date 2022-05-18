using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Client.Common;

namespace AgileIM.Client.Models
{
    public class MainTipModel
    {
        public MainTipModel()
        {

        }
        public MainTipModel(MainTipType type, int count)
        {
            Count = count;
            Type = type;
        }
        public MainTipType Type { get; set; }
        public int Count { get; set; }
    }
}
