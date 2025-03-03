using Newtonsoft.Json;
using OsteoMAUIApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Event
{
    public class EventRequestModel : BaseViewModel
    {
        public string _title;
        public string title
        {
            get => _title;
            set
            {
                if (this._title != value)
                {
                    SetProperty(ref _title, value);
                }
            }
        }

        public string _fromDateStr;
        public string fromDateStr
        {
            get => _fromDateStr;
            set
            {
                if (this._fromDateStr != value)
                {
                    SetProperty(ref _fromDateStr, value);
                }
            }
        }
        public string _toDateStr;
        public string toDateStr
        {
            get => _toDateStr;
            set
            {
                if (this._toDateStr != value)
                {
                    SetProperty(ref _toDateStr, value);
                }
            }
        }

        public string _fromTime;
        public string fromTime
        {
            get => _fromTime;
            set
            {
                if (this._fromTime != value)
                {
                    SetProperty(ref _fromTime, value);
                }
            }
        }
        public string _toTime;
        public string toTime
        {
            get => _toTime;
            set
            {
                if (this._toTime != value)
                {
                    SetProperty(ref _toTime, value);
                }
            }
        }
        public string sessionDate { get; set; }

        public string _location;
        public string location
        {
            get => _location;
            set
            {
                if (this._location != value)
                {
                    SetProperty(ref _location, value);
                }
            }
        }

        public int _participantId;
        public int participantId
        {
            get => _participantId;
            set
            {
                if (this._participantId != value)
                {
                    SetProperty(ref _participantId, value);
                }
            }
        }
        public bool _status = true;
        public bool status
        {
            get => _status;
            set
            {
                if (this._status != value)
                {
                    SetProperty(ref _status, value);
                }
            }
        }
        public double _ratting = 0;
        public double ratting
        {
            get => _ratting;
            set
            {
                if (this._ratting != value)
                {
                    SetProperty(ref _ratting, value);
                }
            }
        }
        public int _distance = 0;
        public int distance
        {
            get => _distance;
            set
            {
                if (this._distance != value)
                {
                    SetProperty(ref _distance, value);
                }
            }
        }
        public int _skip = 0;
        public int skip
        {
            get => _skip;
            set
            {
                if (this._skip != value)
                {
                    SetProperty(ref _skip, value);
                }
            }
        }

        #region |Serialization for submission|

        public string SerializeEventRequestFilterFields()
        {
            var data = new Dictionary<string, object>
            {
                { "currentDate", DateTime.Now.ToString("MM-dd-yyyy") },
                { "title", title },
                { "sessionDate", sessionDate },
                { "fromDateStr", fromDateStr },
                { "toDateStr", toDateStr },
                { "fromTime", fromTime },
                { "toTime", toTime},
                { "location", location },
                { "latitude", 31.029933 },
                { "longitude", 71.930011 },
                { "participantId",participantId},
                { "status",status},
                { "ratting",ratting},
                { "distance",distance},
                { "length",15},
                { "start",skip},

            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        #endregion
    }
}
