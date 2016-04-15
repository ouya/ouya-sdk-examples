package tv.ouya.examples.android.assetvideoplayer;

import android.app.Activity;
import android.content.res.AssetFileDescriptor;
import android.content.res.AssetManager;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.util.Log;
import android.view.SurfaceHolder;
import android.widget.FrameLayout;
import android.widget.VideoView;

import java.io.IOException;

/**
 * Created by tgraupmann on 4/15/2016.
 */
public class MainActivity extends Activity implements
        MediaPlayer.OnPreparedListener, MediaPlayer.OnCompletionListener,
        SurfaceHolder.Callback
{
    private final static String TAG = MainActivity.class.getSimpleName();
    private final String VIDEO_ASSET = "video.mp4";
    private MediaPlayer mMediaPlayer = null;
    private VideoView mVideoView = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main_layout);

        FrameLayout content = (FrameLayout)findViewById(android.R.id.content);
        if (null != content) {
            mVideoView = new VideoView(this);
            content.addView(mVideoView);

            mMediaPlayer = new MediaPlayer();
            mMediaPlayer.setOnPreparedListener(this);
            mMediaPlayer.setOnCompletionListener(this);

            SurfaceHolder holder = mVideoView.getHolder();
            holder.addCallback(this);
        }
    }

    private void playVideo()
    {
        new Thread(new Runnable()
        {
            public void run()
            {
                try
                {
                    AssetFileDescriptor afd = null;
                    try {
                        AssetManager assetManager = getAssets();
                        afd = assetManager.openFd(VIDEO_ASSET);
                    } catch (IOException e) {
                        // TODO Auto-generated catch block
                        e.printStackTrace();
                    }
                    if (null == afd) {
                        Log.e(TAG, "Failed to load asset: " + VIDEO_ASSET);
                        return;
                    } else {
                        try {
                            mMediaPlayer.setDataSource(afd.getFileDescriptor(),
                                    afd.getStartOffset(),
                                    afd.getLength());
                            mMediaPlayer.prepareAsync();
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
                    }
                }
                catch (Exception e)
                {
                    Log.e(TAG, "Error while playing video: " + VIDEO_ASSET);
                    e.printStackTrace();
                }
            }
        }).start();
    }

    @Override
    public void surfaceCreated(SurfaceHolder holder) {
        playVideo();
        mMediaPlayer.setDisplay(holder);
    }

    @Override
    public void surfaceChanged(SurfaceHolder holder, int format, int width, int height) {

    }

    @Override
    public void surfaceDestroyed(SurfaceHolder holder) {

    }

    @Override
    public void onCompletion(MediaPlayer mp) {
        finish();
    }

    @Override
    public void onPrepared(MediaPlayer mp) {
        mMediaPlayer.start();
    }
}
