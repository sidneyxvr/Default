using System;
using System.Collections.Generic;
using System.Text;

namespace Default.Business.Models
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public Guid? LastUpdateById { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Enable { get; set; }
        public bool Removed { get; set; }
    }
}
