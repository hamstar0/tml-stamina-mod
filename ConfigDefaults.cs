using HamstarHelpers.Utilities.Config;
using System;
using System.Collections.Generic;


namespace Stamina {
	public class StaminaConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 2, 0, 1 );
		public readonly static string ConfigFileName = "Stamina Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public bool DEBUG_INFO = false;
		public bool DEBUG_VIEW_DRAINERS = false;

		public int InitialStamina = 100;
		public int MaxStaminaAmount = 400;
		public int ExerciseGrowthAmount = 3;
		public float ScaleAllStaminaRates = 1f;

		public float RechargeRate = 0.45f;
		public float EnergizedRate = 0.1f;

		public float SingularExertionRate = 12f;
		public float ItemUseRate = 0.501f;
		public float MagicItemUseRate = 0.2f;
		public float GrappleRate = 0.45f;
		public float SprintRate = 0.5f;
		public float JumpBegin = 6.5f;
		public float JumpHoldRate = 0.65f;
		public float SwimBegin = 2f;
		public float SwimHoldRate = 0.5f;
		public float DashRate = 28f;
		public float GravitationPotionDrainRate = 0.1f;

		public int ExhaustionDuration = 160;
		public float ExhaustionRecover = 18f;
		public bool ExhaustionLowersDefense = true;
		public bool ExhaustionBlocksItems = true;
		public bool ExhaustionSlowsMovement = true;

		public bool ConsumableStars = true;
		public bool ConsumableBottledWater = true;
		public bool CraftableEnergyDrinks = true;
		public bool CraftableAthletePotions = true;
		public bool CraftableRageHeadbands = true;
		public bool CraftableExerciseSupplements = true;
		public bool CraftableMuscleBelts = true;
		public bool CraftableJointBracers = true;
		public bool CraftableLegSprings = true;
		public bool CraftableExoskeletons = true;
		
		public int StarStaminaHeal = 50;
		public int BottledWaterFatigueHeal = 35;
		public float RageHeadbandDamageMultiplier = 5f;
		public int ExerciseSupplementAddedGrowthAmount = 2;
		public float MuscleBeltStaminaDrainScale = 0.7f;
		public float JointBracerStaminaDrainScale = 0.7f;
		public float LegSpringsStaminaDrainScale = 0.7f;

		public int EnergyPotionDuration = 30 * 60;
		public int AthletePotionDuration = 120 * 60;

		public float PercentOfDamageAdrenalineBurst = 0.08f;

		public float FatigueAmountFromExhaustion = 12f;
		public int FatigueRecoverDuration = 60;
		public int FatigueForExerciseAmountRemoved = 0;
		public float FatigueAsMaxStaminaPercentAmountNeeededForExercise = 0.24f;

		public int CustomStaminaBarPositionX = -1;
		public int CustomStaminaBarPositionY = -1;

		public int PlayerStaminaBarOffsetX = 0;
		public int PlayerStaminaBarOffsetY = 0;

		public IDictionary<string, float> CustomItemUseRate = new Dictionary<string, float> {
			{ "Bug Net", 0.1f },
			{ "Golden Bug Net", 0.15f }
		};

		public bool ShowMiniStaminaBar = true;


		////////////////

		public string _OLD_SETTINGS_BELOW_ = "";

		public float FatigueAmount = 12f;
		public float FatigueExerciseThresholdPercentOfMaxStamina = 0.32f;


		////////////////

		public readonly static int _1_4_11_ExerciseGrowthAmount = 2;
		public readonly static int _1_5_0_ExhaustionDuration = 180;
		public readonly static float _1_5_0_EnergizedRate = 0.25f;
		public readonly static int _1_5_0_BottledWaterFatigueHeal = 50;



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
				this.FatigueAsMaxStaminaPercentAmountNeeededForExercise = new_config.FatigueAsMaxStaminaPercentAmountNeeededForExercise;
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
				if( this.ExerciseGrowthAmount == StaminaConfigData._1_4_11_ExerciseGrowthAmount ) {
					this.ExerciseGrowthAmount = new_config.ExerciseGrowthAmount;
				}
			}
			if( vers_since < new Version( 2, 0, 0 ) ) {
				if( this.ExhaustionDuration == StaminaConfigData._1_5_0_ExhaustionDuration ) {
					this.ExhaustionDuration = new_config.ExhaustionDuration;
				}
				if( this.EnergizedRate == StaminaConfigData._1_5_0_EnergizedRate ) {
					this.EnergizedRate = new_config.EnergizedRate;
				}
				if( this.BottledWaterFatigueHeal == StaminaConfigData._1_5_0_BottledWaterFatigueHeal ) {
					this.BottledWaterFatigueHeal = new_config.BottledWaterFatigueHeal;
				}
			}

			this.VersionSinceUpdate = StaminaConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
