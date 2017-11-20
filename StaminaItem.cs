using HamstarHelpers.ItemHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina {
	class StaminaItem : GlobalItem {
		/*public override bool UseItem( Item item, Player player ) {
			StaminaPlayer info = player.GetModPlayer<StaminaPlayer>(this.mod);
			info.AddDrainStamina( StaminaMod.Config.Data.ItemUseRate, "use item" );
Main.NewText("UseItem " + StaminaMod.Config.Data.ItemUseRate);
			
			return base.UseItem( item, player );
		}*/

		public override bool WingUpdate( int wings, Player player, bool in_use ) {
			StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );

			modplayer.IsFlying = in_use;
			modplayer.HasCheckedFlying = true;

			return false;
		}
		
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			if( item.type == 75 ) { // Fallen Star
				tooltips.Add( new TooltipLine( mymod, "StaminaPurpose", "Recovers some stamina on use" ) );
			}
			if( item.type == 126 ) {    // Bottled Water
				tooltips.Add( new TooltipLine( mymod, "StaminaPurpose", "Recovers some fatigue on use" ) );
			}
		}


		public static int[] StarUseCooldown = new int[Main.player.Length];

		public override bool UseItem( Item item, Player player ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return base.UseItem( item, player ); }

			if( item.type == ItemID.FallenStar && item.stack > 0 && mymod.Config.Data.ConsumableStars ) {
				if( StaminaItem.StarUseCooldown[player.whoAmI] == 0 ) {
					StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );

					if( modplayer.Logic.Stamina < modplayer.Logic.MaxStamina ) {
						modplayer.Logic.AddStamina( mymod, mymod.Config.Data.StarStaminaHeal );

						if( --item.stack <= 0 ) {
							ItemHelpers.DestroyItem(item);
						}
					}

					StaminaItem.StarUseCooldown[player.whoAmI]++;
				}
				StaminaItem.StarUseCooldown[player.whoAmI]++;
			}

			return base.UseItem( item, player );
		}


		public override bool ConsumeItem( Item item, Player player ) {
			var mymod = (StaminaMod)this.mod;
			bool can_consume = base.ConsumeItem( item, player );
			if( !mymod.Config.Data.Enabled ) { return can_consume; }

			if( can_consume && item.type == ItemID.BottledWater ) {
				var modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );
				modplayer.Logic.AddFatigue( -mymod.Config.Data.BottledWaterFatigueHeal );
				modplayer.Logic.AddStamina( mymod, ( float)mymod.Config.Data.BottledWaterFatigueHeal * mymod.Config.Data.ScaleAllStaminaRates );
			}
			return can_consume;
		}
	}
}