
using IMSBusinessLogic;
using IMSCommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class AddDepartment : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Convert.ToInt32(Session["dId"].ToString()) > 0)
                {
                    DepartmentName.Text = Session["dname"].ToString();
                    DepartmentCode.Text = Session["dcode"].ToString();
                    btnSaveDepartment.Text = "Update";
                }
            }

        }

        protected void btnSaveDepartment_Click(object sender, EventArgs e)
        {
            DepartmentBLL depManager = new DepartmentBLL();

            if (Convert.ToInt32(Session["dId"].ToString()) > 0)
            {
                int selectedId = int.Parse(Session["dId"].ToString());
                Department depToUpdate = new Department();
                depToUpdate.DepartmentID = selectedId;
                depToUpdate.Name = DepartmentName.Text;
                depToUpdate.Code = DepartmentCode.Text;
                depManager.Update(depToUpdate, connection); 
            }
            else
            {
                Department depToAdd = new Department();
                depToAdd.Name = DepartmentName.Text;
                depToAdd.Code = DepartmentCode.Text;

                depManager.Add(depToAdd, connection);
            }
            Session.Remove("dname");
            Session.Remove("dcode");
            Session.Remove("dId"); 

            Response.Redirect("ManageDepartment.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            DepartmentName.Text = "";
            DepartmentCode.Text = "";
            Response.Redirect("ManageDepartment.aspx");
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageDepartment.aspx");
        }
    }
}