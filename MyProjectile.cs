using Terraria;
using Terraria.ModLoader;


namespace Stamina {
	class MyProjectile : GlobalProjectile {
		public override void UseGrapple( Player player, ref int type ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			var modplayer = player.GetModPlayer<MyPlayer>( this.mod );
			modplayer.Logic.DrainStamina( mymod.Config.Data.SingularExertionRate, "grapple begin" );
//Main.NewText("UseGrapple " + StaminaMod.Config.Data.SingularExertionRate);
		}
	}
}