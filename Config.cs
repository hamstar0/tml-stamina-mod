using System;
using System.Collections.Generic;
using Terraria.ModLoader.Config;


namespace Stamina {
	public class StaminaConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;
		

		////
		d
		public bool Enabled = true;

		public bool DebugModeInfo = false;
		public bool DebugModeInfoDrainers = false;

		public int InitialStamina = 100;
		public int MaxStaminaAmount = 400;
		public int ExerciseGrowthAmount = 3;
		public float ScaleAllStaminaRates = 1f;

		public float RechargeRate = 0.45f;
		public float EnergizedRate = 0.125f;

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
		public bool CraftableChampionBelts = true;
		public bool CraftableJointBracers = true;
		public bool CraftableLegSprings = true;
		public bool CraftableExoskeletons = true;
		
		public int StarStaminaHeal = 50;
		public int BottledWaterFatigueHeal = 35;
		public float RageHeadbandDamageMultiplier = 5f;
		public int ExerciseSupplementAddedGrowthAmount = 2;
		public float MuscleBeltStaminaDrainScale = 0.8f;
		public float JointBracerStaminaDrainScale = 0.8f;
		public float LegSpringsStaminaDrainScale = 0.8f;

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

		public IDictionary<string, float> CustomItemUseRate = new Dictionary<string, float>();
		
		public bool ShowMainStaminaBar = true;
		public bool ShowMiniStaminaBar = true;



		////////////////

		public void SetDefaults() {
			this.CustomItemUseRate = new Dictionary<string, float>{
				{ "Bug Net", 0.1f },
				{ "Golden Bug Net", 0.15f }
			};
		}
	}
}
