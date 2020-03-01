using System;
using System.Collections.Generic;
using System.Text;

namespace Default.Business.Models.Views
{
    public class PagedList<TEntity> where TEntity : Entity
    {
        public List<TEntity> Collection { get; set; }
        public int Amount { get; set; }
    }
}
