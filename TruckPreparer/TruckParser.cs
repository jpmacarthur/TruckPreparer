using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExcelDataReader;
using LCP.Common.Json;

namespace TruckPreparer
{
    public class TruckParser
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<TruckItem> parseTruck(string path)
        {
            log.Debug("Parsing Truck");
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; data source={0}; Extended Properties=Excel 8.0;", path);
            try
            {
                adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
            }
            catch (Exception) { throw; }
            var ds = new DataSet();

            adapter.Fill(ds, "anyNameHere");

            var data = ds.Tables["anyNameHere"];

            List<string> names = new List<string>();
            List<Quantity> quans = new List<Quantity>();
            List<TruckItem> items = new List<TruckItem>();

            int num = data.Rows.Count;
            for(int i = 1; i < num; i++)
            {
                TruckItem item = new TruckItem();
                Quantity quan = new Quantity();
                item.Name = data.Rows[i].ItemArray[0].ToString();
                item.size = data.Rows[i].ItemArray[1].ToString();
                quan.cases = data.Rows[i].ItemArray[10].ToString();
               quan.units = data.Rows[i].ItemArray[11].ToString();
                item.onTruck = quan;
                item.itemcode = data.Rows[i].ItemArray[3].ToString();
                items.Add(item);
            }
            log.Debug("Returning a list with " + items.Count.ToString() + " in it");
            return items;

        }
        public Dictionary<string, TruckItem> parseInv(string path)
        {
            log.Debug("Parsing Inventory");
            DataSet result = new DataSet();
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {

                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    result = reader.AsDataSet();

                    // The result of each spreadsheet is in result.Tables
                }
            }
            DataTable data = result.Tables[0];

            Dictionary<string, TruckItem> final = new Dictionary<string, TruckItem>();
            List<string> names = new List<string>();

            List<TruckItem> items = new List<TruckItem>();

            int num = data.Rows.Count;
            for (int i = 5; i < num-5; i++)
            {
                Quantity quan = new Quantity();
                TruckItem item = new TruckItem();
                List<Quantity> quans = new List<Quantity>();
                item.Name = data.Rows[i].ItemArray[1].ToString();
                item.size = data.Rows[i].ItemArray[2].ToString();
                quan.cases = data.Rows[i].ItemArray[17].ToString();
                quan.units = data.Rows[i].ItemArray[18].ToString();
                quan.unitsPerCase = data.Rows[i].ItemArray[3].ToString();
                item.inStore = quan;
                item.itemcode = data.Rows[i].ItemArray[0].ToString();
                item.fiveweek = data.Rows[i].ItemArray[19].ToString();
                item.location = data.Rows[i].ItemArray[8].ToString();
                items.Add(item);
            }
            foreach(TruckItem tr in items)
            {
                final.Add(tr.itemcode, tr);
            }
            log.Debug("returning a list with " + final.Count.ToString() + " items in it");
            return final;
        }
        public List<TruckItem> parseTruckExcelData(string path)
        {
            log.Debug("Using ExcelDataTHing");
            DataSet result = new DataSet();
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {

                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    result = reader.AsDataSet();

                    // The result of each spreadsheet is in result.Tables
                }
            }
            DataTable data = result.Tables[0];

            List<string> names = new List<string>();
            List<Quantity> quans = new List<Quantity>();
            List<TruckItem> items = new List<TruckItem>();

            int num = data.Rows.Count;
            for (int i = 1; i <= num - 1; i++)
            {
                TruckItem item = new TruckItem();
                Quantity quan = new Quantity();
                item.Name = data.Rows[i].ItemArray[0].ToString();
                item.size = data.Rows[i].ItemArray[1].ToString();
                quan.cases = data.Rows[i].ItemArray[10].ToString();
                quan.units = data.Rows[i].ItemArray[11].ToString();
                item.onTruck = quan;
                item.itemcode = data.Rows[i].ItemArray[3].ToString();
                items.Add(item);
            }
            log.Debug("Returning a list with " + items.Count.ToString() + " in it");
            return items;
        }
    }

}
