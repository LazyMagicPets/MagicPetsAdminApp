export default {
    staticAssets: [
        // System
        { "system/base/System/": "PreCache" },
        { "system/en-US/System/": "PreCache" },
        { "system/es-MX/System/": "LazyCache" },

        // AdminApp
        { "system/base/AdminApp/": "PreCache" },
        { "system/en-US/AdminApp/": "PreCache" },
        { "system/es-MX/AdminApp/": "LazyCache" },

        // Tenancy
        { "tenancy/base/System/": "PreCache" },
        { "tenancy/base/AdminApp/": "PreCache" },
        { "tenancy/en-US/AdminApp/": "PreCache" },
        { "tenancy/es-MX/AdminApp/": "LazyCache" },

        // Subtenancy
        { "subtenancy/base/System/": "PreCache" },
        { "subtenancy/base/AdminApp/": "PreCache" },
        { "subtenancy/en-US/AdminApp/": "PreCache" },
        { "subtenancy/es-MX/AdminApp/": "LazyCache" },

    ]
};