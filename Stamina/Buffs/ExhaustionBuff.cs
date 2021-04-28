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
			StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>();

			modplayer.WillApplyExhaustion = true;
			ExhaustionBuff.ApplyStatusExhaustion( player );
		}


		public static void ApplyStatusExhaustion( Player player ) {
			var mymod = StaminaMod.Instance;
			if( mymod.Config.ExhaustionLowersDefense ) {
				player.statDefense -= 5;
			}
			if( mymod.Config.ExhaustionBlocksItems ) {
				player.noItems = true;
			}
		}

		public static void ApplyMovementExhaustion( Player player ) {
			var mymod = StaminaMod.Instance;

			if( mymod.Config.ExhaustionSlowsMovement ) {
				player.maxRunSpeed *= 0.65f;
				player.accRunSpeed = player.maxRunSpeed;
				player.moveSpeed *= 0.65f;

				int maxJump = (int)(Player.jumpHeight * 0.65);
				if( player.jump > maxJump ) { player.jump = maxJump; }
			}
		}
	}
}
