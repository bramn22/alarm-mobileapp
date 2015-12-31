using System;
using System.Collections.Generic;

namespace Alarm
{
	public class AlarmService
	{
		private List<Alarm> alarms;

		public AlarmService ()
		{
			alarms = new List<Alarm>();
		}

		public List<Alarm> Alarms {
			get { return alarms; }
			set {
				alarms = value;
			}
		}

		public void AddAlarm(Alarm alarm) {
			alarms.Add (alarm);
			alarms.Sort ();
		}

		public void DeleteAlarm(Alarm alarm) {
			alarms.Remove (alarm);
			alarms.Sort ();
		}

		public void DeleteAlarmAt(int id) {
			if (alarms.Count > id) {
				alarms.RemoveAt (id);
				alarms.Sort ();
			}

		}

		public void CreateDummyAlarms() {
			alarms.Add (new Alarm(12, 30, true));
		}

		public Alarm GetNextEnabledAlarm() {
			int day = (int)(DateTime.Now.DayOfWeek +6)%7;
			//Check for alarm in current day
			foreach (Alarm a in Alarms) {
				//Check if enabled
				if (a.Enabled) {
					//Check for this day
					if (a.Days [day]) {
						//Check if time hasn't passed yet
						if (a.Time > DateTime.Now.TimeOfDay) {
							Console.WriteLine("First alarm today at: " + a.Time);
							return a;
						}
					}
				}
			}

			//Check for alarm in following days
			for (int i = day + 1; i <= day + 7; i++) {
				foreach (Alarm a in Alarms) {
					if (a.Enabled) {
						if (a.Days [i%7] == true) {
							Console.WriteLine("First alarm on " + (i%7) + ", at: " + a.Time);
							return a;
						}
					}
				}
			}

			return null;
		}
	}
}

