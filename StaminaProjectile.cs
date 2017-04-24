using Terraria;
using Terraria.ModLoader;


namespace Stamina {
	class StaminaProjectile : GlobalProjectile {
		public override void UseGrapple( Player player, ref int type ) {
			StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );
			var mymod = (StaminaMod)this.mod;

			modplayer.DrainStamina( mymod.Config.Data.SingularExertionRate, "grapple begin" );
//Main.NewText("UseGrapple " + StaminaMod.Config.Data.SingularExertionRate);
		}
	}
}