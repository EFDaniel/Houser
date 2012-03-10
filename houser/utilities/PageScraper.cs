using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;

namespace houser.utilities
{
    // look at this http://www.dotnetperls.com/scraping-html for some regex and to see what you were doing.
    public static class PageScraper
    {
        public static Dictionary<int, Dictionary<string, string>> Find(string file)
        {
            Dictionary<int, Dictionary<string, string>> propertyGroup = new Dictionary<int, Dictionary<string, string>>();
            Dictionary<string, string> field = new Dictionary<string, string>();
            MatchCollection propertyListing = Regex.Matches(file, "<table cellpadding=\"0\" cellspacing=\"1\">\r\n\t(.*?)</table", RegexOptions.Singleline);
            int propertyID = 0;
            foreach (Match pl in propertyListing)
            {
                string property = pl.Groups[1].Value;
                List<string> fieldTrackerC1 = new List<string>();
                List<string> fieldTrackerC2 = new List<string>();
                MatchCollection PropertyRow = Regex.Matches(property, "<tr valign=\"top\"*>(.*?)</tr>",
                RegexOptions.Singleline);
                int fieldID = 0;
                foreach (Match m in PropertyRow)
                {
                    string propertyItem = m.Groups[1].Value.Trim();
                    fieldID++;
                    MatchCollection propertyItemKey = Regex.Matches(propertyItem, "<td><strong><font class=\"featureFont\">(.*?)</font>", RegexOptions.Singleline);
                    MatchCollection propertyItemValue = Regex.Matches(propertyItem, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline);
                    MatchCollection propertyRlink = Regex.Matches(propertyItem, "<td colspan=\"2\"><strong><font class=\"featureFont\"><a href=\"(.*?)\" target=\"_blank\">", RegexOptions.Singleline);
                    foreach (Match pf in propertyItemKey)
                    {
                        fieldTrackerC1.Add(pf.Groups[1].Value.Trim());
                    }
                    foreach (Match pf in propertyItemValue)
                    {
                        fieldTrackerC2.Add(pf.Groups[1].Value.Trim());
                    }
                    foreach (Match pf in propertyRlink)
                    {
                        field.Add(fieldID.ToString(), pf.Groups[1].Value.Trim());
                    }
                }

                foreach (var ft in fieldTrackerC1)
                {
                    int fieldIndex = fieldTrackerC1.IndexOf(ft);
                    field.Add(fieldTrackerC1[fieldIndex], fieldTrackerC2[fieldIndex]);
                }
                propertyGroup.Add(propertyID, new Dictionary<string, string>(field));
                propertyID++;
                field.Clear();
            }
            return propertyGroup;
        }

        public static string GetPropertyData(string file)
        {
            MatchCollection propertyDataSubSet = Regex.Matches(file, "<table width=\\\"700\\\"(.*?)</table", RegexOptions.Singleline);
            
            string propertyType = Regex.Match(propertyDataSubSet[0].Groups[1].Value.Trim(), "Type:</font></b><font size=\\\"2\\\" color=\\\"#FF0000\\\">(.*?)</font", RegexOptions.Singleline).Groups[1].Value.Trim();
            string salesDocsDataSet = Regex.Match(file, ">Sales Documents/Deed History(.*?)>Non Sales Documents/Deed History", RegexOptions.Singleline).Groups[1].Value.Trim();
            string saleDate = Regex.Match(salesDocsDataSet, "&nbsp;</font><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
            string salePrice = Regex.Match(salesDocsDataSet, "<p align=\\\"right\\\"><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
            return saleDate+salePrice;
        }

        public static List<string> GetSheriffSaleDates(string file)
        {
            string dataSubSet = Regex.Match(file, "<form name=\"SheriffSale\"(.*?)</form>", RegexOptions.Singleline).Groups[1].Value.Trim();
            MatchCollection dateMatches = Regex.Matches(dataSubSet, "<option value=\"(.*?)\">", RegexOptions.Singleline);
            List<string> dates = new List<string>();
            foreach (Match dt in dateMatches)
            {
                dates.Add(dt.Groups[1].Value);
            }
            
            return dates;
        }
    }
}