using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class limo_transportDuration : adminBasePage
    {
        protected class TmpCustom
        {
            public int hour { get; set; }
            public string title { get; set; }
            public TmpCustom(int _hour, string _title)
            {
                hour = _hour;
                title = _title;
            }
        }
        private static List<TmpCustom> CURRENT_HOURLIST_;
        private static List<TmpCustom> CURRENT_HOURLIST
        {
            get
            {
                if (CURRENT_HOURLIST_ == null)
                {
                    List<TmpCustom> _tmp = new List<TmpCustom>();
                    for (int i = 0; i < 24; i++)
                        _tmp.Add(new TmpCustom(i, i + ":00"));
                    CURRENT_HOURLIST_ = _tmp;
                }
                return CURRENT_HOURLIST_;
            }
            set
            {
                CURRENT_HOURLIST_ = value;
            }
        }

        protected magaLimo_DataContext DC_LIMO;
        public int IdZone
        {
            get { return HF_pidZone.Value.ToInt32(); }
            set { HF_pidZone.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_LIMO = maga_DataContext.DC_LIMO;
            if (!IsPostBack)
            {
                IdZone = Request.QueryString["IdZone"].objToInt32();
                LOC_LN_ZONE _zone = maga_DataContext.DC_LOCATION.LOC_LN_ZONEs.SingleOrDefault(x => x.pid_lang == 1 && x.pid_zone == IdZone);
                if (_zone == null) return;
                ltr_zoneTitle.Text = _zone.title;
                fillList();
                Bind_drp_hourFromTo();
                Bind_drp_PickupPlace();
                Bind_chkList_TransportType();
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "durationMinSet", "durationMinSet();", true);
        }
        protected void fillList()
        {
            LV.DataSource = limoProps.PICKUP_PLACE_WITH_TRANSPORTTYPEs;
            LV.DataBind();
        }
        protected void Bind_drp_hourFromTo()
        {
            drp_hourFrom.DataSource = CURRENT_HOURLIST;
            drp_hourFrom.DataTextField = "title";
            drp_hourFrom.DataValueField = "hour";
            drp_hourFrom.DataBind();
            drp_hourTo.DataSource = CURRENT_HOURLIST;
            drp_hourTo.DataTextField = "title";
            drp_hourTo.DataValueField = "hour";
            drp_hourTo.DataBind();
        }
        protected void Bind_drp_PickupPlace()
        {
            drp_PickupPlace.DataSource = limoProps.LIMO_TB_PICKUP_PLACE.Where(x => x.isActive == 1);
            drp_PickupPlace.DataTextField = "title";
            drp_PickupPlace.DataValueField = "id";
            drp_PickupPlace.DataBind();
            drp_PickupPlace.Items.Insert(0, new ListItem("-seleziona-", "0"));
        }
        protected void Bind_chkList_TransportType()
        {
            chkList_TransportType.DataSource = limoProps.LIMO_LK_TRANSPORTTYPE.Where(x => x.isActive == 1);
            chkList_TransportType.DataTextField = "title";
            chkList_TransportType.DataValueField = "code";
            chkList_TransportType.DataBind();
        }
        protected void LV_DataBound(object sender, EventArgs e)
        {
            ListView LV_inner = LV.FindControl("LV_inner") as ListView;
            LV_inner.DataSource = CURRENT_HOURLIST;
            LV_inner.DataBind();
            foreach (ListViewDataItem _itemLV in LV.Items)
            {
                Label lbl_PickupPlace = _itemLV.FindControl("lbl_PickupPlace") as Label;
                Label lbl_TransportType = _itemLV.FindControl("lbl_TransportType") as Label;
                int pidPickupPlace = lbl_PickupPlace.Text.ToInt32();
                string transportType = lbl_TransportType.Text;
                LV_inner = _itemLV.FindControl("LV_inner") as ListView;
                LIMO_RL_TRANSPORT_DURATION _rl = limoProps.LIMO_RL_TRANSPORT_DURATION.SingleOrDefault(x => x.pidZone == IdZone && x.pidPickupPlace == pidPickupPlace && x.transportType == transportType);
                foreach (ListViewDataItem _itemLV_inner in LV_inner.Items)
                {
                    Label lbl_hour = _itemLV_inner.FindControl("lbl_hour") as Label;
                    TextBox txtAt = _itemLV_inner.FindControl("txtAt") as TextBox;
                    if (_rl == null)
                    {
                        txtAt.Text = "0";
                        continue;
                    }
                    switch (lbl_hour.Text)
                    {
                        case "0":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt00.ToString() : _rl.outAt00.ToString();
                            break;
                        case "1":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt01.ToString() : _rl.outAt01.ToString();
                            break;
                        case "2":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt02.ToString() : _rl.outAt02.ToString();
                            break;
                        case "3":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt03.ToString() : _rl.outAt03.ToString();
                            break;
                        case "4":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt04.ToString() : _rl.outAt04.ToString();
                            break;
                        case "5":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt05.ToString() : _rl.outAt05.ToString();
                            break;
                        case "6":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt06.ToString() : _rl.outAt06.ToString();
                            break;
                        case "7":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt07.ToString() : _rl.outAt07.ToString();
                            break;
                        case "8":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt08.ToString() : _rl.outAt08.ToString();
                            break;
                        case "9":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt09.ToString() : _rl.outAt09.ToString();
                            break;
                        case "10":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt10.ToString() : _rl.outAt10.ToString();
                            break;
                        case "11":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt11.ToString() : _rl.outAt11.ToString();
                            break;
                        case "12":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt12.ToString() : _rl.outAt12.ToString();
                            break;
                        case "13":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt13.ToString() : _rl.outAt13.ToString();
                            break;
                        case "14":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt14.ToString() : _rl.outAt14.ToString();
                            break;
                        case "15":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt15.ToString() : _rl.outAt15.ToString();
                            break;
                        case "16":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt16.ToString() : _rl.outAt16.ToString();
                            break;
                        case "17":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt17.ToString() : _rl.outAt17.ToString();
                            break;
                        case "18":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt18.ToString() : _rl.outAt18.ToString();
                            break;
                        case "19":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt19.ToString() : _rl.outAt19.ToString();
                            break;
                        case "20":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt20.ToString() : _rl.outAt20.ToString();
                            break;
                        case "21":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt21.ToString() : _rl.outAt21.ToString();
                            break;
                        case "22":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt22.ToString() : _rl.outAt22.ToString();
                            break;
                        case "23":
                            txtAt.Text = HF_inOut.Value == "in" ? _rl.inAt23.ToString() : _rl.outAt23.ToString();
                            break;
                        default:
                            txtAt.Text = "0";
                            break;
                    }
                }
            }
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView LV_inner = e.Item.FindControl("LV_inner") as ListView;
            LV_inner.DataSource = CURRENT_HOURLIST;
            LV_inner.DataBind();
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            if (drp_PickupPlace.getSelectedValueInt(0) == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('Seleziona Trasporto da/a');", true);
                return;
            }
            if (chkList_TransportType.getSelectedValueList().Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('Seleziona almeno un Tipo di trasporto');", true);
                return;
            }
            if (drp_hourTo.getSelectedValueInt(0) < drp_hourFrom.getSelectedValueInt(0))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorAlert", "alert('Orario di Inizio non puo essere successivo al orario di fine');", true);
                return;
            }
            saveCustom();
            fillList();
            UpdatePanel1.Update();
        }
        protected void lnk_saveTable_Click(object sender, EventArgs e)
        {
            saveTable();
            fillList();
        }
        protected void saveCustom()
        {
            int pidPickupPlace = drp_PickupPlace.getSelectedValueInt(0).objToInt32();
            List<string> TransportTypeList = chkList_TransportType.getSelectedValueList();
            ListView LV_inner;
            int _hourTo = drp_hourTo.getSelectedValueInt(0).objToInt32();
            int _hourFrom = drp_hourFrom.getSelectedValueInt(0).objToInt32();
            int _duration = txtDuration.Text.ToInt32();
            foreach (string transportType in TransportTypeList)
            {
                LIMO_RL_TRANSPORT_DURATION _rl = DC_LIMO.LIMO_RL_TRANSPORT_DURATION.SingleOrDefault(x => x.pidZone == IdZone && x.pidPickupPlace == pidPickupPlace && x.transportType == transportType);
                if (_rl == null)
                {
                    _rl = new LIMO_RL_TRANSPORT_DURATION();
                    _rl.pidZone = IdZone;
                    _rl.pidPickupPlace = pidPickupPlace;
                    _rl.transportType = transportType;
                    DC_LIMO.LIMO_RL_TRANSPORT_DURATION.InsertOnSubmit(_rl);
                }
                foreach (int hour in CURRENT_HOURLIST.Select(x=>x.hour))
                {
                    if (hour < _hourFrom || hour > _hourTo) continue;
                    string hourStr = hour.ToString();
                    if (HF_inOut.Value == "in")
                    {
                        switch (hourStr)
                        {
                            case "0":
                                _rl.inAt00 = _duration;
                                break;
                            case "1":
                                _rl.inAt01 = _duration;
                                break;
                            case "2":
                                _rl.inAt02 = _duration;
                                break;
                            case "3":
                                _rl.inAt03 = _duration;
                                break;
                            case "4":
                                _rl.inAt04 = _duration;
                                break;
                            case "5":
                                _rl.inAt05 = _duration;
                                break;
                            case "6":
                                _rl.inAt06 = _duration;
                                break;
                            case "7":
                                _rl.inAt07 = _duration;
                                break;
                            case "8":
                                _rl.inAt08 = _duration;
                                break;
                            case "9":
                                _rl.inAt09 = _duration;
                                break;
                            case "10":
                                _rl.inAt10 = _duration;
                                break;
                            case "11":
                                _rl.inAt11 = _duration;
                                break;
                            case "12":
                                _rl.inAt12 = _duration;
                                break;
                            case "13":
                                _rl.inAt13 = _duration;
                                break;
                            case "14":
                                _rl.inAt14 = _duration;
                                break;
                            case "15":
                                _rl.inAt15 = _duration;
                                break;
                            case "16":
                                _rl.inAt16 = _duration;
                                break;
                            case "17":
                                _rl.inAt17 = _duration;
                                break;
                            case "18":
                                _rl.inAt18 = _duration;
                                break;
                            case "19":
                                _rl.inAt19 = _duration;
                                break;
                            case "20":
                                _rl.inAt20 = _duration;
                                break;
                            case "21":
                                _rl.inAt21 = _duration;
                                break;
                            case "22":
                                _rl.inAt22 = _duration;
                                break;
                            case "23":
                                _rl.inAt23 = _duration;
                                break;
                        }
                    }
                    else
                    {
                    }

                }
            }
            DC_LIMO.SubmitChanges();
            limoProps.LIMO_RL_TRANSPORT_DURATION = DC_LIMO.LIMO_RL_TRANSPORT_DURATION.ToList();
        }
        protected void saveTable()
        {
            ListView LV_inner;
            foreach (ListViewDataItem _itemLV in LV.Items)
            {
                Label lbl_PickupPlace = _itemLV.FindControl("lbl_PickupPlace") as Label;
                Label lbl_TransportType = _itemLV.FindControl("lbl_TransportType") as Label;
                int pidPickupPlace = lbl_PickupPlace.Text.ToInt32();
                string transportType = lbl_TransportType.Text;
                LV_inner = _itemLV.FindControl("LV_inner") as ListView;
                LIMO_RL_TRANSPORT_DURATION _rl = DC_LIMO.LIMO_RL_TRANSPORT_DURATION.SingleOrDefault(x => x.pidZone == IdZone && x.pidPickupPlace == pidPickupPlace && x.transportType == transportType);
                if (_rl == null)
                {
                    _rl = new LIMO_RL_TRANSPORT_DURATION();
                    _rl.pidZone = IdZone;
                    _rl.pidPickupPlace = pidPickupPlace;
                    _rl.transportType = transportType;
                    DC_LIMO.LIMO_RL_TRANSPORT_DURATION.InsertOnSubmit(_rl);
                }
                foreach (ListViewDataItem _itemLV_inner in LV_inner.Items)
                {
                    Label lbl_hour = _itemLV_inner.FindControl("lbl_hour") as Label;
                    TextBox txtAt = _itemLV_inner.FindControl("txtAt") as TextBox;
                    if (HF_inOut.Value == "in")
                    {
                        switch (lbl_hour.Text)
                        {
                            case "0":
                                _rl.inAt00 = txtAt.Text.ToInt32();
                                break;
                            case "1":
                                _rl.inAt01 = txtAt.Text.ToInt32();
                                break;
                            case "2":
                                _rl.inAt02 = txtAt.Text.ToInt32();
                                break;
                            case "3":
                                _rl.inAt03 = txtAt.Text.ToInt32();
                                break;
                            case "4":
                                _rl.inAt04 = txtAt.Text.ToInt32();
                                break;
                            case "5":
                                _rl.inAt05 = txtAt.Text.ToInt32();
                                break;
                            case "6":
                                _rl.inAt06 = txtAt.Text.ToInt32();
                                break;
                            case "7":
                                _rl.inAt07 = txtAt.Text.ToInt32();
                                break;
                            case "8":
                                _rl.inAt08 = txtAt.Text.ToInt32();
                                break;
                            case "9":
                                _rl.inAt09 = txtAt.Text.ToInt32();
                                break;
                            case "10":
                                _rl.inAt10 = txtAt.Text.ToInt32();
                                break;
                            case "11":
                                _rl.inAt11 = txtAt.Text.ToInt32();
                                break;
                            case "12":
                                _rl.inAt12 = txtAt.Text.ToInt32();
                                break;
                            case "13":
                                _rl.inAt13 = txtAt.Text.ToInt32();
                                break;
                            case "14":
                                _rl.inAt14 = txtAt.Text.ToInt32();
                                break;
                            case "15":
                                _rl.inAt15 = txtAt.Text.ToInt32();
                                break;
                            case "16":
                                _rl.inAt16 = txtAt.Text.ToInt32();
                                break;
                            case "17":
                                _rl.inAt17 = txtAt.Text.ToInt32();
                                break;
                            case "18":
                                _rl.inAt18 = txtAt.Text.ToInt32();
                                break;
                            case "19":
                                _rl.inAt19 = txtAt.Text.ToInt32();
                                break;
                            case "20":
                                _rl.inAt20 = txtAt.Text.ToInt32();
                                break;
                            case "21":
                                _rl.inAt21 = txtAt.Text.ToInt32();
                                break;
                            case "22":
                                _rl.inAt22 = txtAt.Text.ToInt32();
                                break;
                            case "23":
                                _rl.inAt23 = txtAt.Text.ToInt32();
                                break;
                            default:
                                txtAt.Text = "0";
                                break;
                        }
                    }
                    else
                    {
                    }

                }
            }
            DC_LIMO.SubmitChanges();
            limoProps.LIMO_RL_TRANSPORT_DURATION = DC_LIMO.LIMO_RL_TRANSPORT_DURATION.ToList();
        }
    }
}