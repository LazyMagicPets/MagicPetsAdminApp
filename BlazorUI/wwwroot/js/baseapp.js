// ping2
let viewerInstance;

export function initialize(classInstance) {
    console.log("baseapp.js initialize()");
    viewerInstance = classInstance;

    navigator.serviceWorker.addEventListener('message', function (event) {
        //console.log('Received a message from service worker:', event.data);
        if(event.type === 'swOnInstall' || event.type === 'swOnActivate')
            viewerInstance.invokeMethodAsync("OnHello", event.body);
    });

    //window.addEventListener('beforeunload', function (e) {
    //    console.log("beforeunload");
    //    // Cancel the event
    //    e.preventDefault();
    //    viewerInstance.invokeMethodAsync("OnReload");
    //    // Chrome requires returnValue to be set
    //    e.returnValue = 'yada';
    //});
    

    //window.onload = function () {
    //    console.log("window.onload");
    //    if (performance.navigation.type === 1) { // 1 means the page is reloaded
    //        viewerInstance.invokeMethodAsync("OnReload");
    //        //console.log("Page reload detected. Redirecting to root...");
    //        //window.location.href = '/'; // Redirect to the root of the application
    //    }
    //}
}

export function sayHello() {
    viewerInstance.invokeMethodAsync("OnHello","I'm here");
}

