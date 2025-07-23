// JavaScript source code
$(document).ready(function () {
    $('#cusID').change(function (e) {
        //if (!(CheckEmptyString($("#repSDate").val(), 'Need a Report Begin Date:'))) {
        //    $("#repSDate").focus();
        //    return false;
        //}
        //if (!(CheckEmptyString($("#repSEndDate").val(), 'Need a Report Begin Date:'))) {
        //    $("#repSEndDate").focus();
        //    return false;
        //}
        $("#divLoading").show();
        //  ShowLoading();
        var getInvoiceCust = "/Invoice/CreateCustomInvoiceView?&custID=" + $("#cusID").val();
        window.open(getInvoiceCust, '_self');


        // $("#formCustId").submit();

    });
    $("body").on("click", "#btnAdd", function () {
        //Reference the Name and Country TextBoxes.
     
        try {

            var txtDateofService = $("#txtDateofDervice");
            var txtItemQuantity = $("#txtItemQuantity");
            var txtItemCost = $("#txtItemCost");
            var txtItemDescription = $("#txtItemDescription");
             
            if (txtDateofService.val().length == 0) {
                alert("Need to have date of service");
                $("#txtDateofDervice").focus();
                return;
            }
            
            if (txtItemQuantity.val().trim().length === 0) {
                alert("Need Item Quantity");
                $("#txtItemQuantity").focus();
                return;
            }
            if (txtItemCost.val().trim().length === 0) {
                alert("Need Item Cost");
                $("#txtItemCost").focus();
                return;
            }
           

            if (txtItemDescription.val().trim().length === 0) {
                alert("Need Item Description");
                $("#txtItemDescription").focus();
                return;
            }
           
            // alert(txtItemCost.val());
            var txtItemTotal = MultiFloat(txtItemCost.val(), txtItemQuantity.val());
            if (txtItemTotal == "NaN") {
                alert("Invalid entry");
                return;
            }
           // alert(txtItemTotal);
          //    var tCost = $("#totalCost").val();
            // tCost = FloatToTwoDecmials(tCost, 2);
              
         //   tCost = tCost + txtItemTotal;
          //  alert(tCost.fixed(2));
          //  document.getElementById("totalCost").value = tCost.fixed(2);
                //$("#totalCost").val();
          //  alert(tCost);
            //   $("#totalCost").text("");
            //   $("#totalCost").text(tCost);
            //   alert(tCost);
            //   alert($("#totalCost").val());
            // $("#totalCost").val() = tCost.toFixed(2);
            // alert(txtItemTotal );
            //Get the reference of the Table's TBODY element.
            //    document.getElementById("totalCost").innerHTML = FloatToTwoDecmials(tCost, 2);
             
            var tBody = $("#tblCustomers > TBODY")[0];

            //Add Row.
            var row = tBody.insertRow(-1);

            //Add Name cell.
            var cell = $(row.insertCell(-1));
            cell.html(txtDateofService.val());

            //Add Country cell.
            cell = $(row.insertCell(-1));
            cell.html(txtItemQuantity.val());

            cell = $(row.insertCell(-1));
            cell.html(FloatToTwoDecmials(txtItemCost.val(), 2));

            cell = $(row.insertCell(-1));
            cell.html(txtItemDescription.val());
            

            cell = $(row.insertCell(-1));
            cell.html(txtItemTotal.toFixed(2));

            //Add Button cell.
            cell = $(row.insertCell(-1));
            var btnRemove = $("<input />");
            btnRemove.attr("type", "button");
            btnRemove.attr("onclick", "Remove(this);");
            btnRemove.val("Remove");
            cell.append(btnRemove);

            //Clear the TextBoxes.
            txtItemDescription.val("");
            txtItemQuantity.val("");
            txtItemCost.val("");
            
            var strData = "float1=" + $("#totalCost").val().toString() + "&float2=" + txtItemTotal.toString();
            //   alert(strData);
            $("#txtItemTotal").val("");
            var urlPost = "/Invoice/CreateCustomInvoiceView?handler=GetTotalValue"
            $.ajax({
                type: "GET",
                //  url: "/Invoice/CreateCustomInvoiceView?handler=InsertCustomInvoice",
                url: urlPost,
                data: strData,
                //  data: customers.ser,
              //  contentType: 'application/html; charset=utf-8',
                 contentType: "application/json; charset=utf-8",
                dataType: "html",
                 beforeSend: function (xhr) {
                  xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                success: function (r) {
                    var retStr = ReplaceStr(r);
                    $("#totalCost").val(retStr);
                    //alert(r + " record(s) inserted.");
                },
                error: function (request, status, error) {
                 //   $("#divLoading").hide();
                    alert("error");
                    alert(error);
                    alert(request.getAllResponseHeaders());
                    alert(status.show);
                }
            });
        }
        catch (err) {
            alert(err);
        }

    });

    $("body").on("click", "#btnSave", function () {
        //    //    //Loop through the Table rows and build a JSON array.

        //$("#tblCustomers TBODY TR").each(function () {
        //       var row = $(this);


        //     alert(row.find("TD").eq(0).html());
        //     alert(row.find("TD").eq(1).html());
        //     alert(row.find("TD").eq(2).html());
        //      alert(row.find("TD").eq(3).html());
        //     alert(row.find("TD").eq(4).html());
        //  });
        $("#divLoading").show();
        var customers = new Array();
        
        $("#tblCustomers TBODY TR").each(function () {
            var row = $(this);
            //  var customer = { customers: { EdocsCustomerID: $("#idCust").val(), ItemDescription: row.find("TD").eq(0).html(), ItemQuantity: row.find("TD").eq(1).html(), ItemCost: row.find("TD").eq(2).html(), ItemTotal: row.find("TD").eq(4).html(), DateofService: row.find("TD").eq(3).html() } };
          //  var customer = { EdocsCustomerID: $("#idCust").val(), DateofService: row.find("TD").eq(3).html()  , ItemQuantity: row.find("TD").eq(1).html(), ItemCost: row.find("TD").eq(2).html(), ItemTotal: row.find("TD").eq(4).html(), ItemDescription: row.find("TD").eq(0).html()};
            var customer = {EdocsCustomerID: $("#idCust").val(),DateofService: row.find("TD").eq(0).html(), ItemQuantity: row.find("TD").eq(1).html(), ItemCost: row.find("TD").eq(2).html(), ItemTotal: row.find("TD").eq(4).html(), ItemDescription: row.find("TD").eq(3).html() };


            //customers. = ;
            //customers. = row.find("TD").eq(1).html();
            //customers.ItemCost = row.find("TD").eq(2).html();
            //customers.DateofService = row.find("TD").eq(3).html();
            //customers.ItemTotal = row.find("TD").eq(4).html();
            //   customers.ItemCost =   alert(row.find("TD").eq(3).html());
            //   alert(row.find("TD").eq(4).html());
            //  //  customer.ItemQuantity = row.find("TD").eq(1).html() + '-' + $("#idCust").val();
            //    // customer.ItemCost =  = row.find("TD").eq(2).html();
            //    //  customer.DateofService = row.find("TD").eq(3).html();
            //    // customer.ItemTotal = row.find("TD").eq(4).html();
            customers.push(customer);
        });
         

        //     var customers = new Array();
        //    $("#tblCustomers TBODY TR").each(function () {
        //        var row = $(this);
        //        var customer = {};

        //        customer.ItemDescription = row.find("TD").eq(0).html();


        //        customers.push(customer);
        //    });
        // alert(JSON.stringify(customers));
        // return;
        //    //    //Send the JSON array to Controller using AJAX.
        $.ajax({
            type: "POST",
            //  url: "/Invoice/CreateCustomInvoiceView?handler=InsertCustomInvoice",
            url: "/Invoice/CreateCustomInvoiceView",
            data: JSON.stringify(customers),
            //  data: customers.ser,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (r) {
                $("#divLoading").hide();
                var getInvoiceCust = "/Invoice/RePrintInvoiceView?&cusID=" + $("#idCust").val().trim() + "&invNum=" + r;
               //  alert(getInvoiceCust);
                window.open(getInvoiceCust, '_self');
               // alert(r + " record(s) inserted.");
              //  var openWindow
              //  window.open()
            },
             error: function (request, status, error) {
                  $("#divLoading").hide();
                alert("error "+error);
            
                alert(request.getAllResponseHeaders());
                alert(status.show);
            }
        });
    });

});