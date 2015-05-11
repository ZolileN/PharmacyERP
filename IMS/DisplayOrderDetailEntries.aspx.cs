using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class DisplayOrderDetailEntries : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                BindLabels(true);
                BindGrid();
            }
        }

        private void BindLabels(bool onLoad) 
        {
            if (onLoad)
            {
                ProdName.Text = Session["ProdDesc"].ToString();
                lblOrderDetID.Text = Session["ordetailID"].ToString();
                OrdQuantity.Text = Session["ordQuan"].ToString();
                bonusQuanOrg.Text = Session["bonusQuan"].ToString();
                RecQuantity.Text = Session["recQuan"].ToString();
                RemQuantity.Text = Session["remQuan"].ToString();
                defQuantity.Text = Session["defQuan"].ToString();
                retQuantity.Text = Session["retQuan"].ToString();
                lblBarSerial.Text = Session["barserial"].ToString();
                expQuantity.Text = Session["expQuan"].ToString();
                lblOMISD.Text = Session["OMID"].ToString();
                lblPO.Text = Session["isPO"].ToString();
                lblProdID.Text = Session["ProdID"].ToString();
                OrderedbonusQuan.Text = Session["OrderedBonus"].ToString();
              
            }
            else
            {
                try
                {
                    #region Query Execution
                    connection.Open();
                    SqlCommand command = new SqlCommand("Sp_GetPOrderDetails_ID", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_OrderDetailID", Convert.ToInt32(lblOrderDetID.Text));

                    DataSet ds = new DataSet();

                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);


                    OrdQuantity.Text = ds.Tables[0].Rows[0]["OrderedQuantity"].ToString();
                    bonusQuanOrg.Text = ds.Tables[0].Rows[0]["BonusQuantity"].ToString();
                    RecQuantity.Text = ds.Tables[0].Rows[0]["ReceivedQuantity"].ToString();
                    RemQuantity.Text = ds.Tables[0].Rows[0]["RemainingQuantity"].ToString();
                    defQuantity.Text = ds.Tables[0].Rows[0]["DefectedQuantity"].ToString();
                    retQuantity.Text = ds.Tables[0].Rows[0]["ReturnedQuantity"].ToString();
                    expQuantity.Text = ds.Tables[0].Rows[0]["ExpiredQuantity"].ToString();

                    #endregion
                }
                catch (Exception exp) { }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
           
            //OrdQuantity.Text = Session[""].ToString();

        }
        private void BindGrid() 
        {
            try
            {
                #region Query Execution
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetOrderDetail_ReceiveEntries", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_OrderDetailID", Convert.ToInt32(Session["ordetailID"].ToString()));

                DataSet ds = new DataSet();

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ProductSet = ds;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();
                ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddCP")).Text = Session["OrgCP"].ToString();
                ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddSP")).Text = Session["OrgSP"].ToString();
                #endregion
            }
            catch (Exception exp) { }
            finally 
            {
                if (connection.State == ConnectionState.Open) 
                {
                    connection.Close();
                }
            }
        }


        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            BindGrid();
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddRec"))
                {
                    #region Fetching values
                    int recQuan,expQuan,defQuan,bonusQuan,retQuan,bonusOrg,remQuan,orderedQuantity;
                    recQuan = expQuan = defQuan = bonusQuan = retQuan = bonusOrg = remQuan = orderedQuantity=0;

                    float txtDisc, txtCP, txtSP;
                    txtDisc = txtCP = txtSP = 0;

                    string lblRec = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddRecQuan")).Text;
                    string lblExp = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddExpQuan")).Text;
                    string lblDef = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddDefQuan")).Text;
                    string lblRet = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddRetQuan")).Text;
                    string lblBonus = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddBonus")).Text;
                    string lblCP = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddCP")).Text;
                    string lblSP = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddSP")).Text;
                    string lblexpiry = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddExpDate")).Text;
                    string lblDiscount = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddDisPer")).Text;
                    string lblBatch = ((TextBox)StockDisplayGrid.FooterRow.FindControl("txtAddBatch")).Text;
                    
                   // string lblBarcode = ((Label)StockDisplayGrid.FooterRow.FindControl("lblbarCode")).Text;
                    int orderDetID=int.Parse(lblOrderDetID.Text);

                   
                    #endregion
                    
                    #region Parsing fields

                    DateTime expiryDate = new DateTime();
                    DateTime.TryParse(lblexpiry, out expiryDate);
                    bonusOrg = int.Parse(bonusQuanOrg.Text);
                    remQuan = int.Parse(RemQuantity.Text);
                    orderedQuantity = int.Parse(OrdQuantity.Text);
                    int.TryParse(lblRec, out recQuan);
                    int.TryParse(lblBonus, out bonusQuan);
                    int.TryParse(lblExp, out expQuan);
                    int.TryParse(lblDef, out defQuan);
                    int.TryParse(lblRet, out retQuan);
                    
                    float.TryParse(lblCP, out txtCP);
                    float.TryParse(lblSP, out txtSP);
                    float.TryParse(lblDiscount, out txtDisc);
                    #endregion

                    #region Input checks
                    if (bonusQuan == 0 && defQuan == 0 && recQuan == 0 && expQuan == 0 && retQuan == 0)
                    {
                        WebMessageBoxUtil.Show("All values cannot be 0");
                        StockDisplayGrid.EditIndex = -1;
                        BindGrid();
                        return;
                    }

                    if (txtCP < 0 || txtSP < 0)
                    {
                        WebMessageBoxUtil.Show("Entered value cannot be negative");
                        StockDisplayGrid.EditIndex = -1;
                        BindGrid();
                        return;
                    }

                    if (recQuan < 0 || expQuan < 0 || defQuan < 0 || retQuan < 0)
                    {
                        WebMessageBoxUtil.Show("Entered value cannot be negative");
                        StockDisplayGrid.EditIndex = -1;
                        BindGrid();
                        return;
                    }

                    if (recQuan > remQuan||defQuan >remQuan || expQuan >remQuan || retQuan>remQuan)
                    {
                        WebMessageBoxUtil.Show("Entered Quantity cannot exceed remaining quantity " + remQuan);
                        StockDisplayGrid.EditIndex = -1;
                        BindGrid();
                        return;
                    }

                    if (remQuan < (recQuan + expQuan + defQuan + retQuan))
                    {
                        WebMessageBoxUtil.Show("Entered Quantity cannot exceed remaining quantity " + remQuan);
                        StockDisplayGrid.EditIndex = -1;
                        BindGrid();
                        return;
                    }
                    else
                    {
                        
                        remQuan = remQuan - (recQuan + expQuan + defQuan+retQuan);

                    }

                   

                    #endregion

                    #region barcode generation
                    long newBarcode = 0;
                   
                        if (!string.IsNullOrEmpty(lblexpiry))
                        {
                            DateTime dateValue = (Convert.ToDateTime(lblexpiry));

                            string p1;
                            String mm;//= dateValue.Month.ToString();
                            if (dateValue.Month < 10)
                            {
                                mm = dateValue.Month.ToString().PadLeft(2, '0');

                            }
                            else
                            {
                                mm = dateValue.Month.ToString();
                            }
                            String yy = dateValue.ToString("yy", DateTimeFormatInfo.InvariantInfo);
                            p1 = lblBarSerial.Text + mm + yy;

                            if (long.TryParse(p1, out newBarcode))
                            {
                            }
                            else
                            {
                                //post error message 
                            }
                        }
                     
                    #endregion

                    if (orderedQuantity >= (recQuan + expQuan + defQuan + retQuan))
                    {
                        #region Query execution

                            connection.Open();
                            SqlCommand command = new SqlCommand("Sp_StockReceiving", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
                            command.Parameters.AddWithValue("@p_ReceivedQuantity", recQuan);
                            command.Parameters.AddWithValue("@p_ExpiredQuantity", expQuan);
                            command.Parameters.AddWithValue("@p_DefectedQuantity", defQuan);
                            command.Parameters.AddWithValue("@p_ReturnedQuantity", retQuan);

                            command.Parameters.AddWithValue("@p_BarCode", newBarcode);
                            command.Parameters.AddWithValue("@p_DiscountPercentage", txtDisc);
                            command.Parameters.AddWithValue("@p_Bonus", bonusQuan);
                            if (!String.IsNullOrEmpty(lblBatch))
                            {
                                command.Parameters.AddWithValue("@p_BatchNumber", lblBatch);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_BatchNumber", DBNull.Value);
                            }
                            if (string.IsNullOrEmpty(lblexpiry))
                            {
                                command.Parameters.AddWithValue("@p_Expiry", DBNull.Value);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_Expiry", expiryDate);
                            }
                            command.Parameters.AddWithValue("@p_Cost", txtCP);
                            command.Parameters.AddWithValue("@p_Sales", txtSP);

                            command.Parameters.AddWithValue("@p_BonusTotal", bonusQuan + bonusOrg);// total bonus added
                            command.Parameters.AddWithValue("@p_RemainingQuantity", remQuan);
                            command.Parameters.AddWithValue("@p_SystemType", Session["RequestDesRole"].ToString());
                            command.Parameters.AddWithValue("@p_StoreID", Session["UserSys"].ToString());
                            command.Parameters.AddWithValue("@p_orderMasterID", int.Parse(lblOMISD.Text));
                            command.Parameters.AddWithValue("@p_isInternal", "TRUE");
                            command.Parameters.AddWithValue("@p_isPO", lblPO.Text);
                            command.Parameters.AddWithValue("@p_ProductID", int.Parse(lblProdID.Text));
                            
                            command.Parameters.AddWithValue("@p_expiryOriginal", DBNull.Value);
                           
                            if (int.Parse(OrdQuantity.Text) > recQuan)
                            {

                                command.Parameters.AddWithValue("@p_comments", "Sent to Vendor");

                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_comments", "Completed");
                            }
                            command.ExecuteNonQuery();
                            #endregion
                    }
                    WebMessageBoxUtil.Show("Stock Successfully Added");
                    //RemQuantity.Text = remQuan.ToString();
                }
                else if (e.CommandName.Equals("UpdateStock"))
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;
                    #region Fetching values
                    int recQuan, expQuan, defQuan, bonusQuan, retQuan, bonusOrg, remQuan, bonusTotal, recQuanOrg, expQuanOrg, defQuanOrg, retQuanOrg, orderedQuantity;
                    recQuan = expQuan = defQuan = bonusQuan = retQuan = bonusOrg = remQuan = bonusTotal =recQuanOrg=expQuanOrg=defQuanOrg=retQuanOrg = orderedQuantity= 0;

                    float txtDisc, txtCP, txtSP;
                    txtDisc = txtCP = txtSP = 0;

                    string lblRec = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("RecQuanVal")).Text;
                    string lblExp = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("ExpQuanVal")).Text;
                    string lblDef = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("defQuanVal")).Text;
                    string lblRet = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("retQuanVal")).Text;
                    string lblBonus = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("txtBonus")).Text;
                    string lblCP = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("retCP")).Text;
                    string lblSP = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("retSP")).Text;
                    string lblexpiry = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("txtExpDate")).Text;
                    string lblDiscount = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("txtDisc")).Text;
                    string lblBatch = ((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("txtBatch")).Text;

                    int entryID = int.Parse(((Label)StockDisplayGrid.Rows[RowIndex].FindControl("lblentryID")).Text);
                    string lblBarcode = ((Label)StockDisplayGrid.Rows[RowIndex].FindControl("lblbarCode")).Text;
                    int orderDetID = int.Parse(((Label)StockDisplayGrid.Rows[RowIndex].FindControl("lblOrdDet_id")).Text);
                    string lblExpOrg = ((Label)StockDisplayGrid.Rows[RowIndex].FindControl("lblExpOrg")).Text;
                    
                    #endregion

                    #region Parsing fields

                    DateTime expiryOrg = new DateTime();
                    if (!string.IsNullOrEmpty(lblExpOrg))
                    {
                        DateTime.TryParse(lblExpOrg, out expiryOrg);
                    }
                    DateTime expiryDate = new DateTime();
                    DateTime.TryParse(lblexpiry, out expiryDate);
                    bonusTotal = int.Parse(bonusQuanOrg.Text);
                    remQuan= int.Parse(RemQuantity.Text);
                    orderedQuantity = int.Parse(OrdQuantity.Text);
                    bonusOrg = int.Parse(((Label)StockDisplayGrid.Rows[RowIndex].FindControl("lblBonusOrg")).Text);
                    recQuanOrg = int.Parse(((Label)StockDisplayGrid.Rows[RowIndex].FindControl("lblRecQuanOrg")).Text);
                    retQuanOrg = int.Parse(((Label)StockDisplayGrid.Rows[RowIndex].FindControl("lblRetQuanOrg")).Text);
                    expQuanOrg = int.Parse(((Label)StockDisplayGrid.Rows[RowIndex].FindControl("lblExpQuanOrg")).Text);
                    defQuanOrg = int.Parse(((Label)StockDisplayGrid.Rows[RowIndex].FindControl("lblDefQuanOrg")).Text);
                   
                    if (int.TryParse(lblRec, out recQuan))
                    {
                        if (int.TryParse(lblExp, out expQuan))
                        {
                            if (int.TryParse(lblDef, out defQuan))
                            {
                                if (int.TryParse(lblRet, out retQuan))
                                {
                                    if (float.TryParse(lblCP, out txtCP))
                                    {
                                        if (float.TryParse(lblSP, out txtSP)) 
                                        {
                                            if (float.TryParse(lblDiscount, out txtDisc)) 
                                            {
                                                if (int.TryParse(lblBonus, out bonusQuan)) { }
                                                else { }
                                            }
                                            else { }
                                        }
                                        else { }
                                    }
                                    else { }
                                }
                                else { }
                            }
                            else { }

                        }
                        else
                        {
                        }
                    }
                    else
                    {

                    } 
                    #endregion

                    #region Input checks
                    if (bonusQuan == 0 && defQuan == 0 && recQuan == 0 && expQuan == 0 && retQuan == 0)
                    {
                        WebMessageBoxUtil.Show("All values cannot be 0");
                        StockDisplayGrid.EditIndex = -1;
                        BindGrid();
                        return;
                    }

                    if (txtCP < 0 || txtSP < 0)
                    {
                        WebMessageBoxUtil.Show("Entered value cannot be negative");
                        StockDisplayGrid.EditIndex = -1;
                        BindGrid();
                        return;
                    }

                    if (recQuan < 0 || expQuan < 0 || defQuan < 0 || retQuan < 0)
                    {
                        WebMessageBoxUtil.Show("Entered value cannot be negative");
                        StockDisplayGrid.EditIndex = -1;
                        BindGrid();
                        return;
                    }

                    //if (recQuan > remQuan || defQuan > remQuan || expQuan > remQuan || retQuan > remQuan)
                    //{


                    //    if (recQuan == recQuanOrg)
                    //    {
                    //        if (defQuan == defQuanOrg)
                    //        {
                    //            if (expQuan == expQuanOrg)
                    //            {
                    //                if (retQuan == retQuanOrg) { }
                    //                else
                    //                {
                    //                    WebMessageBoxUtil.Show("Entered Quantity cannot exceed remaining quantity " + remQuan);
                    //                    StockDisplayGrid.EditIndex = -1;
                    //                    BindGrid();
                    //                    return;
                    //                }
                    //            }
                    //        }
                    //    }

                    //}


                    if ((remQuan + (recQuanOrg + defQuanOrg + expQuanOrg + retQuanOrg)) < (recQuan + expQuan + defQuan + retQuan))
                    {
                        WebMessageBoxUtil.Show("Entered Quantity cannot exceed remaining quantity " + remQuan);
                        StockDisplayGrid.EditIndex = -1;
                        BindGrid();
                        return;
                    }
                    else
                    {
                        int val = 0;
                        if (recQuan != recQuanOrg)
                        {
                            if (recQuan < recQuanOrg)
                            {
                                val -= (recQuanOrg - recQuan);
                            }
                            else
                            {
                                val += (recQuan - recQuanOrg);
                            }
                        }
                        if (expQuan != expQuanOrg)
                        {
                            if (expQuan < expQuanOrg)
                            {
                                val -= (expQuanOrg - expQuan);
                            }
                            else
                            {
                                val += (expQuan - expQuanOrg);
                            }

                        }
                        if (defQuan != defQuanOrg)
                        {
                            if (defQuan < defQuanOrg)
                            {
                                val -= (defQuanOrg - defQuan);
                            }
                            else
                            {
                                val += (defQuan - defQuanOrg);
                            }
                        }
                        if (retQuan != retQuanOrg)
                        {
                            if (retQuan < retQuanOrg)
                            {
                                val -= (retQuanOrg - retQuan);
                            }
                            else
                            {
                                val += (retQuan - retQuanOrg);
                            }
                        }
                        remQuan = remQuan - (val);

                    }
                    

                   
                    #endregion

                    #region barcode generation
                    long newBarcode = 0;
                    if (lblBarcode.Equals("0")|| (!expiryDate.Equals(expiryOrg)))
                    {
                        if (!string.IsNullOrEmpty(lblexpiry))
                        {
                            DateTime dateValue = (Convert.ToDateTime(lblexpiry));

                            string p1;
                            String mm;//= dateValue.Month.ToString();
                            if (dateValue.Month < 10)
                            {
                                mm = dateValue.Month.ToString().PadLeft(2, '0');

                            }
                            else
                            {
                                mm = dateValue.Month.ToString();
                            }
                            String yy = dateValue.ToString("yy", DateTimeFormatInfo.InvariantInfo);
                            p1 = lblBarSerial.Text + mm + yy;

                            if (long.TryParse(p1, out newBarcode))
                            {
                            }
                            else
                            {
                                //post error message 
                            }
                        }
                    } 
                    #endregion

                    if (orderedQuantity >= (recQuan + expQuan + defQuan + retQuan))
                    {
                        #region Query execution
                        connection.Open();
                        SqlCommand command = new SqlCommand("Sp_StockReceiving", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
                        command.Parameters.AddWithValue("@p_ReceivedQuantity", recQuan);
                        command.Parameters.AddWithValue("@p_ReceivedQuantityOrg", recQuanOrg);
                        command.Parameters.AddWithValue("@p_ExpiredQuantity", expQuan);
                        command.Parameters.AddWithValue("@p_ExpiredQuantityOrg", expQuanOrg);
                        command.Parameters.AddWithValue("@p_DefectedQuantity", defQuan);
                        command.Parameters.AddWithValue("@p_DefectedQuantityOrg", defQuanOrg);
                        command.Parameters.AddWithValue("@p_ReturnedQuantity", retQuan);
                        command.Parameters.AddWithValue("@p_ReturnedQuantityOrg", retQuanOrg);
                        command.Parameters.AddWithValue("@p_ProductID", int.Parse(lblProdID.Text));
                        command.Parameters.AddWithValue("@p_BarCode", newBarcode);
                        command.Parameters.AddWithValue("@p_DiscountPercentage", txtDisc);
                        command.Parameters.AddWithValue("@p_Bonus", bonusQuan);
                        command.Parameters.AddWithValue("@p_bonusOriginal", bonusOrg);
                        if (!String.IsNullOrEmpty(lblBatch))
                        {
                            command.Parameters.AddWithValue("@p_BatchNumber", lblBatch);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_BatchNumber", DBNull.Value);
                        }
                        if (string.IsNullOrEmpty(lblexpiry))
                        {
                            command.Parameters.AddWithValue("@p_Expiry", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_Expiry", expiryDate);
                        }
                        command.Parameters.AddWithValue("@p_Cost", txtCP);
                        command.Parameters.AddWithValue("@p_Sales", txtSP);

                        command.Parameters.AddWithValue("@p_BonusTotal", bonusQuan + (bonusTotal - bonusOrg));// total bonus added
                        command.Parameters.AddWithValue("@p_RemainingQuantity", remQuan);
                        command.Parameters.AddWithValue("@p_SystemType", Session["RequestDesRole"].ToString());
                        command.Parameters.AddWithValue("@p_StoreID", Session["UserSys"].ToString());
                        command.Parameters.AddWithValue("@p_orderMasterID", int.Parse(lblOMISD.Text));
                        command.Parameters.AddWithValue("@p_isInternal", "TRUE");
                        command.Parameters.AddWithValue("@p_isPO", lblPO.Text);
                        command.Parameters.AddWithValue("@p_entryID", entryID);

                        if (!string.IsNullOrWhiteSpace(lblExpOrg))
                        {
                            command.Parameters.AddWithValue("@p_expiryOriginal", expiryOrg);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_expiryOriginal", DBNull.Value);
                        }

                        if (int.Parse(OrdQuantity.Text) > recQuan)
                        {

                            command.Parameters.AddWithValue("@p_comments", "Sent to Vendor");

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_comments", "Completed");
                        }
                        command.ExecuteNonQuery();
                        #endregion
                    }
                    WebMessageBoxUtil.Show("Stock Successfully Updated");
                   // RemQuantity.Text = remQuan.ToString();
                   // bonusQuanOrg.Text = (bonusQuan + (bonusTotal - bonusOrg)).ToString();
                   // retQuantity.Text = retQuan.ToString();

                }
            }
            catch (Exception exp) { }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                StockDisplayGrid.EditIndex = -1;
                BindLabels(false);
                BindGrid();

            }
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            BindGrid();
        }
 
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AcceptPurchaseOrder.aspx", false);
        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try 
            {

                int recQuan, expQuan, defQuan, bonusQuan, retQuan;
                int entryID = int.Parse(((Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("lblentryID")).Text);
                int orderDetID = int.Parse(((Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("lblOrdDet_id")).Text);
                string lblExpOrg = ((Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("lblExpOrg")).Text;
                string lblRec = ((Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("lblRecQuan")).Text;
                string lblExp = ((Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("lblExpQuan")).Text;
                string lblDef = ((Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("lblDefQuan")).Text;
                string lblRet = ((Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("lblRetQuan")).Text;
                string lblBonus = ((Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("lblBonus")).Text;
                int orderedQuantity = int.Parse(OrdQuantity.Text); 
                    
                

                #region Parsing fields

                DateTime expiryOrg = new DateTime();
                if (!string.IsNullOrEmpty(lblExpOrg))
                {
                    DateTime.TryParse(lblExpOrg, out expiryOrg);
                }
                
                int.TryParse(lblRec, out recQuan);
                int.TryParse(lblBonus, out bonusQuan);
                int.TryParse(lblExp, out expQuan);
                int.TryParse(lblDef, out defQuan);
                int.TryParse(lblRet, out retQuan);
                int remainingValue  = int.Parse(RemQuantity.Text);
                
                #endregion
                
                #region Query Execution


                connection.Open();
                SqlCommand command = new SqlCommand("Sp_DeleteOrderDetail_ReceiveEntry", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_entryID", entryID);
                command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
                
                command.Parameters.AddWithValue("@p_ProductID", int.Parse(lblProdID.Text));
                command.Parameters.AddWithValue("@p_StoreID", Session["UserSys"].ToString());
                command.Parameters.AddWithValue("@p_ReceivedQuantity", recQuan);
                command.Parameters.AddWithValue("@p_orderMasterID", int.Parse(lblOMISD.Text));
                if (!string.IsNullOrWhiteSpace(lblExpOrg))
                {
                    command.Parameters.AddWithValue("@p_expiryOriginal", expiryOrg);
                }
                else
                {
                    command.Parameters.AddWithValue("@p_expiryOriginal", DBNull.Value);
                }
                command.Parameters.AddWithValue("@p_ExpiredQuantity", expQuan);
                command.Parameters.AddWithValue("@p_DefectedQuantity", defQuan);
                command.Parameters.AddWithValue("@p_ReturnedQuantity", retQuan);
                command.Parameters.AddWithValue("@p_Bonus", bonusQuan);
                if ((remainingValue + (recQuan + retQuan + defQuan + expQuan)) == orderedQuantity)
                {
                    command.Parameters.AddWithValue("@p_status", "Pending");
                }
                else if ((remainingValue + (recQuan + retQuan + defQuan + expQuan)) < orderedQuantity)
                {
                    command.Parameters.AddWithValue("@p_status", "Partial");
                }
               
                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Entry Successfully Deleted ");
                #endregion
            }
            catch(Exception exp){}
            finally{
                if(connection.State== ConnectionState.Open)
                {
                    connection.Close();
                }
                BindLabels(false);
                BindGrid();
            }
        }
    }
}