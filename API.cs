using Stamina.Logic;
using System;
using Terraria;


namespace Stamina {
	public delegate float StaminaChangeHook( Player player, StaminaDrainTypes drainType, float amount );
	public delegate float FatigueChangeHook( Player player, float amount );




	public static partial class StaminaAPI {
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
			return myplayer.Logic.GetStaminaLossAmountNeededForExercise();
		}
		public static bool IsExercising( Player player ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			return myplayer.Logic.IsExercising;
		}

		////

		public static void DrainStamina( Player player, StaminaDrainTypes type ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.DrainStamina( player, type );
		}
		public static void DrainStaminaViaCustomItem( Player player, string itemName ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.DrainStaminaViaCustomItemUse( player, itemName );
		}
		public static void DrainCustomStaminaAmount( Player player, float amount, string customType ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.DrainStaminaCustomAmount( player, amount, customType );
		}

		public static void AddStamina( Player player, float amt ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.AddStamina( player, amt );
		}
		public static void AddFatigue( Player player, float amt ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.AddFatigue( player, amt );
		}


		public static void AddMaxStamina( Player player, int amt ) {
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.AddMaxStamina( player, amt );
		}

		////////////////

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
