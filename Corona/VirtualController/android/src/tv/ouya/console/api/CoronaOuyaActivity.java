package tv.ouya.console.api;

import android.content.Intent;
import android.os.Bundle;
import android.app.Activity;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;

import java.lang.Override;

/**
 * Extend the OuyaActivity and your onGenericMotionEvent, onKeyUp, and onKeyDown
 * events will behave as though your input device is an OuyaController.
 *
 * Input events will remap supported controllers to an OuyaController layout.
 *
 * The input remapping is data driven and will support new devices without the
 * need to recompile the application or game.
 */
public class CoronaOuyaActivity extends com.ansca.corona.CoronaActivity
{
	private static final String TAG = CoronaOuyaActivity.class.getSimpleName();

    private static final int INPUT_BROADCAST_COOLDOWN_MS = 30 * 1000;
    private static final String ACTION_OUYA_USER_INPUT = "tv.ouya.metrics.action.USER_INPUT";
    private static final String EXTRA_OUYA_INPUT_ANALOG = "analog";

    /**
     * Default flags value.
     */
    public static final int FLAGS_DEFAULT = 0;
    /**
     * Flag to have OuyaActivity not call takeKeyEvents.
     */
    public static final int FLAG_SKIP_TAKE_KEYS = (1 << 0);
    /**
     * Flag to have OuyaActivity not call hideNavigationBar.
     */
    public static final int FLAG_SKIP_HIDE_NAV_BAR = (1 << 1);
    /**
     * Flag to have OuyaActivity call OuyaInputMapper.setEnableControllerDispatch(false) (this will not keep OuyaController classes in sync).
     */
    public static final int FLAG_SKIP_OUYACONTROLLER_DISPATCH = (1 << 2);
    /**
     * Flag to have OuyaActivity not use the OuyaInputMapper for dispatching events (this will disable automatic button remapping).
     */
    public static final int FLAG_SKIP_OUYAINPUTMAPPER_DISPATCHES = (1 << 3);
    private int mFlags = FLAGS_DEFAULT;

    private long mLastAnalogTick;
    private long mLastDigitalTick;

    /**
     * Set the optional flags OuyaActivity.
     * @param flags see the OuyaActivity.FLAG_* constants
     */
    public void setFlags(int flags) {
        mFlags = flags;
    }

	/**
	 * onCreate initializes the OuyaInputMapper to begin remapping input.
	 *
	 * @param savedInstanceState pass to the super activity onCreate.
	 */
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		OuyaInputMapper.init(this);
        final boolean enableControllerDispatch = ((mFlags & FLAG_SKIP_OUYACONTROLLER_DISPATCH) == 0);
        OuyaInputMapper.setEnableControllerDispatch(enableControllerDispatch);

		super.onCreate(savedInstanceState);
	}

	/**
	 * onDestroy calls the OuyaInputMapper to shutdown before exiting.
	 *
	 */
	@Override
	protected void onDestroy() {
		super.onDestroy();
		OuyaInputMapper.shutdown(this);
	}

	/**
	 * Calls the static setContentView method for you.
	 *
	 * @param layoutResID pass to the super activity setContentView
	 */
	@Override
	public void setContentView(int layoutResID) {
		super.setContentView(layoutResID);
        setContentView(this, mFlags);
	}

	/**
     * Calls the static setContentView method for you.
	 *
	 * @param view pass to the super activity setContentView
	 */
	@Override
	public void setContentView(View view) {
		super.setContentView(view);
        setContentView(this, mFlags);
	}

	/**
     * Calls the static setContentView method for you.
	 *
	 * @param view pass to the super activity setContentView
	 * @param params pass to the super activity setContentView
	 */
	@Override
	public void setContentView(View view, ViewGroup.LayoutParams params) {
		super.setContentView(view, params);
        setContentView(this, mFlags);
	}

    /**
     * Helper method to call common window setup methods for you (eg: takeKeyEvents, hideNavigationBar)
     * @param activity the current activity
     * @param flags see the OuyaActivity.FLAG_* constants
     */
    public static void setContentView(Activity activity, int flags) {
        if ((flags & FLAG_SKIP_TAKE_KEYS) == 0) {
            activity.takeKeyEvents(true);
        }
        if ((flags & FLAG_SKIP_HIDE_NAV_BAR) == 0) {
            hideNavigationBar(activity);
        }
    }

    @Override
    protected void onResume() {
        super.onResume();

        if ((mFlags & FLAG_SKIP_HIDE_NAV_BAR) == 0) {
            hideNavigationBar(this);
        }
    }

    /**
     * Helper method to hide Android's navigation bar.
     * @param activity the current activity
     */
    public static void hideNavigationBar(final Activity activity) {
        View decorView = activity.getWindow().getDecorView();
        int uiOptions = View.SYSTEM_UI_FLAG_LOW_PROFILE
                | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
                | View.SYSTEM_UI_FLAG_FULLSCREEN
                | View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                | View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN;
        decorView.setSystemUiVisibility(uiOptions);

        decorView.setOnSystemUiVisibilityChangeListener(new View.OnSystemUiVisibilityChangeListener() {
            @Override
            public void onSystemUiVisibilityChange(int visibility) {
                if ((visibility & (View.SYSTEM_UI_FLAG_HIDE_NAVIGATION | View.SYSTEM_UI_FLAG_FULLSCREEN)) == 0) {
                    hideNavigationBar(activity);
                }
            }
        });
    }

	/**
	 * Dispatch KeyEvents to the OuyaInputMapper. The remapped input will
	 * return to the activity with onGenericMotionEvent, onKeyDown, and onKeyUp
	 * events.
	 *
	 * Be sure to use the remapped input from the above events instead of
	 * the KeyEvent passed to this method that has not been remapped yet.
	 *
	 * @param keyEvent passed passed to the OuyaInputMapper
	 */
	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
        if ((mFlags & FLAG_SKIP_OUYAINPUTMAPPER_DISPATCHES) == 0
                && OuyaInputMapper.shouldHandleInputEvent(keyEvent)) {
            broadcastInputNotification(false);
		    return OuyaInputMapper.dispatchKeyEvent(this, keyEvent);
        } else {
            return super.dispatchKeyEvent(keyEvent);
        }
	}

	/**
	 * Dispatch MotionEvents to the OuyaInputMapper. The remapped input will
	 * return to the activity with onGenericMotionEvent, onKeyDown, and onKeyUp
	 * events.
	 *
	 * Be sure to use the remapped input from the above events instead of
	 * the MotionEvent passed to this method that has not been remapped yet.
	 *
	 * @param motionEvent passed passed to the OuyaInputMapper
	 */
	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
        if ((mFlags & FLAG_SKIP_OUYAINPUTMAPPER_DISPATCHES) == 0
                && OuyaInputMapper.shouldHandleInputEvent(motionEvent)) {
            broadcastInputNotification(true);
		    return OuyaInputMapper.dispatchGenericMotionEvent(this, motionEvent);
        } else {
            return super.dispatchGenericMotionEvent(motionEvent);
        }
    }

    private void broadcastInputNotification(boolean analog) {
        final long curTick = System.nanoTime() / 1000000;
        final long lastTick = analog ? mLastAnalogTick : mLastDigitalTick;
        if(curTick - lastTick < INPUT_BROADCAST_COOLDOWN_MS) {
            return;
        }

        if(analog) {
            mLastAnalogTick = curTick;
        } else {
            mLastDigitalTick = curTick;
        }

        final Intent intent = new Intent(ACTION_OUYA_USER_INPUT);
        intent.putExtra(EXTRA_OUYA_INPUT_ANALOG, analog);
        sendBroadcast(intent);
    }
}