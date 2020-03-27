var jiraCount = 0;
var AzureDevOpsCount = 0;
$(function () {
    var uri = '../Home/LoadData';
    partialGet(uri);
});
$(document).on('click', '#btnLogin', function () {
    $(this).addClass('disabled');
    $('#diLoginLoader').removeClass('d-none');
    $(this).target = "_self";
    getCurrentStatus();
    window.open($(this).prop('href'));
    return false;
});


function getCurrentStatus() {
    jiraCount++;
    var loginId = $('#hdnStatus').attr('loginId');
    var uri = '../Home/GetCurrentStatus?loginId=' + loginId;
    $.ajax({
        type: "Get",
        url: uri,
        context: document.body,
        success: function (result) {
            if (result.Status === 'processing') {
                window.setTimeout('getCurrentStatus()', 5000);
                return false;
            } else
                if (result.Status === 'success') {
                    var uri = '../Home/LoadData';
                    partialGet(uri);
                    return false;
                }
                else
                    if (result.Status === 'failed') {
                        return false;
                    }
        },
        error: function (errorResponse) {
            alert('statusUpdatedError');
        }
    });
}

function partialGet(uri) {
    $.ajax({
        type: "GET",
        dataType: 'html',
        contentType: 'application/html; charset=utf-8',
        url: uri,
        success: function (result) {
            $('#dvLoadLoginData').html(result);
        },
        error: function (errorResponse) {
            alert("Error");
        }
    });
}

$(document).on('click', '#btnContinue', function ()
{ $('#btnContinue').prop('type', 'submit'); }
);
