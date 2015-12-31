using System;
using Android.Content;
using Android.App;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Alarm
{
	[BroadcastReceiver]
	[IntentFilter(new[]{Android.Content.Intent.ActionBootCompleted}, Categories=new[] {Android.Content.Intent.CategoryDefault})]
	public class BootReceiver : BroadcastReceiver
	{
		private ISharedPreferences data;
		private AlarmService alarmService;

		public override void OnReceive (Context context, Intent intent)
		{
			try {
				data = context.GetSharedPreferences ("project_alarm", FileCreationMode.Private);
				if (data != null) {
					ICollection<string> alarmStrings = data.GetStringSet ("alarms", null);
					alarmService = new AlarmService ();
					if (alarmStrings != null) {
						foreach (string a in alarmStrings) {
							alarmService.Alarms.Add (AlarmFactory.StringToAlarm(a));
						}
						alarmService.Alarms.Sort ();
						AlarmMngr.StartAlarm (alarmService.GetNextEnabledAlarm (), context);
					}
				}
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}
	}
}

