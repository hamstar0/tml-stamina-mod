using Stamina.Items.Accessories;
using System;
using Terraria;
using Terraria.DataStructures;


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
		public void DrainStamina( StaminaMod mymod, Player player, StaminaDrainTypes type ) {
			switch( type ) {
			case StaminaDrainTypes.ItemUse:
				this.DrainStaminaViaItemUse( mymod, player );
				break;
			case StaminaDrainTypes.MagicItemUse:
				this.DrainStaminaViaMagicItemUse( mymod, player );
				break;
			//case StaminaDrainTypes.CustomItemUse:
			//	this.DrainStaminaViaCustomItemUse( mymod, player );
			//	break;
			case StaminaDrainTypes.GrappleBegin:
				this.DrainStaminaViaGrappleBegin( mymod, player );
				break;
			case StaminaDrainTypes.GrappleHold:
				this.DrainStaminaViaGrappleHold( mymod, player );
				break;
			case StaminaDrainTypes.Sprint:
				this.DrainStaminaViaSprint( mymod, player );
				break;
			case StaminaDrainTypes.Dash:
				this.DrainStaminaViaDash( mymod, player );
				break;
			case StaminaDrainTypes.SwimBegin:
				this.DrainStaminaViaSwimBegin( mymod, player );
				break;
			case StaminaDrainTypes.SwimHold:
				this.DrainStaminaViaSwimHold( mymod, player );
				break;
			case StaminaDrainTypes.JumpBegin:
				this.DrainStaminaViaJumpBegin( mymod, player );
				break;
			case StaminaDrainTypes.JumpHold:
				this.DrainStaminaViaJumpHold( mymod, player );
				break;
			case StaminaDrainTypes.GravitationPotion:
				this.DrainStaminaViaGravitationPotion( mymod, player );
				break;
			default:
				throw new Exception("No such drain type.");
			}
		}

		////////////////

		public void DrainStaminaViaItemUse( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.ItemUseRate;

			if( modplayer.IsWearingMuscleBelt || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.MuscleBeltStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.ItemUse );
		}

		public void DrainStaminaViaMagicItemUse( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.MagicItemUseRate;

			if( modplayer.IsWearingMuscleBelt || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.MuscleBeltStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.MagicItemUse );
		}

		public void DrainStaminaViaCustomItemUse( StaminaMod mymod, Player player, string item_name ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.CustomItemUseRate[ item_name ];
			
			if( modplayer.IsWearingMuscleBelt || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.MuscleBeltStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.CustomItemUse );
		}

		public void DrainStaminaViaGrappleBegin( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.SingularExertionRate;

			if( modplayer.IsWearingMuscleBelt || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.MuscleBeltStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.GrappleBegin );
		}

		public void DrainStaminaViaGrappleHold( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.GrappleRate;

			if( modplayer.IsWearingJointBracer || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.JointBracerStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.GrappleHold );
		}

		public void DrainStaminaViaSprint( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.SprintRate;

			if( modplayer.IsWearingJointBracer || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.JointBracerStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.Sprint );
		}

		public void DrainStaminaViaDash( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.DashRate;

			if( modplayer.IsWearingJointBracer || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.JointBracerStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.Dash );
		}

		public void DrainStaminaViaSwimBegin( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.SwimBegin;

			if( modplayer.IsWearingLegSprings || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.LegSpringsStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.SwimBegin );
		}

		public void DrainStaminaViaSwimHold( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.SwimHoldRate;

			if( modplayer.IsWearingLegSprings || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.LegSpringsStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.SwimHold );
		}

		public void DrainStaminaViaJumpBegin( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.JumpBegin;

			if( modplayer.IsWearingLegSprings || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.LegSpringsStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.JumpBegin );
		}

		public void DrainStaminaViaJumpHold( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float amt = mymod.Config.Data.JumpHoldRate;

			if( modplayer.IsWearingLegSprings || modplayer.IsWearingExoskeleton ) {
				amt *= mymod.Config.Data.LegSpringsStaminaDrainScale;
			}

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.JumpHold );
		}

		public void DrainStaminaViaGravitationPotion( StaminaMod mymod, Player player ) {
			float amt = mymod.Config.Data.GravitationPotionDrainRate;

			this.DrainStaminaAmount( player, amt, StaminaDrainTypes.GravitationPotion );
		}

		public void DrainStaminaCustomAmount( StaminaMod mymod, Player player, float amount, string custom_type ) {
			this.DrainStaminaAmount( player, amount, StaminaDrainTypes.Custom );
			
			if( this.CurrentDrainTypes.ContainsKey( custom_type ) ) {
				this.CurrentDrainTypes[custom_type] += amount;
			} else {
				this.CurrentDrainTypes[custom_type] = amount;
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

			string base_type = type.ToString();
			if( this.CurrentDrainTypes.ContainsKey( base_type ) ) {
				this.CurrentDrainTypes[base_type] += amount;
			} else {
				this.CurrentDrainTypes[base_type] = amount;
			}
		}
		


		////////////////

		public void CommitStaminaDrains( StaminaMod mymod, Player player ) {
			if( this.CurrentDrainCount == 0 ) {
				this.DrainingFX = false;
				return;
			}

			var modplayer = player.GetModPlayer<StaminaPlayer>();
			float drain = this.CurrentDrainMost + (this.CurrentDrain / ((float)this.CurrentDrainCount * 2f));

			if( this.Stamina == 0 ) {
				this.TiredTimer = (int)drain > this.TiredTimer ? 0d : this.TiredTimer - drain;
			} else {
				float drain_surplus = (drain * mymod.Config.Data.ScaleAllStaminaRates) - this.Stamina;
				this.Stamina -= drain * mymod.Config.Data.ScaleAllStaminaRates;

				if( this.Stamina <= 0 ) {
					if( modplayer.IsWearingRageBandana ) {
						RageHeadbandItem.ApplyDamage( mymod, player, drain_surplus );
						this.Stamina = 1;
					} else {
						this.AddFatigue( player, mymod.Config.Data.FatigueAmount );
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
