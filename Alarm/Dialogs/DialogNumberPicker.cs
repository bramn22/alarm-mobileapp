using System;
using Android.App;
using Android.Widget;
using Android.Views;
using Android.OS;

namespace Alarm
{
	public class DialogNumberPicker : DialogFragment
	{
		private NumberPicker numberpicker;
		private Button saveButton;
		private Button cancelButton;

		private int n;
		private int id;

		public event EventHandler<NumberSetEventArgs> numberSetEventHandler;

		public DialogNumberPicker (int id, int n)
		{
			this.id = id;
			this.n = n;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) 
		{
			base.OnCreateView (inflater, container, savedInstanceState);	

			var view = inflater.Inflate (Resource.Layout.DialogNumberPicker, container, false);

			saveButton = view.FindViewById <Button>(Resource.Id.numberpicker_button_save);
			cancelButton = view.FindViewById <Button>(Resource.Id.numberpicker_button_cancel);
			numberpicker = view.FindViewById <NumberPicker>(Resource.Id.numberpicker);

			saveButton.Click += Save_Click;
			cancelButton.Click += Cancel_Click;

			numberpicker.MinValue = 1;
			numberpicker.MaxValue = 60;
			numberpicker.Value = n;
			numberpicker.WrapSelectorWheel = true;
			return view;
		}

		void Save_Click(object sender, EventArgs e) 
		{
			numberSetEventHandler.Invoke (this, new NumberSetEventArgs (id, numberpicker.Value));
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

	public class NumberSetEventArgs : EventArgs
	{
		public NumberSetEventArgs(int id, int n) : base()
		{
			ID = id;
			N = n;
		}
		public int ID {
			get;
			set;
		}

		public int N {
			get;
			set;
		}
	}
}

