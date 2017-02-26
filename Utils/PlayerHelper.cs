using System;
using Terraria;

namespace Utils {
	class PlayerHelper {
		public static float MinimumRunSpeed( Player player ) {
			float max = (player.accRunSpeed + player.maxRunSpeed) / 2f;
			float wind = 0f;

			if( player.windPushed && (!player.mount.Active || player.velocity.Y != 0f) ) {
				wind = Math.Sign(Main.windSpeed) * 0.07f;
				if( Math.Abs(Main.windSpeed) > 0.5f ) {
					wind *= 1.37f;
				}
				if( player.velocity.Y != 0f ) {
					wind *= 1.5f;
				}
				if( player.controlLeft || player.controlRight ) {
					wind *= 0.8f;
				}
				if( Math.Sign(player.direction) != Math.Sign(wind) ) {
					max -= Math.Abs(wind) * 40f;
				}
			}

			return max;
		}
		

		public static bool IsFlying( Player player ) {
			bool wing_fly = !player.pulley && player.grappling[0] == -1 && !player.tongued &&
				player.controlJump && player.wingTime > 0f && (
				( player.wingsLogic > 0 && !player.jumpAgainCloud && player.jump == 0 && player.velocity.Y != 0f ) ||
				( player.controlDown && (
					player.wingsLogic == 22 ||
					player.wingsLogic == 28 ||
					player.wingsLogic == 30 ||
					player.wingsLogic == 32 ||
					player.wingsLogic == 29 ||
					player.wingsLogic == 33 ||
					player.wingsLogic == 35 ||
					player.wingsLogic == 37 )
				)
			);

			bool rocket_fly = (player.wingTime == 0f || player.wingsLogic == 0) &&
				player.rocketBoots > 0 &&
				player.controlJump &&
				player.canRocket &&
				player.rocketRelease &&
				!player.jumpAgainCloud &&
				player.rocketTime > 0;

			return wing_fly || rocket_fly;
		}


		public static int GetBuff( Player player, int type ) {
			for( int i = 0; i < 22; i++ ) {
				if( player.buffType[i] == type ) {
					return i;
				}
			}
			return -1;
		}
	}
}
