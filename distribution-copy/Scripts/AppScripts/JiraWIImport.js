
$('#ddlJiraAccount').on('change', function () {

    var selectedCloudId = $('#ddlJiraAccount option:selected').attr('cloud-id');
    partialGet('../JiraAccount/LoadJiraProjects?cloudId=' + selectedCloudId);

});

//$(document).on('change', '#ddlJiraProject', function () {
    
//    var selectedCloudId = $('#ddlJiraAccount option:selected').attr('cloud-id');
//    var selectedProjectName = $('#ddlJiraProject option:selected').text();
//    var selectedProjectId = $('#ddlJiraProject option:selected').attr('project-id');
//    partialGet('../JiraAccount/LoadWorkItems?cloudId=' + selectedCloudId + "&projectId=" + selectedProjectId + "&projectName=" + selectedProjectName);

//});

function post(uri, requestBody) {
    $.ajax({
        type: "POST",
        data: requestBody,
        contentType: 'application/json; charset=utf-8',
        url: uri,
        success: function (data) {

            return data;
        },
        error: function (errorResponse) {
            return errorResponse;
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
            alert(result);
            $('#dvJiraProjects').html(result);
        },
        error: function (errorResponse) {
            alert("Error");
        }
    });
}
function get(uri, riParams) {
    var response = null;
    $.ajax({
        type: "Get",
        url: uri + riParams,
        context: document.body,
        success: function (result) {
            response = JSON.stringify(result);
        },
        error: function (errorResponse) {

            response = JSON.stringify(errorResponse);
        }
    });

    return response;
}


