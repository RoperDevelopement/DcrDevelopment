// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function LoadPdf(fName) {
     
    document.getElementById('pdfName').value = fName;
    document.getElementById('pdfbutt').click();
    
     

};
function LostFocous() {
     
    var txtItemQuantity = document.getElementById("txtItemQuantity").value;
    var txtItemCost = document.getElementById("txtItemCost").value;

    if ((txtItemCost.value !== "") && (txtItemQuantity.value !== "")) {
       var fCost = ConvertToFloat(txtItemCost);
        var fQuantity = ConvertToFloat(txtItemQuantity);
        
        
        var itCost = MultiFloat(fCost, fQuantity);

         document.getElementById("txtItemTotal").value = ConvertToFloat(itCost);


    }
}
function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    
    
    if (confirm("Do you want to delete: " + name)) {
        //Get the reference of the Table.
        var costItem = ConvertToFloat($("TD", row).eq(4).html());
        var totCoast = $("#totalCost").val();
        var newCost = Math.abs(totCoast - costItem);
        $("#totalCost").val(newCost.toFixed(2));
        var table = $("#tblCustomers")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};
function OpenUrl(url, id) {
    
    if (id !== undefined) {
        document.getElementById(id).style.display = "none";
    }
    
    window.open(url, target ='_self');
}
function IsNumeric(str, totalPaidID) {
   // if (typeof str != "string") return false // we only process strings!  
  //  return !isNaN(str) && // use type coercion to parse the _entirety_ of the string (`parseFloat` alone does not do this)...
   //     !isNaN(parseFloat(str)) // ...and ensure strings of whitespace fail
   /// return !isNaN(str - parseFloat(str));
    for (var i = 0; i < str.length; ++i) {

    
        if ((str[i] > '0') && (str[i] < '9')) {
            continue;

        }

        else {
            if (totalPaidID != undefined) {
            if (str[i] != '.') {
                document.getElementById(totalPaidID).value = "0.00";
                document.getElementById("errorAmount_" + totalPaidID).innerHTML = "Invalid Paying amount entered " + str;
                document.getElementById("errorAmount_" + totalPaidID).style.display = "block";
                document.getElementById(totalPaidID).focus();
            }
                return false;
            }
           
        }
            

    }
    return true;
} 
function CheckOnlyDigits(digits) {
    return !isNaN(digits);
}
function ConvertToFloat(valueFloat ) {

    try {

         
        // Using parseFloat() method
        var floatValue = parseFloat(valueFloat);

        // Return float value
    }
    catch (e) {
        //alert("Invalid amount paid entered " + valueFloat);
        floatValue = -1;
    }
    return floatValue;
}
function MultiFloat(valueFloat,number) {

    try {
        
        // Using parseFloat() method
        var floatValue = parseFloat(valueFloat);

        floatValue = floatValue * number;
      //  alert(floatValue);
        // Return float value
    }
    catch (e) {
        //alert("Invalid amount paid entered " + valueFloat);
        floatValue = -1;
    }
    return floatValue;
}
function FloatToTwoDecmials(float, decPlaces) {
   // alert(parseFloat(float).toFixed(decPlaces));
    return parseFloat(float).toFixed(decPlaces);
}
function AddTwoNumbers(firstNumber, secondNumber) {

    return parseFloat(firstNumber).toFixed(2) + parseFloat(secondNumber).toFixed(2);

}
function ReplaceStr(str,oldChar) {

    var retStr = str.replaceAll("\\", '');
    retStr = retStr.replaceAll(/\"/g,'');
    
  //  alert(retStr);
    return retStr;


}

function GetInvoiceAmount(totalPaidID) {

    try {
       
        if ((totalPaidID == undefined))
            return;
        // alert(totalPaid);
   //     var elementID = document.getElementById(totalPaid);
        var totalP = document.getElementById(totalPaidID).value;
    
        if (totalP.length == 0) {
            document.getElementById("errorAmount_" + totalPaidID).innerHTML = "Need to enter amount paying";
            document.getElementById("errorAmount_" + totalPaidID).style.display = "block";
            document.getElementById(totalPaidID).focus();
            return;
         }

        if (totalP.replaceAll(" ", "") == "") {
             document.getElementById(totalPaidID).value = totalP.replaceAll(" ", "");
             document.getElementById("errorAmount_" + totalPaidID).innerHTML = "Need to enter amount paying";
            document.getElementById("errorAmount_" + totalPaidID).style.display = "block";
            document.getElementById(totalPaidID).focus();

           return;
        }

        if (totalP < 0) {
            document.getElementById(totalPaidID).value = totalP.replaceAll(" ", "");
            document.getElementById("errorAmount_" + totalPaidID).innerHTML = "Amount Paying cannot be negitave " + totalP;
            document.getElementById("errorAmount_" + totalPaidID).style.display = "block";
            document.getElementById(totalPaidID).focus();

            return;
        }
       // var t = CheckOnlyDigits(totalPaidID);
        // alert(t);
        var floatValue = IsNumeric(totalP, totalPaidID);
        
        if (!(floatValue)) {
          
            return;
        

        }
        floatValue = ConvertToFloat(totalP);
        //  var paidTot = IsNumeric(document.getElementById("totalPaidAmount_" + totalPaidID).html);
        var paidTot = ConvertToFloat(document.getElementById("totalPaid").innerHTML);
        var pTot = paidTot;
        //alert(document.getElementById("totalPaid").innerHTML);
        //  document.getElementById("totalPaid").innerHTML = paidTot - floatValue;
       
       
        paidTot = Math.abs(floatValue+ paidTot);
        //   var totalAmountPaid = document.getElementById(totalPaid).innerHTML;
        document.getElementById("totalPaid").innerHTML = paidTot;
        document.getElementById("totalPaidAmount_" + totalPaidID).innerHTML = paidTot;
        paidTot = ConvertToFloat(document.getElementById("totalOwe").innerHTML);
        document.getElementById("totalOwe").innerHTML = Math.abs(paidTot - pTot);
        document.getElementById("owe_" + totalPaidID).innerHTML = Math.abs(paidTot - pTot);
     //   //  var totalAmountPaid = document.getElementById(totalPaid).innerHTML;
     ////   alert(totalP);
     //   // alert(document.getElementById(totalPaid).value);
     //   //alert(totalP);
     //   //if (Number.isInteger(totalp.value))
     //   //    alert("int");
       // var invNumber = elementID.getAttribute("numInv");
      //  alert($('#' + invNumber));
     //   var paidToDay = totalP;
        /////  alert(invNumber);
        //  //alert(invNumber);
        //  //alert(document.getElementById("totalamount_" + invNumber).value);
        //  document.getElementById("totalOwe").innerText = "500";
       // if (document.getElementById("totalPaidAmount_" + invNumber) != undefined) {
       //     paidToDay = parseInt(document.getElementById("totalPaidAmount_" + invNumber).innerHTML) + parseInt(totalP);
        // }
       // var amountTotal = document.getElementById("totalAmount").innerHTML;
      //  alert(amountTotal);
      //  amountTotal = amountTotal + paidToDay;
       // document.getElementById("totalAmount").innerHTML = amountTotal;
        //      alert(document.getElementById(totalPaid).value);
       // alert(document.getElementById("totalPaidAmount_" + invNumber));
        //var paidToDay = document.getElementById("totalPaidAmount_" + invNumber).value + totalP;
          //  document.getElementById("totalPaidAmount_" + invNumber).innerHTML = paidToDay;
      //  }
        //      alert(paidToDay);
        // document.getElementById("totalamountPaid_" + invNumber).innerText = paidToDay;
   
              //var inNum = totalP.ge("data_value1").value;
       // for (int i = 0; i < totalP.getAttributeNames.length; i++) {

      //  }
      //  for (var i = 0; i < totalP.attributes.length; i++) {
       //     var attrib = totalP.attributes[i];
           // if (attrib.specified) {
               // alert(attrib.name + " = " + attrib.value);
           // }
       // }

      //  if (totalP == "")
       
    
          //return;
    }
    catch (e) {
        alert(e);
    }
}
function onlyNumberKey(evt) {
    
    // Only ASCII charactar in that range allowed 
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;
    return true;
}
function GetPDFile(fileNamePDF) {
    alert(fileNamePDF);
}
function CallApi(jString, uri) {
    try {
        alert(jString);
        var xhr = new XMLHttpRequest();
        xhr.open('GET', uri + jString );
        xhr.onload = function () {
            if (xhr.status === 200) {
                alert('Response is ' + xhr.responseText);
                console.log('Response is ' + xhr.responseText);
            }
            else {
                alert('Request failed.  Returned status of ' + xhr.status);
                console.log('Request failed.  Returned status of ' + xhr.status);
            }
        };
        xhr.send();
    
    //fetch(uri, {
    //    method: 'POST',
    //    headers: {
    //        'Accept': 'application/json',
    //        'Content-Type': 'application/json'
    //    },
    //    body: (jString)
    //})
    //    .then(response => response.json())
    //    .then(() => {
    //        getItems();
    //       // addNameTextbox.value = '';
    //    })
    //        .catch(error => console.error('Unable to add item.', error));
    }
    catch (e) {
        alert(e);
    }
}
function JsonString(elementID) {
    try {
         alert(elementID);
        //{ 'num': HTML }
        var html =  document.getElementById(elementID).innerText;
        //var html = document.body.innerHTML;
        //alert(html);
      //   const obj = "[\n " + JSON.stringify(html) + " \n]";
        const obj = "{HtmlData:" + JSON.stringify(html) + "}";
        //const obj = JSON.stringify(html);
      //  const obj = JSON.stringify(html);
        var addHtmlFile = "http://localhost:5555/api/EdocsITSUploadHtmlFiles/";
        CallApi(obj, addHtmlFile);
       // const obj = JSON.stringify(html);
      // const obj = { 'html': JSON.stringify(html) };
        //alert(obj);
        // alert("'" + html + "'");
        return obj;
       // return obj.replace(/^(\w{1,3})\r\n/, "") // remove initial chunks info
       //     .replace(/\r\n(\w{1,3})\r\n/, "") // remove in-body chunks info
       //     .replace(/(\r\n0\r\n\r\n)$/, ""); // remove end chunks info;
    } catch (e) {
        alert(e);
    }
    
}

function CompleteAjaxCall() {
    
    $("#divLoading").hide();
    // $("#sOptions").click();
    $('#dtlUsers').DataTable({ "pagingType": "full" });
    $('.dataTables_length').addClass('bs-select');
}
function printOut(divId) {
    //document.getElementById("id").style.property = "hide"
    var printOutContent = document.getElementById(divId).innerHTML;
    var originalContent = document.body.innerHTML;
    document.body.innerHTML = printOutContent;
   // alert(printOutContent);
    window.print();
    window.close();
    window.open("/index", '_self');
    // document.body.innerHTML = originalContent;
    // newPrint = window.open("");
    // newPrint.document.write(originalContent);
    //  newPrint.print();
    // newPrint.close();
};
function SubmitForm(timeout, formSub, formField, clickFocus) {
    
    if ($('#timerStarted').val() == "0") {
        $('#timerStarted').val("1");
        
    
    
     
    if (timeout == 0) {
        timeout = 2000;

    }

    var seconds = timeout / 1000;


    setInterval(function () {
        seconds--;
    }, 1000);

    setTimeout(function () {
        //window.location =   window.location.href;
        //if ($('#BinRegistorModel_LabRecNumber').val().length > 5) {


        //    $('#subcrbatch').click();
        //}
        if ($(formField).val().length >= 6) {

            if (clickFocus == "click") {
                $(formSub).click();
            }
            else {
                $(formSub).focus();
            }
            
        }
    }, timeout);
}
};
function CheckEmptyString(str,message) {
    if (!str) {
        alert(message)
        return false;
    }
    return true;
}

function run() {
 
    // Creating Our XMLHttpRequest object 
    var xhr = new XMLHttpRequest();

    // Making our connection  
  //  var url = 'http://localhost:2681/Pages/Partial_Pages/UploadImageView/file=' + document.getElementById('postedFiles').value;
    var url = '/EdocsITSCustomers/EdocsITSAddNewCustomers/';
    
    xhr.open("GET", url, true);
    xhr.overrideMimeType('text/plain; charset=x-user-defined');
    // function execute after request is successful 
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            alert(this.responseText);
        }
        else if (xhr.readyState == 4 && xhr.status != 200) {
            alert(xhr.statusText);
        }
    }
    // Sending our request 
    xhr.send();
}
function previewFile(file) {
    try {
        var files = $('#postedFiles').prop("files");
        file = $('#postedFiles').val();
        alert(file);
        let reader = new FileReader();
        reader.readAsBinaryString(file[0]);
        reader.onloadend = function () {
            //let img = document.createElement('img')
            let img = document.getElementById('imgFile');
            img.src = reader.result
            // document.getElementById('gallery').appendChild(img)
        }
    }
    catch (e) {
        alert(e);
    }
}
function SendFormData(dataform) {
    try {
        var files = $('#postedFiles').prop("files");
        var formData = new FormData();
        //  formData.append('file', $('#postedFiles')[0].files[0]); 
        formData.append('MyUploader', files[0]);
        // Creating Our XMLHttpRequest object 
        var xhr = new XMLHttpRequest();

        // Making our connection  
        //  var url = 'http://localhost:2681/Pages/Partial_Pages/UploadImageView/file=' + document.getElementById('postedFiles').value;
      //  var url = "http://localhost:2681/EdocsITSCustomers/ImageView";
        var url =  "/EdocsITSAddNewCustomers?handler=MyUploader";
        xhr.open("post", url, true);
        xhr.setRequestHeader("Content-Type", 'false');
        xhr.setRequestHeader("processData ", 'false');
     //    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        //  xhr.overrideMimeType('text/plain; charset=x-user-defined');
        // function execute after request is successful 
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                alert(this.responseText);
            }
            else if (xhr.readyState == 4 && xhr.status != 200) {
                alert(xhr.statusText);
            }
        }
        // Sending our request 
        xhr.send(formData);
    }
    catch (e) {
        alert(e);
    }
}

//var dataToSend = JSON.stringify({ 'num': HTML });
//$.ajax({
//    url: "EditingTextarea.aspx/GetValue",
//    type: "POST",
//    contentType: "application/json; charset=utf-8",
//    dataType: "json",
//    data: dataToSend, // pass that text to the server as a correct JSON String
//    success: function (msg) { alert(msg.d); },
//    error: function (type) { alert("ERROR!!" + type.responseText); }

//});

//ou can use this

//var HTML = escape($("#t").val());
//and on server end you can decode it to get the html string as

//    HttpUtility.UrlDecode(num, System.Text.Encoding.Default);