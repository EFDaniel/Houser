using System;
using System.Collections.Generic;
using System.Web;

namespace houser.utilities
{
    public class RateProps
    {
        public static int CompareProp(Dictionary<int, Dictionary<string, string>> property)
        {
        // Comparable property specs
            // Subject property    
            int S_SheriffValue = Convert.ToInt32(property[0]["Appraisal Value"]);
            double S_SalePrice = Convert.ToDouble(property[0].ContainsKey("SalePrice") ? property[0]["SalePrice"].Replace("$", string.Empty).Replace(",",string.Empty) : "0");
            DateTime S_SaleDate = Convert.ToDateTime(property[0].ContainsKey("SaleDate") ? property[0]["SaleDate"] : "01/01/1901");
            int S_SQFT = Convert.ToInt32(property[0].ContainsKey("SQFT") ? property[0]["SQFT"].Replace(",", string.Empty) : "0");
            int S_Beds = Convert.ToInt32(property[0].ContainsKey("Bedrooms") ? property[0]["Bedrooms"] : "0");
            double S_Baths = Convert.ToDouble(property[0].ContainsKey("Bathrooms") ? property[0]["Bathrooms"] : "0");

            // Compare Property
            double totWeight = 0;
            double totPPSQFT = 0;
            double avgSale = 0;
            double C_SQFTPRICE = -1;
            DateTime C_SaleDate;
            int C_SQFT = -1;
            double C_Beds = -1;
            double C_Baths = -1;
            int Index = 1;
            double weight = 100;
            while (property.Count > Index)
            {
                avgSale += Convert.ToDouble(property[Index]["C_SalePrice"].Replace(",", string.Empty)) / Convert.ToDouble(property[Index]["C_SQFT"].Replace(",", string.Empty));
                Index++;
            }
            avgSale = avgSale / property.Count;
            Index = 1;
            while (property.Count > Index)
            {
                C_SaleDate = Convert.ToDateTime(property[Index]["C_SaleDate"]);
                C_SQFT = Convert.ToInt32(property[Index]["C_SQFT"].Replace(",",string.Empty));
                C_SQFTPRICE = (Convert.ToDouble(property[Index]["C_SalePrice"].Replace(",", string.Empty)) / C_SQFT);
                if (C_SQFTPRICE < avgSale + .8 || C_SQFTPRICE > avgSale * 1.2)
                    C_SQFTPRICE = 0;
                C_Beds = Convert.ToDouble(property[Index]["C_Bedrooms"]);
                C_Baths = Convert.ToDouble(property[Index]["C_Bathrooms"]);
                weight = 100;
                weight -= C_SQFTPRICE == 0 ? 100 : 0;
                weight -= (Math.Abs(S_SQFT - C_SQFT)/10);
                weight -= (DateTime.Now - C_SaleDate).Days < 365 ? 0 :
                    (S_SaleDate - C_SaleDate).Days < 2 * 365 ? 5 :
                    (S_SaleDate - C_SaleDate).Days < 3 * 365 ? 11 :
                    (S_SaleDate - C_SaleDate).Days < 4 * 365 ? 20 :
                    (S_SaleDate - C_SaleDate).Days < 5 * 365 ? 30 :
                    (S_SaleDate - C_SaleDate).Days < 6 * 365 ? 45 :
                    (S_SaleDate - C_SaleDate).Days < 7 * 365 ? 60 :
                    (S_SaleDate - C_SaleDate).Days < 8 * 365 ? 80 : 100;
                weight -= (Math.Abs(S_Beds - C_Beds) * 30);
                weight -= (Math.Abs(S_Baths - C_Baths) * 30);
                weight = weight > 0 ? weight : 0;
                if (weight > 0)
                {
                    totWeight += weight;
                    totPPSQFT += C_SQFTPRICE*weight;
                }
                Index++;
            }
            double weightedPPSQFT = (totPPSQFT / totWeight) > 0 ? (totPPSQFT / totWeight) : 0;

            if (S_SheriffValue == 0 || S_SQFT == 0 || weightedPPSQFT == 0)
                return 999;
            else
                return Convert.ToInt32((S_SheriffValue/S_SQFT)-weightedPPSQFT);
        }
    }
}