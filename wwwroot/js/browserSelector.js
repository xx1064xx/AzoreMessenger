function browserSelector() {
    var browserName = navigator.userAgent;
    var browser;

    if (browserName.indexOf("Chrome") !== -1) {
        console.log("Du benutzt Chrome!");
        browser = "Chrome";
    } else if (browserName.indexOf("Firefox") !== -1) {
        console.log("Du benutzt Firefox!");
        browser = "Firefox";
    } else {
        console.log("Browsertyp nicht erkannt oder unterst�tzt.");
        browser = "Undefined";
    }

    return browser;

}


