using System;
using System.Collections.Generic;
using Android.Media;
using Android.Content;
using System.Timers;

namespace Alarm
{
	public class ActionMngr
	{
		protected static MediaPlayer player;
		protected static Timer timer;
		private static List<AlarmAction> actions = new List<AlarmAction>{
			new AlarmAction ("Chopin", Resource.Raw.chopin),
			new AlarmAction ("Pretty Pimpin", Resource.Raw.prettypimpin)
		};
		public static List<AlarmAction> Actions{ get { return actions; }}

		public static void StartPlayer(Context context, AlarmAction action, int duration, float volume)
		{
			if (player != null) {
				StopPlayer ();
			}

			player = MediaPlayer.Create(context, action.ResourceID);
			player.Looping = true;
			player.SetVolume (volume, volume);
			timer = new Timer (duration * 1000);
			timer.Elapsed += Timer_Elapsed;
			player.Start();
		}

		static void Timer_Elapsed (object sender, ElapsedEventArgs e)
		{
			StopPlayer ();
		}

		public static void StopPlayer()
		{
			if (player != null) {
				player.Stop ();
				player.Release ();
				if (timer != null) {
					timer.Stop ();
				}
				player = null;
			}
		}
	}
}

