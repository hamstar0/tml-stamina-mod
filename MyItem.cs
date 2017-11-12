using HamstarHelpers.ItemHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina {
	class MyItem : GlobalItem {
		/*public override bool UseItem( Item item, Player player ) {
			StaminaPlayer info = player.GetModPlayer<StaminaPlayer>(this.mod);
			info.AddDrainStamina( StaminaMod.Config.Data.ItemUseRate, "use item" );
Main.NewText("UseItem " + StaminaMod.Config.Data.ItemUseRate);
			
			return base.UseItem( item, player );
		}*/

		public override bool WingUpdate( int wings, Player player, bool in_use ) {
			MyPlayer modplayer = player.GetModPlayer<MyPlayer>( this.mod );

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
				if( MyItem.StarUseCooldown[player.whoAmI] == 0 ) {
					MyPlayer modplayer = player.GetModPlayer<MyPlayer>( this.mod );

					if( modplayer.GetStamina() < modplayer.GetMaxStamina() ) {
						modplayer.AddStamina( mymod.Config.Data.StarStaminaHeal );

						if( --item.stack <= 0 ) {
							ItemHelpers.DestroyItem(item);
						}
					}

					MyItem.StarUseCooldown[player.whoAmI]++;
				}
				MyItem.StarUseCooldown[player.whoAmI]++;
			}

			return base.UseItem( item, player );
		}


		public override bool ConsumeItem( Item item, Player player ) {
			var mymod = (StaminaMod)this.mod;
			bool can_consume = base.ConsumeItem( item, player );
			if( !mymod.Config.Data.Enabled ) { return can_consume; }

			if( can_consume && item.type == ItemID.BottledWater ) {
				var modplayer = player.GetModPlayer<MyPlayer>( this.mod );
				modplayer.AddFatigue( -mymod.Config.Data.BottledWaterFatigueHeal );
				modplayer.AddStamina( (float)mymod.Config.Data.BottledWaterFatigueHeal * mymod.Config.Data.ScaleAllStaminaRates );
			}
			return can_consume;
		}
	}
}