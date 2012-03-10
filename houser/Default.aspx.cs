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
            if (!IsPostBack)
            {
                string sheriffSaleDatePage = GetWebRequest("http://oklahomacounty.org/sheriff/SheriffSales/");
                List<string> dates = PageScraper.GetSheriffSaleDates(sheriffSaleDatePage);
                foreach (var date in dates)
                {
                    ddlSaleDate.Items.Add(date);
                }
            }
        }

        /// <summary>
        /// Build the entire data structure for all properties.  Includes comparable data, property specs, ect.  03%2f15%2f2012
        /// </summary>
        private static void BuildAllPropertyData(string saleDate)
        {
            string sherifSaleUrl = "http://oklahomacounty.org/sheriff/SheriffSales/saledetail.asp?SaleDates="+saleDate;
            string sherifSaleWebRequestData = GetWebRequest(sherifSaleUrl);
            Dictionary<int, Dictionary<string, string>> propertyDictionary = PageScraper.Find(sherifSaleWebRequestData);
            foreach (var property in propertyDictionary)
            {
                string propertyAccountURL = property.Value["8"];
                string propertyAssessorData = GetWebRequest(propertyAccountURL);
                Dictionary<string, string> scrapedData = new Dictionary<string, string>(PageScraper.GetPropertyData(propertyAssessorData));
                string similarPropertyData = GetWebRequest(scrapedData["SimilarPropURL"]);
                Dictionary<string, string> scrapedCoreData = new Dictionary<string,string>(PageScraper.GetSimilarData(similarPropertyData));
            }
            string test = "break";
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

        #region UI events
        

        protected void btnPopulateData_Click(object sender, EventArgs e)
        {
            string saleDate = ddlSaleDate.SelectedItem.Value.Replace("/", "%2f"); 
            BuildAllPropertyData(saleDate);
        }

        protected void ddlSaleDate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        

    }
}