using Stamina.Items.Accessories;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace Stamina {
	partial class StaminaPlayer : ModPlayer {
		public override bool PreHurt( bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource ) {
			if( damageSource != null ) {
				string srcReason = damageSource.SourceCustomReason;
				string rageReason = RageHeadbandItem.DamageType.SourceCustomReason;

				if( srcReason != null && srcReason.Equals( rageReason ) ) {
					customDamage = true;
					crit = false;
				}
			}
			return base.PreHurt( pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource );
		}


		public override void PostHurt( bool pvp, bool quiet, double damage, int hitDirection, bool crit ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }
			if( quiet ) { return; }

			float yike = (float)damage * mymod.Config.PercentOfDamageAdrenalineBurst * (crit ? 2f : 1f);

			this.Logic.AddStamina( this.player, yike );
		}

		
		public override bool PreItemCheck() {
			bool prechecked = base.PreItemCheck();
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Enabled ) { return prechecked; }
			if( this.Logic == null ) { return prechecked; }

			Item item = this.player.inventory[ this.player.selectedItem ];

			if( item.type != 0 && !this.player.noItems ) {
				/*if( (player.itemTime == 0 && player.controlUseItem && player.releaseUseItem) ||
					(player.itemTime == 1 && player.controlUseItem && item.autoReuse) ) {
Main.NewText("PreItemCheck "+ StaminaMod.Config.Data.SingularExertionRate * ((float)item.useTime / 30f));
					this.DrainStamina( StaminaMod.Config.Data.SingularExertionRate * ((float)item.useTime/30f) );
				}*/
				if( this.player.controlUseItem && this.player.itemTime <= 1 ) {
					this.Logic.SetItemUseHoldDurationForStaminaDrain( 36 );
				}
			}

			return prechecked;
		}
	}
}
