using System.Collections.ObjectModel;

namespace Program.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<ItemData> DataItems { get; set; }

        public MainViewModel()
        {
            DBAItem db = new DBAItem();
            var list = db.GetAllItems();
            DataItems = new ObservableCollection<ItemData>(list);
        }
    }
}
