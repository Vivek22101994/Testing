using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateChnlAtraveo_main : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";           
        }
        public class MsgClass
        {
            public string Msg { get; set; }
            public string Type { get; set; }
            public MsgClass(string msg, string type)
            {
                Msg = msg;
                Type = type;
            }
        }
        private static string LastMsgSessionKey = "EstateChnlAtraveo_unit_LastMsg";
        protected MsgClass LastMsg
        {
            get
            {
                if (HttpContext.Current.Session[LastMsgSessionKey] != null)
                {
                    return (MsgClass)HttpContext.Current.Session[LastMsgSessionKey];
                }
                return new MsgClass("", "");
            }
            set
            {
                HttpContext.Current.Session[LastMsgSessionKey] = value;
            }
        }
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }
        private List<dbRntChnlAtraveoEstateLN> CURRENT_LANG_;
        private List<dbRntChnlAtraveoEstateLN> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(dbRntChnlAtraveoEstateLN)).Cast<dbRntChnlAtraveoEstateLN>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<dbRntChnlAtraveoEstateLN>();

                return CURRENT_LANG_;
            }
            set
            {
                ViewState["CURRENT_LANG"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_LANG_ = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }

                if (Request.QueryString["updatefrommagarental"] == "true")
                {
                    ChnlAtraveoUtils.updateEstateFromMagarental(currEstate);
                    LastMsg = new MsgClass("<b>Successfully updated</b>", "mainline alert-ok");
                    Response.Redirect("EstateChnlAtraveo_main.aspx?id=" + IdEstate);
                    return;
                }
                ltr_apartment.Text = currEstate.code + " / " + "rif. " + IdEstate;
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                fillDrps();
                FillControls();
                if (LastMsg.Msg != "")
                {
                    pnlError.Visible = true;
                    pnlError.Attributes["class"] = LastMsg.Type;
                    ltrErorr.Text = LastMsg.Msg;
                    LastMsg = new MsgClass("", "");
                }
            }
        }
        private void drp_pidMasterEstate_DataBind()
        {
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            {
                List<int> estateIds = dcChnl.dbRntChnlAtraveoEstateTBs.Where(x => x.isActive == 1 && x.pidMasterEstate == 0 && x.id != IdEstate).Select(x => x.id).ToList();
                var list = AppSettings.RNT_TB_ESTATE.Where(x => estateIds.Contains(x.id)).ToList();
                drp_pidMasterEstate.Items.Clear();
                foreach (var t in list)
                {
                    drp_pidMasterEstate.Items.Add(new ListItem(t.code, "" + t.id));
                }
                drp_pidMasterEstate.Items.Insert(0, new ListItem("- no master -", "0"));
            }
        } 
        protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
            lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current lang_" + lbl_id.Text : "tab_item lang_" + lbl_id.Text;
        }

        protected void LV_langs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                Save_lang();
                HF_lang.Value = lbl_id.Text;
                Fill_lang(HF_lang.Value);
                LV_langs.DataBind();
            }
        }
        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pidLang == HF_lang.Value);
            if (rlLang == null)
            {
                rlLang = new dbRntChnlAtraveoEstateLN();
                rlLang.pidEstate = IdEstate;
                rlLang.pidLang = HF_lang.Value;
                CURRENT_LANG.Add(rlLang);

            }
            rlLang.DescriptionFacilities = reDescriptionFacilities.Content;
            rlLang.Description = reDescription.Content;
            rlLang.AddInfos = reAddInfos.Content;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang(string pidLang)
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pidLang == pidLang);
            if (rlLang == null)
            {
                rlLang = new dbRntChnlAtraveoEstateLN();
            }

            reDescriptionFacilities.Content = rlLang.DescriptionFacilities;
            reDescription.Content = rlLang.Description;
            reAddInfos.Content = rlLang.AddInfos;

        }

        protected void Save_RL_langs()
        {
            Save_lang();
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            {
                var curr_rl_langs = dcChnl.dbRntChnlAtraveoEstateLNs.Where(x => x.pidEstate == IdEstate).ToList();
                foreach (var rl in CURRENT_LANG)
                {
                    if (!curr_rl_langs.Exists(x => x.pidLang == rl.pidLang))
                    {
                        rl.pidEstate = IdEstate;
                        dcChnl.Add(rl);
                    }
                    else
                    {
                        var curr_rl = curr_rl_langs.Single(x => x.pidLang == rl.pidLang);
                        curr_rl.DescriptionFacilities = rl.DescriptionFacilities;
                        curr_rl.Description = rl.Description;
                        curr_rl.AddInfos = rl.AddInfos;

                    }

                }
                dcChnl.SaveChanges(); 
            }
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
        protected void fillDrps()
        {

            chkList_ArrivalDays.Items.Clear();
            chkList_ArrivalDays.Items.Add(new ListItem("Sun", "0"));
            chkList_ArrivalDays.Items.Add(new ListItem("Mon", "1"));
            chkList_ArrivalDays.Items.Add(new ListItem("Tue", "2"));
            chkList_ArrivalDays.Items.Add(new ListItem("Wed", "3"));
            chkList_ArrivalDays.Items.Add(new ListItem("Thu", "4"));
            chkList_ArrivalDays.Items.Add(new ListItem("Fri", "5"));
            chkList_ArrivalDays.Items.Add(new ListItem("Sat", "6"));
        }
        protected void FillControls()
        {
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            {
                var currTbl = dcChnl.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                drp_isActive.setSelectedValue(currTbl.isActive);
                drpType.setSelectedValue(currTbl.Type);
                drp_pidMasterEstate_DataBind();
                drp_pidMasterEstate.setSelectedValue(currTbl.pidMasterEstate);
                txt_Name.Text = currTbl.Name;
                List<string> ArrivalDays = new List<string>();
                currTbl.ArrivalDays = (""+currTbl.ArrivalDays).fillString("N", 7, true);
                for (var i = 0; i < currTbl.ArrivalDays.Length; i++)
                    if (currTbl.ArrivalDays[i] == 'Y')
                        ArrivalDays.Add(i + "");
                chkList_ArrivalDays.setSelectedValues(ArrivalDays);
                ntxtMinStay.Value = currTbl.MinStay;
                CURRENT_LANG = dcChnl.dbRntChnlAtraveoEstateLNs.Where(x => x.pidEstate == currTbl.id).ToList();
                HF_lang.Value = "it";
                Fill_lang(HF_lang.Value);
                LV_langs.DataBind();


            }
        }
        protected void FillDataFromControls()
        {
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            {

                var currTbl = dcChnl.dbRntChnlAtraveoEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }

                currTbl.isActive = drp_isActive.getSelectedValueInt();
                currTbl.Type = drpType.getSelectedValueInt();
                currTbl.pidMasterEstate = drp_pidMasterEstate.getSelectedValueInt();
                currTbl.Name = txt_Name.Text;
                currTbl.ArrivalDays = "";
                foreach (ListItem item in chkList_ArrivalDays.Items)
                    currTbl.ArrivalDays += (item.Selected ? "Y" : "N");
                currTbl.MinStay = ntxtMinStay.Value.objToInt32();

                dcChnl.SaveChanges();

                Save_RL_langs();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate correttamente.\", 340, 110);", true);
                FillControls();
            }
        }
                  
      
    }
}