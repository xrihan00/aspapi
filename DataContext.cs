using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;


namespace aspapi
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Data> Data { get; set; }
    }

    public class Data
    {
        [Key]
        public int Id { get; set; }
        public int Value { get; set; }
    }
}