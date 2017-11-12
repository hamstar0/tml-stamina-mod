using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Stamina.NetProtocol {
	static class ClientPacketHandlers {
		public static void HandlePacket( StaminaMod mymod, BinaryReader reader ) {
			StaminaNetProtocolTypes protocol = (StaminaNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case StaminaNetProtocolTypes.ModSettings:
				ClientPacketHandlers.ReceiveSettingsOnClient( mymod, reader );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////////////////////
		// Senders (Client)
		////////////////////////////////

		public static void SendSettingsRequestFromClient( Mod mod, Player player ) {
			if( Main.netMode != 1 ) { return; } // Client only

			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)StaminaNetProtocolTypes.RequestModSettings );

			packet.Send();
		}



		////////////////////////////////
		// Recipients (Client)
		////////////////////////////////

		private static void ReceiveSettingsOnClient( StaminaMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			mymod.Config.DeserializeMe( reader.ReadString() );
		}
	}
}
