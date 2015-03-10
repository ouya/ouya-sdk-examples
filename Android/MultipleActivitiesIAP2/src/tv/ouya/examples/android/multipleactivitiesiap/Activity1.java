/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
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
package tv.ouya.examples.android.multipleactivitiesiap;

import java.util.Collection;
import java.util.List;

import tv.ouya.console.api.GamerInfo;
import tv.ouya.console.api.Product;
import tv.ouya.console.api.PurchaseResult;
import tv.ouya.console.api.Receipt;
import android.content.Intent;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.FrameLayout;
import android.widget.TextView;
import android.os.Bundle;


public class Activity1 extends ActivityCommon
{
	Button mBtnActivity = null;
	Button mBtnRequestGamerInfo = null;
	Button mBtnRequestProducts = null;
	Button mBtnRequestPurchase = null;
	Button mBtnRequestReceipts = null;
	Button mBtnExit = null;
	TextView mTxtStatus = null;
	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        
        setContentView(R.layout.layout_activity1);
        
        FrameLayout content = (FrameLayout)findViewById(android.R.id.content);
        if (null != content) {
            // Disable screensaver
            content.setKeepScreenOn(true);
        }
        
        mBtnActivity = (Button)findViewById(R.id.btnActivity);
        mBtnRequestGamerInfo = (Button)findViewById(R.id.btnRequestGamerInfo);
        mBtnRequestProducts = (Button)findViewById(R.id.btnRequestProducts);
        mBtnRequestPurchase = (Button)findViewById(R.id.btnRequestPurchase);
        mBtnRequestReceipts = (Button)findViewById(R.id.btnRequestReceipts);
        mBtnExit = (Button)findViewById(R.id.btnExit);
        mTxtStatus = (TextView)findViewById(R.id.txtStatus);
        
        mBtnActivity.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent openIntent = new Intent(getPackageName()+".ACTION2");
		        startActivity(openIntent);
		        // don't finish the activity
			}
		});
        
        mBtnRequestGamerInfo.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				mTxtStatus.setText("Status: Requesting GamerInfo...");
				requestGamerInfo();
			}
		});
        
        mBtnRequestProducts.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				mTxtStatus.setText("Status: Requesting Products...");
				requestProducts();
			}
		});
        
        mBtnRequestPurchase.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				mTxtStatus.setText("Status: Requesting Purchase...");
				requestPurchase();
			}
		});
        
        mBtnRequestReceipts.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				mTxtStatus.setText("Status: Requesting Receipts...");
				requestReceipts();
			}
		});
        
        mBtnExit.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				mTxtStatus.setText("Status: Exiting...");
				finish();
			}
		});
    }
    
    @Override
	public void onResume() {
		super.onResume();
		mTxtStatus.setText("Status: Loaded "+getClass().getSimpleName());		
	}
    
	// override this method in the activity to show in the text status field
    @Override
	protected void onSuccessRequestGamerInfo(GamerInfo info) {
    	mTxtStatus.setText("requestGamerInfo onSuccess uuid="+info.getUuid()+" username="+info.getUsername());
	}

	// override this method in the activity to show in the text status field
	@Override
	protected void onFailureRequestGamerInfo(int errorCode, String errorMessage, Bundle optionalData) {
		mTxtStatus.setText("requestGamerInfo onFailure: errorCode="+errorCode+" errorMessage="+errorMessage);
	}
	
	// override this method in the activity to show in the text status field
	@Override
	protected void onSuccessRequestProducts(final List<Product> products) {
		mTxtStatus.setText("requestProducts onSuccess: received "+products.size() + " products");
	}
	
	// override this method in the activity to show in the text status field
	@Override
	protected void onFailureRequestProducts(int errorCode, String errorMessage, Bundle optionalData) {
		mTxtStatus.setText("requestProducts onFailure: errorCode="+errorCode+" errorMessage="+errorMessage);
	}
    
	// override this method in the activity to show in the text status field
    @Override
    protected void onSuccessRequestPurchase(PurchaseResult result) {
    	mTxtStatus.setText("requestPurchase onSuccess: "+result.getProductIdentifier());
	}

	// override this method in the activity to show in the text status field
	@Override
	protected void onFailureRequestPurchase(int errorCode, String errorMessage, Bundle optionalData) {
		mTxtStatus.setText("requestPurchase onFailure: errorCode="+errorCode+" errorMessage="+errorMessage);
	}
	
	// override this method in the activity to show in the text status field
	@Override
	protected void onCancelRequestPurchase() {
		mTxtStatus.setText("requestPurchase onCancel");
	}
    
    // override this method in the activity to show in the text status field
    @Override
    protected void onSuccessRequestReceipts(Collection<Receipt> receipts) {
    	mTxtStatus.setText("requestReceipts onSuccess: received "+receipts.size() + " receipts");
 	}

 	// override this method in the activity to show in the text status field
    @Override
    protected void onFailureRequestReceipts(int errorCode, String errorMessage, Bundle optionalData) {
    	mTxtStatus.setText("requestReceipts onFailure: errorCode="+errorCode+" errorMessage="+errorMessage);
 	}
    
    // override this method in the activity to show in the text status field
    protected void onCancelRequestReceipts() {
    	mTxtStatus.setText("requestReceipts onCancel");
    }
}
