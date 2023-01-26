using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarcodeApp.Model
{
    [Table("Products_v1", Schema = "Stocks")]
    public class Product
    {
        public Product()
        {
        }

        public void BeforeSaving()
        {
            this.Bijection_Id = 0;
            this.NomCorrespondance = "";
            this.Gencod2 = "";
            this.Supplier = "";
            this.IsSoldItem = false;
            this.IsBoughtItem = false;
            this.IsUnboxedBoughtItem = false;
            this.IsTerminalItem = false;
            this.IsRawItem = false;
            this.IsPrimitiveVaisselle = false;
            this.IsAgregatVaisselle = false;
            this.IsPrimitiveRH = false;
            this.IsAgregatRH = false;
            this.CategoryFr = "";
            this.IsMainDish = false;
            this.IsDesert = false;
            this.IsDirtyPlate = false;
            this.IsDirtyGlass = false;
            this.IsDirtyCutlury = false;
            this.IsDrink = false;
            this.IsAlias = false;
            this.IsMarchandise = false;
            this.IsConsummable = false;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        [Column("name fr")]
        [StringLength(76)]
        public string NameFr
        {
            get;

            set;
        }

        [Column("name_en")]
        [StringLength(80)]
        public string NameEn
        {
            get;

            set;
        }

        [Column("gencod")]
        public string Gencode
        {
            get;

            set;
        }

        [Column("Bijection_Id")]
        public int Bijection_Id
        {
            get;

            set;
        }

        [Column("Nom correspondance")]
        public string NomCorrespondance
        {
            get;

            set;
        }


        [Column("gencod2")]
        public string Gencod2
        {
            get;
            set;
        }

        [Column("Supplier")]
        public string Supplier
        {
            get;
            set;
        }

        [Column("Is_sold item")]
        public bool IsSoldItem
        {
            get;
            set;
        }

        [Column("Is_bought item")]
        public bool IsBoughtItem
        {
            get;
            set;
        }

        [Column("Is_unboxed_bought_item")]
        public bool IsUnboxedBoughtItem
        {
            get;
            set;
        }

        [Column("Is_Terminal_item")]
        public bool IsTerminalItem
        {
            get;
            set;
        }

        [Column("Is_raw item")]
        public bool IsRawItem
        {
            get;
            set;
        }

        [Column("Is_Primitive_vaisselle")]
        public bool IsPrimitiveVaisselle
        {
            get;
            set;
        }

        [Column("Is_Agregat_vaisselle")]
        public bool IsAgregatVaisselle
        {
            get;
            set;
        }

        [Column("Is_Primitive_RH")]
        public bool IsPrimitiveRH
        {
            get;
            set;
        }

        [Column("Is_Agregat_RH")]
        public bool IsAgregatRH
        {
            get;
            set;
        }

        [Column("category fr")]
        public string CategoryFr
        {
            get;
            set;
        }

        [Column("Is_Main_dish")]
        public bool IsMainDish
        {
            get;
            set;
        }

        [Column("Is_desert")]
        public bool IsDesert
        {
            get;
            set;
        }

        [Column("Is_dirty_plate")]
        public bool IsDirtyPlate
        {
            get;
            set;
        }

        [Column("Is_dirty_glass")]
        public bool IsDirtyGlass
        {
            get;
            set;
        }

        [Column("Is_dirty_cutlury")]
        public bool IsDirtyCutlury
        {
            get;
            set;
        }

        [Column("Is_drink")]
        public bool IsDrink
        {
            get;
            set;
        }

        [Column("Is_without_receipe")]
        
        public bool IsWithoutReceipe
        {
            get;
            set;
        }

        [Column("Is_alias")]
        
        public bool IsAlias
        {
            get;
            set;
        }

        [Column("Is_marchandise")]
        
        public bool IsMarchandise
        {
            get;
            set;
        }

        [Column("Is_consummable")]
        public bool IsConsummable
        {
            get;
            set;
        }
    }
}
