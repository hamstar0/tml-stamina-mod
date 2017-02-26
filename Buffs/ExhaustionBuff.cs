using Terraria;
using Terraria.ModLoader;

namespace Stamina.Buffs {
	public class ExhaustionBuff : ModBuff {
		public override void SetDefaults() {
			Main.buffName[this.Type] = "Exhaustion";
			Main.buffTip[this.Type] = "You're out of steam";
			Main.debuff[this.Type] = true;
		}

		public override void Update( Player player, ref int buffIndex ) {
			StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );
			modplayer.WillApplyExhaustion = true;

			ExhaustionBuff.ApplyStatusExhaustion( player );
		}


		public static void ApplyStatusExhaustion( Player player ) {
			player.statDefense -= 5;
			player.noItems = true;
		}

		public static void ApplyMovementExhaustion( Player player ) {
			player.maxRunSpeed *= 0.65f;
			player.accRunSpeed = player.maxRunSpeed;
			player.moveSpeed *= 0.65f;

			int maxJump = (int)(Player.jumpHeight * 0.65);
			if( player.jump > maxJump ) { player.jump = maxJump; }
		}
	}
}
