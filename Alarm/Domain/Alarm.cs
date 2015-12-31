using System;
using Android.OS;
using Java.Interop;

namespace Alarm
{
	public class Alarm : Java.Lang.Object, IParcelable, IComparable
	{
		public TimeSpan Time { get; set; }
		public bool Enabled { get; set; }
		public bool[] Days { get; set; }
		public int ActionID { get; set; }
		public int SnoozeTime { get; set; }
		public int Duration { get; set; }
		public float Volume { get; set; }


		[ExportField("CREATOR")]
		public static IParcelableCreator GetCreator()
		{
			return new AlarmParcelableCreator ();
		}

		public Alarm (int hour, int minute, bool enabled = false) : this(new TimeSpan(hour, minute, 0), enabled)
		{
		}

		public Alarm (TimeSpan time, bool enabled = false) 
		{
			if (time == null) {
				time = DateTime.Now.TimeOfDay;
			} else {
				Time = time;
			}
			Enabled = enabled;
			Days = new bool[7];
			ActionID = 0;
			SnoozeTime = 10;
			Duration = 10;
			Volume = 0.8f;
		}
			
		int IParcelable.DescribeContents ()
		{
			return 0;
		}
		void IParcelable.WriteToParcel (Parcel dest, ParcelableWriteFlags flags)
		{
			dest.WriteInt(Time.Hours);
			dest.WriteInt(Time.Minutes);
			dest.WriteByte((sbyte) (Enabled ? 1: 0));
			dest.WriteBooleanArray (Days);
			//dest.WriteParcelable (Action, flags);
			dest.WriteInt(ActionID);
			dest.WriteInt (SnoozeTime);
			dest.WriteInt (Duration);
			dest.WriteFloat (Volume);
		}

		int IComparable.CompareTo (object obj)
		{
			Alarm a = (Alarm)obj;
			return TimeSpan.Compare (this.Time, ((Alarm)obj).Time);
		}

		public override string ToString ()
		{
			string daysString = "";
			for (int i = 0; i < Days.Length; i++) {
				daysString += Days [i] ? 1 : 0;
			}
			return string.Format ("[Alarm: Time={0}, Enabled={1}, Days={2}, Action={3}, Snooze={4}, Duration={5}, Volume={6}]", Time.ToString(), Enabled, daysString, ActionID, SnoozeTime, Duration, Volume.ToString());
		}


		class AlarmParcelableCreator : Java.Lang.Object, IParcelableCreator
		{
			Java.Lang.Object IParcelableCreator.CreateFromParcel (Parcel source)
			{
				int hour = source.ReadInt ();
				int minute = source.ReadInt ();
				bool enabled = source.ReadByte () != 0;
				Alarm a = new Alarm (hour, minute, enabled);
				source.ReadBooleanArray(a.Days);
				//a.Action = (AlarmAction) source.ReadParcelable(new AlarmAction().Class.ClassLoader);
				a.ActionID = source.ReadInt();
				a.SnoozeTime = source.ReadInt ();
				a.Duration = source.ReadInt ();
				a.Volume = source.ReadFloat ();
				return  a;
			}

			Java.Lang.Object[] IParcelableCreator.NewArray (int size)
			{
				return new Java.Lang.Object[size];
			}


		}
	}
}

