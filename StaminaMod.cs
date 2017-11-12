using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Utilities.Config;
using ReLogic.Graphics;
using Stamina.NetProtocol;
using System;


namespace Stamina {
	public class StaminaMod : Mod {
		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-stamina-mod"; } }

		public static string ConfigRelativeFilePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + StaminaConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( StaminaMod.Instance != null ) {
				StaminaMod.Instance.Config.LoadFile();
			}
		}

		public static StaminaMod Instance { get; private set; }


		////////////////

		public JsonConfig<StaminaConfigData> Config { get; private set; }


		////////////////

		public StaminaMod() : base() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.Config = new JsonConfig<StaminaConfigData>( StaminaConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new StaminaConfigData() );
		}

		////////////////

		public override void Load() {
			StaminaMod.Instance = this;

			this.LoadConfigs();
		}

		private void LoadConfigs() {
			var old_config = new JsonConfig<StaminaConfigData>( "Stamina 1.2.0.json", "", new StaminaConfigData() );
			// Update old config to new location
			if( old_config.LoadFile() ) {
				old_config.DestroyFile();
				old_config.SetFilePath( this.Config.FileName, ConfigurationDataBase.RelativePath );
				this.Config = old_config;
			}

			if( !this.Config.LoadFile() ) {
				this.Config.SaveFile();
			}

			if( this.Config.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Stamina updated to " + StaminaConfigData.ConfigVersion.ToString() );
				this.Config.SaveFile();
			}
		}

		public override void Unload() {
			StaminaMod.Instance = null;
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

		public override void PostDrawInterface( SpriteBatch sb ) {
			if( !this.Config.Data.Enabled ) { return; }

			Player player = Main.player[ Main.myPlayer ];
			MyPlayer modplayer = player.GetModPlayer<MyPlayer>( this );

			if( modplayer.IsInitialized ) {
				int x = Main.screenWidth - 172;
				int y = 78;
				float alpha = modplayer.GetDrainingFX() ? 1f : 0.65f;
				int stamina = (int)modplayer.GetStamina();
				int max_stamina = modplayer.GetMaxStamina();
				float fatigue = modplayer.GetFatigue();
				bool is_exercising = modplayer.IsExercising();
				int threshold = fatigue > 0 ? modplayer.GetExerciseThreshold() : -1;

				if( this.Config.Data.CustomStaminaBarPositionX >= 0 ) {
					x = this.Config.Data.CustomStaminaBarPositionX;
				}
				if( this.Config.Data.CustomStaminaBarPositionY >= 0 ) {
					y = this.Config.Data.CustomStaminaBarPositionY;
				}

				StaminaUI.DrawStaminaBar( sb, x, y, stamina, max_stamina, (int)fatigue, threshold, is_exercising, alpha, 1f );

				//if( DebugHelper.DEBUGMODE ) {
				//	this.PrintStaminaDrainers(sb, modplayer);
				//}
			}
		}


		private void PrintStaminaDrainers( SpriteBatch sb, MyPlayer modplayer ) {
			var dict = modplayer.GetCurrentDrainTypes();
			int i = 0;

			foreach( string key in dict.Keys.ToList() ) {
				string msg = key + ":  " + dict[key];
				sb.DrawString( Main.fontMouseText, msg, new Vector2( 8, (Main.screenHeight - 32) - (i * 24) ), Color.White );

				dict[key] = 0f;
				i++;
			}
		}
	}
}
