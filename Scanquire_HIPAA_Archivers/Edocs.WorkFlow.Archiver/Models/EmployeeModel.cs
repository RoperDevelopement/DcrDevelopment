using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edocs.WorkFlow.Archiver.InterFaces;
namespace Edocs.WorkFlow.Archiver.Models
{
    class EmployeeModel : IEmployee
    {
        public DateTime? EmpStDate
        {
            get;
            set;
        }
        public string EmpFName
        {
            get;
            set;
        }
        public string EmpLName
        {
            get;
            set;
        }
        public string EmpAddress
        {
            get;
            set;
        }
        public string EmpCity
        {
            get;
            set;
        }


        public string EmpState
        {
            get;
            set;
        }
        public string EmpZip
        {
            get;
            set;
        }
        public string EmpHomePhone
        {
            get;
            set;
        }
        public string EmpCellPhone
        {
            get;
            set;
        }
        public string EmpPay
        {
            get;
            set;
        }
        public string Comments
        {
            get;
            set;
        }
       public string EmpEmail
        {
            get;
            set;
        }
       public string EmpSSN
        {
            get;
            set;
        }
       public string EmpID
        {
            get;
            set;
        }
        public string PosApply
        {
            get;
            set;
        }
    }
}
