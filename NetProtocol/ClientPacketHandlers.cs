using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Stamina.NetProtocol {
	static class ClientPacketHandlers {
		public static void HandlePacket( BinaryReader reader ) {
			StaminaNetProtocolTypes protocol = (StaminaNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case StaminaNetProtocolTypes.ModSettings:
				ClientPacketHandlers.ReceiveSettingsOnClient( reader );
				break;
			default:
				LogHelpers.Warn( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////////////////////
		// Senders (Client)
		////////////////////////////////

		public static void SendSettingsRequestFromClient( Player player ) {
			var mymod = StaminaMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)StaminaNetProtocolTypes.RequestModSettings );

			packet.Send();
		}



		////////////////////////////////
		// Recipients (Client)
		////////////////////////////////

		private static void ReceiveSettingsOnClient( BinaryReader reader ) {
			var mymod = StaminaMod.Instance;
			bool success;

			mymod.ConfigJson.DeserializeMe( reader.ReadString(), out success );

			if( !success ) {
				throw new HamstarException( "Could not deserialize settings." );
			}

			var modplayer = Main.LocalPlayer.GetModPlayer<StaminaPlayer>();
			modplayer.OnReceiveServerSettings();
		}
	}
}
