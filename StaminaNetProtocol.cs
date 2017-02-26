using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Stamina {
	public enum StaminaNetProtocolTypes : byte {
		SendSettingsRequest,
		SendSettings
	}


	public class StaminaNetProtocol {
		public static void RoutePacket( Mod mod, BinaryReader reader ) {
			StaminaNetProtocolTypes protocol = (StaminaNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case StaminaNetProtocolTypes.SendSettingsRequest:
				StaminaNetProtocol.ReceiveSettingsRequestOnServer( mod, reader );
				break;
			case StaminaNetProtocolTypes.SendSettings:
				StaminaNetProtocol.ReceiveSettingsOnClient( mod, reader );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////////////////////
		// Senders (Client)
		////////////////////////////////

		public static void RequestSettingsFromServer( Mod mod, Player player ) {
			if( Main.netMode != 1 ) { return; } // Client only

			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)StaminaNetProtocolTypes.SendSettingsRequest );
			packet.Write( (int)player.whoAmI );

			packet.Send();
		}

		////////////////////////////////
		// Senders (Server)
		////////////////////////////////

		public static void SendSettingsFromServer( Mod mod, Player player ) {
			if( Main.netMode != 2 ) { return; } // Server only
			
			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)StaminaNetProtocolTypes.SendSettings );
			packet.Write( (int)StaminaMod.Config.Data.InitialStamina );
			packet.Write( (float)StaminaMod.Config.Data.RechargeRate );
			packet.Write( (float)StaminaMod.Config.Data.EnergizedRate );
			packet.Write( (float)StaminaMod.Config.Data.SingularExertionRate );
			packet.Write( (float)StaminaMod.Config.Data.ItemUseRate );
			packet.Write( (float)StaminaMod.Config.Data.MagicItemUseRate );
			packet.Write( (float)StaminaMod.Config.Data.GrappleRate );
			packet.Write( (float)StaminaMod.Config.Data.SprintRate );
			packet.Write( (float)StaminaMod.Config.Data.JumpBegin );
			packet.Write( (float)StaminaMod.Config.Data.JumpHoldRate );
			packet.Write( (float)StaminaMod.Config.Data.DashRate );
			packet.Write( (float)StaminaMod.Config.Data.ScaleAllStaminaRates );
			packet.Write( (int)StaminaMod.Config.Data.ExhaustionDuration );
			packet.Write( (float)StaminaMod.Config.Data.ExhaustionRecover );
			packet.Write( (bool)StaminaMod.Config.Data.CraftableEnergyDrinks );
			packet.Write( (bool)StaminaMod.Config.Data.ConsumableStars );
			packet.Write( (int)StaminaMod.Config.Data.StarStaminaHeal );
			packet.Write( (float)StaminaMod.Config.Data.PercentOfDamageAdrenalineBurst );
			packet.Write( (float)StaminaMod.Config.Data.FatigueAmount );
			packet.Write( (int)StaminaMod.Config.Data.FatigueRecoverDuration );
			packet.Write( (int)StaminaMod.Config.Data.ExerciseGrowthAmount );
			packet.Write( (int)StaminaMod.Config.Data.MaxStaminaAmount );
			packet.Write( (int)StaminaMod.Config.Data.BottledWaterFatigueHeal );

			packet.Send( (int)player.whoAmI );
		}



		////////////////////////////////
		// Recipients (Client)
		////////////////////////////////

		private static void ReceiveSettingsOnClient( Mod mod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			StaminaMod.Config.Data.InitialStamina = (int)reader.ReadInt32();
			StaminaMod.Config.Data.RechargeRate = (float)reader.ReadSingle();
			StaminaMod.Config.Data.EnergizedRate = (float)reader.ReadSingle();
			StaminaMod.Config.Data.SingularExertionRate = (float)reader.ReadSingle();
			StaminaMod.Config.Data.ItemUseRate = (float)reader.ReadSingle();
			StaminaMod.Config.Data.MagicItemUseRate = (float)reader.ReadSingle();
			StaminaMod.Config.Data.GrappleRate = (float)reader.ReadSingle();
			StaminaMod.Config.Data.SprintRate = (float)reader.ReadSingle();
			StaminaMod.Config.Data.JumpBegin = (float)reader.ReadSingle();
			StaminaMod.Config.Data.JumpHoldRate = (float)reader.ReadSingle();
			StaminaMod.Config.Data.DashRate = (float)reader.ReadSingle();
			StaminaMod.Config.Data.ScaleAllStaminaRates = (float)reader.ReadSingle();
			StaminaMod.Config.Data.ExhaustionDuration = (int)reader.ReadInt32();
			StaminaMod.Config.Data.ExhaustionRecover = (float)reader.ReadSingle();
			StaminaMod.Config.Data.CraftableEnergyDrinks = (bool)reader.ReadBoolean();
			StaminaMod.Config.Data.ConsumableStars = (bool)reader.ReadBoolean();
			StaminaMod.Config.Data.StarStaminaHeal = (int)reader.ReadInt32();
			StaminaMod.Config.Data.PercentOfDamageAdrenalineBurst = (float)reader.ReadSingle();
			StaminaMod.Config.Data.FatigueAmount = (float)reader.ReadSingle();
			StaminaMod.Config.Data.FatigueRecoverDuration = (int)reader.ReadInt32();
			StaminaMod.Config.Data.ExerciseGrowthAmount = (int)reader.ReadInt32();
			StaminaMod.Config.Data.MaxStaminaAmount = (int)reader.ReadInt32();
			StaminaMod.Config.Data.BottledWaterFatigueHeal = (int)reader.ReadInt32();
		}
		

		////////////////////////////////
		// Recipients (Server)
		////////////////////////////////

		private static void ReceiveSettingsRequestOnServer( Mod mod, BinaryReader reader ) {
			if( Main.netMode != 2 ) { return; } // Server only

			int who = reader.ReadInt32();

			if( who < 0 || who >= Main.player.Length || Main.player[who] == null ) {
				ErrorLogger.Log( "StaminaNetProtocol.ReceiveSettingsRequestOnServer - Invalid player whoAmI. " + who );
				return;
			}

			StaminaNetProtocol.SendSettingsFromServer( mod, Main.player[who] );
		}
	}
}
