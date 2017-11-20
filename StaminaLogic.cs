using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace Stamina {
	class StaminaLogic {
		private float _stamina;
		public float Stamina {
			get {
				return this._stamina;
			}
			private set {
				float max = (float)this.MaxStamina - this.Fatigue;
				value = value < 0 ? 0 : value > max ? max : value;
				this._stamina = value;
			}
		}

		public float CurrentDrain { get; private set; }
		public float CurrentDrainMost { get; private set; }
		public int CurrentDrainCount { get; private set; }

		public int MaxStamina { get; private set; }

		public float Fatigue { get; private set; }
		public int FatigueRecoverTimer { get; private set; }

		public bool DrainingFX { get; private set; }
		public IDictionary<string, float> CurrentDrainTypes = new Dictionary<string, float>();

		public bool HasStaminaSet { get; private set; }
		public double TiredTimer { get; private set; }

		public int ItemUseDuration { get; private set; }
		public bool IsExercising { get; private set; }


		////////////////

		public StaminaLogic( StaminaMod mymod, int max_stamina, bool has_stamina ) {
			this.MaxStamina = max_stamina;
			this.HasStaminaSet = has_stamina;

			if( !this.HasStaminaSet ) {
				this.MaxStamina = mymod.Config.Data.InitialStamina;
				this.HasStaminaSet = true;
			}

			this.Stamina = 1;
			this.TiredTimer = 0d;
		}

		////////////////

		public int GetExerciseThreshold( StaminaMod mymod ) {
			int threshold = (int)((float)this.MaxStamina * mymod.Config.Data.FatigueExerciseThresholdPercentOfMaxStamina);
			return threshold - mymod.Config.Data.FatigueExerciseThresholdAmountRemoved;
		}
		
		////////////////

		public void PassiveFatigueRecover( StaminaMod mymod, Player player ) {
			if( this.Fatigue > 0 ) {
				if( (this.MaxStamina - this.Stamina) <= this.Fatigue ) {
					this.FatigueRecoverTimer++;
					int duration = mymod.Config.Data.FatigueRecoverDuration;

					if( this.FatigueRecoverTimer >= duration ) {
						this.FatigueRecoverTimer = 0;
						this.Fatigue--;
					}
				}
				
				if( this.Fatigue >= this.GetExerciseThreshold( mymod ) ) {
					this.IsExercising = true;
				}
			} else {
				if( this.IsExercising ) {
					this.IsExercising = false;
					if( this.MaxStamina < mymod.Config.Data.MaxStaminaAmount ) {
						this.MaxStamina += mymod.Config.Data.ExerciseGrowthAmount;

						string msg = "+" + mymod.Config.Data.ExerciseGrowthAmount + " Stamina";
						PlayerMessage.AddPlayerLabel( player, msg, Color.Chartreuse, 60*3, true );

						Main.PlaySound( SoundID.Item47.WithVolume(0.5f) );
					}
				}
			}
		}

		public void PassiveStaminaRegen( StaminaMod mymod, Player player ) {
			//if( this.Player.suffocating || this.Player.breath <= 0 ) { return; }

			if( this.Stamina > 0 ) {
				float rate = mymod.Config.Data.RechargeRate * mymod.Config.Data.ScaleAllStaminaRates;

				// Gravitation Potion
				if( player.FindBuffIndex(18) >= 0 ) {
					this.Stamina -= mymod.Config.Data.GravitationPotionDrainRate;
				}

				this.Stamina += rate;
				this.TiredTimer = 0d;
			} else {
				if( this.TiredTimer >= mymod.Config.Data.ExhaustionDuration ) {
					this.TiredTimer = 0d;
					this.Stamina = mymod.Config.Data.ExhaustionRecover;
				}
				this.TiredTimer += 1d;
			}
		}

		////////////////

		public void SetItemUseDuration( int amt ) {
			this.ItemUseDuration = amt;
		}

		////////////////

		public void GatherPassiveStaminaDrains( StaminaMod mymod, Player player ) {
			// Is grappling?
			if( player.grappling[0] >= 0 ) {
				this.DrainStamina( mymod.Config.Data.GrappleRate, "grapple" );
			}

			// Is item in use?
			if( this.ItemUseDuration > 0 ) {
				this.ItemUseDuration--;

				Item curr_item = player.inventory[ player.selectedItem ];
				if( curr_item != null && !curr_item.IsAir ) {
					bool is_pewpew = curr_item.type == ItemID.SpaceGun || curr_item.type == ItemID.LaserRifle;
					bool is_spaceman = player.armor[0].type == ItemID.MeteorHelmet &&
						player.armor[1].type == ItemID.MeteorSuit &&
						player.armor[2].type == ItemID.MeteorLeggings;

					if( mymod.Config.Data.CustomItemUseRate.ContainsKey( curr_item.Name ) ) {
						float custom_rate = mymod.Config.Data.CustomItemUseRate[ curr_item.Name ];
						this.DrainStamina( custom_rate, "custom item use" );
					} else {
						if( curr_item.magic && !(is_pewpew && is_spaceman) ) {
							this.DrainStamina( mymod.Config.Data.MagicItemUseRate, "magic item use" );
						} else {
							this.DrainStamina( mymod.Config.Data.ItemUseRate, "item use" );
						}
					}
				}
//Main.NewText("GatherPassiveStaminaDrains " + StaminaMod.Config.Data.ItemUseRate + ", " + this.ItemUseDuration);
			}
		}

		public void GatherActivityStaminaDrains( StaminaMod mymod, Player player ) {
			var modplayer = player.GetModPlayer<StaminaPlayer>( mymod );

			// Is sprinting?
			if( !player.mount.Active && player.velocity.Y == 0f && player.dashDelay >= 0 ) {
				float runMin = PlayerMovementHelpers.MinimumRunSpeed( player );
				float acc = player.accRunSpeed + 0.1f;
				float velX = player.velocity.X;

				if( (player.controlRight && velX > runMin && velX < acc) ||
					(player.controlLeft && velX < -runMin && velX > -acc) ) {
//Main.NewText("runMin:"+ runMin+ ",acc:"+ acc+ ",velX:"+ velX+",maxRunSpeed:"+ this.Player.maxRunSpeed);
					this.DrainStamina( mymod.Config.Data.SprintRate, "sprint" );
				}
			}

			// Is dashing?
			if( !modplayer.IsDashing ) {
				if( player.dash != 0 && player.dashDelay == -1 ) {
					this.DrainStamina( mymod.Config.Data.DashRate, "dash" );
					modplayer.IsDashing = true;
				}
			} else if( player.dashDelay != -1 ) {
				modplayer.IsDashing = false;
			}

			// Is (attempting) jump?
			if( player.controlJump ) {
				if( !modplayer.IsJumping && !PlayerMovementHelpers.IsFlying( player ) ) {
					if( player.swimTime > 0 ) {
						this.DrainStamina( mymod.Config.Data.SwimBegin, "swim" );
					} else {
						this.DrainStamina( mymod.Config.Data.JumpBegin, "jump" );
					}
					modplayer.IsJumping = true;
				}

				if( player.jump > 0 || PlayerMovementHelpers.IsFlying( player ) ) {
					this.DrainStamina( mymod.Config.Data.JumpHoldRate, "jump hold" );
				}
			} else if( modplayer.IsJumping ) {
				modplayer.IsJumping = false;
			}
		}

		////////////////

		public void AddStamina( StaminaMod mymod, float amt ) {
			float add = amt * mymod.Config.Data.ScaleAllStaminaRates;
			if( this.Stamina == 0 ) {
				this.TiredTimer += add / 2;
			} else {
				this.Stamina += add;
			}
		}

		public void DrainStamina( float amt, string type ) {
			this.CurrentDrainCount++;

			if( amt > this.CurrentDrainMost ) {
				this.CurrentDrain += this.CurrentDrainMost;
				this.CurrentDrainMost = amt;
			} else {
				this.CurrentDrain += amt;
			}

			this.CurrentDrainTypes[type] = amt;
		}

		////////////////

		public void CommitStaminaDrains( StaminaMod mymod ) {
			if( this.CurrentDrainCount == 0 ) {
				this.DrainingFX = false;
				return;
			}

			float drain = this.CurrentDrainMost + (this.CurrentDrain / ((float)this.CurrentDrainCount * 2f));

			if( this.Stamina == 0 ) {
				this.TiredTimer = (int)drain > this.TiredTimer ? 0d : this.TiredTimer - drain;
			} else {
				this.Stamina -= drain * mymod.Config.Data.ScaleAllStaminaRates;

				// If newly exhausted, also add a bit to fatigue
				if( this.Stamina <= 0 ) {
					this.AddFatigue( mymod.Config.Data.FatigueAmount );
				}
			}

			this.DrainingFX = true;

			this.CurrentDrain = 0;
			this.CurrentDrainMost = 0;
			this.CurrentDrainCount = 0;
		}

		////////////////

		public void AddFatigue( float amt ) {
			this.Fatigue += amt;
			this.Fatigue = MathHelper.Clamp( this.Fatigue, 0, this.MaxStamina );
		}
	}
}
