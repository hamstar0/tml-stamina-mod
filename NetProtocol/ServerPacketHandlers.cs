using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Stamina.NetProtocol {
	static class ServerPacketHandlers {
		public static void HandlePacket( BinaryReader reader, int playerWho ) {
			StaminaNetProtocolTypes protocol = (StaminaNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case StaminaNetProtocolTypes.RequestModSettings:
				ServerPacketHandlers.ReceiveSettingsRequestOnServer( reader, playerWho );
				break;
			default:
				LogHelpers.Warn( "Invalid packet protocol: " + protocol );
				break;
			}
		}


		
		////////////////////////////////
		// Senders (Server)
		////////////////////////////////

		public static void SendSettingsFromServer( Player player ) {
			if( Main.netMode != 2 ) { return; } // Server only

			var mymod = StaminaMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)StaminaNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.ConfigJson.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}

		
		////////////////////////////////
		// Recipients (Server)
		////////////////////////////////

		private static void ReceiveSettingsRequestOnServer( BinaryReader reader, int playerWho ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ServerPacketHandlers.SendSettingsFromServer( Main.player[playerWho] );
		}
	}
}
