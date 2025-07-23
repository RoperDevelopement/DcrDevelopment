// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
 
function openCity(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}
function OpenWindow(wName) {
    window.open(wName);
}
function ShowHideByStyle(name) {
    var tabcontent;
    try {
        alert(document.getElementById("register").style.display);
        tabcontent = document.getElementById("register").style.display;
        tabcontent.style.display = "normal";
        alert(tabcontent.style.display);
        // alert(document.getElementById(name).style.display);
        if (document.getElementById("register").style.display == 'none') {
            alert("set to normal")
            document.getElementById("register").style.display = "normal";
            alert(document.getElementById("register").style.display);
        }
        else {
            alert("set to none")
            document.getElementById(name).style.display = "none";
        }

        document.getElementById("register").style.display = 'normal';
        
    }
    catch (err) {
        alert(err)
        alert(err.message);
    }

}
function ChangeColor(btn) {

    document.getElementById("currentBtn").value = btn;
    document.getElementById("btnChangeColor").click();
    

}
function colorToHex(color) {
    if (color.substr(0, 1) === '#') {
        return color;
    }
    var digits = /(.*?)rgb\((\d+), (\d+), (\d+)\)/.exec(color);

    var red = parseInt(digits[2]);
    var green = parseInt(digits[3]);
    var blue = parseInt(digits[4]);

    var rgb = blue | (green << 8) | (red << 16);
    return digits[1] + '#' + rgb.toString(16);
};

function CheckChanges() {

    var foundChanges = false;
    try {
        
        document.getElementById("invalidTime").style = "none";
        if (document.getElementById("newColor").value.length > 0) {
            foundChanges = true;
        }
       
        if (document.getElementById("CategoryCheckPointModel_Duration").value != document.getElementById("currentDuration").value) {
        
           // var regTime = '/^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/';
            //var dur = document.getElementById("CategoryCheckPointModel_Duration").value;
            //if (!dur.match(regTime)) {
              //  document.getElementById("invalidTime").style = "inline-block";
               // return false;
           // }
          foundChanges = true;
        }
        var checked = document.getElementById("CategoryCheckPointModel_Flash");
        var notCHecked = "False";
        if (checked.checked) {
            notCHecked = "True";
        }

        if (notCHecked.toLowerCase() != document.getElementById("currentFlash").value.toLowerCase()) {
            foundChanges = true;
        }
        checked = document.getElementById("CategoryCheckPointModel_EmailAlerts");
        notCHecked = "False";
        if (checked.checked) {
            notCHecked = "True";
        }
        if (notCHecked.toLowerCase() != document.getElementById("currentEmailAlerts").value.toLowerCase()) {
            foundChanges = true;
        }
       
        if (document.getElementById("selemail").value.length > 5) {
            foundChanges = true;
            //if (document.getElementById("emTo")
        }
        //    foundChanges = true;
        //}
        //var allElements = document.querySelectorAll('*[id]');
        //var allIds = [];
        //for (var i = 0;i< allElements.length;  ++i) {
          //  alert(allElements[i].id);
       // }
        if (!(foundChanges)) {
            document.getElementById("noChangesFound").style = "inline-block";
        }
        document.getElementById("invalidTime").style = "none";
        }
    
    catch (err) {
        alert(err);
    }


    return foundChanges;

        
}

function MoveProgressBar(barName) {
    var elem = document.getElementById(barName);
    var width = 1;
    var id = setInterval(frame, 10);
    function frame() {
        alert("frame");
        if (width >= 100) {
            clearInterval(id);
        } else {
            width++;
            elem.style.width = width + '%';
        }
    }
}
function ShowHideLoding(idName) {

    var sh = document.getElementById(idName);
    
    if (sh.style.display == 'block') {
        sh.style.display = "none";
    }
        else {
        sh.style.display = "block";
    
    }
    

}
function CheckForId(idName) {
    try {
        var idValid = document.getElementById(idName).value;
        
        return true;
    }
    catch (err) {
        return false;
    }
}
//function ChangeColor(btn) {
    
    
  //  document.getElementById("currentBtn").value = btn;
  //  document.getElementById("btnChangeColor").click();
//}
function ShowLoadingDialog(parMessage) {
    alert(parMessage);
    $("#parBmUpScreen").text(parMessage);
    $("#divLoading").show();


}


function CompleteAjaxCall() {
    $("#divLoading").hide();
 
}


//$(function () {
//    $("#dialogSessionExp").dialog({
//        autoOpen: false,
//        modal: true,
//        title: "Session Expiring",
//        buttons: {
//            Ok: function () {
//                ResetSession();
//            },
//            Close: function () {
//                $(this).dialog('close');
//            }
//        }
//    });
//});
function SessionExpireAlert(timeout) {
    
    
    if (timeout == undefined) {
        timeout = ‭120000‬;
    }
    alert(timeout);
    var seconds = timeout / 1000;
    //document.getElementById("headingSessionExp").style.display = 'none';
   
  
    
    setInterval(function () {
        seconds--;
  
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
        
        window.location = window.location.href;
    }, timeout);
};
 