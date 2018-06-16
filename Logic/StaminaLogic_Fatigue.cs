using HamstarHelpers.Services.Messages;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace Stamina.Logic {
	partial class StaminaLogic {
		public int GetStaminaLossAmountNeededForExercise( StaminaMod mymod ) {
			int threshold = (int)((float)this.MaxStamina2 * mymod.Config.FatigueAsMaxStaminaPercentAmountNeeededForExercise);
			return threshold - mymod.Config.FatigueForExerciseAmountRemoved;
		}

		////////////////

		public void PassiveFatigueRecover( StaminaMod mymod, Player player ) {
			if( this.Fatigue > 0 ) {
				if( (this.MaxStamina2 - this.Stamina) <= this.Fatigue ) {
					this.FatigueRecoverTimer++;
					int duration = mymod.Config.FatigueRecoverDuration;

					if( this.FatigueRecoverTimer >= duration ) {
						this.FatigueRecoverTimer = 0;
						this.AddFatigue( player, - 1 );
					}
				}

				if( this.Fatigue >= this.GetStaminaLossAmountNeededForExercise( mymod ) ) {
					this.IsExercising = true;
				}
			} else {
				if( this.IsExercising ) {
					this.IsExercising = false;
					this.ApplyExercise( mymod, player );
				}
			}
		}

		////////////////

		public bool ApplyExercise( StaminaMod mymod, Player player ) {
			if( this.MaxStamina >= mymod.Config.MaxStaminaAmount ) { return false; }

			var modplayer = player.GetModPlayer<StaminaPlayer>();
			
			this.MaxStamina += mymod.Config.ExerciseGrowthAmount;

			if( modplayer.IsUsingSupplements ) {
				this.MaxStamina += mymod.Config.ExerciseSupplementAddedGrowthAmount;
			}

			if( this.MaxStamina > mymod.Config.MaxStaminaAmount ) {
				this.MaxStamina = mymod.Config.MaxStaminaAmount;
			}

			string msg = "+" + mymod.Config.ExerciseGrowthAmount + " Stamina";
			PlayerMessages.AddPlayerLabel( player, msg, Color.Chartreuse, 60 * 3, true );

			Main.PlaySound( SoundID.Item47.WithVolume( 0.5f ) );

			return true;
		}

		////////////////

		public void AddFatigue( Player player, float amount ) {
			foreach( var hook in FatigueChangeHooks ) {
				amount = hook( player, amount );
			}

			this.Fatigue += amount;
			this.Fatigue = MathHelper.Clamp( this.Fatigue, 0, this.MaxStamina2 );
		}
	}
}
