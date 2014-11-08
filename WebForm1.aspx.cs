using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace WebApplication2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        SqlDataAdapter da;
        DataTable dt;
        SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGrid();
            }
        }

        private void FillGrid()
        {
            da = new SqlDataAdapter("select * from state", con);
            dt = new DataTable();
            da.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                cmd = new SqlCommand("insert into state values(@sname)", con);
                cmd.Parameters.AddWithValue("@sname", ((TextBox)GridView1.HeaderRow.FindControl("TextBox2")).Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                FillGrid();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            FillGrid();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            FillGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            cmd = new SqlCommand("update state set statename=@sname where stateid=@SID",con);
            cmd.Parameters.AddWithValue("@sname",((TextBox)GridView1.Rows[e.RowIndex].FindControl("TextBox1")).Text);
            cmd.Parameters.AddWithValue("@sid", Convert.ToInt16(GridView1.DataKeys[e.RowIndex].Value));
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            GridView1.EditIndex = -1;
            FillGrid();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            cmd = new SqlCommand("delete from state where stateid=@SID", con);
            cmd.Parameters.AddWithValue("@sid", Convert.ToInt16(GridView1.DataKeys[e.RowIndex].Value));
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            FillGrid();
        }
    }
}