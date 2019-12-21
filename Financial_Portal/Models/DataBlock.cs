using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Models
{
    public class DataBlock
    {
        public List<string> Labels { get; set; }
        public string BarLabel1 { get; set; }
        public string BarLabel2 { get; set; }
        public string BarLabel3 { get; set; }

        public string BGColor1 { get; set; }
        public string BGColor2 { get; set; }
        public string BGColor3 { get; set; }

        public List<double> Data1 { get; set; }
        public List<double> Data2 { get; set; }
        public List<double> Data3 { get; set; }

    }
}