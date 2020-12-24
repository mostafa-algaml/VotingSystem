function beginLoading() {
    $("body").LoadingOverlay("show", {
        fade: true,
        color: "rgba(255, 255, 255, 0.5)",
        zIndex: 99999999999
    });
}
function finishLoading() {
    $("body").LoadingOverlay("hide", {
        fade: true
    });
}
function ShowNotify(type, title, msg) {
    var icon = "fa fa-bell";
    if (type == "success") {
        tata.success(title, msg, {
            duration: 5000,
            position: 'tr'
        })
    }
    else if (type == "danger") {
        tata.error(title, msg, {
            duration: 5000,
            position: 'tr'
        })
    }
    else if (type == "warning") {
        tata.warn(title, msg, {
            duration: 5000,
            position: 'tr'
        })
    }
  
    //tata.text('Hello World', 'CSSScript.Com')
    //tata.log('Hello World', 'CSSScript.Com')
    //tata.info('Hello World', 'CSSScript.Com')
    //tata.success('Hello World', 'CSSScript.Com')
    //tata.warning('Hello World', 'CSSScript.Com')
    //tata.error('Hello World', 'CSSScript.Com')
}