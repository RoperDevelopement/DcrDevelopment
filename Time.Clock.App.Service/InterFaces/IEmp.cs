using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.Employees.Time.Clock.App.Service.InterFaces
{
    interface IEmp
    {

        string EmpAddress { get; set; }
        string EmpCity { get; set; }
        string EmpState { get; set; }

        string EmpCellPhone { get; set; }
        string EmpHomePhone { get; set; }
        float EmpPayRate { get; set; }
        float EmpHolidayPayRate { get; set; }
        float FedTaxesPercent { get; set; }
        float StateTaxPercent { get; set; }
        DateTime EmpStartDate { get; set; }
        DateTime EmpTerminationDate { get; set; }
        string EmpZipCode { get; set; }
        string Comments { get; set; }
    }
    public interface IEmpClockTimeWorkWeekReport
    {

        DateTime TimeClockWorkWeekStartDate { get; set; }
        DateTime TimeClockWorkWeekEndDate { get; set; }
    }
    public interface IEmpName
    {
        string EmpFirstName
        { get; set; }
        string EmpLastName { get; set; }
    }
    public interface IEmpID
    {
        string EmpID
        { get; set; }
    }
    public interface IEmpPW
    {
        string EmpPW { get; set; }
    }
    public interface IEmpEmail
    {
        string EmpEmailAddress { get; set; }
    }
    public interface IEmpAdmin
    {
        bool EdocsAdmin { get; set; }
    }
    public interface IEmpActive
    {
        bool EmpActive { get; set; }
    }
    public interface IRecordID
    {
        int ID { get; set; }
    }
    public interface ITimeClockDurHours
    {
        string TimeSpanDur { get; set; }
    }
    public interface IEmpClockInOut
    {

        DateTime ClockInTime { get; set; }
        DateTime ClockOutTime { get; set; }
    }
}
