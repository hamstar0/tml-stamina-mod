using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace Stamina {
	partial class StaminaPlayer : ModPlayer {
		public override void DrawEffects( PlayerDrawInfo draw_info, ref float r, ref float g, ref float b, ref float a, ref bool full_bright ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }
			if( this.Logic == null ) { return; }
			if( this.Logic.Stamina > 0 ) { return; }
			
			if( --this.SweatDelay <= 0 ) {
				this.SweatDelay = 3;

				int width = 32;
				int height = 16;

				var pos = new Vector2( this.player.position.X - (this.player.width / 2), this.player.position.Y );
				if( this.player.gravDir < 0 ) {
					pos.Y += this.player.height;
				}

				var vel = this.player.velocity;
				vel.X += 6f - (Main.rand.NextFloat() * 8f);
				vel.Y += Main.rand.NextFloat() * -8f;

				int dust = Dust.NewDust( pos, width, height, 52, vel.X, vel.Y );    // 33, 98-105, 154(-), 266
				//Main.dust[dust].alpha = 0;
			}
		}
	}
}
