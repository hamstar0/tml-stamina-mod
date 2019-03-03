using System.IO;
using Terraria;
using Terraria.ModLoader;
using Stamina.NetProtocol;
using System;
using Terraria.ID;
using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;


namespace Stamina {
	partial class StaminaMod : Mod {
		public static StaminaMod Instance { get; private set; }



		////////////////

		internal JsonConfig<StaminaConfigData> ConfigJson { get; private set; }
		public StaminaConfigData Config => this.ConfigJson.Data;



		////////////////

		public StaminaMod() {
			StaminaMod.Instance = this;
			
			this.ConfigJson = new JsonConfig<StaminaConfigData>( StaminaConfigData.ConfigFileName,
				JsonConfig.ConfigSubfolder, new StaminaConfigData() );
		}

		////////////////

		public override void Load() {
			this.LoadConfigs();
		}

		private void LoadConfigs() {
			string depErr = TmlHelpers.ReportBadDependencyMods( this );
			if( depErr != null ) { throw new HamstarException( depErr ); }

			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Stamina updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			StaminaMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( StaminaAPI ), args );
		}


		////////////////
		
		public override void HandlePacket( BinaryReader reader, int playerWho ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( reader, playerWho );
			}
		}


		////////////////

		public override void AddRecipeGroups() {
			var ninjaItem = new RecipeGroup( delegate () { return Lang.misc[37]+" martial arts master item"; },
				ItemID.Tabi, ItemID.BlackBelt );
			var greaterHook = new RecipeGroup( delegate () { return Lang.misc[37] + " greater biome hook"; },
				ItemID.IlluminantHook, ItemID.WormHook, ItemID.TendonHook, ItemID.ThornHook );

			RecipeGroup.RegisterGroup( "Stamina:MartialArtsMasterItems", ninjaItem );
			RecipeGroup.RegisterGroup( "Stamina:GreaterBiomeHook", greaterHook );
		}
	}
}
