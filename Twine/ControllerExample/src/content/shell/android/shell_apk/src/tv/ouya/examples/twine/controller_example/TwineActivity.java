package tv.ouya.examples.twine.controller_example;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;

import org.chromium.content_shell_apk.ContentShellActivity;
import org.chromium.content_shell.ShellManager;

/**
 * The main Twine Activity
 */
public class TwineActivity extends ContentShellActivity {

    // Specify the path to your twine exported HTML5
    private final String HOME_URL = "http://developer.android.com/reference/android/app/Activity.html#onStart()";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ShellManager sm = getShellManager();
        if (null != sm) {
            sm.setStartupUrl(HOME_URL);
        }

        Intent homeIntent = new Intent("android.intent.action.MAIN");
        homeIntent.addCategory("android.intent.category.LAUNCHER");
        homeIntent.setClassName("tv.ouya.examples.twine.controller_example", "tv.ouya.examples.twine.controller_example.TwineActivity");
        homeIntent.setData(Uri.parse(HOME_URL));
        SetHomeIntent(homeIntent);
    }
}
