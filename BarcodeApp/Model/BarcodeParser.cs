using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeApp.Model
{
    public enum BARCODE_ACTION_TYPE
    {
        INCREASE_PRODUCT,
        DECREASE_PRODUCT,
        SET_WORKER
    }

    public class BarcodeParseResult
    {
        public BARCODE_ACTION_TYPE Type;

        public string Barcode;

        public Dictionary<string, object> ExtraData = new Dictionary<string, object>();
    }

    class BarcodeParser
    {
        public static BarcodeParseResult Parse(string code)
        {
            BarcodeParseResult result = new BarcodeParseResult();

            string[] parts = code.Split('_');
            if(parts.Length < 3)
            {
                return null;
            }

            if(parts[0].StartsWith("location", StringComparison.InvariantCultureIgnoreCase))
            {
                if(parts.Length != 4)
                {
                    return null;
                }
                switch(parts[1])
                {
                    case "up":
                        result.Type = BARCODE_ACTION_TYPE.INCREASE_PRODUCT;
                        break;
                    case "down":
                        result.Type = BARCODE_ACTION_TYPE.DECREASE_PRODUCT;
                        break;
                }
                Dictionary<string, object> extra = new Dictionary<string, object>();
                extra.Add("location_id", parts[2]);
                extra.Add("product_id", parts[3]);
                result.Barcode = parts[3];
                result.ExtraData = extra;                
            }
            else if(parts[0].StartsWith("worker"))
            {
                result.Type = BARCODE_ACTION_TYPE.SET_WORKER;
                Dictionary<string, object> extra = new Dictionary<string, object>();
                extra.Add("worker_id", parts[1]);
                extra.Add("product_id", parts[2]);
                result.Barcode = parts[2];
                result.ExtraData = extra;
            }
            else
            {
                return null;
            }

            return result;
        }
    }
}
