using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace InputView
{
	[Activity (Label = "Xamarin InputView", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar.Fullscreen")]
	[IntentFilter(new[] { Intent.ActionMain }
		, Categories = new[] { Intent.CategoryLauncher, CategoryGame })]
	public class MainActivity : Activity
	{
		private const String TAG = "MainActivity";
		private const String CategoryGame = "tv.ouya.intent.category.GAME";
		private List<VirtualControllerView> _controllers = new List<VirtualControllerView> ();
		private bool _keepRunning = true;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			using (var ignore = new TV.Ouya.Sdk.OuyaInputView(this))
			{
				// do nothing
			}

			FrameLayout content = (FrameLayout)FindViewById(Android.Resource.Id.Content);
			if (null == content) {
				Log.Info(TAG, "Failed to get content");
				return;
			}

			RelativeLayout relativeLayout = new RelativeLayout (this);
			content.AddView (relativeLayout);
			FrameLayout.LayoutParams relativeLayoutParams = new FrameLayout.LayoutParams (FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent);
			relativeLayout.LayoutParameters = relativeLayoutParams;
			relativeLayout.SetPadding(0, 0, 0, 0);

			LinearLayout verticalLayout = new LinearLayout (this);
			relativeLayout.AddView (verticalLayout);
			RelativeLayout.LayoutParams verticalLayoutParams = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
			verticalLayout.LayoutParameters = verticalLayoutParams;
			verticalLayout.Orientation = Orientation.Vertical;
			verticalLayout.SetGravity (GravityFlags.Center);
			verticalLayout.SetPadding(0, 0, 0, 0);

			for (int y = 0; y < 2; ++y) {
				LinearLayout horizontalLayout = new LinearLayout (this);
				verticalLayout.AddView (horizontalLayout);
				LinearLayout.LayoutParams horizontalLayoutParams = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
				horizontalLayout.LayoutParameters = horizontalLayoutParams;
				horizontalLayout.Orientation = Orientation.Horizontal;
				horizontalLayout.SetGravity (GravityFlags.Center);
				horizontalLayout.SetPadding(0, 0, 0, 0);

				int[] resources = {
					Resource.Drawable.controller,
					Resource.Drawable.a,
					Resource.Drawable.dpad_down,
					Resource.Drawable.dpad_left,
					Resource.Drawable.dpad_right,
					Resource.Drawable.dpad_up,
					Resource.Drawable.lb,
					Resource.Drawable.lt,
					Resource.Drawable.l_stick,
					Resource.Drawable.menu,
					Resource.Drawable.o,
					Resource.Drawable.rb,
					Resource.Drawable.rt,
					Resource.Drawable.r_stick,
					Resource.Drawable.thumbl,
					Resource.Drawable.thumbr,
					Resource.Drawable.u,
					Resource.Drawable.y,
				};

				for (int x = 0; x < 2; ++x) {
					RelativeLayout controllerView = new RelativeLayout (this);
					horizontalLayout.AddView (controllerView);
					LinearLayout.LayoutParams controllerLayoutParams = new LinearLayout.LayoutParams (512, 512);
					controllerView.LayoutParameters = controllerLayoutParams;
					controllerView.SetGravity (GravityFlags.Center);
					controllerView.SetPadding(100, 0, 0, 0);

					Dictionary<int, ImageView> views = new Dictionary<int, ImageView> ();

					foreach (int resourceId in resources) {

						ImageView imageView = new ImageView (this);
						controllerView.AddView (imageView);
						imageView.SetImageResource (resourceId);
						views [resourceId] = imageView;
					}

					VirtualControllerView controller = new VirtualControllerView (_controllers.Count, 
						views[Resource.Drawable.controller],
						views[Resource.Drawable.a],
						views[Resource.Drawable.dpad_down],
						views[Resource.Drawable.dpad_left],
						views[Resource.Drawable.dpad_right],
						views[Resource.Drawable.dpad_up],
						views[Resource.Drawable.lb],
						views[Resource.Drawable.lt],
						views[Resource.Drawable.l_stick],
						views[Resource.Drawable.menu],
						views[Resource.Drawable.o],
						views[Resource.Drawable.rb],
						views[Resource.Drawable.rt],
						views[Resource.Drawable.r_stick],
						views[Resource.Drawable.thumbl],
						views[Resource.Drawable.thumbr],
						views[Resource.Drawable.u],
						views[Resource.Drawable.y]);

					_controllers.Add (controller);
				}
			}

			ThreadStart ts = new ThreadStart (UpdateWorker);
			Thread thread = new Thread (ts);
			thread.Start ();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_keepRunning = false;
		}

		void UpdateWorker()
		{
			while (_keepRunning) {
				Action action = () => {
					foreach (VirtualControllerView controller in _controllers) {
						controller.Update ();
					}

					NdkWrapper.clearButtonStates();
				};

				RunOnUiThread (action);
				Thread.Sleep (10);
			}
		}
	}
}
