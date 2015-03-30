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
            	"com.xiaomi.sdk.permission.PAYMENT",
            	"android.permission.GET_TASKS",
            	"android.permission.READ_PHONE_STATE",
                "android.permission.SYSTEM_ALERT_WINDOW",
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
