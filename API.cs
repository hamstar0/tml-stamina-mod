using Stamina.Logic;
using System;
using Terraria;


namespace Stamina {
	public delegate float StaminaChangeHook( Player player, StaminaDrainTypes drain_type, float amount );
	public delegate float FatigueChangeHook( Player player, float amount );



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
		public static int GetMaxStamina2( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.MaxStamina2;
		}
		public static float GetFatigue( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.Fatigue;
		}
		public static int GetExerciseThreshold( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.GetStaminaLossAmountNeededForExercise( StaminaMod.Instance );
		}
		public static bool IsExercising( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.IsExercising;
		}

		public static void DrainStamina( Player player, StaminaDrainTypes type ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.DrainStamina( StaminaMod.Instance, player, type );
		}
		public static void DrainStaminaViaCustomItem( Player player, string item_name ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.DrainStaminaViaCustomItemUse( StaminaMod.Instance, player, item_name );
		}
		public static void DrainCustomStaminaAmount( Player player, float amount, string custom_type ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.DrainStaminaCustomAmount( StaminaMod.Instance, player, amount, custom_type );
		}

		public static void AddStamina( Player player, float amt ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.AddStamina( StaminaMod.Instance, player, amt );
		}
		public static void AddFatigue( Player player, float amt ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.AddFatigue( player, amt );
		}


		public static void OnStaminaChange( Player player, StaminaChangeHook hook ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.StaminaChangeHooks.Add( hook );
		}

		public static void OnFatigueChange( Player player, FatigueChangeHook hook ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.FatigueChangeHooks.Add( hook );
		}
	}
}
