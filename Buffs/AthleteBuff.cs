using Terraria;
using Terraria.ModLoader;


namespace Stamina.Buffs {
	class AthleteBuff : ModBuff {
		public static int MaxStaminaAdd { get { return 100; } }


		////////////////

		public override void SetDefaults() {
			this.DisplayName.SetDefault( "Athletic" );
			this.Description.SetDefault( "A winner is you" );

			Main.debuff[ this.Type ] = false;
		}
	}
}
