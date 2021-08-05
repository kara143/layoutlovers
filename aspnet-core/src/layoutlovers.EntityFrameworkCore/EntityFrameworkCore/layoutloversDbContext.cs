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
using layoutlovers.Products;
using layoutlovers.Categories;
using layoutlovers.Amazon;
using layoutlovers.Favorites;
using layoutlovers.FilterTags;
using layoutlovers.ProductFilterTags;
using layoutlovers.ShoppingCarts;

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
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<AmazonS3File> AmazonS3Files { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<FilterTag> FilterTags { get; set; }
        public virtual DbSet<ProductFilterTag> ProductFilterTags { set; get; }
        public virtual DbSet<ShoppingCart>ShoppingCarts { set; get; }

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

            modelBuilder.Entity<Product>()
                .HasOne(pt => pt.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(pt => pt.CategoryId);

            modelBuilder.Entity<AmazonS3File>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.AmazonS3Files)
                .HasForeignKey(pt => pt.ProductId);
            //Favorite
            modelBuilder.Entity<Favorite>()
             .HasOne(pt => pt.Product)
             .WithMany(p => p.Favorites)
             .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<Favorite>()
             .HasOne(pt => pt.User)
             .WithMany(p => p.Favorites)
             .HasForeignKey(pt => pt.UserId);
            
            //ProductFilterTag
            modelBuilder.Entity<ProductFilterTag>()
             .HasOne(pt => pt.Product)
             .WithMany(p => p.ProductFilterTags)
             .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<ProductFilterTag>()
             .HasOne(pt => pt.FilterTag)
             .WithMany(p => p.ProductFilterTags)
             .HasForeignKey(pt => pt.FilterTagId);

            //ShoppingCart
            modelBuilder.Entity<ShoppingCart>()
             .HasOne(pt => pt.Product)
             .WithMany(p => p.ShoppingCarts)
             .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<ShoppingCart>()
             .HasOne(pt => pt.User)
             .WithMany(p => p.ShoppingCarts)
             .HasForeignKey(pt => pt.UserId);

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
