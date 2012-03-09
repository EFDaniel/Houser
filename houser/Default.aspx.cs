using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using houser.utilities;
using System.Text.RegularExpressions;
using System.Data;

namespace houser
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Build the entire data structure for all properties.  Includes comparable data, property specs, ect.
        /// </summary>
        private static void BuildAllPropertyData()
        {
            string sherifSaleUrl = "http://oklahomacounty.org/sheriff/SheriffSales/saledetail.asp?SaleDates=03%2f15%2f2012";
            string sherifSaleWebRequestData = GetWebRequest(sherifSaleUrl);
            
            Dictionary<int, Dictionary<string, string>> propertyDictionary = ScrapeSherifSaleListingsX(sherifSaleWebRequestData);
            foreach (var property in propertyDictionary)
            {
                foreach (var field in property.Value)
                {
                    string key = field.Key;
                    string value = field.Value;
                    if (field.Key == "8")
                    {
                        string propertyAssessorData = GetWebRequest(field.Value);
                        ScrapePropertyAssessorData(propertyAssessorData);
                    }

                }
            }
        }

        private static string GetWebRequest(string url)
        {
            string strResults = "";
            WebResponse objResponse;
            WebRequest objRequest = System.Net.HttpWebRequest.Create(url);

            objResponse = objRequest.GetResponse();

            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                strResults = sr.ReadToEnd();
                sr.Close();
                return strResults;
            }
            
        }

        private static Dictionary<int, Dictionary<string, string>> ScrapeSherifSaleListingsX(string webRequestData)
        {
            return PageScraper.Find(webRequestData);
        }

        private static string ScrapePropertyAssessorData(string webRequestData)
        {
            return PageScraper.GetPropertyData(webRequestData);
        }

        protected void btnPopulateData_Click(object sender, EventArgs e)
        {
            BuildAllPropertyData();
        }

        
    }
}