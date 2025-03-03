using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Event
{
    public class ParticepantModel
    {
        public string FullName { get; set; }
        public string Age { get; set; }
        public string PaymentStr { get; set; }
        public string AttendanceStr { get; set; }
        public int PractitionerFeedbackRating { get; set; }
        public string UserGuid { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string Location { get; set; }
    }
    public class ParticepantResponseModel
    {
        public List<ParticepantModel> Data { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
