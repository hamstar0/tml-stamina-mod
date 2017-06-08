using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Stamina {
	public enum StaminaNetProtocolTypes : byte {
		RequestModSettings,
		ModSettings
	}


	public static class StaminaNetProtocol {
		public static void RoutePacket( StaminaMod mymod, BinaryReader reader ) {
			StaminaNetProtocolTypes protocol = (StaminaNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case StaminaNetProtocolTypes.RequestModSettings:
				StaminaNetProtocol.ReceiveSettingsRequestOnServer( mymod, reader );
				break;
			case StaminaNetProtocolTypes.ModSettings:
				StaminaNetProtocol.ReceiveSettingsOnClient( mymod, reader );
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
			packet.Write( (int)player.whoAmI );

			packet.Send();
		}

		////////////////////////////////
		// Senders (Server)
		////////////////////////////////

		public static void SendSettingsFromServer( StaminaMod mymod, Player player ) {
			if( Main.netMode != 2 ) { return; } // Server only
			
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)StaminaNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.Config.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}



		////////////////////////////////
		// Recipients (Client)
		////////////////////////////////

		private static void ReceiveSettingsOnClient( StaminaMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			mymod.Config.DeserializeMe( reader.ReadString() );
		}
		

		////////////////////////////////
		// Recipients (Server)
		////////////////////////////////

		private static void ReceiveSettingsRequestOnServer( StaminaMod mymod, BinaryReader reader ) {
			if( Main.netMode != 2 ) { return; } // Server only

			int who = reader.ReadInt32();

			if( who < 0 || who >= Main.player.Length || Main.player[who] == null ) {
				ErrorLogger.Log( "StaminaNetProtocol.ReceiveSettingsRequestOnServer - Invalid player whoAmI. " + who );
				return;
			}

			StaminaNetProtocol.SendSettingsFromServer( mymod, Main.player[who] );
		}
	}
}
