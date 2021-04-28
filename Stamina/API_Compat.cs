using Stamina.Logic;
using System;
using Terraria;


namespace Stamina {
	public static partial class StaminaAPI {
		public static void ResetPlayerModData( Player player ) {    // <- In accordance with Mod Helpers convention
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			myplayer.Logic = new StaminaLogic( -1, false );
		}
	}
}
