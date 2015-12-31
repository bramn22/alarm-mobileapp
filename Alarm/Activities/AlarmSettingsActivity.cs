
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Alarm
{
	[Activity (Label = "AlarmSettingsActivity", MainLauncher = false)]			
	public class AlarmSettingsActivity : Activity
	{
		private TextView timeTextView;
		private TextView actionTextView;
		private Button saveButton;
		private Button deleteButton;
		private Alarm alarm;
		private int id;
		private CheckBox[] daysCheckboxes;
		private SeekBar volumeSlider;
		private TextView alarmDuration;
		private TextView snoozeTime;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature (WindowFeatures.NoTitle);

			SetContentView (Resource.Layout.AlarmSettings);
			alarm = (Alarm) this.Intent.GetParcelableExtra("alarm");

			id = this.Intent.GetIntExtra ("id", -1);
			timeTextView = FindViewById<TextView>(Resource.Id.alarm_settings_time);
			saveButton = FindViewById<Button> (Resource.Id.alarm_settings_button_save);
			deleteButton = FindViewById<Button> (Resource.Id.alarm_settings_button_delete);
			actionTextView = FindViewById<TextView> (Resource.Id.alarm_settings_action);
			volumeSlider = FindViewById<SeekBar> (Resource.Id.alarm_settings_volume_slider);
			snoozeTime = FindViewById<TextView> (Resource.Id.alarm_settings_snooze_time);
			alarmDuration = FindViewById<TextView> (Resource.Id.alarm_settings_duration);

			volumeSlider.Indeterminate = false;
			volumeSlider.Progress  = (int) (alarm.Volume * 50);
			UpdateSnoozeTimeText ();
			UpdateAlarmDurationText ();
			daysCheckboxes = new CheckBox[7];
			daysCheckboxes [0] = FindViewById<CheckBox> (Resource.Id.daypicker_monday);
			daysCheckboxes [1] = FindViewById<CheckBox> (Resource.Id.daypicker_tuesday);
			daysCheckboxes [2] = FindViewById<CheckBox> (Resource.Id.daypicker_wednesday);
			daysCheckboxes [3] = FindViewById<CheckBox> (Resource.Id.daypicker_thursday);
			daysCheckboxes [4] = FindViewById<CheckBox> (Resource.Id.daypicker_friday);
			daysCheckboxes [5] = FindViewById<CheckBox> (Resource.Id.daypicker_saturday);
			daysCheckboxes [6] = FindViewById<CheckBox> (Resource.Id.daypicker_sunday);
			for (int i = 0; i < 7; i++) {
				daysCheckboxes [i].Checked = alarm.Days [i];
			}

			alarmDuration.Click += delegate {
				FragmentTransaction transaction = FragmentManager.BeginTransaction();
				DialogNumberPicker numberpicker = new DialogNumberPicker(1, alarm.Duration);
				numberpicker.Show(transaction, "numberpicker");
				numberpicker.numberSetEventHandler += DialogNumberPicker_TimeSet;
			};
			snoozeTime.Click += delegate {
				FragmentTransaction transaction = FragmentManager.BeginTransaction();
				DialogNumberPicker numberpicker = new DialogNumberPicker(2, alarm.SnoozeTime);
				numberpicker.Show(transaction, "numberpicker");
				numberpicker.numberSetEventHandler += DialogNumberPicker_TimeSet;
			};
			timeTextView.Click += delegate {
				FragmentTransaction transaction = FragmentManager.BeginTransaction();
				DialogTimepicker timepicker = new DialogTimepicker(alarm.Time);
				timepicker.Show(transaction, "timepicker");
				timepicker.timeSetEventHandler += DialogTimepicker_TimeSet;
			};
			actionTextView.Click += delegate {
				FragmentTransaction transaction = FragmentManager.BeginTransaction();
				DialogActionpicker actionpicker = new DialogActionpicker(alarm.ActionID);
				actionpicker.Show(transaction, "actionpicker");
				actionpicker.actionSetEventHandler += Actionpicker_ActionSet;
				//transaction.Commit();
			};
			saveButton.Click += delegate {
				Intent data = new Intent();
				bool[] days = new bool[daysCheckboxes.Length];
				for (int i = 0; i < daysCheckboxes.Length; i++) {
					if (daysCheckboxes[i].Checked) {
						days[i] = true;
					}
				}
				alarm.Days = days;
				alarm.Volume = (float) volumeSlider.Progress/50;
				data.PutExtra("alarm", alarm);
				if (id != -1) {
					data.PutExtra("id", id);
				}
				this.SetResult(Result.Ok, data);
				Console.WriteLine("saved");
				Finish ();
			};
			deleteButton.Click += delegate {
				Intent data = new Intent();
				data.PutExtra("alarm", (Alarm)null);
				if (id != -1) {
					data.PutExtra("id", id);
				}
				this.SetResult(Result.Ok, data);
				Console.WriteLine("deleted");
				Finish ();
			};

			UpdateTimeText ();
			UpdateActionText ();

			if (id == -1) {
				timeTextView.CallOnClick ();
			}
		}

		//onBackPressed()

		void DialogTimepicker_TimeSet(object sender, TimeSetEventArgs e)
		{
			alarm.Time = new TimeSpan(e.Hour, e.Minute, 0);
			UpdateTimeText ();
		}

		void DialogNumberPicker_TimeSet(object sender, NumberSetEventArgs e)
		{
			if (e.ID == 1) {
				alarm.Duration = e.N;
				UpdateAlarmDurationText();
			} else if (e.ID == 2) {
				alarm.SnoozeTime = e.N;
				UpdateSnoozeTimeText ();
			}
		}

		void Actionpicker_ActionSet (object sender, ActionSetEventArgs e)
		{
			alarm.ActionID = e.ActionID;
			UpdateActionText ();
		}

		private void UpdateTimeText ()
		{
			string time = string.Format ("{0}.{1}", alarm.Time.Hours.ToString(), alarm.Time.Minutes.ToString ().PadLeft (2, '0'));
			timeTextView.Text = time;
		}

		private void UpdateActionText () 
		{
			actionTextView.Text = ActionMngr.Actions[alarm.ActionID].Title;
		}

		private void UpdateSnoozeTimeText ()
		{
			snoozeTime.Text = Resources.GetText(Resource.String.snoozeTime) + " " + alarm.SnoozeTime;
		}

		private void UpdateAlarmDurationText ()
		{
			alarmDuration.Text = Resources.GetText(Resource.String.duration) + " " + alarm.Duration;
		}
	}
}

