using HamstarHelpers.Utilities.Config;
using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Stamina {
	public class StaminaConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 1, 4, 13 );
		public readonly static string ConfigFileName = "Stamina Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public int InitialStamina = 100;
		public int MaxStaminaAmount = 400;
		public int ExerciseGrowthAmount = 3;
		public float ScaleAllStaminaRates = 1f;

		public float RechargeRate = 0.45f;
		public float EnergizedRate = 0.25f;

		public float SingularExertionRate = 12f;
		public float ItemUseRate = 0.51f;
		public float MagicItemUseRate = 0.2f;
		public float GrappleRate = 0.45f;
		public float SprintRate = 0.5f;
		public float JumpBegin = 5f;
		public float JumpHoldRate = 0.75f;
		public float SwimBegin = 2f;
		public float DashRate = 28f;
		public float GravitationPotionDrainRate = 0.1f;

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

		public IDictionary<string, float> CustomItemUseRate = new Dictionary<string, float> {
			{ "Bug Net", 0.1f },
			{ "Golden Bug Net", 0.15f }
		};


		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new StaminaConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= StaminaConfigData.ConfigVersion ) {
				return false;
			}

			if( vers_since < new Version( 1, 3, 3 ) ) {
				this.GrappleRate = new_config.GrappleRate;
				this.JumpHoldRate = new_config.JumpHoldRate;
				this.DashRate = new_config.DashRate;
				this.ExhaustionRecover = new_config.ExhaustionRecover;
			}
			if( vers_since < new Version( 1, 3, 4 ) ) {
				this.StarStaminaHeal = new_config.StarStaminaHeal;
			}
			if( vers_since < new Version( 1, 4, 1 ) ) {
				this.FatigueAmount = new_config.FatigueAmount;
				this.BottledWaterFatigueHeal = new_config.BottledWaterFatigueHeal;
			}
			if( vers_since < new Version( 1, 4, 3 ) ) {
				this.FatigueExerciseThresholdPercentOfMaxStamina = new_config.FatigueExerciseThresholdPercentOfMaxStamina;
			}
			if( vers_since < new Version( 1, 4, 5 ) ) {
				this.JumpBegin = new_config.JumpBegin;
			}
			if( vers_since < new Version( 1, 4, 6 ) ) {
				this.MagicItemUseRate = new_config.MagicItemUseRate;
			}
			if( vers_since < new Version( 1, 4, 8 ) ) {
				this.SprintRate = new_config.SprintRate;
			}
			if( vers_since < new Version( 1, 4, 9 ) ) {
				this.ItemUseRate = new_config.ItemUseRate;
			}
			if( vers_since < new Version( 1, 4, 12 ) ) {
				if( this.ExerciseGrowthAmount == 2 ) {  // Only update if different from old default
					this.ExerciseGrowthAmount = new_config.ExerciseGrowthAmount;
				}
			}

			this.VersionSinceUpdate = StaminaConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
