using Terraria;
using Terraria.ModLoader;

namespace Stamina.Buffs {
	public class EnergizedBuff : ModBuff {
		public override void SetDefaults() {
			Main.buffName[this.Type] = "Energized";
			Main.buffTip[this.Type] = "Boing!";
			Main.debuff[this.Type] = false;
		}

		public override void Update( Player player, ref int buffIndex ) {
			StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>(mod);
			var mymod = (StaminaMod)this.mod;

			modplayer.AddStamina( mymod.Config.Data.EnergizedRate );
		}
	}
}
