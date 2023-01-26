using System.Data.Entity;
using System.Linq;
using BarcodeApp.Model;

namespace BarcodeApp.Database
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<ScannedBarcode> ScannedBarcodes
        {
            get;
            set;
        }

        public DbSet<Product> Products
        {
            get;
            set;
        }
    }
}
