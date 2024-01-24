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

            const localstorageContent = browser + browserId;

            localStorage.setItem(userID + "BrowserId", localstorageContent);

        }
        else {
            console.log("BrowserId vorhanden");


        }

    } catch (error) {
        console.error("Hauptfunktion: Ein Fehler ist aufgetreten", error);
    }

    



}


