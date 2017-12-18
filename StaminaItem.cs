using HamstarHelpers.ItemHelpers;
using Stamina.Buffs;
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

		////////////////

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

		////////////////

		public override bool CanUseItem( Item item, Player player ) {
			if( player.FindBuffIndex( this.mod.BuffType<ExhaustionBuff>() ) != -1 ) {
				var mymod = (StaminaMod)this.mod;
				if( mymod.Config.Data.ExhaustionBlocksItems ) {
					return false;
				}
			}
			return base.CanUseItem( item, player );
		}


		public static int[] StarUseCooldown = new int[Main.player.Length];

		public override bool UseItem( Item item, Player player ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return base.UseItem( item, player ); }

			if( mymod.Config.Data.ConsumableStars && item.type == ItemID.FallenStar && item.stack > 0 ) {
				if( StaminaItem.StarUseCooldown[player.whoAmI] == 0 ) {
					StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );

					if( modplayer.Logic.Stamina < modplayer.Logic.MaxStamina2 ) {
						modplayer.Logic.AddStamina( mymod, player, mymod.Config.Data.StarStaminaHeal );

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
			var config = mymod.Config.Data;
			if( !config.Enabled ) { return base.ConsumeItem( item, player ); }

			if( config.ConsumableBottledWater && item.type == ItemID.BottledWater ) {
				var modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );
				modplayer.Logic.AddFatigue( player, -config.BottledWaterFatigueHeal );
				modplayer.Logic.AddStamina( mymod, player, (float)config.BottledWaterFatigueHeal );
			}
			return base.ConsumeItem( item, player );
		}
	}
}