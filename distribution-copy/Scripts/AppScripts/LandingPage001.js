var jiraCount = 0;
var AzureDevOpsCount = 0;
$(function () {
    updateJiraDetails();
    updateADOLoginStatus();

});

$(document).on('click', '#btnJiraLogin', function () {
    $('#btnJiraLogin').addClass('disabled');
    $(this).target = "_self";
    window.open($(this).prop('href'));

    return false;
});

$(document).on('click', '#btnAzureDevOpsLogin', function () {
    $('#btnAzureDevOpsLogin').addClass('disabled');
    $(this).target = "_self";
    window.open($(this).prop('href'));
    return false;
});

function updateJiraDetails() {
    jiraCount++;
    var checkJiraLoginStatus = $(document).get('../JiraAccount/JiraCurrentLoginStatus', '');
    var uri = '../JiraAccount/JiraCurrentLoginStatus';
    $.ajax({
        type: "Get",
        url: uri,
        context: document.body,
        success: function (result) {
            if (result.Status === 'processing') {
                processingJiraHidden();
                window.setTimeout('updateJiraDetails()', 5000);
                return false;
            } else
                if (result.Status === 'success') {
                    $('#btnJiraLogin').text('Jira login success');
                    processedJiraHidden();
                    jiraCount = 0;
                    return false;
                }
                else
                    if (result.Status === 'success') {
                        processedJiraHidden();
                        return updateJiraDetails();
                    }
        },
        error: function (errorResponse) {
            alert('statusUpdatedError');
        }
    });
    // get('../JiraAccount/JiraCurrentLoginStatus', '');
    debugger;

}

function updateJiraHiddenField(addClass, removeClass) {
    $('#hdnJiraLoginClick').removeClass(removeClass);
    $('#hdnJiraLoginClick').add(addClass);

}

function resetJiraHidden() {
    var jiraHdnClass = $('#hdnJiraLoginClick');
    if (jiraHdnClass.hasClass('processing')) {
        jiraHdnClass.removeClass('processing');
    }
    else if (jiraHdnClass.hasClass('processed')) {
        jiraHdnClass.removeClass('processed');
    }
    jiraHdnClass.add('waiting');
    return;
}

function processingJiraHidden() {
    var jiraHdnClass = $('#hdnJiraLoginClick');
    if (jiraHdnClass.hasClass('waiting')) {
        jiraHdnClass.removeClass('waiting');
    } else if ($('#hdnJiraLoginClick').hasClass('processed')) {
        jiraHdnClass.removeClass('processed');
    }

    jiraHdnClass.add('processing');
    return;
}

function processedJiraHidden() {
    var jiraHdnClass = $('#hdnJiraLoginClick');
    if (jiraHdnClass.hasClass('processing')) {
        jiraHdnClass.removeClass('processing');
    } else if ($('#hdnJiraLoginClick').hasClass('waiting')) {
        jiraHdnClass.removeClass('waiting');
    }
    $('#btnJiraLogin').addClass('disabled');
    jiraHdnClass.add('processed');
    return;
}


function updateADOLoginStatus() {
    AzureDevOpsCount++;
    var uri = '../AzureDevOps/ADOCurrentLoginStatus';
    $.ajax({
        type: "Get",
        url: uri,
        context: document.body,
        success: function (result) {
            debugger;
            if (result.Status === 'processing') {
                processingADOHidden();
                window.setTimeout('updateADOLoginStatus()', 5000);
                return false;
            } else
                if (result.Status === 'success') {
                    $('#btnAzureDevOpsLogin').text('Azure Dev Ops login success');
                    processedADOHidden();
                    AzureDevOpsCount = 0;
                    return true;
                }
                else {
                    processedADOHidden();
                    AzureDevOpsCount = 0;
                    return true;
                }
        },
        error: function (errorResponse) {
            alert('error');
            return false;
        }
    });
}

function resetADOHidden() {
    var jiraHdnClass = $('#hdnADOLoginClick');
    if (jiraHdnClass.hasClass('processing')) {
        jiraHdnClass.removeClass('processing');
    }
    else if (jiraHdnClass.hasClass('processed')) {
        jiraHdnClass.removeClass('processed');
    }
    jiraHdnClass.add('waiting');
    return;
}

function processingADOHidden() {
    var hdnClass = $('#hdnADOLoginClick');
    if (hdnClass.hasClass('waiting')) {
        hdnClass.removeClass('waiting');
    } else if (hdnClass.hasClass('processed')) {
        hdnClass.removeClass('processed');
    }
    hdnClass.add('processing');
    return;
}

function processedADOHidden() {
    var hdnClass = $('#hdnADOLoginClick');
    if (hdnClass.hasClass('processing')) {
        hdnClass.removeClass('processing');
    } else if (hdnClass.hasClass('waiting')) {
        hdnClass.removeClass('waiting');
    }
    $('#btnAzureDevOpsLogin').addClass('disabled');
    hdnClass.add('processed');
    return;
}

//$('#btnJiraLoadItems').on('click', function () {

//    $('#btnJiraLoadItems').addClass('disabled');
//    var uri='../JiraAccount/LoadJiraAccounts';

//    $.ajax({
//        type: "GET",
//        dataType: 'html',
//        contentType: 'application/html; charset=utf-8',
//        url: uri,
//        success: function (result) {
//            alert(result);
//            $('#dvLoadJiraAccounts').html(result);
//        },
//        error: function (errorResponse) {
//            alert("Error");
//        }
//    });

//});

$('#btnJiraLoadItems').on('click', function () {

    $('#btnJiraLoadItems').prop('type','submit');
});