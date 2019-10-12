using HamstarHelpers.Classes.Errors;
using Stamina.Items.Accessories;
using System;
using Terraria;
using Terraria.ModLoader.Config;


namespace Stamina.Logic {
	public enum StaminaDrainTypes {
		ItemUse,
		MagicItemUse,
		GrappleBegin,
		GrappleHold,
		Sprint,
		Dash,
		SwimBegin,
		SwimHold,
		JumpBegin,
		JumpHold,
		GravitationPotion,
		Custom,
		CustomItemUse,
		Recover
	}



	partial class StaminaLogic {
		public void DrainStamina( Player player, StaminaDrainTypes type ) {
			var mymod = StaminaMod.Instance;

			switch( type ) {
			case StaminaDrainTypes.ItemUse:
				this.DrainStaminaViaItemUse( player );
				break;
			case StaminaDrainTypes.MagicItemUse:
				this.DrainStaminaViaMagicItemUse( player );
				break;
			//case StaminaDrainTypes.CustomItemUse:
			//	this.DrainStaminaViaCustomItemUse( player );
			//	break;
			case StaminaDrainTypes.GrappleBegin:
				this.DrainStaminaViaGrappleBegin( player );
				break;
			case StaminaDrainTypes.GrappleHold:
				this.DrainStaminaViaGrappleHold( player );
				break;
			case StaminaDrainTypes.Sprint:
				this.DrainStaminaViaSprint( player );
				break;
			case StaminaDrainTypes.Dash:
				this.DrainStaminaViaDash( player );
				break;
			case StaminaDrainTypes.SwimBegin:
				this.DrainStaminaViaSwimBegin( player );
				break;
			case StaminaDrainTypes.SwimHold:
				this.DrainStaminaViaSwimHold( player );
				break;
			case StaminaDrainTypes.JumpBegin:
				this.DrainStaminaViaJumpBegin( player );
				break;
			case StaminaDrainTypes.JumpHold:
				this.DrainStaminaViaJumpHold( player );
				break;
			case StaminaDrainTypes.GravitationPotion:
				this.DrainStaminaViaGravitationPotion( player );
				break;
			default:
				throw new ModHelpersException("No such drain type.");
			}
		}

		////////////////

		public void DrainStaminaViaItemUse( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.ItemUseRate;

			if( myplayer.IsWearingMuscleBelt || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.MuscleBeltStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.ItemUse );
		}

		public void DrainStaminaViaMagicItemUse( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.MagicItemUseRate;

			if( myplayer.IsWearingMuscleBelt || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.MuscleBeltStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.MagicItemUse );
		}

		public void DrainStaminaViaCustomItemUse( Player player, ItemDefinition itemDef ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.CustomItemUseRates[ itemDef ].Percent;
			
			if( myplayer.IsWearingMuscleBelt || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.MuscleBeltStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.CustomItemUse );
		}

		public void DrainStaminaViaGrappleBegin( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.SingularExertionRate;

			if( myplayer.IsWearingMuscleBelt || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.MuscleBeltStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.GrappleBegin );
		}

		public void DrainStaminaViaGrappleHold( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.GrappleRate;

			if( myplayer.IsWearingJointBracer || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.JointBracerStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.GrappleHold );
		}

		public void DrainStaminaViaSprint( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.SprintRate;

			if( myplayer.IsWearingJointBracer || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.JointBracerStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.Sprint );
		}

		public void DrainStaminaViaDash( Player player ) {
			var mymod = StaminaMod.Instance;
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.DashRate;

			if( modplayer.IsWearingJointBracer || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.JointBracerStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.Dash );
		}

		public void DrainStaminaViaSwimBegin( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.SwimBegin;

			if( myplayer.IsWearingLegSprings || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.LegSpringsStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.SwimBegin );
		}

		public void DrainStaminaViaSwimHold( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.SwimHoldRate;

			if( myplayer.IsWearingLegSprings || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.LegSpringsStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.SwimHold );
		}

		public void DrainStaminaViaJumpBegin( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.JumpBegin;

			if( myplayer.IsWearingLegSprings || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.LegSpringsStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.JumpBegin );
		}

		public void DrainStaminaViaJumpHold( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.JumpHoldRate;

			if( myplayer.IsWearingLegSprings || myplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.LegSpringsStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.JumpHold );
		}

		public void DrainStaminaViaGravitationPotion( Player player ) {
			var mymod = StaminaMod.Instance;
			float amt = mymod.Config.GravitationPotionDrainRate;

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.GravitationPotion );
		}

		public void DrainStaminaCustomAmount( Player player, float amount, string customType ) {
			this.DrainStaminaAmount( player, amount, StaminaDrainTypes.Custom );
			
			if( this.CurrentDrainTypes.ContainsKey( customType ) ) {
				this.CurrentDrainTypes[customType] += amount;
			} else {
				this.CurrentDrainTypes[customType] = amount;
			}
		}

		////////////////

		public void DrainStaminaAmount( Player player, float amount, StaminaDrainTypes type ) {
			this.CurrentDrainCount++;

			foreach( var hook in this.StaminaChangeHooks ) {
				amount = hook( player, type, amount );
			}

			if( amount > this.CurrentDrainMost ) {
				this.CurrentDrain += this.CurrentDrainMost;
				this.CurrentDrainMost = amount;
			} else {
				this.CurrentDrain += amount;
			}

			string baseType = type.ToString();
			if( this.CurrentDrainTypes.ContainsKey( baseType ) ) {
				this.CurrentDrainTypes[baseType] += amount;
			} else {
				this.CurrentDrainTypes[baseType] = amount;
			}
		}
		


		////////////////

		public void CommitStaminaDrains( Player player ) {
			if( this.CurrentDrainCount == 0 ) {
				this.DrainingFX = false;
				return;
			}

			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();
			float drain = this.CurrentDrainMost + (this.CurrentDrain / ((float)this.CurrentDrainCount * 2f));

			if( this.Stamina == 0 ) {
				this.TiredTimer = (int)drain > this.TiredTimer ? 0d : this.TiredTimer - drain;
			} else {
				float drainSurplus = (drain * mymod.Config.ScaleAllStaminaRates) - this.Stamina;
				this.Stamina -= drain * mymod.Config.ScaleAllStaminaRates;

				if( this.Stamina <= 0 ) {
					if( myplayer.IsWearingRageBandana ) {
						RageHeadbandItem.ApplyDamage( mymod, player, drainSurplus );
						this.Stamina = 1;
					} else {
						this.AddFatigue( player, mymod.Config.FatigueAmountFromExhaustion );
					}
				}
			}

			this.DrainingFX = true;

			this.CurrentDrain = 0;
			this.CurrentDrainMost = 0;
			this.CurrentDrainCount = 0;
		}
	}
}
