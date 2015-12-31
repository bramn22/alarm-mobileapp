using System;
using Android.OS;
using Java.Interop;

namespace Alarm
{
	public class AlarmAction : Java.Lang.Object, IParcelable
	{
		public string Title { get; set; }
		public int ResourceID { get; set; }

		public AlarmAction (string title = "Chopin", int resourceID = Resource.Raw.chopin)
		{
			Title = title;
			ResourceID = resourceID;
		}

		public override string ToString ()
		{
			return string.Format ("{{[Action: Title={0}, ResourceID={1}]}}", Title, ResourceID.ToString());
		}

		[ExportField("CREATOR")]
		public static IParcelableCreator GetCreator()
		{
			return new AlarmActionParcelableCreator ();
		}

		int IParcelable.DescribeContents ()
		{
			return 0;
		}

		void IParcelable.WriteToParcel (Parcel dest, ParcelableWriteFlags flags)
		{
			dest.WriteString (Title);
			dest.WriteInt(ResourceID);
		}

		class AlarmActionParcelableCreator : Java.Lang.Object, IParcelableCreator
		{
			Java.Lang.Object IParcelableCreator.CreateFromParcel (Parcel source)
			{
				string title = source.ReadString();
				int resourceID = source.ReadInt ();
				return  new AlarmAction(title, resourceID);
			}

			Java.Lang.Object[] IParcelableCreator.NewArray (int size)
			{
				return new Java.Lang.Object[size];
			}


		}

	}


}

