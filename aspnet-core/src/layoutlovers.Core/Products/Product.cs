using layoutlovers.Amazon;
using layoutlovers.Categories;
using layoutlovers.Favorites;
using layoutlovers.ProductFilterTags;
using layoutlovers.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.Products
{
    [Table("AppProducts")]
    public class Product: FullAuditedEntityWithName<Guid>, IProduct
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ProductType ProductType { get; set; }
        public virtual ICollection<AmazonS3File> AmazonS3Files { set; get; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<ProductFilterTag> ProductFilterTags { get; set; }
        
    }
}
