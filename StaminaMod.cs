using System.IO;
using Terraria;
using Terraria.ModLoader;
using Stamina.NetProtocol;
using System;
using Terraria.ID;
using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Components.Config;


namespace Stamina {
	partial class StaminaMod : Mod {
		public static StaminaMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-stamina-mod"; } }

		public static string ConfigFileRelativePath {
			get { return JsonConfig.ConfigSubfolder + Path.DirectorySeparatorChar + StaminaConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( StaminaMod.Instance != null ) {
				if( !StaminaMod.Instance.ConfigJson.LoadFile() ) {
					StaminaMod.Instance.ConfigJson.SaveFile();
				}
			}
		}


		////////////////

		internal JsonConfig<StaminaConfigData> ConfigJson { get; private set; }
		public StaminaConfigData Config { get { return this.ConfigJson.Data; } }


		////////////////

		public StaminaMod() : base() {
			StaminaMod.Instance = this;

			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.ConfigJson = new JsonConfig<StaminaConfigData>( StaminaConfigData.ConfigFileName,
				JsonConfig.ConfigSubfolder, new StaminaConfigData() );
		}

		////////////////

		public override void Load() {
			this.LoadConfigs();
		}

		private void LoadConfigs() {
			var old_config = new JsonConfig<StaminaConfigData>( "Stamina 1.2.0.json", "", new StaminaConfigData() );
			// Update old config to new location
			if( old_config.LoadFile() ) {
				old_config.DestroyFile();
				old_config.SetFilePath( this.ConfigJson.FileName, JsonConfig.ConfigSubfolder );
				this.ConfigJson = old_config;
			}

			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Stamina updated to " + StaminaConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			StaminaMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			if( args.Length == 0 ) { throw new Exception( "Undefined call type." ); }

			string call_type = args[0] as string;
			if( args == null ) { throw new Exception( "Invalid call type." ); }

			var new_args = new object[args.Length - 1];
			Array.Copy( args, 1, new_args, 0, args.Length - 1 );

			return StaminaAPI.Call( call_type, new_args );
		}


		////////////////
		
		public override void HandlePacket( BinaryReader reader, int player_who ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( this, reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( this, reader, player_who );
			}
		}


		////////////////

		public override void AddRecipeGroups() {
			var ninja_item = new RecipeGroup( delegate () { return Lang.misc[37]+" martial arts master item"; },
				ItemID.Tabi, ItemID.BlackBelt );
			var greater_hook = new RecipeGroup( delegate () { return Lang.misc[37] + " greater biome hook"; },
				ItemID.IlluminantHook, ItemID.WormHook, ItemID.TendonHook, ItemID.ThornHook );

			RecipeGroup.RegisterGroup( "Stamina:MartialArtsMasterItems", ninja_item );
			RecipeGroup.RegisterGroup( "Stamina:GreaterBiomeHook", greater_hook );
		}
	}
}
