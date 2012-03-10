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

        public static Dictionary<string, string> GetPropertyData(string file)
        {
            Dictionary<string, string> propertyDate = new Dictionary<string, string>();
            propertyDate.Add("Type", Regex.Match(file, "Type:</font></b><font size=\\\"2\\\" color=\\\"#FF0000\\\">(.*?)</font", RegexOptions.Singleline).Groups[1].Value.Trim());
            string salesDocsDataSet = Regex.Match(file, ">Sales Documents/Deed History(.*?)>Non Sales Documents/Deed History", RegexOptions.Singleline).Groups[1].Value.Trim();
            MatchCollection saleDates = Regex.Matches(salesDocsDataSet, "&nbsp;</font><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline);
            foreach (Match sd in saleDates)
            {
                propertyDate.Add("SaleDate", sd.Groups[1].Value.Trim());            
            }
            MatchCollection salePrices = Regex.Matches(salesDocsDataSet, "<p align=\\\"right\\\"><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline);
            foreach (Match sp in salePrices)
            {
                propertyDate.Add("SalePrice", sp.Groups[1].Value.Trim());
            }
            
            string urlSimilarPropertiesSubSet = Regex.Match(file, "name=\\\"loc2\\\"(.*?)Click for sales of similar properties", RegexOptions.Singleline).Groups[1].Value.Trim();
            propertyDate.Add("SimilarPropURL", Regex.Match(urlSimilarPropertiesSubSet, "'(.*?)'", RegexOptions.Singleline).Groups[1].Value.Trim());
            return propertyDate;
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

        internal static Dictionary<string, string> GetSimilarData(string file)
        {
            Dictionary<string, string> subjectPropertyFields = new Dictionary<string, string>();
            string subjectPropertyTableSubSet = Regex.Match(file, "Property Information</font>(.*?)>Sales are pulled", RegexOptions.Singleline).Groups[1].Value.Trim();
            string subjectPropertyTable = Regex.Match(subjectPropertyTableSubSet, "<tbody>(.*?)</tbody>", RegexOptions.Singleline).Groups[1].Value.Trim();
            string yearBuild;
            string sqft;
            string builtAs;
            string bedrooms;
            string bathrooms;
            string exterior;
            throw new NotImplementedException();
        }
    }
}