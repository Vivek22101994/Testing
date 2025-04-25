using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental
{
    public partial class EstateChnlMagaRentalXml_main : adminBasePage
    {
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
        private static string LastMsgSessionKey = "EstateChnlMagaRentalXml_property_LastMsg";
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
        private List<dbRntChnlMagaRentalXmlEstateLN> CURRENT_LANG_;
        private List<dbRntChnlMagaRentalXmlEstateLN> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(dbRntChnlMagaRentalXmlEstateLN)).Cast<dbRntChnlMagaRentalXmlEstateLN>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<dbRntChnlMagaRentalXmlEstateLN>();

                return CURRENT_LANG_;
            }
            set
            {
                ViewState["CURRENT_LANG"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_LANG_ = value;
            }
        }
        protected magaRental_DataContext DC_RENTAL;


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
                    using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
                    {
                        ChnlMagaRentalXmlUtils.updateEstateFromMagarental(currEstate);
                        LastMsg = new MsgClass("<b>Successfully updated</b>", "mainline alert-ok");
                        Response.Redirect("EstateChnlMagaRentalXml_main.aspx?id=" + IdEstate);
                        return;
                    }
                } 
                if (LastMsg.Msg != "")
                {
                    pnlError.Visible = true;
                    pnlError.Attributes["class"] = LastMsg.Type;
                    ltrErorr.Text = LastMsg.Msg;
                    LastMsg = new MsgClass("", "");
                }

                ltr_apartment.Text = currEstate.code + " / " + "rif. " + IdEstate;
                FillControls();
            }
        }
        protected void FillControls()
        {
            using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
            {
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                CURRENT_LANG = dcChnl.dbRntChnlMagaRentalXmlEstateLNs.Where(x => x.pidEstate == IdEstate).ToList();
                HF_lang.Value = App.DefLangID+"";
                Fill_lang(HF_lang.Value.objToInt32());
                LV_langs.DataBind();
            }
        }

        protected void FillDataFromControls()
        {
            using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
            {
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                Save_RL_langs();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate correttamente.\", 340, 110);", true);
                FillControls();
            }
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            //LV.SelectedIndex = -1;
            //LV.DataBind();
            //pnlContent.Visible = false;
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
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
                Fill_lang(HF_lang.Value.objToInt32());
                LV_langs.DataBind();
            }
        }

        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pidLang == HF_lang.Value.objToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRntChnlMagaRentalXmlEstateLN();
                rlLang.pidEstate = IdEstate;
                rlLang.pidLang = HF_lang.Value.objToInt32();
                CURRENT_LANG.Add(rlLang);

            }

            rlLang.title = txt_title.Text;
            rlLang.summary = txt_summary.Text;
            rlLang.description = re_description.Content;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang(int pidLang)
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pidLang == pidLang);
            if (rlLang == null)
            {
                rlLang = new dbRntChnlMagaRentalXmlEstateLN();
            }

            txt_title.Text = rlLang.title;
            txt_summary.Text = rlLang.summary;
            re_description.Content = rlLang.description;

        }

        protected void Save_RL_langs()
        {
            using (DCchnlMagaRentalXml dcChnl = new DCchnlMagaRentalXml())
            {
                Save_lang();
                var curr_rl_langs = dcChnl.dbRntChnlMagaRentalXmlEstateLNs.Where(x => x.pidEstate == IdEstate).ToList();
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
                        curr_rl.description = rl.description;
                        curr_rl.title = rl.title;
                        curr_rl.summary = rl.summary;

                    }
                }
                 dcChnl.SaveChanges(); 
            }
        }
    }
}