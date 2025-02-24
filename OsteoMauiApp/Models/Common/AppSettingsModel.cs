using OsteoMAUIApp.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Common
{
    public class AppSettingsModel : BaseViewModel
    {
        [PrimaryKey]
        public string id { get; set; } = "1";

        private bool _menuRefreshRequired = true;
        public bool menuRefreshRequired
        {
            get { return this._menuRefreshRequired; }
            set
            {
                if (this._menuRefreshRequired != value)
                {
                    SetProperty(ref _menuRefreshRequired, value);
                }
            }
        }

        private bool _cardRefreshRequired = true;
        public bool cardRefreshRequired
        {
            get { return this._cardRefreshRequired; }
            set
            {
                if (this._cardRefreshRequired != value)
                {
                    SetProperty(ref _cardRefreshRequired, value);
                }
            }
        }

        private string _cardScansLastSync;
        public string CardScansLastSync
        {
            get { return this._cardScansLastSync; }
            set
            {
                if (this._cardScansLastSync != value)
                {
                    SetProperty(ref _cardScansLastSync, value);
                }
            }
        }
    }
}
