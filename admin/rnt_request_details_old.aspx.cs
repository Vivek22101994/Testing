using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_request_details_old : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_request";
            UC_rnt_request_operator1.onChange += new EventHandler(UC_rnt_request_operator_onChange);
        }
        private magaRental_DataContext DC_RENTAL;
        protected string listPage = "rnt_request_list.aspx";
        private RNT_TBL_REQUEST _currTBL;
        public int IdRequest
        {
            get
            {
                int _id;
                if (int.TryParse(HF_id.Value, out _id))
                    return _id;
                return 0;
            }
            set
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                HF_id.Value = value.ToString();
                FillControls();
            }
        }

        void UC_rnt_request_operator_onChange(object sender, EventArgs e)
        {
            UC_rnt_rl_request_state1.IdRequest = IdRequest;
            UpdatePanel_UC_rnt_rl_request_state.Update();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == _id);
                    if (_currTBL == null)
                        Response.Redirect(listPage);
                    IdRequest = _id;
                    FillControls();
                }
                else
                    Response.Redirect(listPage);
            }
        }
        protected void Bind_drp_relatedRequests()
        {
            drp_relatedRequests.Items.Clear();
            drp_relatedRequests.Items.Add(new ListItem("-seleziona-", "-1"));
            List<RNT_TBL_REQUEST> _list = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.email == _currTBL.email && x.id != _currTBL.id && x.pid_related_request == 0).OrderByDescending(x => x.request_date_created).ToList();
            foreach (RNT_TBL_REQUEST _relRequest in _list)
            {
                drp_relatedRequests.Items.Add(new ListItem("rif. " + _relRequest.id + " - " + _relRequest.name_full + " - del " + _relRequest.request_date_created, "" + _relRequest.id));
            }
        }

        private void FillControls()
        {
            _currTBL = new RNT_TBL_REQUEST();
            if (IdRequest != 0)
            {
                _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == IdRequest);
            }
            if (_currTBL == null)
                Response.Redirect(listPage);
            pnl_setRelatedRequest.Visible = false;
            Bind_drp_relatedRequests();
            if (_currTBL.pid_related_request == 0)
            {
                HL_related_request.Visible = false;
                LV_relatedRequests.Visible = true;
                List<RNT_TBL_REQUEST> _relatedRequestList = DC_RENTAL.RNT_TBL_REQUEST.Where(x => x.pid_related_request == _currTBL.id).ToList();
                LV_relatedRequests.DataSource = _relatedRequestList;
                LV_relatedRequests.DataBind();
                pnl_setRelatedRequest.Visible = _relatedRequestList.Count == 0;

            }
            else
            {
                HL_related_request.Visible = true;
                LV_relatedRequests.Visible = false;
                RNT_TBL_REQUEST _relRequest = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(x => x.id == _currTBL.pid_related_request);
                if (_relRequest != null)
                {
                    HL_related_request.Text = "Correlata alla Richiesta Primaria rif. " + _relRequest.id + " - " + _relRequest.name_full + " - del " + _relRequest.request_date_created;
                    HL_related_request.Enabled = true;
                    HL_related_request.NavigateUrl = "rnt_request_details.aspx?id=" + _relRequest.id;
                }
                else
                {
                    HL_related_request.Text = "La richiesta correlata Non trovata!";
                    HL_related_request.Enabled = false;
                }
                //UC_rnt_request_operator1.Enabled = false;
            }

            ltr_date_request.Text = _currTBL.request_date_created.ToString();
            txt_name.Text = _currTBL.name_full;
            txt_email.Text = _currTBL.email;
            txt_phone.Text = _currTBL.phone;
            txt_country.Text = _currTBL.request_country;
            txt_choice_1.Text = _currTBL.request_choice_1;
            txt_choice_2.Text = _currTBL.request_choice_2;
            ltr_area.Text = _currTBL.request_area;
            ltr_price_range.Text = _currTBL.request_price_range;
            HF_date_start.Value = _currTBL.request_date_start.Value.JSCal_dateToString();
            HF_date_end.Value = _currTBL.request_date_end.Value.JSCal_dateToString();
            ltr_date_is_flexible.Text = _currTBL.request_date_is_flexible == 1 ? "SI" : "NO";
            txt_adult_num.Text = _currTBL.request_adult_num.ToString();
            txt_child_num.Text = _currTBL.request_child_num.ToString();
            txt_transport.Text = _currTBL.request_transport;
            ltr_services.Text = _currTBL.request_services;
            ltr_choices.Text = _currTBL.request_choices;
            ltr_notes.Text = _currTBL.request_notes;
            UC_rnt_rl_request_state1.IdRequest = _currTBL.id;
            UC_rnt_request_operator1.IdRequest = _currTBL.id;

            DisableControls();
        }
        private void FillDataFromControls()
        {
            _currTBL = new RNT_TBL_REQUEST();
            if (HF_id.Value != "0")
            {
                _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == IdRequest);
            }
            if (_currTBL == null)
                Response.Redirect(listPage);
            DC_RENTAL.SubmitChanges();
        }
        protected void lnk_setRelatedRequest_Click(object sender, EventArgs e)
        {
            if (IdRequest != 0)
            {
                _currTBL = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == IdRequest);
            }
            if (_currTBL == null)
            {
                Response.Redirect(listPage);
                return;
            }
            int _relatedRequestId = drp_relatedRequests.getSelectedValueInt(0).objToInt32();
            RNT_TBL_REQUEST _relatedRequest = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == _relatedRequestId);
            if (_relatedRequest == null)
            {
                lbl_relatedRequestError.Visible = true;
                lbl_relatedRequestError.InnerHtml = "Selezionare una Richiesta principale";
                return;
            }
            lbl_relatedRequestError.Visible = false;
            _currTBL.pid_related_request = _relatedRequestId;
            DC_RENTAL.SubmitChanges();
            rntUtils.rntRequest_addState(_currTBL.id, 0, UserAuthentication.CurrentUserID, "Correlazione alla richiesta Primaria rif. " + _relatedRequestId, "");
            rntUtils.rntRequest_addState(_relatedRequestId, 0, UserAuthentication.CurrentUserID, "Aggiunta Correlazione alla richiesta Secondaria rif. " + _currTBL.id, "");
            rntUtils.rntRequest_updateOperator(_relatedRequestId, _relatedRequest.pid_operator.objToInt32(), true, true, UserAuthentication.CurrentUserID);
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            FillControls();
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            DisableControls();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_modify_Click(object sender, EventArgs e)
        {
            EnableControls();
        }
        protected void DisableControls()
        {
            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            //lnk_modify.Visible = true;
        }
        protected void EnableControls()
        {
            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
        }

        protected void DeleteRecord(object sender, EventArgs args)
        {
            int id = int.Parse(((System.Web.UI.WebControls.ImageButton)(sender)).CommandArgument);
            var tbl = DC_RENTAL.RNT_TBL_REQUEST.SingleOrDefault(item => item.id == id);
            if (tbl != null)
            {
                var rl =
                        DC_RENTAL.RNT_RL_REQUEST_ITEMs.Where(
                            item => item.pid_request == tbl.id);
                DC_RENTAL.RNT_RL_REQUEST_ITEMs.DeleteAllOnSubmit(rl);
                DC_RENTAL.RNT_TBL_REQUEST.DeleteOnSubmit(tbl);
                DC_RENTAL.SubmitChanges();
            }
        }

    }
}
