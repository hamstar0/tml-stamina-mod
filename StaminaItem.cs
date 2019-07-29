using HamstarHelpers.Helpers.Items;
using Stamina.Buffs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina {
	class StaminaItem : GlobalItem {
		public static int[] StarUseCooldown = new int[Main.player.Length];



		////////////////

		/*public override bool UseItem( Item item, Player player ) {
			StaminaPlayer info = player.GetModPlayer<StaminaPlayer>(this.mod);
			info.AddDrainStamina( StaminaMod.Config.Data.ItemUseRate, "use item" );
Main.NewText("UseItem " + StaminaMod.Config.Data.ItemUseRate);
			
			return base.UseItem( item, player );
		}*/

		public override bool WingUpdate( int wings, Player player, bool inUse ) {
			StaminaPlayer myplayer = player.GetModPlayer<StaminaPlayer>();

			myplayer.IsFlying = inUse;
			myplayer.HasCheckedFlying = true;

			return false;
		}

		////////////////

		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

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
				if( mymod.Config.ExhaustionBlocksItems ) {
					return false;
				}
			}
			return base.CanUseItem( item, player );
		}


		public override bool UseItem( Item item, Player player ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Enabled ) { return base.UseItem( item, player ); }

			if( mymod.Config.ConsumableStars && item.type == ItemID.FallenStar && item.stack > 0 ) {
				if( StaminaItem.StarUseCooldown[player.whoAmI] == 0 ) {
					StaminaPlayer myplayer = player.GetModPlayer<StaminaPlayer>();

					if( myplayer.Logic.Stamina < myplayer.Logic.MaxStamina2 ) {
						myplayer.Logic.AddStamina( player, mymod.Config.StarStaminaHeal );

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
			var config = mymod.Config;
			if( !config.Enabled ) { return base.ConsumeItem( item, player ); }

			if( config.ConsumableBottledWater && item.type == ItemID.BottledWater ) {
				var myplayer = player.GetModPlayer<StaminaPlayer>();
				myplayer.Logic.AddFatigue( player, -config.BottledWaterFatigueHeal );
				myplayer.Logic.AddStamina( player, (float)config.BottledWaterFatigueHeal );
			}
			return base.ConsumeItem( item, player );
		}
	}
}