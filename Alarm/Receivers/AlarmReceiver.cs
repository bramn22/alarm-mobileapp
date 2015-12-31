using System;
using Android.Content;

namespace Alarm
{
	[BroadcastReceiver]
	public class AlarmReceiver : BroadcastReceiver
	{

		public override void OnReceive (Context context, Intent intent)
		{
			Intent i = new Intent (context, typeof(AlarmGoingOffActivity));
			Console.WriteLine("OnReceived called in AlarmReceiver!" + ((Alarm) intent.GetParcelableExtra("startAlarm")).Time.Hours);
				i.PutExtra("startAlarm", (Alarm) intent.GetParcelableExtra("startAlarm"));

			i.AddFlags (ActivityFlags.NewTask);
			context.StartActivity (i);
		}

	}
}

