using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Newtonsoft.Json;
using OsteoMAUIApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Event
{
    public class EventSlotRequestModel : BaseViewModel
    {
        public string guid { get; set; }
        public int _start = 0;
        public int start
        {
            get => _start;
            set
            {
                if (this._start != value)
                {
                    SetProperty(ref _start, value);
                }
            }
        }
        public int _userTypeId = 2;
        public int userTypeId
        {
            get => _userTypeId;
            set
            {
                if (this._userTypeId != value)
                {
                    SetProperty(ref _userTypeId, value);
                }
            }
        }
        public string _searchParam;
        public string searchParam
        {
            get => _searchParam;
            set
            {
                if (this._searchParam != value)
                {
                    SetProperty(ref _searchParam, value);
                }
            }
        }
        #region |Serialization for submission|

        public string SerializeRequestFields()
        {
            var data = new Dictionary<string, object>
            {
                { "eventGuid", guid },
                { "userTypeId", userTypeId },
                { "search", new Dictionary<string, object>
                    {
                        { "value", searchParam }  
                    }
                },
                { "length",25},
                { "start",start},
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        #endregion
    }
}
