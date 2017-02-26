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
			StaminaPlayer info = player.GetModPlayer<StaminaPlayer>(mod);
			info.AddStamina( StaminaMod.Config.Data.EnergizedRate );
		}
	}
}
