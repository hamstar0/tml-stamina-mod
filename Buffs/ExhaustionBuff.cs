using Terraria;
using Terraria.ModLoader;


namespace Stamina.Buffs {
	class ExhaustionBuff : ModBuff {
		public override void SetDefaults() {
			this.DisplayName.SetDefault( "Exhaustion" );
			this.Description.SetDefault( "You're out of steam" );
			
			Main.debuff[this.Type] = true;
		}

		public override void Update( Player player, ref int buffIndex ) {
			MyPlayer modplayer = player.GetModPlayer<MyPlayer>( this.mod );

			modplayer.WillApplyExhaustion = true;
			ExhaustionBuff.ApplyStatusExhaustion( (StaminaMod)this.mod, player );
		}


		public static void ApplyStatusExhaustion( StaminaMod mymod, Player player ) {
			if( mymod.Config.Data.ExhaustionLowersDefense ) {
				player.statDefense -= 5;
			}
			if( mymod.Config.Data.ExhaustionBlocksItems ) {
				player.noItems = true;
			}
		}

		public static void ApplyMovementExhaustion( StaminaMod mymod, Player player ) {
			if( mymod.Config.Data.ExhaustionSlowsMovement ) {
				player.maxRunSpeed *= 0.65f;
				player.accRunSpeed = player.maxRunSpeed;
				player.moveSpeed *= 0.65f;

				int maxJump = (int)(Player.jumpHeight * 0.65);
				if( player.jump > maxJump ) { player.jump = maxJump; }
			}
		}
	}
}
