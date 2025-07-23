using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edocs.WorkFlow.Archiver.InterFaces
{
   public interface IWFUsers
    {
          int ID
        { get;  }
          string FName
        { get; }
          string LName
        { get; }
    }
    public interface  IEmployee
    {
          DateTime? EmpStDate
        {
            get;
            set;
        }
          string EmpFName
        {
            get;
            set;
        }
          string EmpLName
        {
            get;
            set;
        }
          string EmpAddress
        {
            get;
            set;
        }
          string EmpCity
        {
            get;
            set;
        }


         string EmpState
        {
            get;
            set;
        }
          string EmpZip
        {
            get;
            set;
        }
          string EmpHomePhone
        {
            get;
            set;
        }
          string EmpCellPhone
        {
            get;
            set;
        }
          string EmpPay
        {
            get;
            set;
        }
          string Comments
        {
            get;
            set;
        }
        string EmpEmail
        {
            get;
            set;
        }
        string EmpSSN
        {
            get;
            set;
        }
        string EmpID
        {
            get;
            set;
        }
        string PosApply
        {
            get;
            set;
        }
    }
    public interface IWFUsersView
    {
        int EmployeeID
        { get; set; }
    }
}


