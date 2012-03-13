using System;
using System.Collections.Generic;
using System.Web;

namespace houser.utilities
{
    public class RateProps
    {
        public static int CompareProp(KeyValuePair<string, Dictionary<int, Dictionary<string, string>>> property)
        {
        // Comparable property specs
            // Subject property    
            int S_SheriffValue = -1;
            int S_SalePrice = -1;
            int S_SaleDate = -1;
            int S_SQFT = -1;
            int S_Beds = -1;
            int S_Baths = -1;

            // Compare Property
            int C_SalePrice = -1;
            int C_SaleDate = -1;
            int C_SQFT = -1;
            int C_Beds = -1;
            int C_Baths = -1;

            // weight out of 100
            // | sf-sf | /10 = sf weight
            // SaleD - ssDate >1 =5, >2 =11, >3 =20, >4 = 30, >5=45, > 6=60, >7=80
            // beds = -20/bed
            // baths = -10/ (.25bath)
            return 1;
        }
    }
}