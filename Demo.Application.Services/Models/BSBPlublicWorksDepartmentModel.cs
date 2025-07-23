using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.Demo.Application.Services.Models
{
    public class BSBPlublicWorksDepartmentProjectNameModel
    {
        public string ProjectDepartment
        { get; set; }
        public int ProjectYear
        { get; set; }
        public string ProjectName
        { get; set; }
        public string ProdFileName
        { get; set; }
        public string FileUrl
        { get; set; }
    }

    public class BSBPlublicWorksDepartmentYearModel
    {
        public int ProjectYear
        { get; set; }
       
       
    }
    public class BSBPlublicWorksDepartmentModel
    {
        public string ProjectDepartment
        { get; set; }


    }
    public class BSBPlublicWorksProjectNameModel
    {
        public string ProjectName
        { get; set; }


    }
}
