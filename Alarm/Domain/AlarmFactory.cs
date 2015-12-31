using System;
using System.Text.RegularExpressions;

namespace Alarm
{
	public class AlarmFactory
	{

		public static Alarm StringToAlarm(string a) {
			Console.WriteLine ("alarms: " + a);
			string timeString = Regex.Match (a, @"Time=([^(,|\])]*)").Groups [1].Value;
			string enabledString = Regex.Match (a, @"Enabled=([^(,|\])]*)").Groups [1].Value;
			char[] daysCharArray = (Regex.Match (a, @"Days=([^(,|\])]*)").Groups [1].Value).ToCharArray();
			int actionID = Int32.Parse(Regex.Match (a, @"Action=([^(,|\])]*)").Groups [1].Value);
			int snooze = Int32.Parse(Regex.Match (a, @"Snooze=([^(,|\])]*)").Groups [1].Value);
			int duration = Int32.Parse(Regex.Match (a, @"Duration=([^(,|\])]*)").Groups [1].Value);
			float volume = float.Parse(Regex.Match (a, @"Volume=([^(\])]*)").Groups [1].Value);

			bool[] days = new bool[7];
			for (int i = 0; i < 7; i++) {
				if (daysCharArray[i].Equals ('1')) {
					days [i] = true;
				}
			}
			return new Alarm (TimeSpan.Parse (timeString), Convert.ToBoolean (enabledString)) { Days = days, ActionID = actionID, SnoozeTime = snooze, Duration = duration, Volume = volume };
		}

		public static string AlarmToString(Alarm a) {
			return a.ToString ();
		}
	}
}

