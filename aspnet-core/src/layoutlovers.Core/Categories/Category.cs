﻿using Abp.Domain.Entities.Auditing;
using layoutlovers.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.Categories
{
    [Table("AppCategories")]
    public class Category: FullAuditedEntity<Guid>
    {
        public string Name { get; set; }
        public virtual ICollection<Product> Products { set; get; }
    }
}
