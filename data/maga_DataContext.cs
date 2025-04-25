using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalInRome.data
{
    public class maga_DataContext
    {
        public static magaChnlAirbnbDataContext DC_AIRBNB
        {
            get
            {
                magaChnlAirbnbDataContext dc = new magaChnlAirbnbDataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaPdf_DataContext DC_PDF
        {
            get
            {
                magaPdf_DataContext dc = new magaPdf_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaPayPal_DataContext DC_PAYPAL
        {
            get
            {
                magaPayPal_DataContext dc = new magaPayPal_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaInvoice_DataContext DC_INVOICE
        {
            get
            {
                magaInvoice_DataContext dc = new magaInvoice_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaRental_DataContext DC_RENTAL
        {
            get
            {
                magaRental_DataContext dc = new magaRental_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaContent_DataContext DC_CONTENT
        {
            get
            {
                magaContent_DataContext dc = new magaContent_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaUser_DataContext DC_USER
        {
            get
            {
                magaUser_DataContext dc = new magaUser_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaCommon_DataContext DC_COMMON
        {
            get
            {
                magaCommon_DataContext dc = new magaCommon_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaLocation_DataContext DC_LOCATION
        {
            get
            {
                magaLocation_DataContext dc = new magaLocation_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaLimo_DataContext DC_LIMO
        {
            get
            {
                magaLimo_DataContext dc = new magaLimo_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaMail_DataContext DC_MAIL
        {
            get
            {
                magaMail_DataContext dc = new magaMail_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaChnlExpediaDataContext DC_EXP
        {
            get
            {
                magaChnlExpediaDataContext dc = new magaChnlExpediaDataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaAuth_DataContext DC_Auth
        {
            get
            {
                magaAuth_DataContext dc = new magaAuth_DataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaChnlHomeAwayDataContext DC_HOME
        {
            get
            {
                magaChnlHomeAwayDataContext dc = new magaChnlHomeAwayDataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        public static magaAppServerCommonDataContext DC_APP_COMMON
        {
            get
            {
                magaAppServerCommonDataContext dc = new magaAppServerCommonDataContext();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                return dc;
            }
        }
        
        
        
    }
}
