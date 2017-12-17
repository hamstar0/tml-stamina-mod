using Stamina.Logic;
using Terraria;
using Terraria.ModLoader;


namespace Stamina {
	class StaminaProjectile : GlobalProjectile {
		public override void UseGrapple( Player player, ref int type ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			var modplayer = player.GetModPlayer<StaminaPlayer>( this.mod );
			modplayer.Logic.DrainStaminaViaGrappleBegin( mymod, player );
//Main.NewText("UseGrapple " + StaminaMod.Config.Data.SingularExertionRate);
		}
	}
}