using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Stamina.NetProtocol {
	static class ServerPacketHandlers {
		public static void HandlePacket( StaminaMod mymod, BinaryReader reader, int player_who ) {
			StaminaNetProtocolTypes protocol = (StaminaNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case StaminaNetProtocolTypes.RequestModSettings:
				ServerPacketHandlers.ReceiveSettingsRequestOnServer( mymod, reader, player_who );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}


		
		////////////////////////////////
		// Senders (Server)
		////////////////////////////////

		public static void SendSettingsFromServer( StaminaMod mymod, Player player ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)StaminaNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.ConfigJson.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}

		
		////////////////////////////////
		// Recipients (Server)
		////////////////////////////////

		private static void ReceiveSettingsRequestOnServer( StaminaMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ServerPacketHandlers.SendSettingsFromServer( mymod, Main.player[player_who] );
		}
	}
}
