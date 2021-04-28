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
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			var mymod = (StaminaMod)this.mod;

			myplayer.Logic.AddStamina( player, mymod.Config.EnergizedRate );
		}
	}
}
