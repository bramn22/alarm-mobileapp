
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
using Android.Media;
using System.Text.RegularExpressions;

namespace Alarm
{
	[Activity (Label = "AlarmGoingOffActivity")]			
	public class AlarmGoingOffActivity : Activity
	{
		protected MediaPlayer player;
		private Alarm alarm;
		private Button alarmOff;
		private Button alarmSnooze;
		private TextView alarmTime;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature (WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.AlarmGoingOff);
			alarmOff = FindViewById<Button>(Resource.Id.alarmgoingoff_off);
			alarmSnooze = FindViewById<Button> (Resource.Id.alarmgoingoff_snooze);
			alarmTime = FindViewById<TextView> (Resource.Id.alarmgoingoff_time);

			alarmOff.Click += AlarmOff_Click;
			alarmSnooze.Click += AlarmSnooze_Click;

			alarm = (Alarm) this.Intent.GetParcelableExtra ("startAlarm");
			string time = string.Format ("{0}.{1}", alarm.Time.Hours.ToString(), alarm.Time.Minutes.ToString ().PadLeft (2, '0'));
			alarmTime.Text = time;
			AlarmAction action = ActionMngr.Actions[alarm.ActionID];
			ActionMngr.StartPlayer(this, action, alarm.Duration, alarm.Volume);

		}

		void AlarmOff_Click (object sender, EventArgs e)
		{
			AlarmMngr.StopAlarm (this);
			AlarmMngr.SnoozeMode = false;
			ActionMngr.StopPlayer();
			setNewAlarm ();
			Finish ();

			return;
		}

		void AlarmSnooze_Click (object sender, EventArgs e) {
			AlarmMngr.StopAlarm (this);
			AlarmMngr.SnoozeMode = true;
			setSnooze ();

			Finish ();
		}

		private void setNewAlarm() {
			try {
				ISharedPreferences data = GetSharedPreferences ("project_alarm", FileCreationMode.Private);
				if (data != null) {
					ICollection<string> alarmStrings = data.GetStringSet ("alarms", null);
					AlarmService alarmService = new AlarmService ();
					if (alarmStrings != null) {
						foreach (string a in alarmStrings) {
							string timeString = Regex.Match (a, @"Time=([^(,|\])]*)").Groups [1].Value;
							string enabledString = Regex.Match (a, @"Enabled=([^(,|\])]*)").Groups [1].Value;
							char[] daysCharArray = (Regex.Match (a, @"Days=([^(,|\])]*)").Groups [1].Value).ToCharArray();
							Console.WriteLine ("alarms: " + a + "regexstring: " + timeString + ", " + enabledString.ToLower ());
							bool[] days = new bool[7];
							for (int i = 0; i < 7; i++) {
								if (daysCharArray[i].Equals ('1')) {
									days [i] = true;
								}
							}
							alarmService.Alarms.Add (new Alarm (TimeSpan.Parse (timeString), Convert.ToBoolean (enabledString)) {Days = days});
						}
						alarmService.Alarms.Sort ();
						AlarmMngr.StartAlarm (alarmService.GetNextEnabledAlarm (), this);
					}
				}
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}

		private void setSnooze() {
			bool[] days = new bool[7];
			for (int i = 0; i < 7; i++) {
				days [i] = true;
			}
			ActionMngr.StopPlayer();
			AlarmMngr.StartAlarm (new Alarm (DateTime.Now.AddMinutes(alarm.SnoozeTime).TimeOfDay, true) { Days = days, ActionID = alarm.ActionID }, this);
		}
	}


}

