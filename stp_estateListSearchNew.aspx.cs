using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;

namespace RentalInRome
{
    public partial class stp_estateListSearchNew : contStpBasePage
    {
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["force"] == "desktop")
                //    Session["force_desktop"] = "true";
                //if (Request.IsMobile() && Session["force_desktop"] != "true" && (CommonUtilities.getSYS_SETTING("is_mobile_version") == "true" || CommonUtilities.getSYS_SETTING("is_mobile_version").objToInt32() == 1))
                //{
                //    Response.Redirect("/m" + CurrentSource.getPagePath(PAGE_REF_ID + "", "stp", CurrentLang.ID.ToString()));
                //    return;
                //}

                checkReservationsCal();

                // carica ultima ricerca oppure nuova predefinita
                clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
                clSearch _ls = _config.lastSearch;

                // imposta le date
                HF_dtStart.Value = HF_dtStartTmp.Value = _ls.dtStart.JSCal_dateToString();
                HF_dtEnd.Value = HF_dtEndTmp.Value = _ls.dtEnd.JSCal_dateToString();

                // imposta le persone
                int _maxAdult = AppSettings.RNT_TB_ESTATE.Where(x => x.num_persons_max.HasValue).Max(x => x.num_persons_max.Value);
                int _maxChildMin = AppSettings.RNT_TB_ESTATE.Where(x => x.num_persons_child.HasValue).Max(x => x.num_persons_child.Value);
                for (int i = 1; i <= _maxAdult; i++)
                {
                    drp_numPers_adult.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("reqAdults"), "" + i));
                }
                drp_numPers_adult.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("reqAdults"), "0"));
                drp_numPers_adult.setSelectedValue(_ls.numPers_adult.ToString());


                for (int i = 1; i <= (_maxAdult - 2); i++)
                {
                    drp_numPers_children.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("lblChildren3OrOver"), "" + i));
                }
                drp_numPers_children.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblChildren3OrOver"), "0"));
                drp_numPers_children.setSelectedValue(_ls.numPers_childOver.ToString());


                for (int i = 1; i <= _maxChildMin; i++)
                {
                    drp_numPers_infants.Items.Add(new ListItem(i + " " + CurrentSource.getSysLangValue("lblChildrenUnder3"), "" + i));
                }
                drp_numPers_infants.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("lblChildrenUnder3"), "0"));
                drp_numPers_infants.setSelectedValue(_ls.numPers_childMin.ToString());

                HF_numPers_adult.Value = Convert.ToString(_ls.numPers_adult);
                HF_numPers_childOver.Value = Convert.ToString(_ls.numPers_childOver);
                HF_numPers_childMin.Value = Convert.ToString(_ls.numPers_childMin);
                // HF_numSleeps.Value = Convert.ToString(_ls.numPersCount);

                HF_currZoneList.Value = _ls.currZoneList.listToString("|");
                HF_currExtrasList.Value = _ls.currConfigList.listToString("|");
                HF_currPriceRange.Value = _ls.prMin + "|" + _ls.prMax;
                HF_currVoteRange.Value = _ls.voteMinNew + "|" + _ls.voteMaxNew;

                //// slider
                //HF_prRangeTmp.Value = _ls.prMin + "|" + _ls.prMax;
                //HF_voteRangeTmp.Value = _ls.voteMin + "|" + _ls.voteMax;

                HF_currPage.Value = _ls.currPage.ToString();
                // OrderByHow
                HF_orderBy.Value = _ls.orderBy;
                HF_orderHow.Value = _ls.orderHow;

                txtTitle.Attributes.Add("placeholder", contUtils.getLabel("lblApartmentName"));
                txtTitle.Text = _ls.searchTitle;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal();", true);
        }

        protected void checkReservationsCal()
        {
            string _script = "";
            _script += "function checkCalDates_" + Unique + "(date){var _dtStart = parseInt($('#" + HF_dtStart.ClientID + "').val()); var _dtEnd = parseInt($('#" + HF_dtEnd.ClientID + "').val()); var dateint = JSCal.dateToInt(date);var _class = \"\";var _tooltip = \"\"; var _controls = \"\";var _enabled = true;";
            _script += "if (dateint > _dtStart && dateint < _dtEnd) { _controls += '<span class=\"rntCal sel_f\"></span>'; }";
            _script += "if (dateint == _dtStart) { _controls += '<span class=\"rntCal sel_1\"></span>'; }";
            _script += "if (dateint == _dtEnd) { _controls += '<span class=\"rntCal sel_2\"></span>'; }";
            _script += "if (_controls.indexOf('<span class=\"rntCal nd_2\"></span>') != -1 && _controls.indexOf('<span class=\"rntCal nd_1\"></span>') != -1) { _enabled = false; }";
            _script += "return [_enabled, _class, _tooltip, _controls];";
            _script += "}";
            ltr_checkCalDates.Text = _script;
        }
    }
}