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
		public int ExerciseGrowthAmount = 1;
		public float ScaleAllStaminaRates = 1f;

		public float RechargeRate = 0.45f;
		public float EnergizedRate = 0.25f;

		public float SingularExertionRate = 12f;
		public float ItemUseRate = 0.525f;
		public float MagicItemUseRate = 0.525f / 2f;
		public float GrappleRate = 0.45f;
		public float SprintRate = 0.525f;
		public float JumpBegin = 6f;
		public float JumpHoldRate = 0.75f;
		public float DashRate = 28f;

		public int ExhaustionDuration = 180;
		public float ExhaustionRecover = 18f;

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
		public readonly static Version ConfigVersion = new Version( 1, 4, 3 );
		public static JsonConfig<ConfigurationData> Config { get; private set; }


		public StaminaMod() : base() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			//string filename = "Stamina " + this.Version + ".json";
			string filename = "Stamina Config.json";
			StaminaMod.Config = new JsonConfig<ConfigurationData>( filename, new ConfigurationData() );
		}

		public override void Load() {
			if( !StaminaMod.Config.Load() ) {
				StaminaMod.Config.Save();
			} else {
				Version vers_since = StaminaMod.Config.Data.VersionSinceUpdate != "" ?
					new Version( StaminaMod.Config.Data.VersionSinceUpdate ) :
					new Version();

				if( vers_since < StaminaMod.ConfigVersion ) {
					ErrorLogger.Log( "Stamina config updated to " + StaminaMod.ConfigVersion.ToString() );

					if( vers_since < new Version(1, 3, 3) ) {
						StaminaMod.Config.Data.ItemUseRate = new ConfigurationData().ItemUseRate;
						StaminaMod.Config.Data.GrappleRate = new ConfigurationData().GrappleRate;
						StaminaMod.Config.Data.SprintRate = new ConfigurationData().SprintRate;
						StaminaMod.Config.Data.JumpBegin = new ConfigurationData().JumpBegin;
						StaminaMod.Config.Data.JumpHoldRate = new ConfigurationData().JumpHoldRate;
						StaminaMod.Config.Data.DashRate = new ConfigurationData().DashRate;
						StaminaMod.Config.Data.ExhaustionRecover = new ConfigurationData().ExhaustionRecover;
					}
					if( vers_since < new Version( 1, 3, 4 ) ) {
						StaminaMod.Config.Data.StarStaminaHeal = new ConfigurationData().StarStaminaHeal;
					}
					if( vers_since < new Version( 1, 4, 1 ) ) {
						StaminaMod.Config.Data.FatigueAmount = new ConfigurationData().FatigueAmount;
						StaminaMod.Config.Data.BottledWaterFatigueHeal = new ConfigurationData().BottledWaterFatigueHeal;
					}
					if( vers_since < new Version( 1, 4, 2 ) ) {
						StaminaMod.Config.Data.ExerciseGrowthAmount = new ConfigurationData().ExerciseGrowthAmount;
					}
					if( vers_since < new Version( 1, 4, 3 ) ) {
						StaminaMod.Config.Data.FatigueExerciseThresholdPercentOfMaxStamina = new ConfigurationData().FatigueExerciseThresholdPercentOfMaxStamina;
					}
					StaminaMod.Config.Data.VersionSinceUpdate = StaminaMod.ConfigVersion.ToString();

					StaminaMod.Config.Save();
				}
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

				if( StaminaMod.Config.Data.CustomStaminaBarPositionX >= 0 ) {
					x = StaminaMod.Config.Data.CustomStaminaBarPositionX;
				}
				if( StaminaMod.Config.Data.CustomStaminaBarPositionY >= 0 ) {
					y = StaminaMod.Config.Data.CustomStaminaBarPositionY;
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
