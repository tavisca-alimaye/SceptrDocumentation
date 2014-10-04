using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace SceptrDocumentation.Models
{
    public class Client
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Supplier> Suppliers { get; set; }

    }

    public class Supplier
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Client> Clients { get; set; }

    }

    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public class SupplierProduct
    {
        [Key, Column(Order = 0)]
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        [Key, Column(Order = 1)]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }


    public class ClientSupplierMap
    {
        [Key]
        public int Id { get; set; }

        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }

    public class Verb
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }



    public class Question
    {
        [Key]
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public string Keyword { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int VerbId { get; set; }
        [ForeignKey("VerbId")]
        public Verb Verb { get; set; }
    }

    public class QuestionAnswerMapper
    {
        [Key, Column(Order = 0)]
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        [Key, Column(Order = 1)]
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        public string Answer { get; set; }
    }


    public class SceptrDocumentationDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierProduct> SupplierProducts{ get; set; }

        public DbSet<ClientSupplierMap> ClientSupplierMaps { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswerMapper> QuestionAnswerMappers { get; set; }
        public DbSet<Verb> Verbs { get; set; }
    }
}