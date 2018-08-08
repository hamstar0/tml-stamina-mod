using System.IO;
using Terraria;
using Terraria.ModLoader;
using System;
using HamstarHelpers.Components.Config;


namespace Stamina {
	partial class StaminaMod : Mod {
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

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var new_config = new StaminaConfigData();
			new_config.SetDefaults();

			StaminaMod.Instance.ConfigJson.SetData( new_config );
			StaminaMod.Instance.ConfigJson.SaveFile();
		}
	}
}
