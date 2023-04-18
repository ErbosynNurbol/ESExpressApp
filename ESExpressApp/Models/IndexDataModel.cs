using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ESExpressApp.Models
{
    public partial class IndexDataModel
    {
        public ObservableCollection<ArticleModel> FocusNewsList { get; set; }
        public ObservableCollection<TariffModel> TariffList { get; set; }
        public ObservableCollection<HWorkModel> HWorkList { get; set; }
        public ObservableCollection<ClientssayModel> ClientsSayList { get; set; }
    }
}
