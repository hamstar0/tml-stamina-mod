using System.Collections.Generic;
using Terraria;


namespace Stamina {
	public static class StaminaAPI {
		public static StaminaConfigData GetModSettings() {
			if( StaminaMod.Instance == null ) { return null; }
			return StaminaMod.Instance.Config.Data;
		}


		public static bool IsShowingDrainingFX( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.DrainingFX;
		}
		public static float GetStamina( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.Stamina;
		}
		public static int GetMaxStamina( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.MaxStamina;
		}
		public static float GetFatigue( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.Fatigue;
		}
		public static IDictionary<string, float> GetCurrentDrainTypes( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.CurrentDrainTypes;
		}
		public static int GetExerciseThreshold( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.GetExerciseThreshold( StaminaMod.Instance );
		}
		public static bool IsExercising( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.IsExercising;
		}

		public static void DrainStamina( Player player, float amt, string type ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.DrainStamina( amt, type );
		}
		public static void AddStamina( Player player, float amt ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.AddStamina( StaminaMod.Instance, amt );
		}
		public static void AddFatigue( Player player, float amt ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.AddFatigue( amt );
		}
	}
}
