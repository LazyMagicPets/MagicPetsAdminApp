if ('serviceWorker' in navigator) {
    (async () => {
        try {
            const registration = await navigator.serviceWorker.register('/service-worker.js');
            console.log('Service Worker registered with scope:', registration.scope);

            if (window.location.hostname !== 'localhost') {
                if (registration.waiting) {
                    const newWorker = registration.waiting;
                    // do we need to test for .controller
                    promptUserToRefresh(newWorker);
                }

                registration.addEventListener('updatefound', () => {
                    const newWorker = registration.installing;
                    if (newWorker) {
                        newWorker.addEventListener('statechange', () => {
                            if (newWorker.state === 'installed') {
                                // Why are we testing for .controller?  returns a ServiceWorker object if its state is activating or activated
                                if (navigator.serviceWorker.controller) {
                                    console.log('index.html script, New service worker installed, prompting user to refresh 1');
                                    promptUserToRefresh(newWorker);
                                }
                            }
                        });
                    } else {
                        newWorker = registration.waiting;
                        if (newWorker) {
                            // Why are we testing for .controller?
                            if (navigator.serviceWorker.controller) {
                                console.log('index.html script, New service worker installed, prompting user to refresh 2');
                                promptUserToRefresh(newWorker);
                            }
                        }
                    }
                });

                navigator.serviceWorker.addEventListener('controllerchange', () => {
                    console.log('index.html script, Controller changed, reloading page');
                    window.location.reload();
                });

            }
        } catch (error) {
            console.error('Service Worker registration failed:', error);
        }

    })();
}

function promptUserToRefresh(worker) {
    const update = confirm('A new version is available. Would you like to update?' + " promptUserToRefresh2");
    if (update) {
        worker.postMessage({ action: 'skipWaiting' });
    }
}
