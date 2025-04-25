using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class TestSetRelatedReq : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
            {
                DateTime dt = new DateTime(2015,05,09);
                List<RNT_TBL_REQUEST> lst = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.IdAdMedia == ChnlHomeAwayProps_V411.IdAdMedia && x.request_date_created >= dt).OrderBy(x => x.request_date_created).ToList();

                foreach (var _request in lst)
                {
                    try
                    {
                        RNT_TBL_REQUEST _relatedRequest = rntRequest_getRelatedRequest(_request);
                        if (_relatedRequest != null)
                        {
                            _request.pid_related_request = _relatedRequest.id;
                            _request.pid_operator = _relatedRequest.pid_operator.Value;
                            rntUtils.rntRequest_addState(_request.id, 0, 1, "Correlazione alla richiesta Primaria rif. " + _relatedRequest.id, "");
                            rntUtils.rntRequest_addState(_relatedRequest.id, 0, 1, "Aggiunta Correlazione alla richiesta Secondaria rif. " + _request.id, "");

                            ErrorLog.addLog("", "Related: " + _request.email, _request.id + " to " + _relatedRequest.id);
                        }
                        else
                        {
                            _request.pid_related_request = 0;
                            ErrorLog.addLog("", "Not Related: " + _request.email, _request.id + "");
                        }
                        DC_RENTAL.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                         ErrorLog.addLog("", "ChnlHomeAway_TestSetRelatedReq:", ex.ToString());
                    }
                }
            }
        }


        public static RNT_TBL_REQUEST rntRequest_getRelatedRequest(RNT_TBL_REQUEST _newRequest)
        {
            RNT_TBL_REQUEST _request = null;
            List<RNT_TBL_REQUEST> _list = new List<RNT_TBL_REQUEST>();
            string _rnt_request_relation_view_fld = CommonUtilities.getSYS_SETTING("rnt_request_relation_view_fld");
            ErrorLog.addLog("", "_rnt_request_relation_view_fld", _rnt_request_relation_view_fld);
            if (_rnt_request_relation_view_fld == "email")
                _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.id < _newRequest.id && (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && x.email == _newRequest.email).ToList();
            else if (_rnt_request_relation_view_fld == "name_full")
                _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.id < _newRequest.id && (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && x.name_full == _newRequest.name_full).ToList();
            else if (_rnt_request_relation_view_fld == "email or name_full")
                _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.id < _newRequest.id && (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && (x.email == _newRequest.email || x.name_full == _newRequest.name_full)).ToList();
            else if (_rnt_request_relation_view_fld == "email and name_full")
                _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.id < _newRequest.id && (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && (x.email == _newRequest.email && x.name_full == _newRequest.name_full)).ToList();
            else
                _list = maga_DataContext.DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.id < _newRequest.id && (x.state_pid < 5) && x.id != _newRequest.id && x.pid_related_request == 0 && (x.pid_city == 0 || _newRequest.pid_city == 0 || x.pid_city == _newRequest.pid_city) && x.email == _newRequest.email).ToList();

            string _rnt_request_relation_in_hours = CommonUtilities.getSYS_SETTING("rnt_request_relation_in_hours");
            DateTime _dtFrom = _newRequest.request_date_created.Value;
            int _hours = _rnt_request_relation_in_hours.ToInt32();
            if (_hours != 0)
                _list = _list.Where(x => x.request_date_created >= _dtFrom.AddHours(-_hours)).ToList();

            string _rnt_request_relation_is_view_date = CommonUtilities.getSYS_SETTING("rnt_request_relation_is_view_date");
            if (_rnt_request_relation_is_view_date == "1")
            {
                if (_newRequest.request_date_start != null && _newRequest.request_date_end != null)
                    _list = _list.Where(x => x.request_date_start == _newRequest.request_date_start && x.request_date_end == _newRequest.request_date_end).ToList();
            }

            if (_list.Count > 0)
                _request = _list[0];
            return _request;
        }
    }
}