using Terraria;
using Terraria.ModLoader;


namespace Stamina.Buffs {
	class EnergizedBuff : ModBuff {
		public override void SetDefaults() {
			this.DisplayName.SetDefault( "Energized" );
			this.Description.SetDefault( "Boing!" );

			Main.debuff[this.Type] = false;
		}

		public override void Update( Player player, ref int buffIndex ) {
			StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>(mod);
			var mymod = (StaminaMod)this.mod;

			modplayer.Logic.AddStamina( (StaminaMod)this.mod, player, mymod.Config.EnergizedRate );
		}
	}
}
