/*
 * Copyright (C) 2012-2014 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package tv.ouya.examples.android.virtualcontroller;

import android.app.Activity;
import android.content.Context;
import android.graphics.drawable.Drawable;
import android.os.Handler;
import android.util.AttributeSet;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import tv.ouya.console.api.DebugInput;
import tv.ouya.console.api.OuyaController;
import tv.ouya.console.api.OuyaInputMapper;

import java.util.HashMap;

public class ControllerView extends RelativeLayout {
	
	private static final String TAG = ControllerView.class.getSimpleName();
	
	private static final float DEAD_ZONE = 0.25f;

    private static HashMap<Integer, Integer> buttons;
    static {
        buttons = new HashMap<Integer, Integer>();
        buttons.put(OuyaController.BUTTON_O, R.drawable.o);
        buttons.put(OuyaController.BUTTON_U, R.drawable.u);
        buttons.put(OuyaController.BUTTON_Y, R.drawable.y);
        buttons.put(OuyaController.BUTTON_A, R.drawable.a);

        buttons.put(OuyaController.BUTTON_DPAD_DOWN, R.drawable.dpad_down);
        buttons.put(OuyaController.BUTTON_DPAD_LEFT, R.drawable.dpad_left);
        buttons.put(OuyaController.BUTTON_DPAD_UP, R.drawable.dpad_up);
        buttons.put(OuyaController.BUTTON_DPAD_RIGHT, R.drawable.dpad_right);

        buttons.put(OuyaController.BUTTON_R1, R.drawable.rb);
        buttons.put(OuyaController.BUTTON_L1, R.drawable.lb);
        buttons.put(OuyaController.BUTTON_R2, R.drawable.rt);
        buttons.put(OuyaController.BUTTON_L2, R.drawable.lt);
        
        buttons.put(OuyaController.BUTTON_MENU, R.drawable.menu);
    }

    private Drawable rstick;
    private Drawable lstick;
    private Drawable thumbr;
    private Drawable thumbl;
    private ImageView rstickView = new ImageView(getContext());
    private ImageView lstickView = new ImageView(getContext());

    public ControllerView(Context context, AttributeSet attrs) {
    	super(context, attrs);
        init();
    }

    public ControllerView(Context context, AttributeSet attrs, int defStyle) {
    	super(context, attrs, defStyle);
        init();
    }

    public ControllerView(Context context) {
        super(context);
        init();
    }

    private void init() {
       	
        ImageView controllerView = new ImageView(getContext());
        controllerView.setImageDrawable(getContext().getResources().getDrawable(R.drawable.controller));
        addView(controllerView);

        lstick = getContext().getResources().getDrawable(R.drawable.l_stick);
        thumbl = getContext().getResources().getDrawable(R.drawable.thumbl);
        lstickView.setImageDrawable(lstick);
        addView(lstickView);

        rstick = getContext().getResources().getDrawable(R.drawable.r_stick);
        thumbr = getContext().getResources().getDrawable(R.drawable.thumbr);
        rstickView.setImageDrawable(rstick);
        addView(rstickView);

        OuyaController.init(getContext());

        for(Integer button : buttons.keySet()) {
            Integer resId = buttons.get(button);
            ImageView buttonView = new ImageView(getContext());

            buttonView.setImageDrawable(getContext().getResources().getDrawable(resId));
            buttonView.setId(button);
            if(button != OuyaController.BUTTON_L2 && button != OuyaController.BUTTON_R2)
                buttonView.setVisibility(View.INVISIBLE);
            else
                buttonView.setAlpha(0f);

            addView(buttonView);
        }
    }
    
    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {    	
    	//Log.i(TAG, "onKeyDown keyCode=" + keyCode);
    	
    	setVisibility(View.VISIBLE);
    	
        if(keyCode == OuyaController.BUTTON_L3) {
            lstickView.setImageDrawable(thumbl);
            return true;
        } else if (keyCode == OuyaController.BUTTON_R3) {
            rstickView.setImageDrawable(thumbr);
            return true;
        } else if (findViewById(keyCode) != null && keyCode != OuyaController.BUTTON_L2 && keyCode != OuyaController.BUTTON_R2) {
            View v = findViewById(keyCode);
            v.setVisibility(View.VISIBLE);
            return true;
        } else {
            return super.onKeyDown(keyCode, event);
        }
    }

    @Override
    public boolean onKeyUp(int keyCode, KeyEvent event) {
    	//Log.i(TAG, "onKeyUp keyCode=" + keyCode);
   	
    	setVisibility(View.VISIBLE);
    	
        if(keyCode == OuyaController.BUTTON_L3) {
            lstickView.setImageDrawable(lstick);
            return true;
        } else if (keyCode == OuyaController.BUTTON_R3) {
            rstickView.setImageDrawable(rstick);
            return true;
        } else if (findViewById(keyCode) != null && keyCode == OuyaController.BUTTON_MENU) {
        	final View v = findViewById(keyCode);
        	v.setVisibility(View.VISIBLE);
        	final Handler handler = new Handler();
        	handler.postDelayed(new Runnable() {
        	  @Override
        	  public void run() {
        		  v.setVisibility(View.INVISIBLE);
        	  }
        	}, 1000);
            return true;
        } else if (findViewById(keyCode) != null && keyCode != OuyaController.BUTTON_L2 && keyCode != OuyaController.BUTTON_R2) {
            View v = findViewById(keyCode);
            v.setVisibility(View.INVISIBLE);
            return true;
        } else {
            return super.onKeyUp(keyCode, event);
        }
    }

    @Override
    public boolean onGenericMotionEvent(MotionEvent event) {
    	//Log.i(TAG, "onGenericMotionEvent");
    	//DebugInput.debugOuyaMotionEvent(event);
    	
    	setVisibility(View.VISIBLE);
    	
    	//rotate Left Stick input by N degrees to match image orientation
    	float degrees = 135f;
    	float radians = degrees / 180f * 3.14f;
    	float cs = (float)Math.cos(radians);
    	float sn = (float)Math.sin(radians);

    	float x = event.getAxisValue(OuyaController.AXIS_LS_X);
    	float y = event.getAxisValue(OuyaController.AXIS_LS_Y);

    	lstickView.setTranslationX((x * cs - y * sn) * 5f);
    	lstickView.setTranslationY((x * sn + y * cs) * 5f);

    	//rotate Right Stick by same degrees to match image orientation
    	x = event.getAxisValue(OuyaController.AXIS_RS_X);
    	y = event.getAxisValue(OuyaController.AXIS_RS_Y);

    	rstickView.setTranslationX((x * cs - y * sn) * 5f);
    	rstickView.setTranslationY((x * sn + y * cs) * 5f);

        float ltrigger = event.getAxisValue(OuyaController.AXIS_L2);
        if(ltrigger > DEAD_ZONE) {
            findViewById(OuyaController.BUTTON_L2).setAlpha(1f);
        } else {
            findViewById(OuyaController.BUTTON_L2).setAlpha(0f);
        }

        float rtrigger = event.getAxisValue(OuyaController.AXIS_R2);
        if(rtrigger > DEAD_ZONE) {
            findViewById(OuyaController.BUTTON_R2).setAlpha(1f);
        } else {
            findViewById(OuyaController.BUTTON_R2).setAlpha(0f);
        }

        return true;
    }
}
