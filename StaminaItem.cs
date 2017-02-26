using Stamina.Utils;
using Terraria;
using Terraria.ModLoader;


namespace Stamina {
	class StaminaItem : GlobalItem {
		/*public override bool UseItem( Item item, Player player ) {
			StaminaPlayer info = player.GetModPlayer<StaminaPlayer>(this.mod);
			info.AddDrainStamina( StaminaMod.Config.Data.ItemUseRate, "use item" );
Main.NewText("UseItem " + StaminaMod.Config.Data.ItemUseRate);
			
			return base.UseItem( item, player );
		}*/

		public override void WingUpdate( int wings, Player player, bool inUse ) {
			StaminaPlayer info = player.GetModPlayer<StaminaPlayer>( this.mod );
			info.IsFlying = inUse;
			info.HasCheckedFlying = true;
		}

		public override void SetDefaults( Item item ) {
			if( item.type == 75 ) {	// Fallen Star
				this.AddTooltip2(item, "Recovers some stamina on use.");
			}
			if( item.type == 126 ) {	// Bottled Water
				this.AddTooltip2( item, "Recovers some fatigue on use." );
			}
			base.SetDefaults( item );
		}


		public static int[] StarUseCooldown = new int[Main.player.Length];

		public override bool UseItem( Item item, Player player ) {
			if( item.type == 75 && item.stack > 0 && StaminaMod.Config.Data.ConsumableStars ) {
				if( StaminaItem.StarUseCooldown[player.whoAmI] == 0 ) {
					StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );

					if( modplayer.GetStamina() < modplayer.GetMaxStamina() ) {
						modplayer.AddStamina( StaminaMod.Config.Data.StarStaminaHeal );

						if( --item.stack <= 0 ) {
							ItemHelper.DestroyItem(item);
						}
					}

					StaminaItem.StarUseCooldown[player.whoAmI]++;
				}
				StaminaItem.StarUseCooldown[player.whoAmI]++;
			}

			return base.UseItem( item, player );
		}


		public override bool ConsumeItem( Item item, Player player ) {
			bool can_consume = base.ConsumeItem( item, player );
			if( can_consume && item.type == 126 ) {	// Bottled Water
				var modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );
				modplayer.AddFatigue( -StaminaMod.Config.Data.BottledWaterFatigueHeal );
				modplayer.AddStamina( (float)StaminaMod.Config.Data.BottledWaterFatigueHeal * StaminaMod.Config.Data.ScaleAllStaminaRates );
			}
			return can_consume;
		}
	}
}