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

		public override bool NewWingUpdate( int wings, Player player, bool in_use ) {
			StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );

			modplayer.IsFlying = in_use;
			modplayer.HasCheckedFlying = true;

			return false;
		}

		public override void SetDefaults( Item item ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			if( item.type == 75 ) {	// Fallen Star
				this.AddTooltip2(item, "Recovers some stamina on use.");
			}
			if( item.type == 126 ) {	// Bottled Water
				this.AddTooltip2( item, "Recovers some fatigue on use." );
			}
		}


		public static int[] StarUseCooldown = new int[Main.player.Length];

		public override bool UseItem( Item item, Player player ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return base.UseItem( item, player ); }

			if( item.type == 75 && item.stack > 0 && mymod.Config.Data.ConsumableStars ) {
				if( StaminaItem.StarUseCooldown[player.whoAmI] == 0 ) {
					StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );

					if( modplayer.GetStamina() < modplayer.GetMaxStamina() ) {
						modplayer.AddStamina( mymod.Config.Data.StarStaminaHeal );

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
			var mymod = (StaminaMod)this.mod;
			bool can_consume = base.ConsumeItem( item, player );
			if( !mymod.Config.Data.Enabled ) { return can_consume; }

			if( can_consume && item.type == 126 ) {	// Bottled Water
				var modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );
				modplayer.AddFatigue( -mymod.Config.Data.BottledWaterFatigueHeal );
				modplayer.AddStamina( (float)mymod.Config.Data.BottledWaterFatigueHeal * mymod.Config.Data.ScaleAllStaminaRates );
			}
			return can_consume;
		}
	}
}