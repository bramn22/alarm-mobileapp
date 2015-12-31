using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;

namespace Alarm
{
	public class ActionListViewAdapter : BaseAdapter<AlarmAction>
	{
		private List<AlarmAction> items;
		private Context context;
		private RadioButton selected;
		private int oldActionID;
		public int SelectedActionID { 
			get { 
				return (int)((RadioButton)selected).GetTag (Resource.Id.action_radio_button);
			}
		}

		public ActionListViewAdapter (Context context, List<AlarmAction> items, int actionID)
		{
			this.items = items;
			this.context = context;
			oldActionID = actionID;
		}

		public override int Count 
		{
			get { return items.Count; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override AlarmAction this [int index] {
			get {
				return items [index];
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View row = convertView;

			if (row == null) {
				row = LayoutInflater.From (context).Inflate (Resource.Layout.ActionListView_row, null, false);
			}

			RadioButton actionRadioButton = row.FindViewById<RadioButton> (Resource.Id.action_radio_button);
			actionRadioButton.SetTag (Resource.Id.action_radio_button, position);
			actionRadioButton.Text = items [position].Title;

			if (position == oldActionID) {
				Console.WriteLine ("position: " + position + actionRadioButton.Text);
				Console.WriteLine ("match found");
				selected = actionRadioButton;
				actionRadioButton.Checked = true;
			} else {
				actionRadioButton.Checked = false;
			}
			actionRadioButton.Click += ActionRadioButton_Click;
			return row;
		}

		void ActionRadioButton_Click (object sender, EventArgs e)
		{
			int position = (int)((RadioButton)sender).GetTag (Resource.Id.action_radio_button);
			if (oldActionID != position) {
				Console.WriteLine ("oldActionID != position" + oldActionID + "," + position);
				selected.Checked = false;
				oldActionID = position;
				selected = (RadioButton)sender;
			}

			ActionMngr.StartPlayer (context , ActionMngr.Actions[SelectedActionID], 1, 1);

		}

	}
}

