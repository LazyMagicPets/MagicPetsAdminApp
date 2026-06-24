export const settings = {
    staticAssets: [
        // System (shared across all subtenants)
        { path: "system/base/System/", cacheType: "PreCache", shared: true },
        { path: "system/en-US/System/", cacheType: "PreCache", shared: true },
        { path: "system/es-MX/System/", cacheType: "LazyCache", shared: true },

        // AdminApp (shared across all subtenants)
        { path: "system/base/AdminApp/", cacheType: "PreCache", shared: true },
        { path: "system/en-US/AdminApp/", cacheType: "PreCache", shared: true },
        { path: "system/es-MX/AdminApp/", cacheType: "LazyCache", shared: true },

        // Tenancy (shared across all subtenants within tenant)
        { path: "tenancy/base/System/", cacheType: "PreCache", shared: true },
        { path: "tenancy/base/AdminApp/", cacheType: "PreCache", shared: true },
        { path: "tenancy/en-US/AdminApp/", cacheType: "PreCache", shared: true },
        { path: "tenancy/es-MX/AdminApp/", cacheType: "LazyCache", shared: true },

        // Subtenancy (subtenant-specific, requires cache swapping)
        { path: "subtenancy/base/System/", cacheType: "PreCache", shared: false },
        { path: "subtenancy/base/AdminApp/", cacheType: "PreCache", shared: false },
        { path: "subtenancy/en-US/AdminApp/", cacheType: "PreCache", shared: false },
        { path: "subtenancy/es-MX/AdminApp/", cacheType: "LazyCache", shared: false },

    ]
};