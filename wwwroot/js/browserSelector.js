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
        console.log("Browsertyp nicht erkannt oder unterstützt.");
        browser = "Undefined";
    }

    return browser;

}

async function browserRecognition() {

    var userID;

    try {
        userID = await getUserId();
        console.log(userID);

        const localBrowserId = localStorage.getItem(userID + "BrowserId")

        if (localBrowserId == null) {


            const browser = browserSelector();

            const browserId = await setBrowser(userID, browser);

            var localstorageContent = browser + "/" + browserId;

            localStorage.setItem(userID + "BrowserId", localstorageContent);

        }
        else {

            userID = await getUserId();

            var storedValue = localStorage.getItem(userID + "BrowserId");

            var parts = storedValue.split('/');
            var storedBrowserName = parts[0];
            var storedBrowserId = parts[1];

            console.log(storedBrowserName);
            console.log(storedBrowserId);




        }

    } catch (error) {
        console.error("Hauptfunktion: Ein Fehler ist aufgetreten", error);
    }

    



}


