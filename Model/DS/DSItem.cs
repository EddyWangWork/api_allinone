﻿using System.ComponentModel.DataAnnotations.Schema;
using demoAPI.Model.School;

namespace demoAPI.Model.DS
{
    public class DSItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<DSItemSub> DSItemSubs { get; set; }
    }
}
