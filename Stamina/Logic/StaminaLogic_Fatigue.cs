using Terraria;


namespace Stamina.Logic {
	partial class StaminaLogic {
		public void PassiveFatigueRecover( Player player ) {
			var mymod = StaminaMod.Instance;

			if( this.Fatigue > 0 ) {
				if( (this.MaxStamina2 - this.Stamina) <= this.Fatigue ) {
					this.FatigueRecoverTimer++;
					int duration = mymod.Config.FatigueRecoverDuration;

					if( this.FatigueRecoverTimer >= duration ) {
						this.FatigueRecoverTimer = 0;
						this.AddFatigue( player, - 1 );
					}
				}

				if( this.Fatigue >= this.GetStaminaLossAmountNeededForExercise() ) {
					this.IsExercising = true;
				}
			} else {
				if( this.IsExercising ) {
					this.IsExercising = false;
					this.ApplyExercise( player );
				}
			}
		}
	}
}
