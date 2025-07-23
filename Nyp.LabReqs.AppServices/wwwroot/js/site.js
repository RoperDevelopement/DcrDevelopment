// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function ShowHideDialog(dialogName) {
     
    var doc = document.getElementById(dialogName);
   
    if (doc.style.display == "none") {
        doc.style.display = "block";
    }
    else {
        doc.style.display = "none";
    }
   
}
function BtnClick(btnClick) {
    
    document.getElementById(btnClick).click();
}

function GetCheckBoxValues() {
    $('[id="cbSearchPartial"]').each(function () {
        if ($(this).prop('checked') == true) {
            $('#partsearch').val("searchpart");
        }
       

    });
}

function SetValueUncehckCheckBoxValues() {
    $('[id="cbSearchPartial"]').each(function () {
        if ($(this).prop('checked') == true) {
            $(this).prop('checked', false)    
            $('#partsearch').val("");
        }
        


    });
   
}
function CompleteAjaxCall() {
    $("#divLoading").hide();
    // $("#sOptions").click();
    if ($("#dtLabReqs").length) {
        $('#dtLabReqs').DataTable({ "pagingType": "full" });
        $('.dataTables_length').addClass('bs-select');
    }
}

function GetSearchStartEndDate() {

    
    if ($('#searchEndDate').val() == '') {

        var currentdate = new Date();
        var datetime = currentdate.getFullYear() + "-" + (currentdate.getMonth() + 1).toString().padStart(2, '0') + "-" + currentdate.getDate().toString().padStart(2, '0');
        $('#searchEndDate').val(datetime);
     //   $('#checkboxSearchDates').css("display", "block");
        //if ($('#searchStartDate').val() > $('#searchEndDate').val()) {
        //    $('#searchEndDate').val($('#searchStartDate').val());
        //}
    }
    //else {
    //    if ($('#searchStartDate').val() > $('#searchEndDate').val()) {
             
    //        $('#searchStartDate').val($('#searchEndDate').val());
    //    }
    //}

    
}

function CheckSearchStartDate() {
    
    if ($('#searchStartDate').val() == '') {
        //var currentdate = new Date();
        //var datetime = currentdate.getFullYear() + "-" + (currentdate.getMonth() + 1) + "-" + (currentdate.getDate()+1);
        $('#searchStartDate').val($('#searchEndDate').val());
        if ($('#checkboxSearchDates').length) {
            $('#checkboxSearchDates').css("display", "block");
        }
        
       
    }
    //else {
    //    if ($('#searchEndDate').val() < $('#searchStartDate').val()) {
    //        $('#searchStartDate').val($('#searchEndDate').val());
            
    //    }

    //}

}

function GetLRSearchStartEndDate() {

    if ($('#scanDate').val() == '') {
        var currentdate = new Date();
        var datetime = currentdate.getFullYear() + "-" + (currentdate.getMonth() + 1).toString().padStart(2, '0') + "-" + currentdate.getDate().toString().padStart(2, '0');
        $('#scanDate').val(datetime);
        
    }
    //else {
    //    if ($('#receiptDate').val() > $('#scanDate').val()) {
    //        $('#receiptDate').val($('#scanDate').val());
    //    }
    //}


}

function CheckLRSearchStartDate() {

    if ($('#receiptDate').val() == '') {
        //var currentdate = new Date();
        //var datetime = currentdate.getFullYear() + "-" + (currentdate.getMonth() + 1) + "-" + (currentdate.getDate()+1);
        $('#receiptDate').val($('#scanDate').val());
    }
    //else {
    //    if ($('#scanDate').val() < $('#receiptDate').val()) {
    //        $('#receiptDate').val($('#scanDate').val());
    //    }

    //}

}

function ShowLoadingDialog(message) {
    
    $("#parLoading").text(message);
    $("#divLoading").show();
    


}

function CheckEmptyStr(str1, str2) {
   
    if (typeof (str2) !== 'undefined') {
        if ((str1 == "") && (str2 == "")) {
            return true;
        }
    }
    else {
        if (str1 == "") {
            return true;
        }
    }

    return false;

}

function SessionExpireAlert(timeout) {

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
        window.location =   window.location.href;
       // window.location = "/Index";
    }, timeout);
};
function ResetSession() {
    //Redirect to refresh Session.
    window.location = window.location.href;
};