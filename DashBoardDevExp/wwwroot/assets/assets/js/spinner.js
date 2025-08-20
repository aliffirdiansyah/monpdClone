document.onreadystatechange = function () {
    if (document.readyState !== "complete") {
        $('#loader').show();
    } else {
        $('#loader').hide();
        document.querySelector("body").style.visibility = "visible";
    }
};