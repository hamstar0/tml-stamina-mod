using Terraria;
using Terraria.ModLoader;


namespace Stamina {
	class StaminaProjectile : GlobalProjectile {
		public override void UseGrapple( Player player, ref int type ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic.DrainStaminaViaGrappleBegin( player );
//Main.NewText("UseGrapple " + StaminaMod.Config.Data.SingularExertionRate);
		}
	}
}
