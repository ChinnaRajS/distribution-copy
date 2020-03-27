
$(document).ready(function () {
    $("#btnCreate").on('click', function () {
        var ddlSubscriptionId = $("#selectedValue").text().trim();
        if (ddlSubscriptionId === 'Select Subscription' || ddlSubscriptionId === null || ddlSubscriptionId === '' || ddlSubscriptionId === 'undefined') {
            alert(ddlSubscriptionId);
        } else {
            alert("dhkaks");
        }
    });
    $(".dropdown-menu li a").click(function () {
        var selText = '';
        selText = $(this).text();
        $(this).parents('.btn-group').find('.dropdown-toggle').html(selText + ' <span class="caret"></span>');
    });
});

