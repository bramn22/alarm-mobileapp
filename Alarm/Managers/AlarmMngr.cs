using System;
using Android.Content;
using Android.App;

namespace Alarm
{
	public class AlarmMngr
	{
		private static PendingIntent pendingIntent;
		public static bool SnoozeMode { get; set; }

		public static void StartAlarm(Alarm a, Context context) {
			if (a == null)
				return;
			AlarmManager manager = (AlarmManager)context.GetSystemService (Context.AlarmService);
			Intent i = new Intent (context, typeof(AlarmReceiver));
			//put sound as extra!!
			Console.WriteLine("alarm started for : " + a.Time.Hours + ":" + a.Time.Minutes);
			i.PutExtra("startAlarm", a);
			if (pendingIntent != null) {
				pendingIntent.Cancel ();
				//pendingIntent = null;
			}

			pendingIntent = PendingIntent.GetBroadcast (context, 5, i, 0);
			int day = (int)(DateTime.Now.DayOfWeek +6)%7;
			int j;
			if (a.Days [day] == true && a.Time > DateTime.Now.TimeOfDay) {
				j = day;
			} else {
				j = day + 1;
				while (a.Days [j % 7] != true && j <= day + 7) {
					j++;
				}
			}

			DateTime alarmDate = DateTime.Today.AddDays (j - day);
			manager.SetExact(AlarmType.RtcWakeup, (long)(Java.Lang.JavaSystem.CurrentTimeMillis () + (new DateTime(alarmDate.Year,alarmDate.Month,alarmDate.Day,a.Time.Hours,a.Time.Minutes,0).ToUniversalTime () - DateTime.Now.ToUniversalTime ()).TotalMilliseconds), pendingIntent);
			Console.WriteLine("Alarm set for " + alarmDate.Day + ", at: " + a.Time.Hours + ":" + a.Time.Minutes + "!");
		}

		public static void StopAlarm(Context context) {
			if (pendingIntent != null) {
				try {
					AlarmManager manager = (AlarmManager)context.GetSystemService (Context.AlarmService);
					manager.Cancel (pendingIntent);
					Console.WriteLine("Alarm canceled");
				}catch (Exception e) {
					Console.WriteLine(e.StackTrace);
				}
			}
		}

	}
}

