using System.Collections.ObjectModel;

namespace Program.ViewModels
{
    public class AdminViewModel
    {
        public ObservableCollection<ItemAnalyticsData> ItemAnalytics { get; set; }
        public ObservableCollection<TableAnalyticsData> TableAnalytics { get; set; }

        public AdminViewModel()
        {
            DBAAnalytics db = new DBAAnalytics();

            ItemAnalytics = new ObservableCollection<ItemAnalyticsData>(db.GetItemAnalytics());
            TableAnalytics = new ObservableCollection<TableAnalyticsData>(db.GetTableAnalytics());
        }
    }
}
