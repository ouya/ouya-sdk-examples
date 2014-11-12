local metadata =
{
    plugin =
    {
        format = 'jar',
        manifest = 
        {
            permissions = {},
            usesPermissions =
            {
            },
            usesFeatures = {},
            applicationChildElements =
            {
		[[
		<activity android:name="tv.ouya.sdk.corona.CoronaOuyaActivity">
		</activity>		
		]],
            },
        },
    },
}

return metadata
