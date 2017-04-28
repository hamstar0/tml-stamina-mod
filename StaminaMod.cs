using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utils;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Utils.JsonConfig;


namespace Stamina {
	public class ConfigurationData {
		public string VersionSinceUpdate = "";

		public int InitialStamina = 100;
		public int MaxStaminaAmount = 400;
		public int ExerciseGrowthAmount = 2;
		public float ScaleAllStaminaRates = 1f;

		public float RechargeRate = 0.45f;
		public float EnergizedRate = 0.25f;

		public float SingularExertionRate = 12f;
		public float ItemUseRate = 0.525f;
		public float MagicItemUseRate = 0.2f;
		public float GrappleRate = 0.45f;
		public float SprintRate = 0.525f;
		public float JumpBegin = 5f;
		public float JumpHoldRate = 0.75f;
		public float DashRate = 28f;

		public int ExhaustionDuration = 180;
		public float ExhaustionRecover = 18f;
		public bool ExhaustionLowersDefense = true;
		public bool ExhaustionBlocksItems = true;
		public bool ExhaustionSlowsMovement = true;

		public bool CraftableEnergyDrinks = true;
		public bool ConsumableStars = true;
		public int StarStaminaHeal = 50;
		public int BottledWaterFatigueHeal = 50;

		public float PercentOfDamageAdrenalineBurst = 0.08f;

		public float FatigueAmount = 12f;
		public int FatigueRecoverDuration = 60;
		public int FatigueExerciseThresholdAmountRemoved = 0;
		public float FatigueExerciseThresholdPercentOfMaxStamina = 0.32f;

		public int CustomStaminaBarPositionX = -1;
		public int CustomStaminaBarPositionY = -1;
	}



	public class StaminaMod : Mod {
		public readonly static Version ConfigVersion = new Version( 1, 4, 6 );
		public JsonConfig<ConfigurationData> Config { get; private set; }


		public StaminaMod() : base() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			string filename = "Stamina Config.json";
			this.Config = new JsonConfig<ConfigurationData>( filename, "Mod Configs", new ConfigurationData() );
		}

		public override void Load() {
			var old_config = new JsonConfig<ConfigurationData>( this.Config.FileName, "", new ConfigurationData() );
			// Update old config to new location
			if( old_config.LoadFile() ) {
				old_config.DestroyFile();
				old_config.SetFilePath( this.Config.FileName, "Mod Configs" );
				this.Config = old_config;
			} else if( !this.Config.LoadFile() ) {
				this.Config.SaveFile();
			}
			
			Version vers_since = this.Config.Data.VersionSinceUpdate != "" ?
				new Version( this.Config.Data.VersionSinceUpdate ) :
				new Version();

			if( vers_since < StaminaMod.ConfigVersion ) {
				var new_config = new ConfigurationData();
				ErrorLogger.Log( "Stamina config updated to " + StaminaMod.ConfigVersion.ToString() );

				if( vers_since < new Version(1, 3, 3) ) {
					this.Config.Data.ItemUseRate = new_config.ItemUseRate;
					this.Config.Data.GrappleRate = new_config.GrappleRate;
					this.Config.Data.SprintRate = new_config.SprintRate;
					this.Config.Data.JumpHoldRate = new_config.JumpHoldRate;
					this.Config.Data.DashRate = new_config.DashRate;
					this.Config.Data.ExhaustionRecover = new_config.ExhaustionRecover;
				}
				if( vers_since < new Version( 1, 3, 4 ) ) {
					this.Config.Data.StarStaminaHeal = new_config.StarStaminaHeal;
				}
				if( vers_since < new Version( 1, 4, 1 ) ) {
					this.Config.Data.FatigueAmount = new_config.FatigueAmount;
					this.Config.Data.BottledWaterFatigueHeal = new_config.BottledWaterFatigueHeal;
				}
				if( vers_since < new Version( 1, 4, 3 ) ) {
					this.Config.Data.FatigueExerciseThresholdPercentOfMaxStamina = new_config.FatigueExerciseThresholdPercentOfMaxStamina;
				}
				if( vers_since < new Version( 1, 4, 5 ) ) {
					this.Config.Data.JumpBegin = new_config.JumpBegin;
				}
				if( vers_since < new Version( 1, 4, 6 ) ) {
					this.Config.Data.MagicItemUseRate = new_config.MagicItemUseRate;
					this.Config.Data.ExerciseGrowthAmount = new_config.ExerciseGrowthAmount;
				}

				this.Config.Data.VersionSinceUpdate = StaminaMod.ConfigVersion.ToString();
				this.Config.SaveFile();
			}
		}



		////////////////

		public override void HandlePacket( BinaryReader reader, int whoAmI ) {
			StaminaNetProtocol.RoutePacket( this, reader );
		}


		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			Player player = Main.player[ Main.myPlayer ];
			StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>( this );

			UIHelper.DrawPlayerLabels( sb );

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

				if( DebugHelper.DEBUGMODE ) {
					this.PrintStaminaDrainers(sb, modplayer);
				}
			}

			DebugHelper.PrintToBatch( sb );
		}

		private void PrintStaminaDrainers( SpriteBatch sb, StaminaPlayer modplayer ) {
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
