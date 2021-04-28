using Terraria.ModLoader;


namespace Stamina {
	partial class StaminaPlayer : ModPlayer {
		public override void PreUpdate() {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }
			if( this.Logic == null ) { return; }
			
			this.Logic.UpdateMaxStamina2( this.player );
			
			if( !this.player.dead ) {
				this.Logic.PassiveFatigueRecover( this.player );
				this.Logic.PassiveStaminaRegen( this.player );
				this.Logic.GatherPassiveStaminaDrains( this.player );
				this.Logic.CommitStaminaDrains( this.player );

				if( this.Logic.Stamina == 0 ) {
					this.ApplyDebuffs();
				}
			}

			if( StaminaItem.StarUseCooldown[this.player.whoAmI] > 0 ) {
				StaminaItem.StarUseCooldown[this.player.whoAmI]--;
			}
		}

		public override void PostUpdateRunSpeeds() {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }
			if( this.Logic == null ) { return; }

			if( !this.player.dead ) {
				this.Logic.GatherActivityStaminaDrains( this.player );

				if( this.WillApplyExhaustion ) {
					this.ApplyExhaustionEffect();
					this.WillApplyExhaustion = false;
				}
			}
		}
	}
}
