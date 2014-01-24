// Copyright 2012 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.


package org.chromium.content_shell_apk;

import android.app.Activity;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.FrameLayout;
import android.widget.TextView;
import android.widget.Toast;

import org.chromium.base.BaseSwitches;
import org.chromium.base.CommandLine;
import org.chromium.base.MemoryPressureListener;
import org.chromium.content.app.LibraryLoader;
import org.chromium.content.browser.ActivityContentVideoViewClient;
import org.chromium.content.browser.BrowserStartupController;
import org.chromium.content.browser.ContentVideoViewClient;
import org.chromium.content.browser.ContentView;
import org.chromium.content.browser.ContentViewClient;
import org.chromium.content.browser.ContentViewCore;
import org.chromium.content.browser.DeviceUtils;
import org.chromium.content.common.ContentSwitches;
import org.chromium.content.common.ProcessInitException;
import org.chromium.content_shell.Shell;
import org.chromium.content_shell.ShellManager;
import org.chromium.ui.base.ActivityWindowAndroid;
import org.chromium.ui.base.WindowAndroid;

import java.util.ArrayList;
import java.util.List;

import tv.ouya.console.api.OuyaController;

/**
 * Activity for managing the Content Shell.
 */
public class ContentShellActivity extends Activity {

    public static final String COMMAND_LINE_FILE = "/data/local/tmp/content-shell-command-line";
    private static final String TAG = "ContentShellActivity";

    private static final String ACTIVE_SHELL_URL_KEY = "activeUrl";
    public static final String COMMAND_LINE_ARGS_KEY = "commandLineArgs";

    private ShellManager mShellManager;
    private WindowAndroid mWindowAndroid;

    private boolean mShowToolbar = false;

    private enum OverlayFocus
    {
        BROWSER,
        NETWORK,
        PAUSE
    }

    private OverlayFocus mOverlayFocus = OverlayFocus.BROWSER;

    private FrameLayout mPauseOverlay = null;
    private ArrayList<Button> mButtonsPause = new ArrayList<Button>();
    private int mButtonPauseIndex = 0;

    private FrameLayout mNetworkOverlay = null;
    private ArrayList<Button> mButtonsNetwork = new ArrayList<Button>();
    private int mButtonNetworkIndex = 0;

    private FrameLayout mProgressOverlay = null;
    private TextView mProgress = null;

    private int mButtonLinkIndex = -1;
    private int mButtonLinkCount = 0;

    private boolean m_waitToExit = true;

    private float mScrollX = 0f;

    private float mScrollY = 0f;

    private Intent mHomeIntent = null;

    private Button btnHome = null;
    private Button btnBack = null;
    private Button btnExit = null;

    private Button btnRetryNetwork = null;
    private Button btnNetworkSettings = null;
    private Button btnNetworkExit = null;

    @Override
    protected void onCreate(final Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // Initializing the command line must occur before loading the library.
        if (!CommandLine.isInitialized()) {
            CommandLine.initFromFile(COMMAND_LINE_FILE);
            String[] commandLineParams = getCommandLineParamsFromIntent(getIntent());
            if (commandLineParams != null) {
                CommandLine.getInstance().appendSwitchesAndArguments(commandLineParams);
            }
        }
        waitForDebuggerIfNeeded();

        DeviceUtils.addDeviceSpecificUserAgentSwitch(this);
        try {
            LibraryLoader.ensureInitialized();
        } catch (ProcessInitException e) {
            Log.e(TAG, "ContentView initialization failed.", e);
            // Since the library failed to initialize nothing in the application
            // can work, so kill the whole application not just the activity
            System.exit(-1);
            return;
        }

        setContentView(R.layout.content_shell_activity);
        mShellManager = (ShellManager) findViewById(R.id.shell_container);
        mWindowAndroid = new ActivityWindowAndroid(this);
        mWindowAndroid.restoreInstanceState(savedInstanceState);
        mShellManager.setWindow(mWindowAndroid);

        String startupUrl = getUrlFromIntent(getIntent());
        if (!TextUtils.isEmpty(startupUrl)) {
            mShellManager.setStartupUrl(Shell.sanitizeUrl(startupUrl));
        }

        if (CommandLine.getInstance().hasSwitch(ContentSwitches.DUMP_RENDER_TREE)) {
            try {
                BrowserStartupController.get(this).startBrowserProcessesSync(
                       BrowserStartupController.MAX_RENDERERS_LIMIT);
            } catch (ProcessInitException e) {
                Log.e(TAG, "Failed to load native library.", e);
                System.exit(-1);
            }
        } else {
            try {
                BrowserStartupController.get(this).startBrowserProcessesAsync(
                        new BrowserStartupController.StartupCallback() {
                            @Override
                            public void onSuccess(boolean alreadyStarted) {
                                finishInitialization(savedInstanceState);
                            }

                            @Override
                            public void onFailure() {
                                initializationFailed();
                            }
                        });
            } catch (ProcessInitException e) {
                Log.e(TAG, "Unable to load native library.", e);
                System.exit(-1);
            }
        }

        mPauseOverlay = (FrameLayout) findViewById(R.id.pauseOverlay);

        btnHome = (Button)findViewById(R.id.btnHome);
        btnBack = (Button)findViewById(R.id.btnBack);
        btnExit = (Button)findViewById(R.id.btnExit);

        mButtonsPause.add(btnHome);
        mButtonsPause.add(btnBack);
        mButtonsPause.add(btnExit);

        btnHome.setSelected(true);

        mNetworkOverlay = (FrameLayout) findViewById(R.id.networkOverlay);

        btnRetryNetwork = (Button)findViewById(R.id.btnRetryNetwork);
        btnNetworkSettings = (Button)findViewById(R.id.btnNetworkSettings);
        btnNetworkExit = (Button)findViewById(R.id.btnNetworkExit);

        mButtonsNetwork.add(btnRetryNetwork);
        mButtonsNetwork.add(btnNetworkSettings);
        mButtonsNetwork.add(btnNetworkExit);

        ///*
        CheckNetwork();

        registerReceiver(new BroadcastReceiver() {
            public void onReceive(Context context, Intent intent) {
                CheckNetwork();
            }}, new IntentFilter("android.net.conn.CONNECTIVITY_CHANGE"));
        //*/

        /*
        mOverlayFocus = OverlayFocus.NETWORK;
        UpdateOverlays();
        */

        btnRetryNetwork.setSelected(true);

        mProgressOverlay = (FrameLayout) findViewById(R.id.progressOverlay);
        mProgress = (TextView) findViewById(R.id.txtProgress);

        MonitorProgress();

        takeKeyEvents(true); //added
    }

    @Override
    protected void onResume() {
        super.onResume();

        btnHome.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_MENU));
                if (null != mHomeIntent) {
                    startActivity(mHomeIntent);
                }
            }
        });

        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_MENU));
            }
        });

        btnExit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_MENU));
                ApplicationExit();
            }
        });

        btnRetryNetwork.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                CheckNetwork();
            }
        });

        btnNetworkSettings.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                //this will work after the next OTA
                startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse("ouya://launcher/manage/network")));

                ApplicationExit();
            }
        });

        btnNetworkExit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                ApplicationExit();
            }
        });
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();

        // notify threads it's time to exit
        m_waitToExit = false;
    }

    protected void SetHomeIntent(Intent intent) {
        mHomeIntent = intent;
    }

    private void ApplicationExit() {
        if (null != mShellManager) {
            if (null != mShellManager.getActiveShell()) {
                mShellManager.getActiveShell().loadUrl("about:blank");
            }
        }
        finish();
    }

    private void MonitorProgress() {
        Thread timer = new Thread() {
            public void run() {
                while (m_waitToExit) {

                    Runnable runnable = new Runnable()
                    {
                        public void run()
                        {
                            if (null != mShellManager) {
                                double progress = mShellManager.getProgress();
                                if (progress < 1.0 &&
                                        mOverlayFocus == OverlayFocus.BROWSER) {
                                    if (null != mProgressOverlay) {
                                        mProgressOverlay.setVisibility(View.VISIBLE);
                                    }
                                    if (null != mProgress) {
                                        mProgress.setText(String.format("Loading... %.1f%%", progress * 100));
                                    }
                                } else {
                                    if (null != mProgressOverlay) {
                                        mProgressOverlay.setVisibility(View.GONE);
                                    }
                                }
                            }

                            if (mScrollX < -0.5f) {
                                passDispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_LEFT));
                                passDispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_LEFT));
                            } else if (mScrollX > 0.5f) {
                                passDispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_RIGHT));
                                passDispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_RIGHT));
                            }

                            if (mScrollY < -0.5f) {
                                passDispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_UP));
                                passDispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_UP));
                            } else if (mScrollY > 0.5f) {
                                passDispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_DOWN));
                                passDispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_DOWN));
                            }

                            if (mOverlayFocus == OverlayFocus.BROWSER) {
                                ContentView view = getActiveContentView();
                                if (null != view) {
                                    view.requestFocus();
                                }
                            }
                        }
                    };

                    runOnUiThread(runnable);

                    try {
                        Thread.sleep(50);
                    } catch (InterruptedException e) {
                        // TODO Auto-generated catch block
                        e.printStackTrace();
                    }
                }
            }
        };
        timer.start();
    }

    private void CheckNetwork() {
        if (IsNetworkAvailable()) {
            if (mOverlayFocus == OverlayFocus.NETWORK) {
                mOverlayFocus = OverlayFocus.BROWSER;
                UpdateOverlays();
            }
        } else {
            mOverlayFocus = OverlayFocus.NETWORK;
            UpdateOverlays();
        }
    }

    private boolean IsNetworkAvailable() {
        ConnectivityManager connectivityManager =
                (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
        return activeNetworkInfo != null && activeNetworkInfo.isConnected();
    }

    private void finishInitialization(Bundle savedInstanceState) {
        String shellUrl = mShellManager.getStartupUrl();
        if (savedInstanceState != null
                && savedInstanceState.containsKey(ACTIVE_SHELL_URL_KEY)) {
            shellUrl = savedInstanceState.getString(ACTIVE_SHELL_URL_KEY);
        }
        mShellManager.launchShell(shellUrl);
        getActiveContentView().setContentViewClient(new ContentViewClient() {
            @Override
            public ContentVideoViewClient getContentVideoViewClient() {
                return new ActivityContentVideoViewClient(ContentShellActivity.this) {
                    @Override
                    public void onShowCustomView(View view) {
                        super.onShowCustomView(view);
                        mShellManager.setOverlayVideoMode(true);
                    }

                    @Override
                    public void onDestroyContentVideoView() {
                        super.onDestroyContentVideoView();
                        mShellManager.setOverlayVideoMode(false);
                    }
                };
            }
        });
    }

    private void initializationFailed() {
        Log.e(TAG, "ContentView initialization failed.");
        Toast.makeText(ContentShellActivity.this,
                R.string.browser_process_initialization_failed,
                Toast.LENGTH_SHORT).show();
        finish();
    }

    @Override
    protected void onSaveInstanceState(Bundle outState) {
        super.onSaveInstanceState(outState);
        Shell activeShell = getActiveShell();
        if (activeShell != null) {
            outState.putString(ACTIVE_SHELL_URL_KEY, activeShell.getContentView().getUrl());
        }

        mWindowAndroid.saveInstanceState(outState);
    }

    private void waitForDebuggerIfNeeded() {
        if (CommandLine.getInstance().hasSwitch(BaseSwitches.WAIT_FOR_JAVA_DEBUGGER)) {
            Log.e(TAG, "Waiting for Java debugger to connect...");
            android.os.Debug.waitForDebugger();
            Log.e(TAG, "Java debugger connected. Resuming execution.");
        }
    }

    private boolean passOnKeyDown (int keyCode, KeyEvent event) {
        //Log.i(TAG, "passOnKeyDown keyCode="+keyCode+" event.keyCode="+event.getKeyCode()+" action=" + event.getAction());
        return super.onKeyDown(keyCode, event);
    }

    private boolean passOnKeyUp (int keyCode, KeyEvent event) {
        //Log.i(TAG, "passOnKeyUp keyCode="+keyCode+" event.keyCode="+event.getKeyCode()+" action=" + event.getAction());
        return super.onKeyUp(keyCode, event);
    }

    private boolean passDispatchKeyEvent (KeyEvent event) {
        //Log.i(TAG, "passDispatchKeyEvent keyCode="+event.getKeyCode()+" action=" + event.getAction());
        return super.dispatchKeyEvent(event);
    }

    /*
    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        Log.i(TAG, "onKeyDown keyCode="+keyCode+" event.keyCode="+event.getKeyCode()+" action=" + event.getAction());
        return super.onKeyDown(keyCode, event);
    }

    @Override
    public boolean onKeyUp(int keyCode, KeyEvent event) {
        Log.i(TAG, "onKeyUp keyCode="+keyCode+" event.keyCode="+event.getKeyCode()+" action=" + event.getAction());
        return super.onKeyUp(keyCode, event);
    }

    @Override
    public boolean onGenericMotionEvent(MotionEvent event) {
        //Log.i(TAG, "onGenericMotionEvent action=" + event.getAction());
        return super.onGenericMotionEvent(event);
    }
    */

    @Override
    public boolean dispatchGenericMotionEvent (MotionEvent event) {
        //Log.i(TAG, "dispatchGenericMotionEvent action=" + event.getAction());
        if (mOverlayFocus == OverlayFocus.BROWSER) {
            mScrollX = event.getAxisValue(OuyaController.AXIS_LS_X);
            mScrollY = event.getAxisValue(OuyaController.AXIS_LS_Y);
        }
        return super.dispatchGenericMotionEvent(event);
    }

    public class RunnableJavaScriptCallback implements ContentViewCore.JavaScriptCallback {
        public Runnable mRunnable = null;
        public void handleJavaScriptResult(String jsonResult) {
            //Log.i(TAG, "handleJavaScriptResult jsonResult="+jsonResult);
            mButtonLinkCount = Integer.parseInt(jsonResult);
            if (null != mRunnable) {
                runOnUiThread(mRunnable);
            }
        }
    }

    private void UpdateLinkCount(final ContentView view, Runnable runnable) {
        RunnableJavaScriptCallback callback = new RunnableJavaScriptCallback();
        callback.mRunnable = runnable;

        String js = "(function() { return document.links.length; })();";
        view.evaluateJavaScript(js, callback);
    }

    @Override
    public boolean dispatchKeyEvent (KeyEvent event) {
        Log.i(TAG, "dispatchKeyEvent keyCode="+event.getKeyCode()+" action=" + event.getAction());

        int keyCode = event.getKeyCode();
        int action = event.getAction();
        int source = event.getSource();

        if (null != mPauseOverlay &&
                mOverlayFocus == OverlayFocus.PAUSE) {
            if (keyCode == OuyaController.BUTTON_MENU &&
                action == KeyEvent.ACTION_UP) {
                mOverlayFocus = OverlayFocus.BROWSER;
                UpdateOverlays();
                return true;
            }

            if (keyCode == OuyaController.BUTTON_A &&
                    action == KeyEvent.ACTION_UP) {
                dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_MENU));
                return true;
            }

            if (keyCode == OuyaController.BUTTON_DPAD_UP &&
                    action == KeyEvent.ACTION_UP) {
                if (mButtonPauseIndex > 0) {
                    --mButtonPauseIndex;
                    UpdateButtonsPause();
                }
                return true;
            }

            if (keyCode == OuyaController.BUTTON_DPAD_DOWN &&
                    action == KeyEvent.ACTION_UP) {
                if ((mButtonPauseIndex + 1) < mButtonsPause.size()) {
                    ++mButtonPauseIndex;
                    UpdateButtonsPause();
                }
                return true;
            }

            if (keyCode == OuyaController.BUTTON_O &&
                    action == KeyEvent.ACTION_UP) {

                int index = 0;
                for (Button button : mButtonsPause) {
                    button.setFocusable(true);
                    if (index == mButtonPauseIndex) {
                        button.callOnClick();
                    }
                    ++index;
                }

                return true;
            }

            return true; // eat key events
        } else if (mOverlayFocus == OverlayFocus.NETWORK) {
            if (keyCode == OuyaController.BUTTON_MENU &&
                    action == KeyEvent.ACTION_UP) {
                if (IsNetworkAvailable()) {
                    mOverlayFocus = OverlayFocus.BROWSER;
                    UpdateOverlays();
                }
                return true;
            }
            if (keyCode == OuyaController.BUTTON_DPAD_UP &&
                    action == KeyEvent.ACTION_UP) {
                if (mButtonNetworkIndex > 0) {
                    --mButtonNetworkIndex;
                    UpdateButtonsNetwork();
                }
                return true;
            }

            if (keyCode == OuyaController.BUTTON_DPAD_DOWN &&
                    action == KeyEvent.ACTION_UP) {
                if ((mButtonNetworkIndex + 1) < mButtonsNetwork.size()) {
                    ++mButtonNetworkIndex;
                    UpdateButtonsNetwork();
                }
                return true;
            }

            if (keyCode == OuyaController.BUTTON_O &&
                    action == KeyEvent.ACTION_UP) {

                int index = 0;
                for (Button button : mButtonsNetwork) {
                    button.setFocusable(true);
                    if (index == mButtonNetworkIndex) {
                        button.callOnClick();
                    }
                    ++index;
                }
                return true;
            }

            return true; // eat key events
        } else if (mOverlayFocus == OverlayFocus.BROWSER) {
            if (keyCode == OuyaController.BUTTON_MENU &&
                    action == KeyEvent.ACTION_UP) {
                mOverlayFocus = OverlayFocus.PAUSE;
                UpdateOverlays();
                return true;
            }
            final ContentView view = getActiveContentView();
            if (view != null) {

                if (keyCode == OuyaController.BUTTON_DPAD_UP) {
                    if (source == InputDevice.SOURCE_JOYSTICK) {
                        return false;
                    }
                    if (action == KeyEvent.ACTION_UP) {
                        Runnable runnable = new Runnable()
                        {
                            public void run()
                            {
                                if (mButtonLinkIndex < 0) {
                                    mButtonLinkIndex = 0;
                                }
                                String js = "if ("+mButtonLinkIndex+" < document.links.length) {";
                                js += " document.links["+mButtonLinkIndex+"].style.fontSize = '20px';";
                                //js += " document.links["+mButtonLinkIndex+"].class = 'internalLink';";
                                js += " }";
                                if (mButtonLinkIndex > 0) {
                                    --mButtonLinkIndex;
                                }
                                js += " if ("+mButtonLinkIndex+" < document.links.length) {";
                                js += " document.links["+mButtonLinkIndex+"].style.fontSize = '40px';";
                                //js += " document.links["+mButtonLinkIndex+"].class = 'a.internalLink:hover';";
                                js += " document.links["+mButtonLinkIndex+"].scrollIntoView();";
                                js += " }";
                                view.evaluateJavaScript(js);
                            }
                        };
                        UpdateLinkCount(view, runnable);
                    }
                    return true;
                }

                if (keyCode == OuyaController.BUTTON_DPAD_DOWN) {
                    if (source == InputDevice.SOURCE_JOYSTICK) {
                        return false;
                    }
                    if (action == KeyEvent.ACTION_UP) {
                        Runnable runnable = new Runnable()
                        {
                            public void run()
                            {
                                String js = "";
                                if (mButtonLinkIndex >= 0) {
                                    js = "if ("+mButtonLinkIndex+" < document.links.length) {";
                                    js += " document.links["+mButtonLinkIndex+"].style.fontSize = '20px';";
                                    //js += " document.links["+mButtonLinkIndex+"].class = 'internalLink';";
                                    js += " }";
                                }
                                ++mButtonLinkIndex;
                                if (mButtonLinkCount > 0 &&
                                        mButtonLinkIndex >= mButtonLinkCount) {
                                    mButtonLinkIndex = mButtonLinkCount - 1;
                                } else if (mButtonLinkCount == 0) {
                                    mButtonLinkIndex = 0;
                                }
                                js += " if ("+mButtonLinkIndex+" < document.links.length) {";
                                js += " document.links["+mButtonLinkIndex+"].style.fontSize = '40px';";
                                //js += " document.links["+mButtonLinkIndex+"].class = 'a.internalLink:hover';";
                                js += " document.links["+mButtonLinkIndex+"].scrollIntoView();";
                                js += " }";
                                view.evaluateJavaScript(js);
                            }
                        };
                        UpdateLinkCount(view, runnable);
                    }
                    return true;
                }

                if (keyCode == OuyaController.BUTTON_O) {
                    if (action == KeyEvent.ACTION_UP) {
                        Runnable runnable = new Runnable()
                        {
                            public void run()
                            {
                                if (mButtonLinkIndex < 0) {
                                    mButtonLinkIndex = 0;
                                }
                                if (mButtonLinkIndex < mButtonLinkCount) {
                                    String js = "if ("+mButtonLinkIndex+" < document.links.length) {";
                                    js += " document.links["+mButtonLinkIndex+"].click();";
                                    js += " }";
                                    view.evaluateJavaScript(js);

                                    mButtonLinkIndex = -1;
                                    mButtonLinkCount = 0;
                                }
                            }
                        };
                        UpdateLinkCount(view, runnable);
                    }
                    return true;
                }


                /*
                if (keyCode == OuyaController.BUTTON_O) {
                    if (action == KeyEvent.ACTION_DOWN ||
                        action == KeyEvent.ACTION_UP) {
                        return passOnKeyDown(0xD, new KeyEvent(KeyEvent.ACTION_DOWN, 0xD));
                    }
                }
                if (keyCode == OuyaController.BUTTON_O) {
                    if (action == KeyEvent.ACTION_DOWN) {
                        return passOnKeyDown(0xD, event);
                    } else if (action == KeyEvent.ACTION_UP) {
                        return passOnKeyUp(0xD, new KeyEvent(KeyEvent.ACTION_UP, 0xD));
                    }
                }
                if (keyCode == OuyaController.BUTTON_A) {
                    if (action == KeyEvent.ACTION_DOWN ||
                        action == KeyEvent.ACTION_UP) {
                        return passDispatchKeyEvent(new KeyEvent(action, 0x1B));
                    }
                }

                if (keyCode == OuyaController.BUTTON_L1) {
                    if (action == KeyEvent.ACTION_DOWN ||
                        action == KeyEvent.ACTION_UP) {
                        return passDispatchKeyEvent(new KeyEvent(action, KeyEvent.KEYCODE_MEDIA_PREVIOUS));
                    }
                }
                if (keyCode == OuyaController.BUTTON_R1) {
                    if (action == KeyEvent.ACTION_DOWN ||
                        action == KeyEvent.ACTION_UP) {
                        return passDispatchKeyEvent(new KeyEvent(action, KeyEvent.KEYCODE_MEDIA_NEXT));
                    }
                }

                if (keyCode == OuyaController.BUTTON_L2) {
                    if (action == KeyEvent.ACTION_DOWN ||
                        action == KeyEvent.ACTION_UP) {
                        return passDispatchKeyEvent(new KeyEvent(action, KeyEvent.KEYCODE_MEDIA_REWIND));
                    }
                }

                if (keyCode == OuyaController.BUTTON_R2) {
                    if (action == KeyEvent.ACTION_DOWN ||
                        action == KeyEvent.ACTION_UP) {
                        return passDispatchKeyEvent(new KeyEvent(action, KeyEvent.KEYCODE_MEDIA_FAST_FORWARD));
                    }
                }

                if (keyCode == OuyaController.BUTTON_U) {
                    if (action == KeyEvent.ACTION_DOWN ||
                        action == KeyEvent.ACTION_UP) {
                        return passDispatchKeyEvent(new KeyEvent(action, 0x8)); //BACKSPACE
                    }
                }

                if (keyCode == OuyaController.BUTTON_Y) {
                    if (action == KeyEvent.ACTION_DOWN ||
                        action == KeyEvent.ACTION_UP) {
                        return passDispatchKeyEvent(new KeyEvent(action, 0x53)); //S
                    }
                }

                if (keyCode == OuyaController.BUTTON_R3) {
                    if (action == KeyEvent.ACTION_DOWN ||
                            action == KeyEvent.ACTION_UP) {
                        return passDispatchKeyEvent(new KeyEvent(action, 0x20)); //SPACE
                    }
                }
                */
            }

            /*
            if (keyCode == OuyaController.BUTTON_U) {
                if (action == KeyEvent.ACTION_UP) {
                    mShowToolbar = !mShowToolbar;
                    UpdateToolbar();
                    mShellManager.requestRenderViewFocus();
                    return true;
                }
            }
            */

            /*
            if (action == KeyEvent.ACTION_DOWN) {
                return passOnKeyDown(event.getKeyCode(), event);
            }

            if (action == KeyEvent.ACTION_UP) {
                return passOnKeyUp(event.getKeyCode(), event);
            }
            */
        }
        return passDispatchKeyEvent(event);
    }

    private void UpdateButtonsPause() {
        int index = 0;
        for (Button button : mButtonsPause) {
            button.setFocusable(true);
            if (index == mButtonPauseIndex) {
                button.requestFocus();
            }
            ++index;
        }
    }

    private void UpdateButtonsNetwork() {
        int index = 0;
        for (Button button : mButtonsNetwork) {
            button.setFocusable(true);
            if (index == mButtonNetworkIndex) {
                button.requestFocus();
            }
            ++index;
        }
    }

    private void UpdateOverlays() {
        UpdatePauseOverlay();
        UpdateNetworkOverlay();
    }

    private void UpdatePauseOverlay() {
        if (mOverlayFocus == OverlayFocus.PAUSE) {
            mPauseOverlay.setVisibility(View.VISIBLE);
            mPauseOverlay.requestFocus();
            UpdateButtonsPause();
        } else {
            mPauseOverlay.setVisibility(View.GONE);
            mPauseOverlay.clearFocus();
        }
    }

    private void UpdateNetworkOverlay() {
        if (mOverlayFocus == OverlayFocus.NETWORK) {
            mNetworkOverlay.setVisibility(View.VISIBLE);
            mNetworkOverlay.requestFocus();
            UpdateButtonsNetwork();
        } else {
            mNetworkOverlay.setVisibility(View.GONE);
            mNetworkOverlay.clearFocus();
        }
    }

    private void UpdateToolbar() {
        if (mShowToolbar) {
            if (null != mShellManager) {
                mShellManager.showToolbar();
            }
        } else {
            if (null != mShellManager) {
                mShellManager.hideToolbar();
            }
        }
    }

    /*
    @Override
    public boolean onKeyUp(int keyCode, KeyEvent event) {
        if (keyCode != KeyEvent.KEYCODE_BACK) return super.onKeyUp(keyCode, event);

        Shell activeView = getActiveShell();
        if (activeView != null && activeView.getContentView().canGoBack()) {
            activeView.getContentView().goBack();
            return true;
        }

        return super.onKeyUp(keyCode, event);
    }
    */

    @Override
    protected void onNewIntent(Intent intent) {
        if (getCommandLineParamsFromIntent(intent) != null) {
            Log.i(TAG, "Ignoring command line params: can only be set when creating the activity.");
        }

        if (MemoryPressureListener.handleDebugIntent(this, intent.getAction())) return;

        String url = getUrlFromIntent(intent);
        if (!TextUtils.isEmpty(url)) {
            Shell activeView = getActiveShell();
            if (activeView != null) {
                activeView.loadUrl(url);
            }
        }
    }

    @Override
    protected void onStop() {
        super.onStop();

        ContentView view = getActiveContentView();
        if (view != null) view.onHide();
    }

    @Override
    protected void onStart() {
        super.onStart();

        ContentView view = getActiveContentView();
        if (view != null) view.onShow();
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        mWindowAndroid.onActivityResult(requestCode, resultCode, data);
    }

    private static String getUrlFromIntent(Intent intent) {
        return intent != null ? intent.getDataString() : null;
    }

    private static String[] getCommandLineParamsFromIntent(Intent intent) {
        return intent != null ? intent.getStringArrayExtra(COMMAND_LINE_ARGS_KEY) : null;
    }

    /**
     * @return The {@link ShellManager} configured for the activity or null if it has not been
     *         created yet.
     */
    public ShellManager getShellManager() {
        return mShellManager;
    }

    /**
     * @return The currently visible {@link Shell} or null if one is not showing.
     */
    public Shell getActiveShell() {
        return mShellManager != null ? mShellManager.getActiveShell() : null;
    }

    /**
     * @return The {@link ContentView} owned by the currently visible {@link Shell} or null if one
     *         is not showing.
     */
    public ContentView getActiveContentView() {
        Shell shell = getActiveShell();
        return shell != null ? shell.getContentView() : null;
    }
}
