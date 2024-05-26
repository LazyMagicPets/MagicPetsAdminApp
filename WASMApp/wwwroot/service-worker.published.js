// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations
// V12

const TEMP_CACHE_NAME = `temp-cache`;
const CACHE_NAME = `cache`;
const TEMP_TENANCY_CACHE_NAME = 'temp-tenancy-cache';
const TENANCY_CACHE_NAME = 'tenancy-cache';
let version = '';
let setsversion = '';
let setsCmpversion = '';


try {
    self.importScripts('./service-worker-assets.js'); // assigns self.assetsManifest.assets 
} catch (error) {
    console.error('Error importing scripts:', error);
}

const offlineAssetsInclude = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/, /\.svg$/];
//const offlineAssetsExclude = [/^service-worker\.js$/, /GoogleTag\.js$/]; // Excluding GoogleTag.js because ad blockers block it and this will cause the caching to fail.
const offlineAssetsExclude = [/GoogleTag\.js$/]; // Excluding GoogleTag.js because ad blockers block it and this will cause the caching to fail.

self.addEventListener('install', event => {
    console.info('Service worker installing...');
    event.waitUntil(
        (async () => {

            const assetsRequests = self.assetsManifest.assets
                .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
                .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
                .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
            await loadCache(assetsRequests, TEMP_CACHE_NAME);

            //const tenancyAssets = [...self.setsManifest.assets, ...self.setsCmpManifest.assets];
            //const tenancyRequests = tenancyAssets
            //    .map(asset => new Request(asset.url, { cache: 'no-cache' }));
            //await loadCache(tenancyRequests, TEMP_TENANCY_CACHE_NAME);

            // Let the main UI thread know there is a new service worker waiting to activate
            const clients = await self.clients.matchAll({ type: 'window' });
            for (const client of clients) {
                client.postMessage({
                    action: 'notifyUpdate',
                    message: 'A new version is available. Please reload the page to update.'
                });
            }
        })()
    );
});

async function loadCache(assetsRequests, cacheName) {

    const cache = await caches.open(cacheName);

    // Use Promise.allSettled to handle each request individually
    const results = await Promise.allSettled(assetsRequests.map(request => fetch(request)));
    let successCount = 0;
    let failureCount = 0;

    // Not counting successes and failures?
    for (const result of results) {
        if (result.status === 'fulfilled') {
            try {
                await cache.put(result.value.url, result.value);
                successCount++;
            } catch (error) {
                console.error('Failed to cache (' + cacheName + '):', result.value.url, error);
                failureCount++;
            }
        } else {
            console.error('Fetch failed (' + cacheName + '):', result.reason);
            failureCount++;
        }
    }
    console.log(`Successfully cached ${successCount} assets.` + cacheName);
    console.log(`Failed to cache ${failureCount} assets.` + cacheName);
}



self.addEventListener('activate', event => {
    console.log('Service worker activating... version:' + version);
    event.waitUntil(
        (async () => {
            const cacheNames = await caches.keys();
            await Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheName === CACHE_NAME) {
                        return caches.delete(cacheName);
                    }
                })
            );

            // Open both the temporary and the new cache
            const tempCache = await caches.open(TEMP_CACHE_NAME);
            const cache = await caches.open(CACHE_NAME);

            // Get all the requests from the temporary cache and put them into the new cache
            const tempCacheKeys = await tempCache.keys();
            await Promise.all(
                tempCacheKeys.map(async request => {
                    const response = await tempCache.match(request);
                    await cache.put(request, response);
                })
            );

            // Delete the temporary cache
            await caches.delete(TEMP_CACHE_NAME);

            await self.clients.claim();
        })()
    );
});

self.addEventListener('message', event => {
    console.log('service worker message:' + event.data.action);
    if (event.data.action == 'skipWaiting') {
        console.log('service worker skipWaiting');
        self.skipWaiting();
    }
});

self.addEventListener('controllerchange', () => {
    console.log('service worker controllerchange');
    window.location.reload();
});


self.addEventListener('fetch', event => {
    event.respondWith(
        (async () => {
            let cachedResponse = null;
            // if (event.request.mode === 'navigate') return null;
            if (event.request.method === 'GET') {
                try {
                    // For all navigation requests, try to serve index.html from cache
                    // If you need some URLs to be server-rendered, edit the following check to exclude those URLs
                    const shouldServeIndexHtml = event.request.mode === 'navigate';
                    const request = shouldServeIndexHtml ? '' : event.request;
                    if (shouldServeIndexHtml) {
                        console.log('service worker fetch redirect:' + event.request.url + ' to: /');
                    }

                    // const request = event.request;
                    const cache = await caches.open(CACHE_NAME);
                    cachedResponse = await cache.match(request);
                } catch (error) {
                    console.error('Error during fetch:', error);
                    return null;
                }
            }
            // we don't need awaton the fetch becuase it returns a promise
            return cachedResponse || fetch(event.request);
        })()
    );
});