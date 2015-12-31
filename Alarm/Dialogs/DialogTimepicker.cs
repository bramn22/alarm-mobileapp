using System;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using Java.Lang;

namespace Alarm
{
	public class DialogTimepicker : DialogFragment
	{
		private TimePicker timepicker;
		private Button saveButton;
		private Button cancelButton;

		TimeSpan time;

		public event EventHandler<TimeSetEventArgs> timeSetEventHandler;

		public DialogTimepicker (TimeSpan time)
		{
			this.time = time;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) 
		{
			base.OnCreateView (inflater, container, savedInstanceState);	

			var view = inflater.Inflate (Resource.Layout.DialogTimepicker, container, false);

			saveButton = view.FindViewById <Button>(Resource.Id.timepicker_button_save);
			cancelButton = view.FindViewById <Button>(Resource.Id.timepicker_button_cancel);
			timepicker = view.FindViewById <TimePicker>(Resource.Id.timepicker);

			saveButton.Click += Save_Click;
			cancelButton.Click += Cancel_Click;

			timepicker.SetIs24HourView ((Java.Lang.Boolean)true);
			timepicker.CurrentHour = (Integer)time.Hours;
			timepicker.CurrentMinute = (Integer)time.Minutes;
			return view;
		}

		void Save_Click(object sender, EventArgs e) 
		{
			timeSetEventHandler.Invoke (this, new TimeSetEventArgs (timepicker.CurrentHour.IntValue(), timepicker.CurrentMinute.IntValue()));
			this.Dismiss ();
		}

		void Cancel_Click(object sender, EventArgs e) 
		{
			this.Dismiss ();
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle);
			base.OnActivityCreated (savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.timepicker_animation;
		}
	}

	public class TimeSetEventArgs : EventArgs
	{
		public TimeSetEventArgs(int hour, int minute) : base()
		{
			Hour = hour;
			Minute = minute;
		}

		public int Hour {
			get;
			set;
		}

		public int Minute {
			get;
			set;
		}
	}
}

