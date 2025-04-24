using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class PizzaShopContext : DbContext
{
    public PizzaShopContext()
    {
    }

    public PizzaShopContext(DbContextOptions<PizzaShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Itemmodifiergroup> Itemmodifiergroups { get; set; }

    public virtual DbSet<MenuCategory> MenuCategories { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Modifier> Modifiers { get; set; }

    public virtual DbSet<ModifierGroup> ModifierGroups { get; set; }

    public virtual DbSet<Modifiermapping> Modifiermappings { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderItemModifier> OrderItemModifiers { get; set; }

    public virtual DbSet<OrderTable> OrderTables { get; set; }

    public virtual DbSet<OrderTax> OrderTaxes { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Paymentmode> Paymentmodes { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Rolesandpermission> Rolesandpermissions { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<Taxesandfess> Taxesandfesses { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    public virtual DbSet<Userrole1> Userroles1 { get; set; }

    public virtual DbSet<WaitingToken> WaitingTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=pizza-shop;Username=postgres;         password=Tatva@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Cityid).HasName("city_pkey");

            entity.ToTable("city");

            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Cityname)
                .HasMaxLength(150)
                .HasColumnName("cityname");
            entity.Property(e => e.State).HasColumnName("state");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.Cities)
                .HasForeignKey(d => d.State)
                .HasConstraintName("city_state_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Countryid).HasName("country_pkey");

            entity.ToTable("country");

            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Countryname)
                .HasMaxLength(150)
                .HasColumnName("countryname");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.HasIndex(e => e.Customeremail, "customers_customeremail_key").IsUnique();

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Customeremail)
                .HasMaxLength(250)
                .HasColumnName("customeremail");
            entity.Property(e => e.Customername)
                .HasMaxLength(250)
                .HasColumnName("customername");
            entity.Property(e => e.Isdelete)
                .HasDefaultValueSql("false")
                .HasColumnName("isdelete");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(200)
                .HasColumnName("phonenumber");
            entity.Property(e => e.TotalOrders).HasColumnName("total_orders");
            entity.Property(e => e.TotalPersons).HasColumnName("total_persons");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CustomerCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("customers_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CustomerModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("customers_modified_by_fkey");
        });

        modelBuilder.Entity<Itemmodifiergroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("itemmodifiergroup_pkey");

            entity.ToTable("itemmodifiergroup");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Maxallowed).HasColumnName("maxallowed");
            entity.Property(e => e.Minallowed).HasColumnName("minallowed");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Modifiergroupid).HasColumnName("modifiergroupid");

            entity.HasOne(d => d.Item).WithMany(p => p.Itemmodifiergroups)
                .HasForeignKey(d => d.Itemid)
                .HasConstraintName("itemmodifiergroup_itemid_fkey");

            entity.HasOne(d => d.Modifiergroup).WithMany(p => p.Itemmodifiergroups)
                .HasForeignKey(d => d.Modifiergroupid)
                .HasConstraintName("itemmodifiergroup_modifiergroupid_fkey");
        });

        modelBuilder.Entity<MenuCategory>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("menu_category_pkey");

            entity.ToTable("menu_category");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("menu_items_pkey");

            entity.ToTable("menu_items");

            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValueSql("true")
                .HasColumnName("is_available");
            entity.Property(e => e.IsDefaultTax)
                .HasDefaultValueSql("false")
                .HasColumnName("is_default_tax");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.IsFavourite)
                .HasDefaultValueSql("false")
                .HasColumnName("is_favourite");
            entity.Property(e => e.Itemname)
                .HasMaxLength(150)
                .HasColumnName("itemname");
            entity.Property(e => e.Itemtype)
                .HasMaxLength(150)
                .HasColumnName("itemtype");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rate)
                .HasPrecision(18, 2)
                .HasColumnName("rate");
            entity.Property(e => e.ShortCode)
                .HasMaxLength(200)
                .HasColumnName("short_code");
            entity.Property(e => e.TaxPercentage)
                .HasPrecision(18, 2)
                .HasColumnName("tax_percentage");
            entity.Property(e => e.Unitid).HasColumnName("unitid");

            entity.HasOne(d => d.Category).WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("menu_items_categoryid_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.Unitid)
                .HasConstraintName("menu_items_unitid_fkey");
        });

        modelBuilder.Entity<Modifier>(entity =>
        {
            entity.HasKey(e => e.Modifierid).HasName("modifier_pkey");

            entity.ToTable("modifier");

            entity.Property(e => e.Modifierid).HasColumnName("modifierid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.ModifierGroupId).HasColumnName("modifier_group_id");
            entity.Property(e => e.Modifiername)
                .HasMaxLength(500)
                .HasColumnName("modifiername");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rate)
                .HasPrecision(18, 2)
                .HasColumnName("rate");
            entity.Property(e => e.Unitid).HasColumnName("unitid");

            entity.HasOne(d => d.ModifierGroup).WithMany(p => p.Modifiers)
                .HasForeignKey(d => d.ModifierGroupId)
                .HasConstraintName("modifier_modifier_group_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.Modifiers)
                .HasForeignKey(d => d.Unitid)
                .HasConstraintName("modifier_unitid_fkey");
        });

        modelBuilder.Entity<ModifierGroup>(entity =>
        {
            entity.HasKey(e => e.ModifierGroupId).HasName("modifier_group_pkey");

            entity.ToTable("modifier_group");

            entity.Property(e => e.ModifierGroupId).HasColumnName("modifier_group_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Modifiermapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("modifiermapping_pkey");

            entity.ToTable("modifiermapping");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.ModifierGroupId).HasColumnName("modifier_group_id");
            entity.Property(e => e.ModifierId).HasColumnName("modifier_id");

            entity.HasOne(d => d.ModifierGroup).WithMany(p => p.Modifiermappings)
                .HasForeignKey(d => d.ModifierGroupId)
                .HasConstraintName("modifiermapping_modifier_group_id_fkey");

            entity.HasOne(d => d.Modifier).WithMany(p => p.Modifiermappings)
                .HasForeignKey(d => d.ModifierId)
                .HasConstraintName("modifiermapping_modifier_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Instruction)
                .HasMaxLength(200)
                .HasColumnName("instruction");
            entity.Property(e => e.InvoiceNo)
                .HasColumnType("character varying")
                .HasColumnName("Invoice_no");
            entity.Property(e => e.Isdelete)
                .HasDefaultValueSql("false")
                .HasColumnName("isdelete");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.NoOfPerson).HasColumnName("No_Of_Person");
            entity.Property(e => e.OrderNo)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(1008L, null, null, null, null, null)
                .HasColumnName("order_no");
            entity.Property(e => e.Orderdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("orderdate");
            entity.Property(e => e.OrdereType)
                .HasColumnType("character varying")
                .HasColumnName("ordere_Type");
            entity.Property(e => e.PaymentMode).HasColumnName("payment_mode");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(18, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OrderCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("orders_created_by_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("orders_customer_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.OrderModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("orders_modified_by_fkey");

            entity.HasOne(d => d.PaymentModeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentMode)
                .HasConstraintName("orders_payment_mode_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("orders_status_fkey");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_item_pkey");

            entity.ToTable("order_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasPrecision(18, 2)
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ReadyItem).HasColumnName("Ready_Item");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("order_item_item_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_item_order_id_fkey");
        });

        modelBuilder.Entity<OrderItemModifier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_item_modifiers_pkey");

            entity.ToTable("order_item_modifiers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ModifierId).HasColumnName("modifier_id");
            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.Price)
                .HasPrecision(18, 2)
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Modifier).WithMany(p => p.OrderItemModifiers)
                .HasForeignKey(d => d.ModifierId)
                .HasConstraintName("order_item_modifiers_modifier_id_fkey");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.OrderItemModifiers)
                .HasForeignKey(d => d.OrderItemId)
                .HasConstraintName("order_item_modifiers_order_item_id_fkey");
        });

        modelBuilder.Entity<OrderTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_table_pkey");

            entity.ToTable("order_table");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("false");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.TableId).HasColumnName("table_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderTables)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("order_table_customer_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderTables)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_table_order_id_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.OrderTables)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("order_table_table_id_fkey");
        });

        modelBuilder.Entity<OrderTax>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_tax_pkey");

            entity.ToTable("order_tax");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.TaxFlat)
                .HasPrecision(18, 2)
                .HasColumnName("tax_flat");
            entity.Property(e => e.TaxId).HasColumnName("tax_id");
            entity.Property(e => e.TaxPercentage)
                .HasPrecision(18, 2)
                .HasColumnName("tax_percentage");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderTaxes)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_tax_order_id_fkey");

            entity.HasOne(d => d.Tax).WithMany(p => p.OrderTaxes)
                .HasForeignKey(d => d.TaxId)
                .HasConstraintName("order_tax_tax_id_fkey");
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderstatus_pkey");

            entity.ToTable("orderstatus");

            entity.HasIndex(e => e.Status, "orderstatus_status_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Paymentmode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("paymentmode_pkey");

            entity.ToTable("paymentmode");

            entity.HasIndex(e => e.Status, "paymentmode_status_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Permissionid).HasName("permissions_pkey");

            entity.ToTable("permissions");

            entity.Property(e => e.Permissionid).HasColumnName("permissionid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(150)
                .HasColumnName("permission_name");
        });

        modelBuilder.Entity<Rolesandpermission>(entity =>
        {
            entity.HasKey(e => e.Rolesandpermissionid).HasName("rolesandpermission_pkey");

            entity.ToTable("rolesandpermission");

            entity.Property(e => e.Rolesandpermissionid).HasColumnName("rolesandpermissionid");
            entity.Property(e => e.CanAddEdit).HasColumnName("can_add_edit");
            entity.Property(e => e.CanDelete).HasColumnName("can_delete");
            entity.Property(e => e.CanView).HasColumnName("can_view");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Permissionid).HasColumnName("permissionid");
            entity.Property(e => e.Userroleid).HasColumnName("userroleid");

            entity.HasOne(d => d.Permission).WithMany(p => p.Rolesandpermissions)
                .HasForeignKey(d => d.Permissionid)
                .HasConstraintName("rolesandpermission_permissionid_fkey");

            entity.HasOne(d => d.Userrole).WithMany(p => p.Rolesandpermissions)
                .HasForeignKey(d => d.Userroleid)
                .HasConstraintName("rolesandpermission_userroleid_fkey");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Sectionid).HasName("section_pkey");

            entity.ToTable("section");

            entity.Property(e => e.Sectionid).HasColumnName("sectionid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasColumnName("description");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.SectionName)
                .HasMaxLength(250)
                .HasColumnName("section_name");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Stateid).HasName("state_pkey");

            entity.ToTable("state");

            entity.Property(e => e.Stateid).HasColumnName("stateid");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Statename)
                .HasMaxLength(150)
                .HasColumnName("statename");

            entity.HasOne(d => d.CountryNavigation).WithMany(p => p.States)
                .HasForeignKey(d => d.Country)
                .HasConstraintName("state_country_fkey");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("status");

            entity.HasIndex(e => e.Status1, "status_status_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status1)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Tableid).HasName("tables_pkey");

            entity.ToTable("tables");

            entity.Property(e => e.Tableid).HasColumnName("tableid");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Isavailable)
                .HasDefaultValueSql("true")
                .HasColumnName("isavailable");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Sectionid).HasColumnName("sectionid");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.TableName)
                .HasMaxLength(250)
                .HasColumnName("table_name");

            entity.HasOne(d => d.Customer).WithMany(p => p.Tables)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("tables_customer_id_fkey");

            entity.HasOne(d => d.Section).WithMany(p => p.Tables)
                .HasForeignKey(d => d.Sectionid)
                .HasConstraintName("tables_sectionid_fkey");
        });

        modelBuilder.Entity<Taxesandfess>(entity =>
        {
            entity.HasKey(e => e.Taxid).HasName("taxesandfess_pkey");

            entity.ToTable("taxesandfess");

            entity.Property(e => e.Taxid).HasColumnName("taxid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.FlatAmount)
                .HasPrecision(18, 2)
                .HasColumnName("flat_amount");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdefault)
                .HasDefaultValueSql("true")
                .HasColumnName("isdefault");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Percentage)
                .HasPrecision(18, 2)
                .HasColumnName("percentage");
            entity.Property(e => e.TaxType)
                .HasDefaultValueSql("true")
                .HasColumnName("tax_type");
            entity.Property(e => e.Taxname)
                .HasMaxLength(250)
                .HasColumnName("taxname");
            entity.Property(e => e.Taxvalue)
                .HasPrecision(18, 2)
                .HasColumnName("taxvalue");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Unitid).HasName("units_pkey");

            entity.ToTable("units");

            entity.Property(e => e.Unitid).HasColumnName("unitid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Shortname)
                .HasMaxLength(250)
                .HasColumnName("shortname");
            entity.Property(e => e.Unitname)
                .HasMaxLength(250)
                .HasColumnName("unitname");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Phonenumber, "users_phonenumber_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(400)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(50)
                .HasColumnName("phonenumber");
            entity.Property(e => e.ProfileImage)
                .HasMaxLength(300)
                .HasColumnName("profile_image");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("true")
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(150)
                .HasColumnName("username");
            entity.Property(e => e.Userrole).HasColumnName("userrole");
            entity.Property(e => e.Zipcode).HasColumnName("zipcode");

            entity.HasOne(d => d.CityNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.City)
                .HasConstraintName("users_city_fkey");

            entity.HasOne(d => d.CountryNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Country)
                .HasConstraintName("users_country_fkey");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.State)
                .HasConstraintName("users_state_fkey");

            entity.HasOne(d => d.UserroleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userrole)
                .HasConstraintName("users_userrole_fkey");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("userrole_pkey");

            entity.ToTable("userrole");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(150)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Userrole1>(entity =>
        {
            entity.HasKey(e => e.Userroleid).HasName("userroles_pkey");

            entity.ToTable("userroles");

            entity.Property(e => e.Userroleid).HasColumnName("userroleid");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.RoleName)
                .HasMaxLength(150)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<WaitingToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("waiting_token_pkey");

            entity.ToTable("waiting_token");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignedTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assigned_time");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.IsAssigned)
                .HasDefaultValueSql("false")
                .HasColumnName("is_assigned");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.NoOfPersons).HasColumnName("no_of_persons");
            entity.Property(e => e.SectionId).HasColumnName("section_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WaitingTokenCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("waiting_token_created_by_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.WaitingTokens)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("waiting_token_customerid_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.WaitingTokenModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("waiting_token_modified_by_fkey");

            entity.HasOne(d => d.Section).WithMany(p => p.WaitingTokens)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("waiting_token_section_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
