using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Event
{
   public class EventDetailModel
    {
        public string? Title { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Location { get; set; }
        public int TotalPatients { get; set; }
        public double Distance { get; set; }
    }
}
