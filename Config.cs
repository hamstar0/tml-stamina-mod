using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria.ModLoader.Config;


namespace Stamina {
	public class StaminaConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;
		

		////
		
		[DefaultValue( true )]
		public bool Enabled = true;


		public bool DebugModeInfo = false;

		public bool DebugModeInfoDrainers = false;


		[DefaultValue( 100 )]
		public int InitialStamina = 100;

		[DefaultValue( 400 )]
		public int MaxStaminaAmount = 400;

		[DefaultValue( 3 )]
		public int ExerciseGrowthAmount = 3;

		[DefaultValue( 1f )]
		public float ScaleAllStaminaRates = 1f;


		[DefaultValue( 0.45f )]
		public float RechargeRate = 0.45f;

		[DefaultValue( 0.125f )]
		public float EnergizedRate = 0.125f;


		[DefaultValue( 12f )]
		public float SingularExertionRate = 12f;

		[DefaultValue( 0.501f )]
		public float ItemUseRate = 0.501f;

		[DefaultValue( 0.2f )]
		public float MagicItemUseRate = 0.2f;

		[DefaultValue( 0.45f )]
		public float GrappleRate = 0.45f;

		[DefaultValue( 0.5f )]
		public float SprintRate = 0.5f;

		[DefaultValue( 6.5f )]
		public float JumpBegin = 6.5f;

		[DefaultValue( 0.65f )]
		public float JumpHoldRate = 0.65f;

		[DefaultValue( 2f )]
		public float SwimBegin = 2f;

		[DefaultValue( 0.5f )]
		public float SwimHoldRate = 0.5f;

		[DefaultValue( 28f )]
		public float DashRate = 28f;

		[DefaultValue( 0.1f )]
		public float GravitationPotionDrainRate = 0.1f;


		[DefaultValue( 160 )]
		public int ExhaustionDuration = 160;

		[DefaultValue( 18f )]
		public float ExhaustionRecover = 18f;

		[DefaultValue( true )]
		public bool ExhaustionLowersDefense = true;

		[DefaultValue( true )]
		public bool ExhaustionBlocksItems = true;

		[DefaultValue( true )]
		public bool ExhaustionSlowsMovement = true;


		[DefaultValue( true )]
		public bool ConsumableStars = true;

		[DefaultValue( true )]
		public bool ConsumableBottledWater = true;

		[DefaultValue( true )]
		public bool CraftableEnergyDrinks = true;

		[DefaultValue( true )]
		public bool CraftableAthletePotions = true;

		[DefaultValue( true )]
		public bool CraftableRageHeadbands = true;

		[DefaultValue( true )]
		public bool CraftableExerciseSupplements = true;

		[DefaultValue( true )]
		public bool CraftableChampionBelts = true;

		[DefaultValue( true )]
		public bool CraftableJointBracers = true;

		[DefaultValue( true )]
		public bool CraftableLegSprings = true;

		[DefaultValue( true )]
		public bool CraftableExoskeletons = true;


		[DefaultValue( 50 )]
		public int StarStaminaHeal = 50;

		[DefaultValue( 35 )]
		public int BottledWaterFatigueHeal = 35;

		[DefaultValue( 5f )]
		public float RageHeadbandDamageMultiplier = 5f;

		[DefaultValue( 2 )]
		public int ExerciseSupplementAddedGrowthAmount = 2;

		[DefaultValue( 0.8f )]
		public float MuscleBeltStaminaDrainScale = 0.8f;

		[DefaultValue( 0.8f )]
		public float JointBracerStaminaDrainScale = 0.8f;

		[DefaultValue( 0.8f )]
		public float LegSpringsStaminaDrainScale = 0.8f;


		[DefaultValue( 30 * 60 )]
		public int EnergyPotionDuration = 30 * 60;

		[DefaultValue( 120 * 60 )]
		public int AthletePotionDuration = 120 * 60;


		[DefaultValue( 0.08f )]
		public float PercentOfDamageAdrenalineBurst = 0.08f;


		[DefaultValue( 12f )]
		public float FatigueAmountFromExhaustion = 12f;

		[DefaultValue( 60 )]
		public int FatigueRecoverDuration = 60;

		[DefaultValue( 0 )]
		public int FatigueForExerciseAmountRemoved = 0;

		[DefaultValue( 0.24f )]
		public float FatigueAsMaxStaminaPercentAmountNeeededForExercise = 0.24f;


		[DefaultValue( -1 )]
		public int CustomStaminaBarPositionX = -1;

		[DefaultValue( -1 )]
		public int CustomStaminaBarPositionY = -1;


		[DefaultValue( 0 )]
		public int PlayerStaminaBarOffsetX = 0;

		[DefaultValue( 0 )]
		public int PlayerStaminaBarOffsetY = 0;


		public IDictionary<string, float> CustomItemUseRate = new Dictionary<string, float>();


		[DefaultValue( true )]
		public bool ShowMainStaminaBar = true;
		[DefaultValue( true )]
		public bool ShowMiniStaminaBar = true;



		////////////////

		[OnDeserialized]
		internal void OnDeserializedMethod( StreamingContext context ) {
			if( this.CustomItemUseRate != null ) {
				return;
			}

			this.CustomItemUseRate = new Dictionary<string, float>{
				{ "Bug Net", 0.1f },
				{ "Golden Bug Net", 0.15f }
			};
		}
	}
}
