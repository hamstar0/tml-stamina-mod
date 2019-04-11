using Microsoft.Xna.Framework;
using Terraria;


namespace Stamina.Logic {
	partial class StaminaLogic {
		public void AddStamina( Player player, float amount ) {
			var mymod = StaminaMod.Instance;

			amount *= mymod.Config.ScaleAllStaminaRates;

			if( this.Stamina == 0 ) {
				this.TiredTimer += amount / 2;
			} else {
				foreach( var hook in this.StaminaChangeHooks ) {
					amount = hook( player, StaminaDrainTypes.Recover, amount );
				}

				this.Stamina += amount;
			}
		}


		public void AddFatigue( Player player, float amount ) {
			foreach( var hook in this.FatigueChangeHooks ) {
				amount = hook( player, amount );
			}

			this.Fatigue += amount;
			this.Fatigue = MathHelper.Clamp( this.Fatigue, 0, this.MaxStamina2 );
		}


		public void AddMaxStamina( Player player, int amount ) {
			this.MaxStamina += amount;
			this.UpdateMaxStamina2( player );

			if( amount < 0 ) {
				if( this.Stamina > this.MaxStamina2 ) {
					this.Stamina = this.MaxStamina2;
				}
			}
		}
	}
}
