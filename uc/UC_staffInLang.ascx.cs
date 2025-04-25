using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.uc
{
    public partial class UC_staffInLang : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                List<USR_RL_ADMIN_LANG> _rlList = maga_DataContext.DC_USER.USR_RL_ADMIN_LANG.Where(x => x.pid_lang == CurrentLang.ID).ToList();
                int _count = _rlList.Count;
                if (_count == 0) return;
                int i = RandomNumber(0, _count - 1);
                USR_RL_ADMIN_LANG _rl = _rlList[i];
                USR_ADMIN _admin = maga_DataContext.DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == _rl.pid_admin && x.is_active == 1 && x.is_deleted != 1);
                if (_admin == null) return;
                ltr_email.Text = _admin.email;
                ltr_name_full.Text = _admin.name + " " + _admin.surname;
                ltr_img_thumb.Text = _rl.img_thumb;
                pnlCont.Visible = true;
            }
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public string currPagePath
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                if (m == null)
                    return "";
                string pt = m.PAGE_TYPE;
                int pid = m.PAGE_REF_ID;
                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + CurrentSource.getPagePath(pid.ToString(), pt, CurrentLang.ID.ToString());
            }
        }
    }
}