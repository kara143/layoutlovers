using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using layoutlovers.Authorization.Delegation;
using layoutlovers.Authorization.Roles;
using layoutlovers.Authorization.Users;
using layoutlovers.Chat;
using layoutlovers.Editions;
using layoutlovers.Friendships;
using layoutlovers.MultiTenancy;
using layoutlovers.MultiTenancy.Accounting;
using layoutlovers.MultiTenancy.Payments;
using layoutlovers.Storage;
using layoutlovers.Categories;
using layoutlovers.Amazon;
using layoutlovers.Favorites;
using layoutlovers.FilterTags;
using layoutlovers.ProductFilterTags;
using layoutlovers.ShoppingCarts;
using layoutlovers.Purchases;
using layoutlovers.DownloadRestrictions;
using layoutlovers.DownloadAmazonS3Files;
using layoutlovers.LayoutProducts;

namespace layoutlovers.EntityFrameworkCore
{
    public class layoutloversDbContext : AbpZeroDbContext<Tenant, Role, User, layoutloversDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }
        public virtual DbSet<LayoutProduct> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<AmazonS3File> AmazonS3Files { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<FilterTag> FilterTags { get; set; }
        public virtual DbSet<ProductFilterTag> ProductFilterTags { set; get; }
        public virtual DbSet<ShoppingCart>ShoppingCarts { set; get; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<DownloadAmazonS3File> DownloadAmazonS3Files { get; set; }
        public virtual DbSet<DownloadRestriction> DownloadRestrictions { get; set; }
        
        public layoutloversDbContext(DbContextOptions<layoutloversDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            //Product
            modelBuilder.Entity<LayoutProduct>()
                .HasOne(pt => pt.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(pt => pt.CategoryId);

            //AmazonS3File
            modelBuilder.Entity<AmazonS3File>()
                .HasOne(pt => pt.LayoutProduct)
                .WithMany(p => p.AmazonS3Files)
                .HasForeignKey(pt => pt.LayoutProductId);
            //Favorite
            modelBuilder.Entity<Favorite>()
             .HasOne(pt => pt.LayoutProduct)
             .WithMany(p => p.Favorites)
             .HasForeignKey(pt => pt.LayoutProductId);

            modelBuilder.Entity<Favorite>()
             .HasOne(pt => pt.User)
             .WithMany(p => p.Favorites)
             .HasForeignKey(pt => pt.UserId);
            
            //ProductFilterTag
            modelBuilder.Entity<ProductFilterTag>()
             .HasOne(pt => pt.LayoutProduct)
             .WithMany(p => p.ProductFilterTags)
             .HasForeignKey(pt => pt.LayoutProductId);

            modelBuilder.Entity<ProductFilterTag>()
             .HasOne(pt => pt.FilterTag)
             .WithMany(p => p.ProductFilterTags)
             .HasForeignKey(pt => pt.FilterTagId);

            //ShoppingCart
            modelBuilder.Entity<ShoppingCart>()
             .HasOne(pt => pt.LayoutProduct)
             .WithMany(p => p.ShoppingCarts)
             .HasForeignKey(pt => pt.LayoutProductId);

            modelBuilder.Entity<ShoppingCart>()
             .HasOne(pt => pt.User)
             .WithMany(p => p.ShoppingCarts)
             .HasForeignKey(pt => pt.UserId);

            //Purchase
            modelBuilder.Entity<Purchase>()
             .HasOne(pt => pt.LayoutProduct)
             .WithMany(p => p.Purchases)
             .HasForeignKey(pt => pt.LayoutProductId);

            modelBuilder.Entity<Purchase>()
             .HasOne(pt => pt.User)
             .WithMany(p => p.Purchases)
             .HasForeignKey(pt => pt.UserId);

            //DownloadAmazonS3File
            modelBuilder.Entity<DownloadAmazonS3File>()
             .HasOne(pt => pt.AmazonS3File)
             .WithMany(p => p.DownloadAmazonS3Files)
             .HasForeignKey(pt => pt.AmazonS3FileId);

            modelBuilder.Entity<DownloadAmazonS3File>()
             .HasOne(pt => pt.User)
             .WithMany(p => p.DownloadAmazonS3Files)
             .HasForeignKey(pt => pt.UserId);

            //DownloadRestriction
            modelBuilder.Entity<DownloadRestriction>()
                .HasOne(pt => pt.SubscribableEdition)
                .WithMany(p => p.DownloadRestrictions)
                .HasForeignKey(pt => pt.SubscribableEditionId);

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
