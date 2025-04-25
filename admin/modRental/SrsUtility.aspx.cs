using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental
{
    public partial class SrsUtility : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnk_update_Click(object sender, EventArgs e)
        {
            //Action<object> action = (object obj) => { updateSrs(); };
            //AppUtilsTaskScheduler.AddTask(action, "lnk_update_Click");
            ThreadStart start = new ThreadStart(updateSrs);
            Thread t = new Thread(start);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }

        protected void updateSrs()
        {
            List<int?> ids = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_srs == 1).Select(x => (int?)x.id).ToList();
            List<RNT_TBL_RESERVATION> _list = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(x => ids.Contains(x.pid_estate) && x.dtEnd.HasValue && x.dtEnd.Value.Date >= DateTime.Now.AddDays(-30) && x.state_pid == 3).ToList();
            foreach (RNT_TBL_RESERVATION _res in _list)
            {
                Srs_WS.LocationEvent_Delete(_res);
            }
        }
        protected void lnk_updateSrs_Click(object sender, EventArgs e)
        {


        }
    }
}