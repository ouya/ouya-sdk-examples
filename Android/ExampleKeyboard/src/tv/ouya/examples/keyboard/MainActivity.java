package tv.ouya.examples.keyboard;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

import tv.ouya.console.api.OuyaActivity;
import tv.ouya.console.api.OuyaController;
import android.os.Bundle;
import android.graphics.Color;
import android.graphics.Typeface;
import android.util.Log;
import android.view.Gravity;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup.LayoutParams;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.widget.TextView;

public class MainActivity extends OuyaActivity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private ArrayList<TextView> mKeys = new ArrayList<TextView>();
	
	private HashMap<String, TextView> mKeyMap = new HashMap<String, TextView>();
	
	private float DEADZONE = 0.25f;
	
	private final String COLOR_NORMAL = "#ffffffff";
	private final String COLOR_SKIPPED = "#ff666666";
	private final String COLOR_INACTIVE = "#ff222222";
	private final String COLOR_LEFT_A = "#ff440000";
	private final String COLOR_LEFT_B = "#ff004400";
	private final String COLOR_RIGHT_A = "#ff000044";
	private final String COLOR_RIGHT_B = "#ff444400";
	
	private final String[] LAYOUT_ROW_1 = new String[]{"Q","W","E","R","T","Y","U","I","O","P"};
	private final String[] LAYOUT_ROW_2 = new String[]{"A","S","D","F","G","H","J","K","L"};
	private final String[] LAYOUT_ROW_3 = new String[]{"Z","X","C","V","B","N","M"};
	private final String[] LAYOUT_ROW_4 = new String[]{"1","2","3","4","5","6","7","8","9","0"};
	
	private final String[] SUBSET_KEYS_1 = new String[]{"Q","W","E","A"};
	private final String[] SUBSET_KEYS_2 = new String[]{"S","Z","X",""};
	private final String[] SUBSET_KEYS_3 = new String[]{"R","T","D",""};
	private final String[] SUBSET_KEYS_4 = new String[]{"F","G","C",""};
	private final String[] SUBSET_KEYS_5 = new String[]{"Y","U","I","J"};
	private final String[] SUBSET_KEYS_6 = new String[]{"H","V","B",""};
	private final String[] SUBSET_KEYS_7 = new String[]{"O","P","L",""};
	private final String[] SUBSET_KEYS_8 = new String[]{"K","N","M",""};
	
	private int mButtonPressed = 0;
	private int mSelectedGroup = 0;
	
	private TextView mSelectedLetter = null;
	
	private TextView mOverlayO = null;
	private TextView mOverlayU = null;
	private TextView mOverlayY = null;
	private TextView mOverlayA = null;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		FrameLayout content = (FrameLayout)findViewById(android.R.id.content);
		if (null != content) {
			
			LinearLayout rows = new LinearLayout(this);
			rows.setLayoutParams(new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT));
			rows.setOrientation(LinearLayout.VERTICAL);
			rows.setGravity(Gravity.CENTER);
			
			TextView hint = new TextView(this);
			hint.setGravity(Gravity.CENTER);
			hint.setText("Hold the BUTTON_O with AXIS_LS or AXIS_RS to select a letter.");
			rows.addView(hint);
			
			hint = new TextView(this);
			hint.setGravity(Gravity.CENTER);
			hint.setText("Hold the BUTTON_Y with AXIS_LS to select a letter.");
			rows.addView(hint);
			
			hint = new TextView(this);
			hint.setGravity(Gravity.CENTER);
			hint.setText("Press the BUTTON_L1 for upper-case letters.");
			rows.addView(hint);
			
			hint = new TextView(this);
			hint.setGravity(Gravity.CENTER);
			hint.setText("Press the BUTTON_R1 for lower-case letters.");
			rows.addView(hint);
			
			mSelectedLetter = new TextView(this);
			mSelectedLetter.setGravity(Gravity.CENTER);
			mSelectedLetter.setText("Selected Letter: ???");
			rows.addView(mSelectedLetter);
			
			LinearLayout row = new LinearLayout(this);
			row.setLayoutParams(new LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT));
			row.setOrientation(LinearLayout.HORIZONTAL);
			addKeys(row, LAYOUT_ROW_1);
			rows.addView(row);
			
			row = new LinearLayout(this);
			row.setLayoutParams(new LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT));
			row.setOrientation(LinearLayout.HORIZONTAL);
			addKeys(row, LAYOUT_ROW_2);
			rows.addView(row);			
			
			row = new LinearLayout(this);
			row.setLayoutParams(new LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT));
			row.setOrientation(LinearLayout.HORIZONTAL);
			addKeys(row, LAYOUT_ROW_3);
			rows.addView(row);
			
			content.addView(rows);
			
			mOverlayO = new TextView(this);
			mOverlayO.setText("[O]");
			content.addView(mOverlayO);
			
			mOverlayU = new TextView(this);
			mOverlayU.setText("[U]");
			content.addView(mOverlayU);
			
			mOverlayY = new TextView(this);
			mOverlayY.setText("[Y]");
			content.addView(mOverlayY);
			
			mOverlayA = new TextView(this);
			mOverlayA.setText("[A]");
			content.addView(mOverlayA);
			
			hideOverlay();
		}
	}
	
	@Override
	protected void onStart() {
		super.onStart();
		setDefaultBackgroundColor();
		setDefaultColor();
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		if (keyCode == OuyaController.BUTTON_L1) { 
			for (TextView t : mKeys) {
				String text = t.getText().toString();
				t.setText(text.toUpperCase());
			}
		} else if (keyCode == OuyaController.BUTTON_R1) { 
			for (TextView t : mKeys) {
				String text = t.getText().toString();
				t.setText(text.toLowerCase());
			}
		} else {
			if (keyEvent.getRepeatCount() == 0) {			
				if (mSelectedGroup > 4) {
					selectLetter(keyCode);
				} else {
					if (keyCode == OuyaController.BUTTON_O) {		
						mButtonPressed = OuyaController.BUTTON_O;
						setSelectedGroup(0);
					} else if (keyCode == OuyaController.BUTTON_Y) {		
						mButtonPressed = OuyaController.BUTTON_Y;
						setSelectedGroup(0);
					}
					showSelectedGroup();
				}
			}
		}
		return true;
	}
	
	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		if (keyCode == OuyaController.BUTTON_O &&
			mButtonPressed == OuyaController.BUTTON_O) {
			mButtonPressed = 0;
			showSelectedGroup();
			hideInactiveKeys();
		} else if (keyCode == OuyaController.BUTTON_Y &&
			mButtonPressed == OuyaController.BUTTON_Y) {
			mButtonPressed = 0;
			showSelectedGroup();
			hideInactiveKeys();
		}
		return true;
	}
	
	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		
		float lx = motionEvent.getAxisValue(OuyaController.AXIS_LS_X);
		float ly = motionEvent.getAxisValue(OuyaController.AXIS_LS_Y);
		float rx = motionEvent.getAxisValue(OuyaController.AXIS_RS_X);
		float ry = motionEvent.getAxisValue(OuyaController.AXIS_RS_Y);
		
		if (mButtonPressed == OuyaController.BUTTON_O) {		
			if (Math.abs(lx) > DEADZONE &&
				Math.abs(ly) > DEADZONE) {
				if (lx < 0 &&
					ly < 0) {
					setSelectedGroup(5);
				} else if (lx < 0 &&
					ly > 0) {
					setSelectedGroup(6);
				} else if (lx > 0 &&
					ly < 0) {
					setSelectedGroup(7);
				} else if (lx > 0 &&
					ly > 0) {
					setSelectedGroup(8);
				}
			} else if (Math.abs(rx) > DEADZONE &&
				Math.abs(ry) > DEADZONE) {
				if (rx < 0 &&
					ry < 0) {
					setSelectedGroup(9);
				} else if (rx < 0 &&
					ry > 0) {
					setSelectedGroup(10);
				} else if (rx > 0 &&
					ry < 0) {
					setSelectedGroup(11);
				} else if (rx > 0 &&
					ry > 0) {
					setSelectedGroup(12);
				}
			} else {
				setSelectedGroup(0);
				hideOverlay();
			}
		} else if (mButtonPressed == OuyaController.BUTTON_Y) {
			if (Math.abs(lx) > DEADZONE &&
				Math.abs(ly) > DEADZONE) {
				if (lx < 0 &&
					ly < 0) {
					setSelectedGroup(9);
				} else if (lx < 0 &&
					ly > 0) {
					setSelectedGroup(10);
				} else if (lx > 0 &&
					ly < 0) {
					setSelectedGroup(11);
				} else if (lx > 0 &&
					ly > 0) {
					setSelectedGroup(12);
				}			
			} else {
				setSelectedGroup(0);
				hideOverlay();
			}
		} else if (mSelectedGroup == 0) {
			if (Math.abs(lx) > DEADZONE) {
				if (lx < 0) {
					for (String key : LAYOUT_ROW_1) {
						mKeyMap.get(key).setText(key);
					}
				} else {
					int index = 0;
					for (String key : LAYOUT_ROW_1) {
						mKeyMap.get(key).setText(LAYOUT_ROW_4[index]);
						++index;
					}
				}
			}
		}
		
		return true;
	}
	
	private String[] getLetters() {
		switch (mSelectedGroup) {
		case 5:
			return SUBSET_KEYS_1;
		case 6:
			return SUBSET_KEYS_2;
		case 7:
			return SUBSET_KEYS_3;
		case 8:
			return SUBSET_KEYS_4;
		case 9:
			return SUBSET_KEYS_5;
		case 10:
			return SUBSET_KEYS_6;
		case 11:
			return SUBSET_KEYS_7;
		case 12:
			return SUBSET_KEYS_8;
		}
		return null;
	}
	
	// Use the O U Y A buttons to select the letter
	private void selectLetter(int keyCode) {
		
		String[] letters = getLetters();		
		if (null == letters) {
			return;
		}
		
		switch (keyCode) {
		case OuyaController.BUTTON_O:
			if (!letters[2].equals("")) {
				mSelectedLetter.setText("Selected: "+mKeyMap.get(letters[2]).getText().toString());				
				setSelectedGroup(0);
				hideOverlay();
				showAllKeys();
			}
			break;
		case OuyaController.BUTTON_U:
			if (!letters[0].equals("")) {
				mSelectedLetter.setText("Selected: "+mKeyMap.get(letters[0]).getText().toString());
				setSelectedGroup(0);
				hideOverlay();
				showAllKeys();
			}
			break;
		case OuyaController.BUTTON_Y:
			if (!letters[1].equals("")) {
				mSelectedLetter.setText("Selected: "+mKeyMap.get(letters[1]).getText().toString());
				setSelectedGroup(0);
				hideOverlay();
				showAllKeys();
			}
			break;
		case OuyaController.BUTTON_A:
			if (!letters[3].equals("")) {
				mSelectedLetter.setText("Selected: "+mKeyMap.get(letters[3]).getText().toString());
				setSelectedGroup(0);
				hideOverlay();
				showAllKeys();
			}
			break;
		}
	}
	
	private void addKeys(LinearLayout layout, String[] keys) {
		for (String key : keys) {
			if (key.equals("")) {
				continue;
			}
			LinearLayout border = new LinearLayout(this);
			border.setPadding(10, 10, 10, 10);
			TextView t = new TextView(this);
			t.setText(key);
			t.setTextSize(36);
			t.setTypeface(null, Typeface.BOLD);
			t.setWidth(125);
			t.setHeight(125);
			t.setGravity(Gravity.CENTER);
			border.addView(t);
			layout.addView(border);
			mKeys.add(t);
			mKeyMap.put(key.toUpperCase(), t);
		}
	}
	
	private void setBackgroundColors(String colorString, String[] keys) {
		int c = Color.parseColor(colorString);
		for (String key : keys) {
			if (key.equals("")) {
				continue;
			}
			TextView t = mKeyMap.get(key);
			if (null != t) {
				t.setBackgroundColor(c);
			}
		}
	}
	
	private void setColors(String colorString, String[] keys) {
		int c = Color.parseColor(colorString);
		for (String key : keys) {
			if (key.equals("")) {
				continue;
			}
			TextView t = mKeyMap.get(key);
			if (null != t) {
				t.setTextColor(c);
			}
		}
	}
	
	private void setDefaultBackgroundColor() {
		setBackgroundColors(COLOR_LEFT_A, SUBSET_KEYS_1);
		setBackgroundColors(COLOR_LEFT_A, SUBSET_KEYS_2);
		setBackgroundColors(COLOR_LEFT_B, SUBSET_KEYS_3);
		setBackgroundColors(COLOR_LEFT_B, SUBSET_KEYS_4);
		setBackgroundColors(COLOR_RIGHT_A, SUBSET_KEYS_5);
		setBackgroundColors(COLOR_RIGHT_A, SUBSET_KEYS_6);
		setBackgroundColors(COLOR_RIGHT_B, SUBSET_KEYS_7);
		setBackgroundColors(COLOR_RIGHT_B, SUBSET_KEYS_8);
	}
	
	private void setDefaultColor() {
		setColors(COLOR_NORMAL, SUBSET_KEYS_1);
		setColors(COLOR_NORMAL, SUBSET_KEYS_2);
		setColors(COLOR_NORMAL, SUBSET_KEYS_3);
		setColors(COLOR_NORMAL, SUBSET_KEYS_4);
		setColors(COLOR_NORMAL, SUBSET_KEYS_5);
		setColors(COLOR_NORMAL, SUBSET_KEYS_6);
		setColors(COLOR_NORMAL, SUBSET_KEYS_7);
		setColors(COLOR_NORMAL, SUBSET_KEYS_8);
	}
	
	private void setInactiveColor() {
		setColors(COLOR_INACTIVE, SUBSET_KEYS_1);
		setColors(COLOR_INACTIVE, SUBSET_KEYS_2);
		setColors(COLOR_INACTIVE, SUBSET_KEYS_3);
		setColors(COLOR_INACTIVE, SUBSET_KEYS_4);
		setColors(COLOR_INACTIVE, SUBSET_KEYS_5);
		setColors(COLOR_INACTIVE, SUBSET_KEYS_6);
		setColors(COLOR_INACTIVE, SUBSET_KEYS_7);
		setColors(COLOR_INACTIVE, SUBSET_KEYS_8);
	}
	
	private void showSelectedGroup() {
		if (mSelectedGroup != 0) {
			setInactiveColor();			
		}
		switch (mSelectedGroup) {
		case 0:
			setDefaultBackgroundColor();
			if (mButtonPressed != 0) {
				setInactiveColor();
			} else {
				setDefaultColor();
			}
			break;
		case 5:
			setColors(COLOR_NORMAL, SUBSET_KEYS_1);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_2);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_3);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_4);
			break;
		case 6:
			setColors(COLOR_SKIPPED, SUBSET_KEYS_1);
			setColors(COLOR_NORMAL, SUBSET_KEYS_2);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_3);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_4);
			break;
		case 7:
			setColors(COLOR_SKIPPED, SUBSET_KEYS_1);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_2);
			setColors(COLOR_NORMAL, SUBSET_KEYS_3);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_4);
			break;
		case 8:
			setColors(COLOR_SKIPPED, SUBSET_KEYS_1);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_2);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_3);
			setColors(COLOR_NORMAL, SUBSET_KEYS_4);
			break;
		case 9:
			setColors(COLOR_NORMAL, SUBSET_KEYS_5);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_6);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_7);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_8);
			break;
		case 10:
			setColors(COLOR_SKIPPED, SUBSET_KEYS_5);
			setColors(COLOR_NORMAL, SUBSET_KEYS_6);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_7);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_8);
			break;
		case 11:
			setColors(COLOR_SKIPPED, SUBSET_KEYS_5);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_6);
			setColors(COLOR_NORMAL, SUBSET_KEYS_7);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_8);
			break;
		case 12:
			setColors(COLOR_SKIPPED, SUBSET_KEYS_5);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_6);
			setColors(COLOR_SKIPPED, SUBSET_KEYS_7);
			setColors(COLOR_NORMAL, SUBSET_KEYS_8);
			break;
		}
		
		String[] letters = getLetters();
		if (null != letters) {
			moveToLetter(letters[2], mOverlayO);
			moveToLetter(letters[0], mOverlayU);
			moveToLetter(letters[1], mOverlayY);
			moveToLetter(letters[3], mOverlayA);
		}
	}
	
	private void moveToLetter(final String letter, TextView overlay) {
		if (letter.equals("")) {
			overlay.setVisibility(View.INVISIBLE);
			return;
		}
		
		overlay.setVisibility(View.VISIBLE);
		
		TextView t = mKeyMap.get(letter);
		int[] location = new int[2];
		t.getLocationOnScreen(location);
		overlay.setX(location[0]);
		overlay.setY(location[1]);		
	}
	
	private void setSelectedGroup(int group) {
		//Log.i(TAG, "Set Group: " + group);
		mSelectedGroup = group;
		showSelectedGroup();
	}
	
	private void hideOverlay() {
		mOverlayO.setVisibility(View.INVISIBLE);
		mOverlayU.setVisibility(View.INVISIBLE);
		mOverlayY.setVisibility(View.INVISIBLE);
		mOverlayA.setVisibility(View.INVISIBLE);
	}
	
	private void showAllKeys() {
		setDefaultBackgroundColor();
	}
	
	private void hideInactiveKeys() {
		
		if (mSelectedGroup == 0) {
			return;
		}
		
		setBackgroundColors(COLOR_INACTIVE, SUBSET_KEYS_1);
		setBackgroundColors(COLOR_INACTIVE, SUBSET_KEYS_2);
		setBackgroundColors(COLOR_INACTIVE, SUBSET_KEYS_3);
		setBackgroundColors(COLOR_INACTIVE, SUBSET_KEYS_4);
		setBackgroundColors(COLOR_INACTIVE, SUBSET_KEYS_5);
		setBackgroundColors(COLOR_INACTIVE, SUBSET_KEYS_6);
		setBackgroundColors(COLOR_INACTIVE, SUBSET_KEYS_7);
		setBackgroundColors(COLOR_INACTIVE, SUBSET_KEYS_8);
		
		switch (mSelectedGroup) {
		case 5:
			setBackgroundColors(COLOR_LEFT_A, SUBSET_KEYS_1);
			break;
		case 6:
			setBackgroundColors(COLOR_LEFT_A, SUBSET_KEYS_2);
			break;
		case 7:
			setBackgroundColors(COLOR_LEFT_B, SUBSET_KEYS_3);
			break;
		case 8:
			setBackgroundColors(COLOR_LEFT_B, SUBSET_KEYS_4);
			break;
		case 9:
			setBackgroundColors(COLOR_RIGHT_A, SUBSET_KEYS_5);
			break;
		case 10:
			setBackgroundColors(COLOR_RIGHT_A, SUBSET_KEYS_6);
			break;
		case 11:
			setBackgroundColors(COLOR_RIGHT_B, SUBSET_KEYS_7);
			break;
		case 12:
			setBackgroundColors(COLOR_RIGHT_B, SUBSET_KEYS_8);
			break;
		}
	}
}
