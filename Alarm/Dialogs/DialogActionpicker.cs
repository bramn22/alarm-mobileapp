using System;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Widget;

namespace Alarm
{
	public class DialogActionpicker : DialogFragment
	{

		private Button saveButton;
		private Button cancelButton;
		private ListView actionListView;
		private ActionListViewAdapter actionListViewAdapter; 
		private int actionID;

		public event EventHandler<ActionSetEventArgs> actionSetEventHandler;

		public DialogActionpicker (int actionID)
		{
			this.actionID = actionID;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) 
		{
			base.OnCreateView (inflater, container, savedInstanceState);	

			var view = inflater.Inflate (Resource.Layout.DialogActionpicker, container, false);

			// Remove the title bar from the dialog
			this.Dialog.RequestWindowFeature ((int)WindowFeatures.NoTitle);

			// give the dialog a custom in/out transition animation
			this.Dialog.Window.SetWindowAnimations (Resource.Style.timepicker_animation);

			saveButton = view.FindViewById <Button>(Resource.Id.actionpicker_button_save);
			cancelButton = view.FindViewById <Button>(Resource.Id.actionpicker_button_cancel);
			actionListView = view.FindViewById<ListView> (Resource.Id.actionpicker_listview);
			saveButton.Click += Save_Click;
			cancelButton.Click += Cancel_Click;
			return view;
		}

		void Save_Click(object sender, EventArgs e) 
		{
			actionSetEventHandler.Invoke (this, new ActionSetEventArgs (actionListViewAdapter.SelectedActionID));
			ActionMngr.StopPlayer ();
			this.Dismiss ();
		}

		void Cancel_Click(object sender, EventArgs e) 
		{
			ActionMngr.StopPlayer ();
			this.Dismiss ();
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
			actionListViewAdapter = new ActionListViewAdapter (Activity,ActionMngr.Actions, actionID);
			actionListView.Adapter = actionListViewAdapter;
			actionListView.ChoiceMode = ChoiceMode.Single;
			actionListView.ItemClick += ActionsListView_ItemClick;
		}

		void ActionsListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			Console.WriteLine ("Itemclick registered");
		}
	}

	public class ActionSetEventArgs : EventArgs
	{
		public ActionSetEventArgs(int actionID) : base()
		{
			ActionID = actionID;
		}

		public int ActionID {
			get;
			set;
		}
	}
}

