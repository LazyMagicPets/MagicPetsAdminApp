// The purpose of this script is to pull any app specific index.html content into the
// BlazorUI project. This script is run by the index.html file in the WASMApp and MAUIApp
// projects. This allows the index.html file to contain only that content which is specific 
// to each target or always the same across targets.

document.title = "Admin App";
var metaCharset = document.createElement('meta');
metaCharset.setAttribute('charset', 'utf-8');
document.head.appendChild(metaCharset);

// Link tags
var links = [
    { href: '_content/Tenancy/favicon.png', rel: 'icon', type: 'image/png' },
    { href: '_content/Tenancy/icon-512.png', rel: 'apple-touch-icon', sizes: '512x512' },
    { href: '_content/Tenancy/icon-192.png', rel: 'apple-touch-icon', sizes: '192x192' },
    { href: '_content/MudBlazor/MudBlazor.min.css', rel: 'stylesheet' }
];

links.forEach(function (linkInfo) {
    var link = document.createElement('link');
    Object.keys(linkInfo).forEach(function (key) {
        link.setAttribute(key, linkInfo[key]);
    });
    document.head.appendChild(link);
});
