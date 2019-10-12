using HamstarHelpers.Classes.UI.ModConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Stamina {
	class MyFloatInputElement : FloatInputElement { }




	public class FloatPercentSetting {
		[Range( 0f, 10f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float Percent = 1f;
	}




	public class StaminaConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue( true )]
		public bool Enabled = true;


		[DefaultValue( false )]
		public bool DebugModeInfo = false;

		[DefaultValue( false )]
		public bool DebugModeInfoDrainers = false;

		////

		[Range( 0, 1000 )]
		[DefaultValue( 100 )]
		public int InitialStamina = 100;

		[Range( 0, 1000 )]
		[DefaultValue( 400 )]
		public int MaxStaminaAmount = 400;

		[Range( 0, 1000 )]
		[DefaultValue( 3 )]
		public int ExerciseGrowthAmount = 3;

		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ScaleAllStaminaRates = 1f;


		[Range( 0f, 100f )]
		[DefaultValue( 0.45f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float RechargeRate = 0.45f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.125f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float EnergizedRate = 0.125f;


		[Range( 0f, 100f )]
		[DefaultValue( 12f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float SingularExertionRate = 12f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.501f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ItemUseRate = 0.501f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.2f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float MagicItemUseRate = 0.2f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.45f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float GrappleRate = 0.45f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float SprintRate = 0.5f;

		[Range( 0f, 100f )]
		[DefaultValue( 6.5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float JumpBegin = 6.5f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.65f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float JumpHoldRate = 0.65f;

		[Range( 0f, 100f )]
		[DefaultValue( 2f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float SwimBegin = 2f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float SwimHoldRate = 0.5f;

		[Range( 0f, 100f )]
		[DefaultValue( 28f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float DashRate = 28f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float GravitationPotionDrainRate = 0.1f;

		[Range( 0, 60 * 60 * 60 )]
		[DefaultValue( 160 )]
		public int ExhaustionDuration = 160;

		[Range( 0, 60 * 60 * 60 )]
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


		[Range( 0, 10000 )]
		[DefaultValue( 50 )]
		public int StarStaminaHeal = 50;

		[Range( 0, 10000 )]
		[DefaultValue( 35 )]
		public int BottledWaterFatigueHeal = 35;

		[Range( 0f, 100f )]
		[DefaultValue( 5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float RageHeadbandDamageMultiplier = 5f;

		[Range( 0, 10000 )]
		[DefaultValue( 2 )]
		public int ExerciseSupplementAddedGrowthAmount = 2;

		[Range( 0f, 100f )]
		[DefaultValue( 0.8f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float MuscleBeltStaminaDrainScale = 0.8f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.8f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float JointBracerStaminaDrainScale = 0.8f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.8f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float LegSpringsStaminaDrainScale = 0.8f;


		[Range( 0, 60 * 60 * 60 * 24 )]
		[DefaultValue( 30 * 60 )]
		public int EnergyPotionDuration = 30 * 60;

		[Range( 0, 60 * 60 * 60 * 24 )]
		[DefaultValue( 120 * 60 )]
		public int AthletePotionDuration = 120 * 60;


		[Range( 0f, 1f )]
		[DefaultValue( 0.08f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float PercentOfDamageAdrenalineBurst = 0.08f;


		[Range( 0f, 1000f )]
		[DefaultValue( 12f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float FatigueAmountFromExhaustion = 12f;

		[Range( 0, 60 * 60 * 60 * 24 )]
		[DefaultValue( 60 )]
		public int FatigueRecoverDuration = 60;

		[Range( 0, 1000 )]
		[DefaultValue( 0 )]
		public int FatigueForExerciseAmountRemoved = 0;

		[Range( 0f, 1f )]
		[DefaultValue( 0.24f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float FatigueAsMaxStaminaPercentAmountNeeededForExercise = 0.24f;


		[Range( -1, 2048 )]
		[DefaultValue( -1 )]
		public int CustomStaminaBarPositionX = -1;

		[Range( -1, 1024 )]
		[DefaultValue( -1 )]
		public int CustomStaminaBarPositionY = -1;


		[Range( -1, 2048 )]
		[DefaultValue( 0 )]
		public int PlayerStaminaBarOffsetX = 0;

		[Range( -1, 1024 )]
		[DefaultValue( 0 )]
		public int PlayerStaminaBarOffsetY = 0;

		
		public Dictionary<ItemDefinition, FloatPercentSetting> CustomItemUseRates = new Dictionary<ItemDefinition, FloatPercentSetting>{
			{ new ItemDefinition(ItemID.BugNet), new FloatPercentSetting { Percent = 0.1f } },
			{ new ItemDefinition(ItemID.GoldenBugNet), new FloatPercentSetting { Percent = 0.15f } }
		};


		[DefaultValue( true )]
		public bool ShowMainStaminaBar = true;
		[DefaultValue( true )]
		public bool ShowMiniStaminaBar = true;


		[Header( "\n \nOBSOLETE SETTINGS BELOW" )]
		public Dictionary<ItemDefinition, float> CustomItemUseRate = new Dictionary<ItemDefinition, float>{
			{ new ItemDefinition(ItemID.BugNet), 0.1f },
			{ new ItemDefinition(ItemID.GoldenBugNet), 0.15f }
		};



		////////////////

		public override ModConfig Clone() {
			var clone = (StaminaConfig)this.MemberwiseClone();

			clone.CustomItemUseRates = this.CustomItemUseRates.ToDictionary(
				kv => kv.Key,
				kv => new FloatPercentSetting { Percent = kv.Value.Percent }
			);

			return clone;
		}
	}
}
