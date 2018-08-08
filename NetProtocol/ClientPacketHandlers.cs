using HamstarHelpers.Components.Errors;
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
			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)StaminaNetProtocolTypes.RequestModSettings );

			packet.Send();
		}



		////////////////////////////////
		// Recipients (Client)
		////////////////////////////////

		private static void ReceiveSettingsOnClient( StaminaMod mymod, BinaryReader reader ) {
			bool success;

			mymod.ConfigJson.DeserializeMe( reader.ReadString(), out success );

			if( success ) {
				throw new HamstarException( "Stamina.NetProtocols.ClientPacketHandlers.ReceiveSettingsOnClient - Could not deserialize settings." );
			}

			var modplayer = Main.LocalPlayer.GetModPlayer<StaminaPlayer>();
			modplayer.OnReceiveServerSettings();
		}
	}
}
