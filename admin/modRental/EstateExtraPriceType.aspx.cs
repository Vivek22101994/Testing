using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;

namespace RentalInRome.admin.modRental
{
    public partial class EstateExtraPriceType : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "extras";
        }
        private dbRntExtraPriceTypesTB currTBL;
        private List<dbRntExtraPriceTypesLN> TMPcurrLangs;
        private dbRntExtraPriceTypesLN currLN;
        private List<dbRntExtraPriceTypesTB> currTBLList;
        private List<dbRntExtraPriceTypesLN> currTBLListLN;
        private List<dbRntExtraPriceTypesLN> currTBLListLNFinal = new List<dbRntExtraPriceTypesLN>();

        private List<dbRntExtraPriceTypesLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbRntExtraPriceTypesLN)).Cast<dbRntExtraPriceTypesLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbRntExtraPriceTypesLN>();

                return TMPcurrLangs;
            }
            set
            {
                ViewState["currLangs"] = PConv.SerialList(value.Cast<object>().ToList());
                TMPcurrLangs = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                HfId.Value = Request.QueryString["id"].ToInt64().ToString();
                fillPriceType();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
                //fillGroup();

            }
        }
        private void fillPriceType()
        {

            using (DCmodRental dc = new DCmodRental())
            {

                dbRntEstateExtrasLN currExtra = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == HfId.Value.ToInt32() && x.pidLang == 1);
                ltrTitle.Text = "Aggiunta di tipo prezzo #" + currExtra.title;
                fillLV();
            }

            //drp_price_group.SelectedValue = "0";


        }

        private void fillLV()
        {

            using (DCmodRental dc = new DCmodRental())
            {
                currTBLList = dc.dbRntExtraPriceTypesTBs.Where(x => x.pidExtra == HfId.Value.ToInt32() && x.isActive == 1).ToList();
                foreach (dbRntExtraPriceTypesTB objExtraPriceType in currTBLList)
                {
                    currTBLListLN = dc.dbRntExtraPriceTypesLNs.Where(x => x.pidPriceType == objExtraPriceType.id).ToList();
                    currTBLListLNFinal.AddRange(currTBLListLN);
                }
                // currLangs = currTBLListLNFinal;
                currTBLListLNFinal = currTBLListLNFinal.FindAll(x => x.pidLang == 1).ToList();
                if (currTBLListLNFinal != null && currTBLListLNFinal.Count > 0)
                {
                    LVPrice.DataSource = currTBLListLNFinal;
                    LVPrice.DataBind();
                }
                else
                {
                    LVPrice.DataSource = null;
                    LVPrice.DataBind();
                }
            }

        }
        private void SavePrice()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntExtraPriceTypesTBs.SingleOrDefault(x => x.id == hfd_price.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRntExtraPriceTypesTB();
                    dc.Add(currTBL);
                }

                //currPriceTBL.pidCarRent=HfId.Value.ToInt32();
                //currPriceTBL.GroupId = Convert.ToInt32(drp_price_group.SelectedValue);
                currTBL.pidExtra = Convert.ToInt32(HfId.Value);
                currTBL.isActive = 1;
                dc.SaveChanges();
                //currLN = dc.dbRntExtraPriceTypesLNs.SingleOrDefault(x => x.pidPriceType == currTBL.id);
                //if (currLN == null)
                //{
                //    currLN = new dbRntExtraPriceTypesLN();
                //    dc.Add(currLN);
                //}
                //currLN.title = txt_title.Text;
                //currLN.subTitle = txt_SubTitle.Text;
                //currLN.description = txt_Description.Text;
                //currLN.pidLang = 1;
                //currLN.pidPriceType = currTBL.id;
                //dc.SaveChanges();
                HfId.Value = Convert.ToString(currTBL.id);
                SaveAllLangs(currTBL.id);
                fillPriceType();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Tipo di prezzo salvato con successo.\", 340, 110);", true);

            }
        }

        protected void LvPrice_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            int id = Convert.ToInt32(lbl_id.Text);


            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntExtraPriceTypesTBs.SingleOrDefault(x => x.id == id);

                if (e.CommandName == "DeletePrice")
                {
                    LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                    //if (lnkDelete.Text == "attivare")
                    //{
                    //    currTBLDelete.isActive = 0;
                    //}
                    //else if (lnkDelete.Text == "disattivare")
                    //{
                    //    currTBLDelete.isActive = 1;
                    //}
                    currTBL.isActive = 0;
                    dc.SaveChanges();
                    fillPriceType();
                }
                if (e.CommandName == "EditPrice")
                {
                    hfd_price.Value = Convert.ToString(id);
                    HfLang.Value = "1";
                    FillLang();
                    //currLN = dc.dbRntExtraPriceTypesLNs.SingleOrDefault(x => x.pidPriceType == Convert.ToInt32(hfd_price.Value) && x.pidLang == 1);
                    //txt_title.Text = currLN.title;
                    //txt_SubTitle.Text = currLN.subTitle;
                    //txt_description.Content = currLN.description;

                }
            }



        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            SavePrice();

            //CloseRadWindow("reload");
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillPriceType();
        }


        protected void BindLvLangs()
        {
            LvLangs.DataSource = contProps.LangTBL.Where(x => x.is_active == 1).OrderBy(x => x.id);
            LvLangs.DataBind();
        }
        protected void LvLangs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnkLang");
            lnk.CssClass = HfLang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
        }
        protected void LvLangs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                SaveLang();
                HfLang.Value = lbl_id.Text;
                FillLang();
                BindLvLangs();
            }
        }
        protected void SaveLang()
        {
            var currLangsTmp = currLangs;
            var rlLang = currLangsTmp.Where(x => x.pidLang == HfLang.Value.ToInt32()).ToList() != null && currLangsTmp.Where(x => x.pidLang == HfLang.Value.ToInt32()).ToList().Count > 0 ? currLangsTmp.Where(x => x.pidLang == HfLang.Value.ToInt32()).ToList()[0] : null;
            if (rlLang == null)
            {
                rlLang = new dbRntExtraPriceTypesLN();
                rlLang.pidPriceType = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.title = txt_title.Text;
            rlLang.subTitle = txt_SubTitle.Text;
            rlLang.description = txt_description.Content;
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.Where(x => x.pidLang == HfLang.Value.ToInt32()).ToList() != null && currLangs.Where(x => x.pidLang == HfLang.Value.ToInt32()).ToList().Count > 0 ? currLangs.Where(x => x.pidLang == HfLang.Value.ToInt32()).ToList()[0] : null;
            if (rlLang == null)
            {
                rlLang = new dbRntExtraPriceTypesLN();
            }
            txt_title.Text = rlLang.title;
            txt_SubTitle.Text = rlLang.subTitle;
            txt_description.Content = rlLang.description;
        }
        protected void SaveAllLangs(int id)
        {
            SaveLang();
            using (DCmodRental dc = new DCmodRental())
            {
                foreach (var rl in currLangs)
                {
                    if (dc.dbRntExtraPriceTypesLNs.SingleOrDefault(x => x.pidPriceType == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidPriceType = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbRntExtraPriceTypesLNs.Single(x => x.pidPriceType == id && x.pidLang == rl.pidLang);
                        //rl.title = currLN.title;
                        //rl.subTitle = currLN.subTitle;
                        //rl.description = currLN.description;
                        rl.CopyToPriceType(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }
    }
}