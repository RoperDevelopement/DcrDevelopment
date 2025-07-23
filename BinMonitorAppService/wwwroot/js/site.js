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
function DisplayBinStatus(binID) {


    $('#srchBinID').val(binID);
    $('#btnSearch').click();



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
    $("#parBmUpScreen").text(parMessage);
    $("#divLoading").show();


}

function CloseDialog(dialogName) {
    document.getElementById(dialogName).style.display = 'none';
    $('#usequickkeys').val('yes');
}

function CompleteAjaxCall() {

    $("#divLoading").hide();
    
    
 
}



function SessionExpireAlert(timeout) {
    
    if (timeout == 0) {
        timeout = 3600000;
        
    }

    var seconds = Math.round(timeout / 1000);
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
        //window.location = "/Index";
    }, timeout);
};


function ResetSession() {
    //Redirect to refresh Session.
    window.location = window.location.href;
};


function SubmitLabRec(timeout) {

     
    if (timeout == 0) {
        timeout = 4000;

    }

     var seconds = timeout / 1000;


    setInterval(function () {
        seconds--;
    }, 1000);

    setTimeout(function () {
        //window.location =   window.location.href;
        if ($('#BinRegistorModel_LabRecNumber').val().length >=9) {

        
            $('#subcrbatch').click();
        }
    }, timeout);
};

function CheckTransToFer(tranCat) {
    
    for (i = 0; i < fTransCat.selNewCategoryName.options.length; i++) {
        if (fTransCat.selNewCategoryName.options[i].selected) {
            var catName = fTransCat.selNewCategoryName.options[i].value;
            var newCatName = catName.split('-');
          
            if (newCatName[1] == tranCat) {
                 
                document.getElementById('parseloldcat').innerHTML = "New category name " + tranCat + " cannot be the same as old category name " + newCatName[1];;
               // getElementById('parseloldcat').innerText = 
              //  getElementById('parseloldcat').style.display = 'block';
              //  alert(getElementById('parseloldcat').html);
                return false;
            }
            
        }
    }
    return true;
};

function CheckTransferLabReqs(binID) {
    
    for (i = 0; i < fTransLabReqs.selOldLRN.options.length; i++) {
        if (fTransLabReqs.selOldLRN.options[i].selected) {
            var binName = fTransLabReqs.selOldLRN.options[i].value;
            var newBinName = binName.split('-');
            if (newBinName[0] == binID) {
                document.getElementById('parseloldlr').innerHTML = "LabReq " + newBinName[1] + " found in binid " + binID;
                return false;
            }

        }
    }
    return true;
};
function RenewSession(pvUrl) {
     
    $.ajax({
        type: "GET",
        url: pvUrl,
        contentType: 'application/html; charset=utf-8',
        dataType: 'html',
        success: function (result) {
            //   alert(result);
    
        },
        complete: function () {
        },
        error: function (request, status, error) {
            alert("Error status " + status + " error " + error + " request " + request);
        }
    });
};
function DelLabReq(labReqNum) {
    
    var answer = confirm("Delete LabReq Number: " + labReqNum);

    if (answer) {
        $("#divLoading").show();
        return true;
    }

    return false;
};

function GetDate(id, minusdate) {
    // body...
    var today = new Date();


    var dd = today.getDate();
    if (minusdate > 0)
        dd = today.getDate() - minusdate;
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }

    today = yyyy + '-' + mm + '-' + dd;
    document.getElementById(id).defaultValue = today + "";
    // document.getElementById("eDate").defaultValue = today + "";

};