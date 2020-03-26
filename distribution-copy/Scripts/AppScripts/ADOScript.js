$('#ddlADOAccount').on('change', function () {
    $(".projectloader").css("display", "inline");
    $("#btnGetWorkItems").prop("disabled", true);
    var selectedAccountId = $('#ddlADOAccount option:selected').attr('account-id');
    partialGet('../ExportWIAttachments/GetProjects?accountName=' + selectedAccountId, 'dvProjectDetails');

});

$(function () {
    $("#btnGetWorkItems").prop("disabled", true);
    var selectedAccountId = $('#ddlADOAccount option:selected').attr('account-id');
    partialGet('../ExportWIAttachments/GetProjects?accountName=' + selectedAccountId, 'dvProjectDetails');
});

function partialGet(uri, viewId) {
    $.ajax({
        type: "GET",
        dataType: 'html',
        contentType: 'application/html; charset=utf-8',
        url: uri,
        success: function (result) {
            $(".projectloader").css("display", "none");
            $("#btnGetWorkItems").show();
            $('#' + viewId).html(result);
            $("#btnGetWorkItems").prop("disabled", false);
        },
        error: function (errorResponse) {
            alert("Error");
        }
    });
}

function workItemGet(uri, viewId) {
    $.ajax({
        type: "GET",
        dataType: 'html',
        contentType: 'application/html; charset=utf-8',
        url: uri,
        success: function (result) {
            $(".loader").css("display", "none");
            $("#dvTable").show();
            $('#' + viewId).html(result);
        },
        error: function (errorResponse) {
            alert("Error");
        }
    });
}
function get(uri) {
    $.ajax({
        type: "GET",
        contentType: 'application/html; charset=utf-8',
        url: uri,
        success: function (result) {
            alert(result);
            $('#dvLoadWorkItems').html(result);
        },
        error: function (errorResponse) {
            alert("Error");
        }
    });
}
$('#btnGetWorkItems').on('click', function () {
    $("#dvTable").hide();
    $(".loader").css("display", "inline");
    $("#noWorkItems").css("display", "none");
    var selectedAccountName = $('#ddlADOAccount option:selected').attr('account-id');
    var selectedProjectName = $('#ddlADOProjects option:selected').attr('project-id');
    workItemGet('/ExportWIAttachments/GetWorkItems?accountName=' + selectedAccountName + '&projectName=' + selectedProjectName, 'dvLoadWorkItems');
});
