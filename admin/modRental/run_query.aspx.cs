using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rentalinrome.admin.modRental
{
    public partial class run_query : adminBasePage
    {
        private SqlConnection con = null;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool Connect()
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["rental_in_romeDBConnectionString"].ConnectionString);
                if (con.State != ConnectionState.Open)
                    con.Open();
            }
            catch (Exception ex)
            {
                divMessage.InnerHtml = ex.Message;
                return false;
            }
            return true;
        }
        protected void btnRun_Click(object sender, EventArgs e)
        {
            DataTable dtResult = new DataTable();

            dtResult = ExecuteSelectCommand(txt_query.Text, CommandType.Text);

            grdResult.DataSource = dtResult;
            grdResult.DataBind();


        }

        public DataTable ExecuteSelectCommand(string CommandName, CommandType cmdType)
        {
            SqlCommand cmd = null;
            DataTable table = new DataTable();
            if (Connect())
            {
                cmd = con.CreateCommand();

                cmd.CommandType = cmdType;
                cmd.CommandText = CommandName;

                try
                {
                    SqlDataAdapter da = null;
                    using (da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(table);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    cmd = null;
                    con.Close();
                }

            }
            return table;
        }
    }
}