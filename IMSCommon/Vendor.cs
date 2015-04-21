using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCommon
{
    public class Vendor
    {
        #region fields
        int Supp_ID;
        string supName,
  Address,
  City,
  state,
  country,
  pincode,
  phone,
  fax,
  mobile,
  pager,
  email,
  conPerson,
  discount,
  credit;

        public string SupName
        {
            get { return supName; }
            set { supName = value; }
        }
        DateTime dateCreated;
        long lineID;
        #endregion

        #region properties

        public string Credit
        {
            get { return credit; }
            set { credit = value; }
        }


        public string Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        public string ConPerson
        {
            get { return conPerson; }
            set { conPerson = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Pager
        {
            get { return pager; }
            set { pager = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Pincode
        {
            get { return pincode; }
            set { pincode = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string city
        {
            get { return City; }
            set { City = value; }
        }

        public string address
        {
            get { return Address; }
            set { Address = value; }
        }



        public int supp_ID
        {
            get { return Supp_ID; }
            set { Supp_ID = value; }
        }


        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }


        public long LineID
        {
            get { return lineID; }
            set { lineID = value; }
        }


        #endregion

    }
}
