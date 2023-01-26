using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarcodeApp.Model
{
    [Table("Scanned_barcodes", Schema ="Barcode")]
    public class ScannedBarcode
    {
        public ScannedBarcode()
        {
            this.Quantity = 0;
        }

        public void BeforeSave()
        {
            // Place here your initialization code.
            this.ScanDatetime = DateTime.UtcNow;
        }

        // XPLiteObject does not have a built-in key and you need to add your own key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Scanned_barcode_Id")]
        public int ID { get; set; }


        [Column("Scanned_text")]
        public string ScannedText
        {
            get;
            set;
        }

        [Column("Scan_datetime")]
        public DateTime ScanDatetime
        {
            get;
            set;
        }

        [Column("Product_id")]
        public long ProductId
        {
            get;
            set;
        }

        [Column("Quantity")]
        public float Quantity
        {
            get;

            set;
        }

        [Column("location_id")]
        public int? LocationID
        {
            get;

            set;
        }

        [Column("worker_id")]
        public int? WorkerID
        {
            get;

            set;
        }
    }
}
