using HamstarHelpers.Services.Messages.Player;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace Stamina.Logic {
	partial class StaminaLogic {
		public int GetStaminaLossAmountNeededForExercise() {
			var mymod = StaminaMod.Instance;
			int threshold = (int)((float)this.MaxStamina2 * mymod.Config.FatigueAsMaxStaminaPercentAmountNeeededForExercise);
			return threshold - mymod.Config.FatigueForExerciseAmountRemoved;
		}

		////////////////

		public bool ApplyExercise( Player player ) {
			var mymod = StaminaMod.Instance;

			if( this.MaxStamina >= mymod.Config.MaxStaminaAmount ) { return false; }

			var myplayer = player.GetModPlayer<StaminaPlayer>();

			this.AddMaxStamina( player, mymod.Config.ExerciseGrowthAmount );

			if( myplayer.IsUsingSupplements ) {
				this.AddMaxStamina( player, mymod.Config.ExerciseSupplementAddedGrowthAmount );
			}

			if( this.MaxStamina > mymod.Config.MaxStaminaAmount ) {
				this.AddMaxStamina( player, mymod.Config.MaxStaminaAmount - this.MaxStamina );
			}

			string msg = "+" + mymod.Config.ExerciseGrowthAmount + " Stamina";
			PlayerMessages.AddPlayerLabel( player, msg, Color.Chartreuse, 60 * 3, true );

			Main.PlaySound( SoundID.Item47.WithVolume( 0.5f ) );

			return true;
		}
	}
}
