using BarcodeApp.Database;
using BarcodeApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodeApp.Forms
{
    public partial class MainForm : Form
    {

        private BarcodeParser BarcodeParser = new BarcodeParser();

        const int PREFIX_LENGTH = 7;
        const String INVALID_FORMAT = "Invalid barcode format!";
        const String PRODUCT_NOT_FOUND = "This product I don't know";
        const String PRODUCT_FOUND = "I found this product and add one unit in stock";
        enum operations
        {
            COLDROOMIN,
            GARBAGE
        }
        enum message_type
        {
            INFO,
            ERROR
        }


        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowInfoMessage(String message, message_type msg_type)
        {
            switch (msg_type)
            {
                case message_type.INFO:
                    labelMessages.ForeColor = System.Drawing.Color.Green;
                    Console.Beep();
                    break;
                case message_type.ERROR:
                    labelMessages.ForeColor = System.Drawing.Color.Red;
                    Console.Beep(800, 1000);
                    break;
            }
            labelMessages.Text = message;

        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void clearScannedCodeButton_Click(object sender, EventArgs e)
        {
            barcodeText.Text = null;
            labelMessages.Text = "";
        }

        private void barcodeText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //clear label
                labelMessages.Text = "";
                if (barcodeText.Text == null)
                {
                    return;
                }
                string codeScanned = barcodeText.Text;

                var action = BarcodeParser.Parse(codeScanned);
                if (action == null)
                {
                    ShowInfoMessage(INVALID_FORMAT, message_type.ERROR);
                    return;
                }
                this.ExecuteAction(action);
                barcodeText.Text = null;
            }
        }

        protected void ExecuteAction(BarcodeParseResult barcodeParseResult)
        {
            //NOTE: THIS IS THE ENTRY FUNCTION FOR ALL THE LOGIC
            bool insertScannedBarcodeRecord = false;
            Product p = null;
            string productCode = barcodeParseResult.ExtraData["product_id"] as string;
            if (productCode == null)
            {
                return;
            }
            p = this.SearchProduct(productCode);
            switch (barcodeParseResult.Type)
            {
                case BARCODE_ACTION_TYPE.INCREASE_PRODUCT:
                case BARCODE_ACTION_TYPE.DECREASE_PRODUCT:
                    if (p == null)
                    {
                        p = this.TriggerProductInsertionFlow(productCode);
                        if (p == null)
                        {
                            return;
                        }
                        insertScannedBarcodeRecord = true;
                        ShowInfoMessage(string.Format("Product {0} with code {1} saved", p.NameFr, p.Gencode), message_type.INFO);
                    }
                    else
                    {
                        insertScannedBarcodeRecord = true;
                    }
                    break;
                case BARCODE_ACTION_TYPE.SET_WORKER:
                    insertScannedBarcodeRecord = true;
                    break;
            }
            if (insertScannedBarcodeRecord)
            {
                this.InsertScannedBarCode(barcodeParseResult, p);
                ShowInfoMessage(string.Format("Scanned code {0} saved", productCode), message_type.INFO);
            }
        }

        protected ScannedBarcode InsertScannedBarCode(BarcodeParseResult barcodeParseResult, Product product)
        {
            ScannedBarcode scannedBarcode = new ScannedBarcode();
            scannedBarcode.BeforeSave();
            Dictionary<string, object> extraData = barcodeParseResult.ExtraData;
            switch (barcodeParseResult.Type)
            {
                case BARCODE_ACTION_TYPE.DECREASE_PRODUCT:
                case BARCODE_ACTION_TYPE.INCREASE_PRODUCT:
                    string locationIdString = extraData["location_id"] as string;
                    int locationId = 0;
                    int.TryParse(locationIdString, out locationId);
                    if (product != null)
                    {
                        float quantity = 0;
                        if (barcodeParseResult.Type == BARCODE_ACTION_TYPE.INCREASE_PRODUCT)
                        {
                            quantity = 1;
                        }
                        else if (barcodeParseResult.Type == BARCODE_ACTION_TYPE.DECREASE_PRODUCT)
                        {
                            quantity = -1;
                        }
                        scannedBarcode.ProductId = product.ID;
                        scannedBarcode.ScannedText = barcodeParseResult.Barcode;
                        scannedBarcode.Quantity = quantity;
                        scannedBarcode.LocationID = locationId;
                        this.InsertNewScannedBarCode(scannedBarcode);
                        return scannedBarcode;
                    }
                    break;
                case BARCODE_ACTION_TYPE.SET_WORKER:
                    string workerIdString = extraData["worker_id"] as string;
                    string productIdString = extraData["product_id"] as string;
                    int workerId = 0;
                    long productId = 0;
                    int.TryParse(workerIdString, out workerId);
                    long.TryParse(productIdString, out productId);
                    if (workerId != 0 && productId != 0)
                    {
                        scannedBarcode.ProductId = productId;
                        scannedBarcode.ScannedText = barcodeParseResult.Barcode;
                        scannedBarcode.Quantity = 0;
                        scannedBarcode.WorkerID = workerId;
                        this.InsertNewScannedBarCode(scannedBarcode);
                        return scannedBarcode;
                    }
                    break;
            }
            return null;
        }

        private Product TriggerProductInsertionFlow(string productCode)
        {
            FormProduct form = new FormProduct();
            form.ProductCode = productCode;
            var result = form.ShowDialog();
            if (result != DialogResult.OK)
            {
                return null;
            }
            Product p = new Product();
            p.BeforeSaving();
            p.Gencode = productCode;
            p.NameEn = form.ProductName;
            p.NameFr = form.ProductName;
            this.InsertNewProduct(p);
            return p;
        }

        protected ScannedBarcode InsertNewScannedBarCode(ScannedBarcode sb)
        {
            using (AppDatabaseContext dbContext = new AppDatabaseContext())
            {
                dbContext.ScannedBarcodes.Add(sb);
                dbContext.SaveChanges();
                return sb;
            }
        }

        protected Product InsertNewProduct(Product p)
        {
            using (AppDatabaseContext dbContext = new AppDatabaseContext())
            {
                dbContext.Products.Add(p);
                dbContext.SaveChanges();
                return p;
            }
        }

        protected Product SearchProduct(string productCode)
        {
            using (AppDatabaseContext dbContext = new AppDatabaseContext())
            {
                //dbContext.Database.CreateIfNotExists();
                Product product = dbContext.Products.Where(p => p.Gencode == productCode).FirstOrDefault();
                return product;
            }
            return null;
        }
    }
}
