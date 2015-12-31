using System;
using System.Collections.Generic;
	
using Android.App;
using Android.Content;
using Android.Widget;
using Android.Views;

namespace Alarm
{
	public class AlarmListViewAdapter : BaseAdapter<Alarm>
	{

		private List<Alarm> items;
		private Context context;

		public AlarmListViewAdapter (Context context, List<Alarm> items)
		{
			this.items = items;
			this.context = context;
		}

		public override int Count 
		{
			get { return items.Count; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Alarm this [int index] {
			get {
				return items [index];
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View row = convertView;

			if (row == null) {
				row = LayoutInflater.From (context).Inflate (Resource.Layout.AlarmListView_row, null, false);
			}

			TextView alarmTime = row.FindViewById<TextView> (Resource.Id.alarm_time);
			alarmTime.Text = string.Format ("{0}.{1}", items [position].Time.Hours.ToString(), items [position].Time.Minutes.ToString().PadLeft (2, '0'));

			Switch alarmToggle = row.FindViewById<Switch> (Resource.Id.alarm_toggle);
			alarmToggle.SetTag (Resource.Id.alarm_toggle, position);
			alarmToggle.Checked = items [position].Enabled;
			alarmToggle.CheckedChange -= AlarmToggle_CheckedChange;
			alarmToggle.CheckedChange += AlarmToggle_CheckedChange;
			return row;
		}

		void AlarmToggle_CheckedChange (object sender, CompoundButton.CheckedChangeEventArgs e)
		{
			int position = (int)((Switch)sender).GetTag (Resource.Id.alarm_toggle);
			items[position].Enabled = e.IsChecked;
			Console.WriteLine("Alarm " + items[position].Time.ToString() +  " inverted: " + items[position].Enabled );
			((MainActivity)context).UpdateAlarmManager ();
		}



	}
}

