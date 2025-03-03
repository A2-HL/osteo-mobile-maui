using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Event
{
   public class EventDetailModel
    {
        public string? Guid { get; set; }
        public string? Title { get; set; }
        public string? SessionDay { get; set; }
        public string? FromTime { get; set; }
        public string? Time { get; set; }
        public string? ToTime { get; set; }
        public string? Location { get; set; }
        public int TotalPatients { get; set; }
        public double DistanceCalucate { get; set; }
        public int Adults { get; set; }
        public int Kids { get; set; }
        public string PatientStr { get; set; }
        public int UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StatusId { get; set; }
        public int EventStatus { get; set; }
        public bool IsActive { get; set; }
        public int Privacy { get; set; }
        public string ParticipantName { get; set; }
        public string EventDate { get; set; }
        public int ParticipantId { get; set; }
        public int EventParticipanStatus { get; set; }
        public string UserGroupStr { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<EventParticipants> EventParticipants { get; set; }
        public List<EventSlotModel> EventSlots { get; set; }
    }
    public class EventParticipants
    {
        public int UserTypeId { get; set; }
        public int StatusId { get; set; }
    }
    public class EventListResponseModel
    {
        public IList<EventDetailModel> UpcomingHostedEvents { get; set; }
        public IList<EventDetailModel> CancelledEvents { get; set; }
        public IList<EventDetailModel> CompletedEvents { get; set; }
        public Data Data { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class Data
    {
        public IList<EventDetailModel> AllEvents { get; set; }
    }
    public class EventSlotModel
    {
        public string Guid { get; set; }
        public string FTime { get; set; }
        public string TTime { get; set; }
        public int StatusId { get; set; }
        public DateTime Date { get; set; }
        public bool IsBlock { get; set; }
        public string AppointmentId { get; set; }
        public string Datestr { get; set; }
        public string StatusStr { get; set; }
        public string ColorTitle { get; set; }
        public string ParticipateInfo { get; set; }
        public string ParticepateGuid { get; set; }
        public string AppointTypeStr { get; set; }
        public string Time { get; set; }
        public BadgeType StatusType { get; set; }
    }

    public class EventResponseModel
    {
        public EventDetailModel Data { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class EventSlotResponseModel
    {
        public List<EventSlotModel> Data { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
