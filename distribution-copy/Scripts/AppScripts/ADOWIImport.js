$(function () {
    partialGet(uri);
});

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