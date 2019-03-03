using System.IO;
using Terraria;
using Terraria.ModLoader;
using System;
using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;


namespace Stamina {
	partial class StaminaMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-stamina-mod";

		public static string ConfigFileRelativePath {
			get { return JsonConfig.ConfigSubfolder + Path.DirectorySeparatorChar + StaminaConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reload configs outside of single player." );
			}
			if( StaminaMod.Instance != null ) {
				if( !StaminaMod.Instance.ConfigJson.LoadFile() ) {
					StaminaMod.Instance.ConfigJson.SaveFile();
				}
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reset to default configs outside of single player." );
			}

			var newConfig = new StaminaConfigData();
			newConfig.SetDefaults();

			StaminaMod.Instance.ConfigJson.SetData( newConfig );
			StaminaMod.Instance.ConfigJson.SaveFile();
		}
	}
}
