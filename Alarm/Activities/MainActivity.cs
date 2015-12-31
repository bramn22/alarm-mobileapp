using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Android.Views;

namespace Alarm
{
	[Activity (Label = "Alarm", MainLauncher = true, Icon = "@mipmap/alarm")]
	public class MainActivity : Activity
	{
		private ISharedPreferences data;
		private AlarmService alarmService;
		private Alarm pendingAlarm;
		private AlarmListViewAdapter alarmsListViewAdapter;
		private ListView alarmsListView;
		private ToggleButton alarmsToggleButton;
		private Button addAlarmButton;
		private bool gotActivityResult;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RequestWindowFeature (WindowFeatures.NoTitle);
			data = this.GetSharedPreferences ("project_alarm", FileCreationMode.Private);
			SetContentView (Resource.Layout.Main);

			alarmService = new AlarmService ();

			addAlarmButton = FindViewById<Button> (Resource.Id.add_alarm_button);
			alarmsListView = FindViewById<ListView> (Resource.Id.alarm_listview);
			alarmsToggleButton = FindViewById<ToggleButton> (Resource.Id.alarm_toggle);

			alarmsListViewAdapter = new AlarmListViewAdapter (this, alarmService.Alarms);
			alarmsListView.Adapter = alarmsListViewAdapter;
			alarmsListView.ChoiceMode = ChoiceMode.Single;

			addAlarmButton.Click += AddAlarmButton_Click;
			alarmsListView.ItemClick += AlarmsListView_ItemClick;
		}

		void AddAlarmButton_Click (object sender, EventArgs e)
		{
			Intent intent = new Intent (this, typeof(AlarmSettingsActivity));
			Console.WriteLine ("New alarm being added...");
			intent.PutExtra ("alarm", new Alarm (DateTime.Now.TimeOfDay, true));
			StartActivityForResult (intent, 1);
		}

		void AlarmsListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			Intent intent = new Intent (this, typeof(AlarmSettingsActivity));
			Console.WriteLine (alarmsListViewAdapter [e.Position].Time.Hours);
			intent.PutExtra ("alarm", alarmsListViewAdapter [e.Position]);
			intent.PutExtra ("id", e.Position);
			StartActivityForResult (intent, 1);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (requestCode == 1 && resultCode == Result.Ok) {
				Alarm alarm = (Alarm)data.GetParcelableExtra ("alarm");
				int id = (int)data.GetIntExtra ("id", -1);
				
				if (id != -1) {
					Console.WriteLine ("id != -1");
					alarmService.DeleteAlarmAt (id);
				}
				if (alarm != null) {
					Console.WriteLine ("alarm != null");
					alarmService.AddAlarm (alarm);
				}
				gotActivityResult = true;
				alarmsListViewAdapter.NotifyDataSetChanged ();
				UpdateAlarmManager ();
			}
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			if (!gotActivityResult) {
				ICollection<string> alarmStrings = data.GetStringSet ("alarms", null);
				alarmService.Alarms.Clear ();
				if (alarmStrings != null) {
					foreach (string a in alarmStrings) {
						alarmService.Alarms.Add (AlarmFactory.StringToAlarm(a));
					}
					alarmService.Alarms.Sort ();
					alarmsListViewAdapter.NotifyDataSetChanged ();
					UpdateAlarmManager ();
				}
			} else {
				gotActivityResult = false;
			}

		}

		protected override void OnPause ()
		{
			base.OnPause ();

			ISharedPreferencesEditor editor = data.Edit ();
			ICollection<string> alarms = new List<string> ();
			foreach (Alarm a in alarmService.Alarms) {
				alarms.Add (AlarmFactory.AlarmToString(a));
			}
			editor.PutStringSet ("alarms", alarms);
			editor.Commit ();
		}

		public void UpdateAlarmManager ()
		{
			Alarm newAlarm = alarmService.GetNextEnabledAlarm ();
			if (newAlarm != null && (pendingAlarm == null || !newAlarm.Equals (pendingAlarm)) && !AlarmMngr.SnoozeMode) {
				if (pendingAlarm != null) {
					AlarmMngr.StopAlarm (this);
				}
				pendingAlarm = newAlarm;
				AlarmMngr.StartAlarm (newAlarm, this);
			} else if (newAlarm == null && pendingAlarm != null) {
				AlarmMngr.StopAlarm (this);
				pendingAlarm = null;
			}
			Console.WriteLine ("AlarmManager updated...");
		}
	}
}


