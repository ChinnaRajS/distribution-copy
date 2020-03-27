$(window).scroll(function () {
    if ($(window).scrollTop() >= 15) {
        $('nav.bg-trans').addClass('bg-col');
    }
    else {
        $('nav.bg-trans').removeClass('bg-col');
    }
});