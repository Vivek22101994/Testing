using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.areariservataprop
{
    public partial class rnt_estate_price : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                //var periods = DC_RENTAL.RNT_VIEW_PERIODs.Where(x => x.pid_lang == 1).ToList();
                //drp_period.DataSource = periods;
                //drp_period.DataBind();
                //drp_period.Items.Insert(0, new ListItem("-seleziona-", "-1"));
                int id = Request.QueryString["id"].ToInt32();
                RNT_VIEW_ESTATE _est = DC_RENTAL.RNT_VIEW_ESTATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);

                if (_est!=null)
                {
                    IdEstate = _est.id;
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                    FillControls();
                }
                else
                {
                    Response.Redirect("rnt_estate_list.aspx");
                }
            }
        }

        public int IdPrice
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
                FillControls();
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
                LV.DataBind();
            }
        }


        protected void FillControls()
        {
            lbl_changeSaved.Visible = false;
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;

            ntxt_price_1.Value = _currTBL.pr_1_2pax.objToDouble();
            ntxt_price_optional_1.Value = _currTBL.pr_1_opt.objToDouble();
            ntxt_price_2.Value = _currTBL.pr_2_2pax.objToDouble();
            ntxt_price_optional_2.Value = _currTBL.pr_2_opt.objToDouble();
            ntxt_price_3.Value = _currTBL.pr_3_2pax.objToDouble();
            ntxt_price_optional_3.Value = _currTBL.pr_3_opt.objToDouble();

            txt_pr_discount7days.Text = _currTBL.pr_discount7days.objToInt32().ToString();
            txt_pr_discount30days.Text = _currTBL.pr_discount30days.objToInt32().ToString();

            txt_lm_inhours.Text = _currTBL.lm_inhours.objToInt32().ToString();
            txt_lm_discount.Text = _currTBL.lm_discount.objToInt32().ToString();
            txt_lm_nights_min.Text = _currTBL.lm_nights_min.objToInt32().ToString();
            txt_lm_nights_max.Text = _currTBL.lm_nights_max.objToInt32().ToString();
            return;
            // vecchia
            //_currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_period == 1);
            //if (_currTBL == null)
            //{
            //    txt_price_1.Text = "0";
            //    txt_price_optional_1.Text = "0";
            //}
            //else
            //{
            //    txt_price_1.Text = _currTBL.price.objToInt32().ToString();
            //    txt_price_optional_1.Text = _currTBL.price_optional.objToInt32().ToString();
            //}

            //_currTBL = DC_RENTAL.RNT_TBL_ESTATE_PRICEs.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_period == 2);
            //if (_currTBL == null)
            //{
            //    txt_price_2.Text = "0";
            //    txt_price_optional_2.Text = "0";
            //}
            //else
            //{
            //    txt_price_2.Text = _currTBL.price.objToInt32().ToString();
            //    txt_price_optional_2.Text = _currTBL.price_optional.objToInt32().ToString();
            //}


            //_currTBL = DC_RENTAL.RNT_TBL_ESTATE_PRICEs.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_period == 3);
            //if (_currTBL == null)
            //{
            //    txt_price_3.Text = "0";
            //    txt_price_optional_3.Text = "0";
            //}
            //else
            //{
            //    txt_price_3.Text = _currTBL.price.objToInt32().ToString();
            //    txt_price_optional_3.Text = _currTBL.price_optional.objToInt32().ToString();
            //}

            //pnlList.Visible = false;
            //pnlContent.Visible = true;
            
            // gestione vecchia
            //return;
            //_currTBL = DC_RENTAL.RNT_TBL_ESTATE_PRICEs.SingleOrDefault(item => item.id == IdPrice);
            //if (_currTBL==null)
            //{
            //    _currTBL = new RNT_TBL_ESTATE_PRICE();
            //}
            //drp_period.SelectedIndex = CommonUtilities.GetIndexOfDrpValue(ref drp_period, _currTBL.pid_period.ToString());
            //txt_price.Value = _currTBL.price.ToString();
            //txt_price_optional.Value = _currTBL.price_optional.ToString();
            //DateTime _dtStart = _currTBL.date_start.HasValue ? _currTBL.date_start.Value : DateTime.Now;
            //DateTime _dtEnd =  _currTBL.date_end.HasValue ? _currTBL.date_end.Value : _dtStart.AddDays(7);
            //HF_dtStart.Value = _dtStart.JSCal_dateToString();
            //HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
            //List<RNT_TBL_ESTATE_PRICE> _list = DC_RENTAL.RNT_TBL_ESTATE_PRICEs.Where(x => x.id != _currTBL.id && x.pid_estate == IdEstate).OrderBy(x=>x.date_start).ToList();
            //bool _isSet = false;
            //bool _hasHole = false;
            //DateTime _dtLast = _dtStart;
            //string _script = "";
            //_script += "function CheckDisabledDate(date){dateint = JSCal.dateToInt(date);";
            //foreach (RNT_TBL_ESTATE_PRICE _pp in _list)
            //{
            //    if (_isSet)
            //    {
            //        if (_dtEnd.AddDays(1) != _pp.date_start.Value)
            //        {
            //            _dtStart = _dtEnd.AddDays(1);
            //            _dtEnd = _pp.date_start.Value.AddDays(-1);
            //            _hasHole = true;
            //        }
            //    }
            //    if (!_hasHole)
            //    {
            //        _dtStart = _pp.date_start.Value;
            //        _dtEnd = _pp.date_end.Value;
            //    }
            //    _isSet = true;
            //    _dtLast = _pp.date_end.Value;
            //    string _intDateFrom = "" + _pp.date_start.Value.JSCal_dateToInt();
            //    string _intDateTo = "" + _pp.date_end.Value.JSCal_dateToInt();
            //    _script += "if(dateint <= " + _intDateTo + " && dateint >= " + _intDateFrom + ") return [false, \"\"];;";
            //}
            //_script += "return [true, \"\"];";
            //_script += "}";
            //if (_hasHole && IdPrice == 0)
            //{
            //    HF_dtStart.Value = _dtStart.JSCal_dateToString();
            //    HF_dtEnd.Value = _dtEnd.JSCal_dateToString();
            //}
            //else if (IdPrice == 0)
            //{
            //    HF_dtStart.Value = _dtLast.AddDays(1).JSCal_dateToString();
            //    HF_dtEnd.Value = _dtLast.AddDays(8).JSCal_dateToString();
            //}
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "CheckDisabledDate", _script, true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "setCal", "setCal(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);
            //pnlContent.Visible = true;
        }

        protected void FillDataFromControls()
        {
            lbl_changeSaved.Visible = false;
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (_currTBL == null) return;
            _currTBL.pr_1_2pax = ntxt_price_1.Value.objToDecimal();
            _currTBL.pr_1_opt = ntxt_price_optional_1.Value.objToDecimal();
            _currTBL.pr_2_2pax = ntxt_price_2.Value.objToDecimal();
            _currTBL.pr_2_opt = ntxt_price_optional_2.Value.objToDecimal();
            _currTBL.pr_3_2pax = ntxt_price_3.Value.objToDecimal();
            _currTBL.pr_3_opt = ntxt_price_optional_3.Value.objToDecimal();

            _currTBL.pr_discount7days = txt_pr_discount7days.Text.ToDecimal();
            _currTBL.pr_discount30days = txt_pr_discount30days.Text.ToDecimal();

            _currTBL.lm_inhours = txt_lm_inhours.Text.ToInt32();
            _currTBL.lm_discount = txt_lm_discount.Text.ToInt32();
            _currTBL.lm_nights_min = txt_lm_nights_min.Text.ToInt32();
            _currTBL.lm_nights_max = txt_lm_nights_max.Text.ToInt32();

            DC_RENTAL.SubmitChanges();
            AppSettings._refreshCache_RNT_ESTATEs();
            AppSettings.RELOAD_SESSION();
            lbl_changeSaved.Visible = true;
            return;
            // vecchia
            //_currTBL = DC_RENTAL.RNT_TBL_ESTATE_PRICEs.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_period == 1);
            //if (_currTBL == null)
            //{
            //    _currTBL = new RNT_TBL_ESTATE_PRICE();
            //    DC_RENTAL.RNT_TBL_ESTATE_PRICEs.InsertOnSubmit(_currTBL);
            //    _currTBL.pid_estate = IdEstate;
            //    _currTBL.is_active = 1;
            //    _currTBL.priority = 1;
            //    _currTBL.pid_period = 1;
            //}
            //_currTBL.price = txt_price_1.Text.ToDecimal();
            //_currTBL.price_optional = txt_price_optional_1.Text.ToDecimal();
            //DC_RENTAL.SubmitChanges();

            //_currTBL = DC_RENTAL.RNT_TBL_ESTATE_PRICEs.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_period == 2);
            //if (_currTBL == null)
            //{
            //    _currTBL = new RNT_TBL_ESTATE_PRICE();
            //    DC_RENTAL.RNT_TBL_ESTATE_PRICEs.InsertOnSubmit(_currTBL);
            //    _currTBL.pid_estate = IdEstate;
            //    _currTBL.priority = 2;
            //    _currTBL.pid_period = 2;
            //    _currTBL.is_active = 1;
            //}
            //_currTBL.price = txt_price_2.Text.ToDecimal();
            //_currTBL.price_optional = txt_price_optional_2.Text.ToDecimal();
            //DC_RENTAL.SubmitChanges();


            //_currTBL = DC_RENTAL.RNT_TBL_ESTATE_PRICEs.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_period == 3);
            //if (_currTBL == null)
            //{
            //    _currTBL = new RNT_TBL_ESTATE_PRICE();
            //    DC_RENTAL.RNT_TBL_ESTATE_PRICEs.InsertOnSubmit(_currTBL);
            //    _currTBL.pid_estate = IdEstate;
            //    _currTBL.priority = 3;
            //    _currTBL.pid_period = 3;
            //    _currTBL.is_active = 1;
            //}
            //_currTBL.price = txt_price_3.Text.ToDecimal();
            //_currTBL.price_optional = txt_price_optional_3.Text.ToDecimal();
            //DC_RENTAL.SubmitChanges();
            //AppSettings.RNT_estatePriceList = null;
            //AppSettings.RNT_estateDatePriceList = null;
            //AppSettings.RELOAD_SESSION();

            //pnlList.Visible = true;
            //pnlContent.Visible = false;
            //LV.SelectedIndex = -1;
            //LV.DataBind();
            //// gestione vecchia
            //return;
            //_currTBL = DC_RENTAL.RNT_TBL_ESTATE_PRICEs.SingleOrDefault(item => item.id == IdPrice);
            //if (_currTBL == null)
            //{
            //    _currTBL = new RNT_TBL_ESTATE_PRICE();
            //    DC_RENTAL.RNT_TBL_ESTATE_PRICEs.InsertOnSubmit(_currTBL);
            //    _currTBL.pid_estate = IdEstate;
            //    _currTBL.is_active = 1;
            //    _currTBL.priority = 1;
            //}
            //_currTBL.pid_period = int.Parse(drp_period.SelectedValue);
            //_currTBL.date_start = HF_dtStart.Value.JSCal_stringToDate();
            //_currTBL.date_end = HF_dtEnd.Value.JSCal_stringToDate();
            //_currTBL.price = txt_price.Value.ToDecimal();
            //_currTBL.price_optional = txt_price_optional.Value.ToDecimal();
            //DC_RENTAL.SubmitChanges();
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
            //LV.SelectedIndex = -1;
            //LV.DataBind();
            //pnlContent.Visible = false;
        }
        protected void lnk_change_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_new_Click(object sender, EventArgs e)
        {
            IdPrice = 0;
        }
        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            var lbl_id = LV.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            IdPrice = lbl_id.Text.ToInt32();
        }

        protected void LV_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }

        protected void DeleteRecord(object sender, EventArgs args)
        {
        }
    }
}