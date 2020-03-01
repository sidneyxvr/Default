using System.Collections.Generic;

namespace Default.Api.ViewModels
{
    public class PagedListViewModel<T> where T : ViewModel
    {
        public List<T> Collection { get; set; }
        public int Amount { get; set; }
    }
}
