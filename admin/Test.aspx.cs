using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace RentalInRome.admin
{
	public partial class test : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            string xml = "<reservations><reservation><guest_counts><guest_count count=\"9\" type=\"adult\" /><guest_count age=\"5\" count=\"1\"  type=\"child\" /></guest_counts></reservation></reservations>";
            try
            {
                int num_adult = 0;
                int num_child = 0;

                XDocument _resource = XDocument.Parse(xml);
                var reservationList = _resource.Descendants("reservations").Elements("reservation");
                if (reservationList == null)
                {
                    //ErrorLog.addLog("", "BcomImport_process", "Empty reservationList");
                    return;
                }
                List<int> lstResChangeEstateId = new List<int>();
                foreach (var reservation in reservationList)
                {
                    var guestCountList = reservation.Descendants("guest_counts").Elements("guest_count");
                    foreach (var guest_counts in guestCountList)
                    {
                        string type = guest_counts.Attribute("type") + "";
                        if (type == "adult")
                        {
                            num_adult = num_adult + guest_counts.Attribute("count").objToInt32();
                        }
                        else if (type == "child")
                        {
                            num_child = num_child + guest_counts.Attribute("count").objToInt32();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.addLog("", "bcom import guest count", ex.ToString());
            }
		}
	}
}