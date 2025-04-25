using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModRental;


public class rntUtilsChnlAll
{
    public static void UpdateRates(int idEstate)
    {
        UpdateRates(idEstate, (DateTime?)null, (DateTime?)null);
    }
    public static void UpdateRates(int idEstate, DateTime? dtStart, DateTime? dtEnd)
    {
        RNT_TB_ESTATE _est = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == idEstate);
        if (_est != null)
        {
            if (_est.bcomEnabled == 1)
                BcomUpdate.BcomUpdate_start(idEstate, "rates");
            ChnlHolidayUpdate.UpdateRates_start(idEstate);
            ChnlExpediaUpdate.UpdateSplitRates_start(idEstate, new List<string>());

            using (DCmodRental dc = new DCmodRental())
            {
                var currAirbnb = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == idEstate);
                if (currAirbnb != null && currAirbnb.hostId != null && currAirbnb.hostId != "" && currAirbnb.airbnb_id != null && currAirbnb.airbnb_id > 0 && currAirbnb.syncCategory != null && currAirbnb.syncCategory != "")
                {
                    ChnlAirbnbUpdate.UpdateRates_start(idEstate);
                    //ChnlAirbnbUpdate.UpdateRatesLOS_start(idEstate);
                }
            }
            //ChnlAirbnbUpdate.UpdateRates_start(idEstate);
            //ChnlExpediaUpdate.UpdateRatesWithSplitDates(idEstate, new List<string>());
        }
    }
    public static void UpdateAvalability(int idEstate, long resId, bool isSendExpediaAvailability = true, bool isSendHolidayAvailability = true)
    {
        UpdateAvalability(idEstate, resId, (DateTime?)null, (DateTime?)null, isSendExpediaAvailability, isSendHolidayAvailability);
    }
    public static void UpdateAvalability(int idEstate, long resId, DateTime? dtStart, DateTime? dtEnd, bool isSendExpediaAvailability = true, bool isSendHolidayAvailability = true)
    {
        //if (resId > 0) ChnlMagaRentalUpdate.BookingUpdate_start(resId);
        RNT_TB_ESTATE _est = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == idEstate);
        if (_est != null)
        {
            if (dtStart.HasValue && dtEnd.HasValue)
            {
                if (_est.bcomEnabled == 1)
                    BcomUpdate.BcomUpdate_start(idEstate, "availability", dtStart.Value, dtEnd.Value);
                if (isSendHolidayAvailability)
                    ChnlHolidayUpdate.UpdateAvailability_start(idEstate, dtStart.Value, dtEnd.Value);
                if (isSendExpediaAvailability)
                    ChnlExpediaUpdate.UpdateAvailability_start(idEstate, dtStart.Value, dtEnd.Value);
                using (DCmodRental dc = new DCmodRental())
                {
                    var currAirbnb = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == idEstate);
                    if (currAirbnb != null && currAirbnb.hostId != null && currAirbnb.hostId != "" && currAirbnb.airbnb_id != null && currAirbnb.airbnb_id > 0 && currAirbnb.syncCategory != null && currAirbnb.syncCategory != "")
                    {
                        ChnlAirbnbUpdate.UpdateAvailabilityUpdate_start(idEstate, dtStart.Value, dtEnd.Value);
                    }
                }
            }
            else
            {
                if (_est.bcomEnabled == 1)
                    BcomUpdate.BcomUpdate_start(idEstate, "availability");
                if (isSendHolidayAvailability)
                    ChnlHolidayUpdate.UpdateAvailability_start(idEstate);
                if (isSendExpediaAvailability)
                    ChnlExpediaUpdate.UpdateAvailability_start(idEstate);

                using (DCmodRental dc = new DCmodRental())
                {
                    var currAirbnb = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == idEstate);
                    if (currAirbnb != null && currAirbnb.hostId != null && currAirbnb.hostId != "" && currAirbnb.airbnb_id != null && currAirbnb.airbnb_id > 0 && currAirbnb.syncCategory != null && currAirbnb.syncCategory != "")
                    {
                        ChnlAirbnbUpdate.UpdateAvailabilityUpdate_start(idEstate);
                    }
                }
            }


            using (DCmodRental dc = new DCmodRental())
            {
                var agentContractTBL = dc.dbRntAgentContractTBLs.FirstOrDefault(x => x.pidAgent == CommonUtilities.getSYS_SETTING("vision_agent_id").objToInt32());
                if (agentContractTBL != null)
                {
                    var agentContratEstateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgentContract == agentContractTBL.id).Select(x => x.pidEstate).ToList();
                    if (agentContratEstateIds.Contains(_est.id))
                        ChnlMagaRentalWsUpdate.BookingUpdate_start(resId);
                }
            }

        }
    }

    public static void updateExpediaHotels(List<int> estateIds)
    {
        using (DCchnlExpedia dcChnl = new DCchnlExpedia())
        {
            var expediaEstates = dcChnl.dbRntChnlExpediaEstateTBLs.Where(x => estateIds.Contains(x.id)).ToList();
            var expediaHotels = dcChnl.dbRntChnlExpediaEstateTBLs.Where(x => estateIds.Contains(x.id)).ToList().Select(x => x.HotelId).Distinct();
            foreach (var hotel in expediaHotels)
            {
                List<int> hotelEstateIds = expediaEstates.Where(x => x.HotelId == hotel).Select(x => x.id).ToList();
                ChnlExpediaUpdate.UpdateHotelAvailability_start(hotel, hotelEstateIds);
            }
        }
    }
}
