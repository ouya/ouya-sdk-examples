package tv.ouya.examples.android.oggexample;

import java.io.IOException;

import android.media.MediaPlayer;
import android.os.Bundle;
import android.app.Activity;
import android.content.res.AssetFileDescriptor;
import android.content.res.AssetManager;
import android.view.View;
//import android.util.Log;
import android.widget.Button;

public class MainActivity extends Activity {
	
	private final long TICKS_SECOND = 1000000000;
	
	public enum States {
		None,
		FadeIn,
		FadeOut,
	}
	
	public class CustomMediaPlayer extends MediaPlayer
	{
		public States mState = States.None;
		public long mTimer = 0;
		
		public void update() {
			if (mTimer == 0) {
				return;
			}
			switch (mState) {
			case FadeIn:
				if (mTimer < System.nanoTime()) {
					setVolume(1f, 1f);
					mTimer = 0;
				} else {
					float volume = 1f - (mTimer - System.nanoTime()) / (float)TICKS_SECOND;
					//Log.i(TAG, "Volume: "+volume);
					setVolume(volume, volume);
				}
				break;
			case FadeOut:
				if (mTimer < System.nanoTime()) {
					if (isPlaying()) {
						stop();
					}
					setVolume(0f, 0f);
					mTimer = 0;
				} else {
					float volume = (mTimer - System.nanoTime()) / (float)TICKS_SECOND;
					//Log.i(TAG, "Volume: "+volume);
					setVolume(volume, volume);
				}
				break;
			default:
				break;
			}
		}
	}
	
	private static final String TAG = "MainActivity";
	
	private boolean mRunning = true;
	
	private static final String SOUND_1 = "sound1.ogg";
	private static final String SOUND_2 = "sound2.ogg";
	
	private Button btnCrossFade1 = null;
	private Button btnCrossFade2 = null;
	private Button btnFadeIn1 = null;
	private Button btnFadeIn2 = null;
	private Button btnFadeOut1 = null;
	private Button btnFadeOut2 = null;
	
	private CustomMediaPlayer mpSound1 = null;
	private CustomMediaPlayer mpSound2 = null;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		View content = (View)findViewById(android.R.id.content);
        if (null != content) {
            // Disable screensaver
            content.setKeepScreenOn(true);
        }
		
		// get the buttons
		btnCrossFade1 = (Button)findViewById(R.id.btnCrossFade1);
		btnCrossFade2 = (Button)findViewById(R.id.btnCrossFade2);
		btnFadeIn1 = (Button)findViewById(R.id.btnFadeIn1);
		btnFadeIn2 = (Button)findViewById(R.id.btnFadeIn2);
		btnFadeOut1 = (Button)findViewById(R.id.btnFadeOut1);
		btnFadeOut2 = (Button)findViewById(R.id.btnFadeOut2);
		
		// create media players
		mpSound1 = new CustomMediaPlayer();
		mpSound2 = new CustomMediaPlayer();
		
		// set up events
		
		btnCrossFade1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	crossFade(mpSound2, mpSound1, SOUND_1);
            }
        });
		
		btnCrossFade2.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	crossFade(mpSound1, mpSound2, SOUND_2);
            }
        });
		
		btnFadeIn1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	fadeIn(mpSound1, SOUND_1);
            }
        });
		
		btnFadeIn2.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	fadeIn(mpSound2, SOUND_2);
            }
        });
		
		btnFadeOut1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	fadeOut(mpSound1);
            }
        });
		
		btnFadeOut2.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	fadeOut(mpSound2);
            }
        });		
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();
		
		mRunning = false;
		
		if (null != mpSound1) {
			if (mpSound1.isPlaying()) {
				mpSound1.stop();
			}
			mpSound1.release();
    	}
		
		if (null != mpSound2) {
			if (mpSound2.isPlaying()) {
				mpSound2.stop();
			}
			mpSound2.release();
    	}
	}
	
	@Override
	public void onStart() {
		super.onStart();
		
		Thread timer = new Thread() {
            public void run() {
                while (mRunning) {
                    Runnable runnable = new Runnable()
                    {
                        public void run()
                        {
                        	if (null != mpSound1) {
                        		mpSound1.update();
                        	}
                        	if (null != mpSound2) {
                        		mpSound2.update();
                        	}
                        }
                    };

                    runOnUiThread(runnable);

                    try {
                        Thread.sleep(30);
                    } catch (InterruptedException e) {
                        // TODO Auto-generated catch block
                        e.printStackTrace();
                    }
                }
            }
        };
        timer.start();
	}
	
	private void fadeIn(CustomMediaPlayer fadeInSound, String soundFile) {
		if (null != fadeInSound) {
			if (fadeInSound.isPlaying()) {
				fadeInSound.stop();
			}
			fadeInSound.mState = States.FadeIn;
			fadeInSound.mTimer = System.nanoTime() + TICKS_SECOND;
			fadeInSound.setVolume(0f, 0f);
			playSound(fadeInSound, soundFile);
		}
	}
	
	private void fadeOut(CustomMediaPlayer fadeOutSound) {
		if (null != fadeOutSound) {
			fadeOutSound.mState = States.FadeOut;
			fadeOutSound.mTimer = System.nanoTime() + TICKS_SECOND;
			fadeOutSound.setVolume(1f, 1f);
		}
	}
	
	private void crossFade(CustomMediaPlayer fadeOutSound, CustomMediaPlayer fadeInSound, String soundFile) {
		if (null != fadeOutSound) {
			fadeOutSound.mState = States.FadeOut;
			fadeOutSound.mTimer = System.nanoTime() + TICKS_SECOND;
			fadeOutSound.setVolume(1f, 1f);
		}
		
		if (null != fadeInSound) {
			fadeInSound.mState = States.FadeIn;
			fadeInSound.mTimer = System.nanoTime() + TICKS_SECOND;
			fadeInSound.setVolume(0f, 0f);
			if (!fadeInSound.isPlaying()) {
				playSound(fadeInSound, soundFile);
			}
		}
	}
	
	private void playSound(CustomMediaPlayer mp, String asset) {
		if (null == mp) {
			return;
		}
		
		AssetManager assetManager = getApplicationContext().getAssets();
		
		AssetFileDescriptor afd = null;
		try {
			afd = assetManager.openFd(asset);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		if (null == afd) {
			return;
		}
		
		mp.reset();
		try {
			mp.setDataSource(afd.getFileDescriptor(),
					afd.getStartOffset(),
					afd.getLength());
		} catch (IllegalArgumentException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IllegalStateException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		try {
			mp.prepare();
		} catch (IllegalStateException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		try {
			afd.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		mp.start();
	}
}
