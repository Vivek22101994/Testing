using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModAppServerCommon
{
    public class BlockedIpTool
    {
        private static List<dbUtlsBlockedIpLST> tmpCurrList;
        public static List<dbUtlsBlockedIpLST> CurrList
        {
            get
            {
                if (tmpCurrList == null)
                    using (DCmodAppServerCommon dc = new DCmodAppServerCommon())
                        tmpCurrList = dc.dbUtlsBlockedIpLSTs.ToList();
                return tmpCurrList;
            }
            set
            {
                tmpCurrList = value;
            }
        }
    }
    public class mailProps
    {
    }
    public class utils
    {
        public static dbSmtpConfigLST getSmtpConfig()
        {
            using (DCmodAppServerCommon dc = new DCmodAppServerCommon())
                return dc.dbSmtpConfigLSTs.Where(x => x.isActive).OrderBy(x => x.sequence).FirstOrDefault();
        }
        public static RntChnlHomeAwayManagerTBL getChnlHomeAwayManager(string id)
        {
            using (magaAppServerCommonDataContext dc = maga_DataContext.DC_APP_COMMON)
                return dc.RntChnlHomeAwayManagerTBL.SingleOrDefault(x => x.id == id);
        }
    }
}