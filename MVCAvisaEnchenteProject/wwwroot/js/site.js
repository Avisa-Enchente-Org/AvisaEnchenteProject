$(function () {

    if ($("div.notification").length) {
        setTimeout(() => {
            $("div.notification").fadeOut();
        }, 4000);
    }

});