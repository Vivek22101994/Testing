using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.webservice
{
    public partial class rnt_requestEstateList : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        private RNT_TBL_REQUEST _currTBL;
        private int IdRequest;
        int currLang;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/html";
                IdRequest = Request.QueryString["id"].ToInt32();
                _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == IdRequest);
                if (_currTBL == null)
                {
                    Response.Write("Errore: ID non trovato");
                    Response.End();
                }
                if (!_currTBL.request_date_start.HasValue || !_currTBL.request_date_end.HasValue)
                {
                    Response.Write("Errore: Richiesta non completa");
                    Response.End();
                }
                currLang = _currTBL.pid_lang.objToInt32();
                if (currLang == 0) currLang = 2;
                List<int> add = Request.QueryString["add"].splitStringToList("|").Select(x => x.ToInt32()).Where(x => x != 0).ToList();
                if (add.Count > 0) 
                {
                    foreach (var tmp in add)
                    {
                        if (DC_RENTAL.RNT_RL_REQUEST_ITEMs.Where(x => x.pid_request == IdRequest && x.pid_estate == tmp).Count() == 0)
                        {
                            RNT_RL_REQUEST_ITEM _item = new RNT_RL_REQUEST_ITEM();
                            _item.pid_estate = tmp;
                            _item.pid_request = IdRequest;
                            _item.sequence = DC_RENTAL.RNT_RL_REQUEST_ITEMs.Where(x => x.pid_request == IdRequest).Count() + 1;
                            DC_RENTAL.RNT_RL_REQUEST_ITEMs.InsertOnSubmit(_item);
                        }
                    }
                    DC_RENTAL.SubmitChanges();
                }
                List<int> remove = Request.QueryString["remove"].splitStringToList("|").Select(x => x.ToInt32()).Where(x => x != 0).ToList();
                if (remove.Count > 0)
                {
                    foreach (var tmp in remove)
                    {
                        var tmpList = DC_RENTAL.RNT_RL_REQUEST_ITEMs.Where(x => x.pid_request == IdRequest && x.pid_estate == tmp);
                        if (tmpList.Count() > 0)
                            DC_RENTAL.RNT_RL_REQUEST_ITEMs.DeleteAllOnSubmit(tmpList);
                    }
                    DC_RENTAL.SubmitChanges();
                }
                Response.Write(fillList());
            }
        }
        protected string fillList()
        {
            int numPersTotal = _currTBL.request_adult_num.objToInt32() + _currTBL.request_child_num.objToInt32();
            List<int> requestItemIds = DC_RENTAL.RNT_RL_REQUEST_ITEMs.Where(x => x.pid_request == IdRequest).Select(x=>x.pid_estate).ToList();
            List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateListAll.Where(x => requestItemIds.Contains(x.id)).Select(x => x.Clone()).ToList();
            List<RNT_TBL_RESERVATION> resListAll = DC_RENTAL.RNT_TBL_RESERVATION.Where(x => x.state_pid != 3 && x.is_deleted != 1 && x.dtStart.HasValue && x.dtEnd.HasValue && (x.dtEnd.Value.Date > _currTBL.request_date_start.Value && x.dtStart.Value.Date < _currTBL.request_date_end.Value)).ToList();
            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            outPrice.dtStart = _currTBL.request_date_start.Value;
            outPrice.dtEnd = _currTBL.request_date_end.Value;
            outPrice.numPersCount = numPersTotal;
            outPrice.pr_discount_owner = 0;
            outPrice.pr_discount_commission = 0;
            foreach (AppSettings.RNT_estate _rntEst in estateList)
            {
                RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == currLang && !string.IsNullOrEmpty(x.title));
                if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 2);
                if (_lang == null) _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == 1);
                if (_lang != null)
                {
                    _rntEst.pid_lang = currLang;
                    _rntEst.title = _lang.title;
                    _rntEst.summary = _lang.summary;
                    _rntEst.page_path = _lang.page_path;
                }
                else
                {
                    continue;
                }
                outPrice.part_percentage = _rntEst.pr_percentage;
                _rntEst.price = rntUtils.rntEstate_getPrice(0, _rntEst.id, ref outPrice);
                _rntEst.zone = CurrentSource.locZone_title(_rntEst.pid_zone.objToInt32(), currLang, "");
            }
            estateList = estateList.OrderBy(x => x.zone).ThenBy(x => x.code).ToList();
            string itemPlaceHolder = "";
            string _ItemTemplate = ltrItemTemplate.Text;
            string estatesOK = "";
            string estatesNO = "";
            foreach (AppSettings.RNT_estate _rntEst in estateList)
            {
                if (_rntEst.page_path == "")
                    continue;
                string _estatePath = CurrentAppSettings.HOST + "/" + _rntEst.page_path;
                if (_rntEst.pid_city == 2) _estatePath = "http://www.rentalinflorence.com/" + _rntEst.page_path;
                if (_rntEst.pid_city == 3) _estatePath = "http://www.rentalinvenice.com/" + _rntEst.page_path;
                string _ItemTemplateSingle = _ItemTemplate.Clone() as string;
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#id#", "" + _rntEst.id);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#title#", "" + _rntEst.title);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#page_path#", "" + _estatePath);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#zoneTitle#", "" + _rntEst.zone);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#price#", (_rntEst.price != 0 ? "&euro; "+_rntEst.price.ToString("N2") : "on request"));
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#pr_percentage#", "" + _rntEst.pr_percentage + "&nbsp;%");
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#is_exclusive#", _rntEst.is_exclusive == 1 ? "SI" : "NO");
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#HL_owner#", "usr_owner_details.aspx?id=" + _rntEst.pid_owner);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("##", "" + _rntEst.id);
                RNT_TBL_RESERVATION _res = resListAll.FirstOrDefault(x => x.pid_estate == _rntEst.id);
                bool _avv = _res == null;
                string _resDetails = "Prenotazione";
                string ltr_avv = "";
                string HL_book = "";
                
                if (_res != null)
                {
                    _resDetails += "...code #" + _res.code;
                    _resDetails += "...stato: " + rntUtils.rntReservation_getStateName(_res.state_pid);
                    if (_res.state_pid == 6)
                    {
                        DateTime _dtExpire = _res.block_expire.Value;
                        _resDetails += "...scadenza: " + _dtExpire.formatCustom("#dd#/#mm#/#yy#", 1, "") + " ore " + _dtExpire.TimeOfDay.JSTime_toString(false, true);
                    }
                }
                if (_avv)
                {
                    if (_rntEst.num_persons_min > numPersTotal)
                        ltr_avv = "<img src=\"images/ico_nodisp.gif\" class=\"apt_foto\" alt=\"NoDisp\" title=\"Min num. persone: " + _rntEst.num_persons_min + "\" />";
                    else if (_rntEst.num_persons_max < numPersTotal)
                        ltr_avv = "<img src=\"images/ico_nodisp.gif\" class=\"apt_foto\" alt=\"NoDisp\" title=\"Max num. persone: " + _rntEst.num_persons_max + "\" />";
                    else
                        ltr_avv = "<img onclick='RNT_openOwnerOpz(\"" + _rntEst.id + "\");' title='clicca per inserire Opz del proprietario' style='cursor:pointer;' src=\"images/ico_disp.gif\" class=\"apt_foto\" alt=\"Disp\" />";
                }
                else
                {
                    ltr_avv = "<img src=\"images/ico_nodisp.gif\" class=\"apt_foto\" alt=\"NoDisp\" title=\"" + _resDetails + "\" />";
                }
                if (_avv && _currTBL.state_pid != 5 && _currTBL.state_pid != 6)
                {
                    HL_book = "<a href=\"#\" onclick=\"RNT_openSelection('" + _rntEst.id + "');return false;\"  class=\"apt\" target=\"_blank\">Prenota</a>";
                }
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#ltr_avv#", "" + ltr_avv);
                _ItemTemplateSingle = _ItemTemplateSingle.Replace("#HL_book#", "" + HL_book);

                
                itemPlaceHolder += _ItemTemplateSingle;
                if (_avv)
                {
                    if (!estatesOK.Contains(_estatePath))
                    {
                        estatesOK += "<tr>";
                        estatesOK += "   <td>";
                        estatesOK += "       " + "<a target='_blank' href='" + _estatePath + "?IdRequest=" + IdRequest + "&dtS=" + outPrice.dtStart.JSCal_dateToString() + "&dtE=" + outPrice.dtEnd.JSCal_dateToString() + "&numPers=" + numPersTotal + "'>" + _rntEst.title + "</a>";
                        estatesOK += "   </td>";
                        estatesOK += "   <td>";
                        estatesOK += "       " + _rntEst.zone;
                        estatesOK += "   </td>";
                        estatesOK += "   <td>";
                        estatesOK += "       " + (_rntEst.price != 0 ? "&euro; " + _rntEst.price.ToString("N2") : "on request");
                        estatesOK += "   </td>";
                        estatesOK += "</tr>";
                    }
                }
                else
                {
                    if (!estatesNO.Contains(_estatePath))
                    {
                        estatesNO += "<tr>";
                        estatesNO += "   <td>";
                        estatesNO += "       " + "<a target='_blank' href='" + _estatePath + "'>" + _rntEst.title + "</a>";
                        estatesNO += "   </td>";
                        estatesNO += "   <td>";
                        estatesNO += "       " + _rntEst.zone;
                        estatesNO += "   </td>";
                        estatesNO += "   <td>";
                        estatesNO += "       " + (_rntEst.price != 0 ? "&euro; " + _rntEst.price.ToString("N2") : "on request");
                        estatesNO += "   </td>";
                        estatesNO += "</tr>";
                    }
                }

            }
            if (itemPlaceHolder == "")
                return ltrEmptyDataTemplate.Text;
            string LayoutTemplate = ltrLayoutTemplate.Text;
            LayoutTemplate = LayoutTemplate.Replace("#itemPlaceHolder#", itemPlaceHolder);
            LayoutTemplate += "<div id='requestEstatesOK' style='display:none;'><table>" + estatesOK + "</table></div>";
            LayoutTemplate += "<div id='requestEstatesNO' style='display:none;'><table>" + estatesNO + "</table></div>";
            return LayoutTemplate;

        }
    }
}
