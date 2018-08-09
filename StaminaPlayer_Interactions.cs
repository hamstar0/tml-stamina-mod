using Stamina.Items.Accessories;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace Stamina {
	partial class StaminaPlayer : ModPlayer {
		public override bool PreHurt( bool pvp, bool quiet, ref int damage, ref int hit_direction, ref bool crit, ref bool custom_damage, ref bool play_sound, ref bool gen_gore, ref PlayerDeathReason damage_source ) {
			if( damage_source != null ) {
				string src_reason = damage_source.SourceCustomReason;
				string rage_reason = RageHeadbandItem.DamageType.SourceCustomReason;

				if( src_reason != null && src_reason.Equals( rage_reason ) ) {
					custom_damage = true;
					crit = false;
				}
			}
			return base.PreHurt( pvp, quiet, ref damage, ref hit_direction, ref crit, ref custom_damage, ref play_sound, ref gen_gore, ref damage_source );
		}


		public override void PostHurt( bool pvp, bool quiet, double damage, int hit_direction, bool crit ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }
			if( quiet ) { return; }

			float yike = (float)damage * mymod.Config.PercentOfDamageAdrenalineBurst * (crit ? 2f : 1f);

			this.Logic.AddStamina( mymod, this.player, yike );
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
