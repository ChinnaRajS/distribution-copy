$('#btnCreateTemplate').on('click', function () {
    
    var subscriptionId = $('#ddlSubscription option:selected').attr('subscription-id');
    var accessToken = $('#hdnAccessToken').val();
    var selectedTempleteId = $('#hdnTemplateId').val();

    var templateModel = {};
    templateModel['AccessToken'] = accessToken;
    templateModel['SelectedTemplateId'] = selectedTempleteId;
    templateModel['SubsriptionId'] = subscriptionId;
    var templateDetails = JSON.stringify(templateModel);

    $("#dvProcessTemplate").removeClass('d-none');
    //$.post("/Environment/StartTemplateCreateProcess", templateDetails, function (data) {
    //    //alert(data);
    //    GetCreationStatus(0);
    //    //if (data !== "True") {
    //    //    var x = "";
    //    //}
    //});

    //var websiteUrl = window.location.href;
    $.ajax({
        type: "POST",
        //dataType: "json",
        data: templateDetails,
        contentType: 'application/json; charset=utf-8',
        url: '../Environment/StartTemplateCreateProcess',
        //context: document.body,
        success: function (data) {
            GetCreationStatus();
            //  GetCreationStatus(0);
            //window.setTimeout('GetCreationStatus()', 500);
        },
        error: function (errorResponse) {
            alert(errorResponse);
        }
    });
    //event.preventDefaultz;
});

$('#btnShowDiscription').on('click', function () {
    var isHidden = $(this).attr('aria-expanded');
    if (isHidden === undefined || isHidden === 'true') {
        $('#btnShowDiscription').text('Show Discription');

    } else {
        $('#btnShowDiscription').text('Hide Discription');
    }
});


//function GetCurrentProgress() {

//    $('#dvProcessStstus').load('/Environment/GetCurrentStatus?statusLock=' + 'locked');

//    //$('#hdnProcessStatus').attr('current-status');
//    //var endProcess = $('#hdnProcessStatus').attr('process-end');
//    //if (endProcess === 'yes') {
//    //    return 'completed';
//    //}
//    //if (endProcess === undefined) {
//    //    return '';
//    //} else {
//    //    $('#dvProcessStstus').load('/Environment/GetCurrentStatus?statusLock=' + 'locked');
//    //}


//}


function GetCreationStatus() {
    var currentProgress = $('#hdnStatusIndictor').val();
    alert(currentProgress);
    //  return;
    //var currentProgress=
    $.ajax({
        type: "Get",
        url: '../Environment/GetCurrentStatus',
        context: document.body,
        success: function (result) {
            if (currrentProgress !== 'end') {

                window.setTimeout('GetCreationStatus()', 1000);
            }
            //var currrentProgress = $('#hdnStatusIndictor').val();
            //if (currrentProgress !== 'end') {

            //    alert(currrentProgress);
            //    //processedCount = result.Count;
            //    GetCreationStatus(result.Count);
            //} else {
            //   // $("#dvProcessTemplate").removeClass('d-none');
            //    return;
            //}

        },
        error: function (errorResponse) {
            window.setTimeout('GetCreationStatus()', 1000);

        }
    });

}
