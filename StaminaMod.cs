using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Utilities.Config;
using Stamina.NetProtocol;
using System;
using Terraria.UI;
using System.Collections.Generic;
using Terraria.ID;
using HamstarHelpers.DebugHelpers;


namespace Stamina {
	class StaminaMod : Mod {
		public static StaminaMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-stamina-mod"; } }

		public static string ConfigFileRelativePath {
			get { return JsonConfig<StaminaConfigData>.RelativePath + Path.DirectorySeparatorChar + StaminaConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( StaminaMod.Instance != null ) {
				if( !StaminaMod.Instance.Config.LoadFile() ) {
					StaminaMod.Instance.Config.SaveFile();
				}
			}
		}


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
				JsonConfig<StaminaConfigData>.RelativePath, new StaminaConfigData() );
		}

		////////////////

		public override void Load() {
			StaminaMod.Instance = this;
			
			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_vers = new Version( 1, 2, 3 );
			if( hamhelpmod.Version < min_vers ) {
				throw new Exception( "Hamstar Helpers must be version " + min_vers.ToString() + " or greater." );
			}

			this.LoadConfigs();
		}

		private void LoadConfigs() {
			var old_config = new JsonConfig<StaminaConfigData>( "Stamina 1.2.0.json", "", new StaminaConfigData() );
			// Update old config to new location
			if( old_config.LoadFile() ) {
				old_config.DestroyFile();
				old_config.SetFilePath( this.Config.FileName, JsonConfig<StaminaConfigData>.RelativePath );
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

		public override void AddRecipeGroups() {
			var ninja_item = new RecipeGroup( delegate () { return Lang.misc[37]+" base ninja item"; },
				ItemID.Tabi, ItemID.BlackBelt );
			var basic_kb_shield = new RecipeGroup( delegate () { return Lang.misc[37] + " base knockback resist shield"; },
				ItemID.CobaltShield, ItemID.ObsidianShield );

			RecipeGroup.RegisterGroup( "Stamina:NinjaItems", ninja_item );
			RecipeGroup.RegisterGroup( "Stamina:BasicKnockbackResistShield", basic_kb_shield );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			if( !this.Config.Data.Enabled ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Resource Bars" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod func = delegate {
				Player player = Main.player[Main.myPlayer];
				StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this );
				if( !modplayer.HasEnteredWorld ) { return true; }

				SpriteBatch sb = Main.spriteBatch;

				int scr_x = Main.screenWidth - 172;
				int scr_y = 78;
				float alpha = modplayer.Logic.DrainingFX ? 1f : 0.65f;
				int stamina = (int)modplayer.Logic.Stamina;
				int max_stamina = modplayer.Logic.MaxStamina2;
				float fatigue = modplayer.Logic.Fatigue;
				bool is_exercising = modplayer.Logic.IsExercising;
				int threshold = fatigue > 0 ? modplayer.Logic.GetStaminaLossAmountNeededForExercise( this ) : -1;

				if( this.Config.Data.CustomStaminaBarPositionX >= 0 ) {
					scr_x = this.Config.Data.CustomStaminaBarPositionX;
				}
				if( this.Config.Data.CustomStaminaBarPositionY >= 0 ) {
					scr_y = this.Config.Data.CustomStaminaBarPositionY;
				}

				StaminaUI.DrawLongStaminaBar( sb, scr_x, scr_y, stamina, max_stamina, (int)fatigue, threshold, is_exercising, alpha, 1f );

				if( this.Config.Data.ShowMiniStaminaBar ) {
					int plr_x = (int)(player.position.X - Main.screenPosition.X) + (player.width / 2);
					int plr_y = (int)(player.position.Y - Main.screenPosition.Y) + player.height;
					plr_x += this.Config.Data.PlayerStaminaBarOffsetX;
					plr_y += this.Config.Data.PlayerStaminaBarOffsetY;

					StaminaUI.DrawShortStaminaBar( sb, plr_x, plr_y, stamina, max_stamina, (int)fatigue, threshold, is_exercising, alpha, 1f );
				}

				if( this.Config.Data.DEBUG_VIEW_DRAINERS ) {
					this.PrintStaminaDrainers( sb, modplayer );
				}

				return true;
			};

			var interface_layer = new LegacyGameInterfaceLayer( "Stamina: Meter", func, InterfaceScaleType.UI );
			layers.Insert( idx, interface_layer );
		}


		private void PrintStaminaDrainers( SpriteBatch sb, StaminaPlayer modplayer ) {
			var dict = modplayer.Logic.CurrentDrainTypes;
			int i = 0;

			foreach( var kv in dict.ToList() ) {
				if( kv.Value == 0f ) { continue; }

				//string msg = kv.Key.ToString() + ":  " + kv.Value;
				//sb.DrawString( Main.fontMouseText, msg, new Vector2( 8, (Main.screenHeight - 32) - (i * 24) ), Color.White );
				DebugHelpers.SetDisplay( kv.Key.ToString(), "" + kv.Value, 30 );

				dict[ kv.Key ] = 0f;
				i++;
			}
		}
	}
}
