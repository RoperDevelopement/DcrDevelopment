// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//const { Alert } = require("bootstrap");
//const { ajax, data } = require("jquery");
var duration;
// Write your Javascript code.
function SessionExpireAlert(timeout, winLoc) {
     
    if (timeout == 0) {
        timeout = 3600000;
        //  timeout = 120000;

    }

    var seconds = timeout / 1000;
    document.getElementById("headingSessionExp").style.display = 'none';

    setInterval(function () {
        seconds--;
        $("#seconds").html(seconds);
        if (seconds <= 60) {
            document.getElementById("headingSessionExp").style.display = 'block';
            $("#seconds").html(seconds);
        }
        if (seconds <= 0) {
            document.getElementById("headingSessionExp").style.display = 'none';
            timeout = 0;
        }

    }, 1000);

    setTimeout(function () {
      //  window.location = window.location.href;
         //window.location = "/Index";
        window.location = winLoc;
    }, timeout);
};
function CheckMessage(message) {
  
    var windowLoc = "/Index";
    var indexAdm = message.indexOf("Admin");
   
     if (indexAdm > 0) {
         windowLoc = "/E-docs_Employees/TimeClockAdminLoginView";
       // window.open(windowLoc, '_blank');
              }

    //alert(windowLoc);
        $('#empId').attr("style", "display:none");
        
    SessionExpireAlert(1000, windowLoc);
     //alert(s.innerHTML);
};
function ResetSession() {
    //Redirect to refresh Session.
    window.location = window.location.href;
};
function CompleteAjaxCall() {
    $("#divLoading").hide();
     
    // $("#sOptions").click();
    if ($("#dtEmpRep").length) {

         
        $('#dtEmpRep').DataTable({ "pagingType": "full_numbers" });
        $('.dataTables_length').addClass('bs-select');
    }
};
function UpdateTimeCard(id, cInTime, coutTime) {
    var clockIn = document.getElementById(cInTime).value;
    var clockOut = document.getElementById(coutTime).value;
   // var dur = document.getElementById(totDur).value;
     
    var empClockInOutUrl = "/Partial_Pages/TimeCLockHrsView/";
   // if (checkdate(clockIn.value) == false)
  //      return;
  //  if (checkdate(clockOut.value) == false)
    //      return;
    ajaxCall(id, clockIn, clockOut);
    //alert("call");
   
    
    // ert(empClockInOutUrl); var empClockInOutUrl = "https://www.facebook.com/";
    //  alert(empClockInOutUrl);
    // alert($('#empId').val());
    //e.preventDefault();

//    // Creating Our XMLHttpRequest object 
//    var xhr = new XMLHttpRequest();

//    // Making our connection  
//    var url = empClockInOutUrl;
//    xhr.open("POST", url, true);

//    // function execute after request is successful 
//    xhr.onreadystatechange = function () {
//        if (this.readyState == 4 && this.status == 200) {
//            console.log(this.responseText);
//        }
    
//    // Sending our request 
//    xhr.send();
//}
};
function GetNewDur(dur) {
  //  var $str1 = $(dur);//this turns your string into real html
  //var j =  $('#message').text(dur.find('p').text());
    // alert(j);
    var indexP = dur.indexOf("message");
    
    if (indexP > 0) {
         
        indexP = indexP + 7;
        var str = dur.substring(indexP);
         
        indexP = str.indexOf(">")+1;
        
        str = str.substring(indexP);
        indexP = str.indexOf("<");
        
        str = str.substring(0, indexP);
        
        duration = str;
        
    }
};
function ajaxCall(id, timeIn, timeOut) {
    var data1 = "&empId=UPDATE" + "&timeClockStartDate=" + timeIn.replace("/", "-") + "&timeClockEndDate=" + timeOut.replace("/", "-") + "&Id=" + id;
    
    data1 = data1.replace("/", "-");
    data1 = data1.replace("/", "-");
   // alert(data1);
    $.ajax({

        // Our sample url to make request \\
        type: "GET",
        url: '/Partial_Pages/TimeCLockHrsView',
        contentType: 'application/html; charset=utf-8',
        // Type of Request
        datatype: "html",
      //  data: "&Id=" + id + "&timeClockStartDate=" + timeIn.replace("/", "-") + "&timeClockEndDate=" + timeOut.replace("/", "-"),
        data: data1,
        cache: false,
 
        // Function to call when to
        // request is ok 
        success: function (data) {
            
            //  var x = JSON.stringify(data);
            //alert(`suCCESS ${data}`);;
          //  GetNewDur(data);
            // document.getElementById(totDur).value = duration;
            $("#btnGetEmpInHrs").click();
        },
        complete: function () {
            //alert("comp");
            // CompleteAjaxCall();
            // $("#sOptions").click();
            //CheckMessage(result);
            //  alert("done");
        },
        // Error handling 
        error: function (error) {
            alert(`Error ${error}`);
            console.log("Error: ", error);
        }
    });
};
function DelTimeCard(delID) {
    $("#divmain").css("display", "none");;
    $('#delTimeEntrymodal').fadeIn(100);
    $('#btno').click(function (e) {
        
        $("#divmain").fadeIn(100);
        $('#delTimeEntrymodal').fadeOut(100);
        //$("#divLoading").show();
       // $("#formEmpId").submit();
    });
    
    $('#btnyes').click(function (e) {

        $("#divmain").fadeIn(100);
        $('#delTimeEntrymodal').fadeOut(100);
        //$("#divLoading").show();
        // $("#formEmpId").submit();
    
    $.ajax({

        // Our sample url to make request \\
        type: "GET",
        url: '/Partial_Pages/TimeCardAddNewEntryView',
        contentType: 'application/html; charset=utf-8',
        // Type of Request
        datatype: "html",
        //  data: "&Id=" + id + "&timeClockStartDate=" + timeIn.replace("/", "-") + "&timeClockEndDate=" + timeOut.replace("/", "-"),
        data: "&empId=DelRec" + "&timeClockStartDate=01/01/2020" + "&timeClockEndDate=01/01/2020" + "&delID=" + delID,
       // data: "&delID=" + delID,
        cache: false,

        // Function to call when to
        // request is ok 
        success: function (data) {
            //  var x = JSON.stringify(data);
            //alert(`suCCESS ${data}`);;
            //  GetNewDur(data);
            // document.getElementById(totDur).value = duration;
            $("#btnGetEmpInHrs").click();
        },
        complete: function () {
            //alert("comp");
            // CompleteAjaxCall();
            // $("#sOptions").click();
            //CheckMessage(result);
            //  alert("done");
        },
        // Error handling 
        error: function (error) {
            alert(`Error ${error}`);
            console.log("Error: ", error);
        }
    });
    });
};
function checkdate(input) {
    var validformat = /^\d{2}\/\d{2}\/\d{4}$/ //Basic check for format validity
    var returnval = false
    if (!validformat.test(input.value))
        alert("Invalid Date Format. Please correct and submit again.")
    else { //Detailed check for valid date ranges
        var monthfield = input.value.split("/")[0]
        var dayfield = input.value.split("/")[1]
        var yearfield = input.value.split("/")[2]
        var dayobj = new Date(yearfield, monthfield - 1, dayfield)
        if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield))
            alert("Invalid Day, Month, or Year range detected. Please correct and submit again.")
        else
            returnval = true
    }
    if (returnval == false) input.select()
    return returnval
};
function printOut(divId) {
    
    var printOutContent = document.getElementById(divId).innerHTML;
    var originalContent = document.body.innerHTML;
    document.body.innerHTML = printOutContent;
    window.print();
    window.close();
   // document.body.innerHTML = originalContent;
   // newPrint = window.open("");
   // newPrint.document.write(originalContent);
  //  newPrint.print();
   // newPrint.close();
};
function OpenNewWIndow(url,target) {
    //window.open(url, '_blank');
    window.open(url, target);
   // location.reload();
};