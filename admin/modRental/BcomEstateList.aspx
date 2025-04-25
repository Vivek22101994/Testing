<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="BcomEstateList.aspx.cs" Inherits="ModRental.admin.modRental.BcomEstateList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:LinqDataSource ID="LDS" runat="server" Where="is_active=1 AND is_deleted=0 AND bcomEnabled=1 AND (bcomHotelId=@bcomHotelId OR bcomRoomId=@bcomRoomId)" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TB_ESTATE" OrderBy="code">
        <WhereParameters>
            <asp:Parameter Name="bcomHotelId" DefaultValue="" Type="String" ConvertEmptyStringToNull="false" />
            <asp:Parameter Name="bcomRoomId" DefaultValue="" Type="String" ConvertEmptyStringToNull="false" />
        </WhereParameters>
    </asp:LinqDataSource>
    <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
        <ItemTemplate>
            <tr>
                <td><%# Eval("id") %></td>
                <td><%# Eval("bcomName")%></td>
                <td><%# Eval("loc_address")%></td>
                <td><%# CurrentSource.loc_cityTitle(Eval("pid_city").objToInt32(), App.DefLangID, "")%></td>
                <td><%# ""+Eval("is_google_maps")=="1" && (""+Eval("google_maps")).splitStringToList("|").Count==2?(""+Eval("google_maps")).splitStringToList("|")[0].Replace(",","."):""%></td>
                <td><%# ""+Eval("is_google_maps")=="1" && (""+Eval("google_maps")).splitStringToList("|").Count==2?(""+Eval("google_maps")).splitStringToList("|")[1].Replace(",","."):""%></td>
                <td><%# Eval("loc_zip_code")%></td>
                <td>IT</td>
                <td>APARTMENT</td>
                <td></td>
                <td><%# Eval("importance_vote").objToInt32()>0?(""+(Eval("importance_vote").objToInt32()/2)).Replace(",","."):""%></td>
                <td>1</td>
                <td>12:00</td>
                <td></td>
                <td></td>
                <td>11:00</td>
                <td>EUR</td>
                <td>Visa|Euro/Mastercard</td>
                <td>IT07824541002</td>
                <td></td>
                <td><%# CurrentSource.rntEstate_summary(Eval("id").objToInt32(), App.DefLangID, "") %></td>
                <td>Car Rental, Airport Shuttle, Private check-in and check-out</td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 4)?"1":"0" %></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 2)||CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 3)||CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 45)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 33)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 5)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 44)?"1":"0" %></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 16)||CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 17)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 7)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 22)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 54)?"1":"0" %></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 29)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 25)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 30)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 36)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td>Matteo Calabrò</td>
                <td>matteo.calabro@rentalinrome.com</td>
                <td>+39 06 3220068</td>
                <td>+39 06 23328717</td>
                <td>IT</td>
                <td>Alessandro D'Arcadia</td>
                <td>alessandro.darcadia@rentalinrome.com</td>
                <td>+39 06 3220068</td>
                <td>IT</td>
                <td>Alessandro D'Arcadia</td>
                <td>alessandro.darcadia@rentalinrome.com</td>
                <td>+39 06 3220068</td>
                <td>IT</td>
                <td>Matteo Calabrò</td>
                <td>matteo.calabro@rentalinrome.com</td>
                <td>+39 06 3220068</td>
                <td>IT</td>
                <td>Matteo Calabrò</td>
                <td>matteo.calabro@rentalinrome.com</td>
                <td>+39 06 3220068</td>
                <td>IT</td>
                <td>Matteo Calabrò</td>
                <td>matteo.calabro@rentalinrome.com</td>
                <td>+39 06 3220068</td>
                <td>IT</td>
                <td>Matteo Calabrò</td>
                <td>matteo.calabro@rentalinrome.com</td>
                <td>+39 06 3220068</td>
                <td>IT</td>
                <td>Matteo Calabrò</td>
                <td>matteo.calabro@rentalinrome.com</td>
                <td>+39 06 3220068</td>
                <td>+39 06 23328717</td>
                <td>IT</td>
                <td>Matteo Calabrò</td>
                <td>matteo.calabro@rentalinrome.com</td>
                <td>+39 06 3220068</td>
                <td>IT</td>
                <td><%# Eval("code")%></td>
                <td>APARTMENT</td>
                <td><%# Eval("num_persons_max")%></td>
                <td><%# Eval("mq_inner")%></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 1)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 6)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 53)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 20)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 16)||CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 17)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 48)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 51)?"1":"0" %></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 25)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 22)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td>1</td>
                <td>1</td>
                <td>0</td>
                <td></td>
                <td>1</td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 46)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td>1</td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 49)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 11)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 9)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 9)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 8)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 34)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 14)?"1":"0" %></td>
                <td>0</td>
                <td>0</td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 12)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td>1</td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 15)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 47)?"1":"0" %></td>
                <td>1</td>
                <td>1</td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 32)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 19)?"1":"0" %></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 18)?"1":"0" %></td>
                <td>1</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 54)?"1":"0" %></td>
                <td><%# CurrentSource.rntEstate_hasConfig(Eval("id").objToInt32(), 44)?"1":"0" %></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td>Rental in Rome Srl
                </td>
                <td>Matteo Calabrò
                </td>
                <td>Via Appia Nuova 677
                </td>
                <td>00179
                </td>
                <td>Roma
                </td>
                <td>Italia
                </td>
                <td></td>
                <td>V
                </td>
                <td>PC
                </td>
                <td>I
                </td>
                <td>1.22</td>
                <td>PPRN
                </td>
                <td>X
                </td>
                <td></td>
                <td>NA
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
        </ItemTemplate>
        <EmptyDataTemplate>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <meta http-equiv="Content-Type" content="charset=utf-8" />
            <table border="0" cellpadding="0" cellspacing="0" style="">
                <tr>
                    <th class="xl91" style="mso-style-parent: style51; font-weight: 700; text-align: center; vertical-align: middle; background: #B6DDE8; mso-pattern: black none;" colspan="20">General Information</th>
                    <th class="xl92" style="mso-style-parent: style23; color: #9C6500; font-weight: 700; text-align: center; vertical-align: middle; background: #FFEB9C; mso-pattern: black none; white-space: normal;" colspan="2">Accommodation Description</th>
                    <th class="xl75" style="mso-style-parent: style21; color: #006100; font-weight: 700; vertical-align: middle; background: #C6EFCE; mso-pattern: black none; white-space: normal;" colspan="55">Accommodation general facilities</th>
                    <th class="xl71" style="mso-style-parent: style34; font-weight: 700; vertical-align: middle; background: #DBE5F1; mso-pattern: black none; white-space: normal;" colspan="38">Contact Person (General, availability, content, invoice, contract, request, reservation, parity)</th>
                    <th class="xl68" style="mso-style-parent: style45; color: white; font-weight: 700; vertical-align: middle; background: #8064A2; mso-pattern: black none; white-space: normal;" colspan="70">Room Information</th>
                    <th class="xl94" style="mso-style-parent: style51; font-weight: 700; vertical-align: middle; background: #B6DDE8; mso-pattern: black none;" colspan="7">Finance(Invoicing Details)</th>
                    <th class="xl74" style="mso-style-parent: style40; color: white; font-weight: 700; vertical-align: middle; background: #D99795; mso-pattern: black none;" colspan="11">VAT / TAX / CHARGES</th>
                </tr>
                <tr>
                    <th>Property Reference ID</th>
                    <th>Property Name</th>
                    <th>Address</th>
                    <th>City</th>
                    <th>Latitude</th>
                    <th>Longitude</th>
                    <th>Zipcode</th>
                    <th>Country code</th>
                    <th>Accommodation Type</th>
                    <th>Chain</th>
                    <th>Star rating</th>
                    <th>Total number of bookable options</th>
                    <th>Check In from</th>
                    <th>Check in until</th>
                    <th>Check out from</th>
                    <th>Check out until</th>
                    <th>Currency Code</th>
                    <th>Accepted Credit Cards</th>
                    <th>VAT number</th>
                    <th>Property URL</th>
                    <th>Quick description</th>
                    <th>Important Information</th>
                    <th>24-hour front desk</th>
                    <th>Pets allowed</th>
                    <th>Parking: On-site/Nearby/None</th>
                    <th>Internet Access (Y/N)</th>
                    <th>All Public and Private spaces  Non-smoking area</th>
                    <th>Allergy free room available</th>
                    <th>Breakfast buffet</th>
                    <th>Elevator</th>
                    <th>Garden</th>
                    <th>Gay friendly</th>
                    <th>Heating</th>
                    <th>Luggage storage</th>
                    <th>non smoking rooms</th>
                    <th>private beach area</th>
                    <th>restaurant</th>
                    <th>rooms/facilities for disabled</th>
                    <th>safe deposit box</th>
                    <th>ski storage</th>
                    <th>sun terrace</th>
                    <th>terrace</th>
                    <th>bbq facilities</th>
                    <th>fitness room</th>
                    <th>outdoor swimming pool (all year)</th>
                    <th>hot tub</th>
                    <th>billiard</th>
                    <th>bowling</th>
                    <th>canoeing</th>
                    <th>children play ground</th>
                    <th>cycling</th>
                    <th>darts</th>
                    <th>diving</th>
                    <th>fishing</th>
                    <th>game room</th>
                    <th>golfcourse (within 3km)</th>
                    <th>hiking</th>
                    <th>horse-riding</th>
                    <th>hot spring bath</th>
                    <th>library</th>
                    <th>minigolf</th>
                    <th>sauna</th>
                    <th>ski school</th>
                    <th>skiing</th>
                    <th>snorkeling</th>
                    <th>solarium</th>
                    <th>spa &amp; wellness centre</th>
                    <th>squash</th>
                    <th>table tennis</th>
                    <th>tennis-court</th>
                    <th>turkish/steam bath</th>
                    <th>windsurfing</th>
                    <th>bicycle rent</th>
                    <th>bikes available (free)</th>
                    <th>ski equipment hire on site</th>
                    <th>ski pass vendor</th>
                    <th>ski-to-door access</th>
                    <th>general contact name</th>
                    <th>general contact email</th>
                    <th>general contact phone</th>
                    <th>general contact fax</th>
                    <th>general contact language</th>
                    <th>availability contact name</th>
                    <th>availability contact email</th>
                    <th>availability contact phone</th>
                    <th>availability contact language</th>
                    <th>central reservations contact
  name</th>
                    <th>central reservations contact
  email</th>
                    <th>central reservations contact
  phone</th>
                    <th>availability contact language</th>
                    <th>invoices contact name</th>
                    <th>invoices contact email</th>
                    <th>invoices contact phone</th>
                    <th>invoices contact language</th>
                    <th>contract contact name</th>
                    <th>contract contact email</th>
                    <th>contract contact phone</th>
                    <th>contract contact language</th>
                    <th>parity contact name</th>
                    <th>parity contact email</th>
                    <th>parity contact phone</th>
                    <th>Parity contact language</th>
                    <th>Special request contact name</th>
                    <th>Special request contact email</th>
                    <th>Special request contact phone</th>
                    <th>Special request contact language</th>
                    <th>Reservation contact name</th>
                    <th>Reservation contact email</th>
                    <th>Reservation contact phone</th>
                    <th>Reservation contact fax</th>
                    <th>Reversation contact language</th>
                    <th>Content contact name</th>
                    <th>Content contact email</th>
                    <th>Content contact phone</th>
                    <th>Content contact language</th>
                    <th>Room Name (Format: Two-Bedroom
  Villa, Three-Bedroom Holiday Home)</th>
                    <th>Room type</th>
                    <th>Max person of stay in the villa</th>
                    <th>Minimum Size of Room</th>
                    <th>Smoking/non smoking</th>
                    <th>airconditioning</th>
                    <th>washing machine</th>
                    <th>clothes dryer</th>
                    <th>desk</th>
                    <th>extra long beds (&gt; 2 meter)</th>
                    <th>fan</th>
                    <th>fireplace</th>
                    <th>heating</th>
                    <th>hot tub</th>
                    <th>interconnecting room(s)
  available</th>
                    <th>clothing iron</th>
                    <th>ironing facilities</th>
                    <th>mosquito net</th>
                    <th>private pool</th>
                    <th>safe deposit box</th>
                    <th>seating area</th>
                    <th>sofa</th>
                    <th>sound proofing</th>
                    <th>trouser press</th>
                    <th>shower</th>
                    <th>bathroom</th>
                    <th>shared bathroom</th>
                    <th>bath or shower</th>
                    <th>bidet</th>
                    <th>hair dryer</th>
                    <th>sauna</th>
                    <th>spa bath</th>
                    <th>toilet</th>
                    <th>cd-player</th>
                    <th>radio</th>
                    <th>ipad</th>
                    <th>ipod docking station</th>
                    <th>cable channels</th>
                    <th>satellite channels</th>
                    <th>tv</th>
                    <th>dvd-player</th>
                    <th>computer</th>
                    <th>laptop</th>
                    <th>laptop safe box</th>
                    <th>game console</th>
                    <th>telephone</th>
                    <th>video games</th>
                    <th>barbecue</th>
                    <th>dining area</th>
                    <th>dishwasher</th>
                    <th>electric kettle</th>
                    <th>kitchen</th>
                    <th>kitchenette</th>
                    <th>kitchenware</th>
                    <th>microwave</th>
                    <th>mini-bar</th>
                    <th>oven</th>
                    <th>refrigerator</th>
                    <th>stove</th>
                    <th>coffee/tea maker</th>
                    <th>toaster</th>
                    <th>alarm clock</th>
                    <th>Balcony</th>
                    <th>Garden view</th>
                    <th>Lake view</th>
                    <th>Landmark view</th>
                    <th>Mountain view</th>
                    <th>Patio</th>
                    <th>pool view</th>
                    <th>sea view</th>
                    <th>Company Name for Invoicing</th>
                    <th>Attention Off</th>
                    <th>Legal Adress</th>
                    <th>Legal Zip Code</th>
                    <th>Legal City</th>
                    <th>Country</th>
                    <th>Invoice Medium</th>
                    <th>VAT/tax</th>
                    <th>VAT type</th>
                    <th>Included / Excluded</th>
                    <th>VAT Amount</th>
                    <th>City tax type</th>
                    <th>City tax status</th>
                    <th>City tax amount</th>
                    <th>Service Charge type</th>
                    <th>Service type excluded</th>
                    <th>Service type amount</th>
                    <th>Service Charge name</th>
                </tr>
                <tr id="itemPlaceholder" runat="server">
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>

</asp:Content>
