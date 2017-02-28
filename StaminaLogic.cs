using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Utils;

namespace Stamina {
	class StaminaLogic {
		private StaminaPlayer ModPlayer;
		private Player Player;

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

		public StaminaLogic( StaminaPlayer modplayer, int max_stamina, bool has_stamina ) {
			this.ModPlayer = modplayer;
			this.Player = modplayer.player;
			this.MaxStamina = max_stamina;
			this.HasStaminaSet = has_stamina;

			if( !this.HasStaminaSet ) {
				this.MaxStamina = StaminaMod.Config.Data.InitialStamina;
				this.HasStaminaSet = true;
			}

			this.Stamina = 1;
			this.TiredTimer = 0d;
		}

		////////////////

		public int GetExerciseThreshold() {
			int threshold = (int)((float)this.MaxStamina * StaminaMod.Config.Data.FatigueExerciseThresholdPercentOfMaxStamina);
			threshold -= StaminaMod.Config.Data.FatigueExerciseThresholdAmountRemoved;
			return threshold;
		}
		
		////////////////

		public void PassiveFatigueRecover() {
			if( this.Fatigue > 0 ) {
				if( (this.MaxStamina - this.Stamina) <= this.Fatigue ) {
					this.FatigueRecoverTimer++;
					int duration = StaminaMod.Config.Data.FatigueRecoverDuration;

					if( this.FatigueRecoverTimer >= duration ) {
						this.FatigueRecoverTimer = 0;
						this.Fatigue--;
					}
				}
				
				if( this.Fatigue >= this.GetExerciseThreshold() ) {
					this.IsExercising = true;
				}
			} else {
				if( this.IsExercising ) {
					this.IsExercising = false;
					if( this.MaxStamina < StaminaMod.Config.Data.MaxStaminaAmount ) {
						this.MaxStamina += StaminaMod.Config.Data.ExerciseGrowthAmount;

						string msg = "+" + StaminaMod.Config.Data.ExerciseGrowthAmount + " Stamina";
						UIHelper.AddPlayerLabel( this.Player, msg, Color.Chartreuse, 60*3, true );

						Main.PlaySound( SoundID.Item47.WithVolume(0.5f) );
					}
				}
			}
		}

		public void PassiveStaminaRegen() {
			//if( this.Player.suffocating || this.Player.breath <= 0 ) { return; }

			if( this.Stamina > 0 ) {
				float rate = StaminaMod.Config.Data.RechargeRate * StaminaMod.Config.Data.ScaleAllStaminaRates;

				if( this.Player.FindBuffIndex(18) == -1 ) {	// Gravitation Potion
					this.Stamina += rate;
				} else {
					this.Stamina += rate / 2;
				}
				this.TiredTimer = 0d;
			} else {
				if( this.TiredTimer >= StaminaMod.Config.Data.ExhaustionDuration ) {
					this.TiredTimer = 0d;
					this.Stamina = StaminaMod.Config.Data.ExhaustionRecover;
				}
				this.TiredTimer += 1d;
			}
		}

		////////////////

		public void SetItemUseDuration( int amt ) {
			this.ItemUseDuration = amt;
		}

		////////////////

		public void GatherPassiveStaminaDrains() {
			// Is grappling?
			if( this.Player.grappling[0] >= 0 ) {
				this.DrainStamina( StaminaMod.Config.Data.GrappleRate, "grapple" );
			}

			if( this.ItemUseDuration > 0 ) {
				this.ItemUseDuration--;

				Item curr_item = this.Player.inventory[this.Player.selectedItem];
				if( curr_item != null && !curr_item.IsAir ) {
					if( curr_item.magic ) {
						this.DrainStamina( StaminaMod.Config.Data.MagicItemUseRate, "magic item use" );
					} else {
						this.DrainStamina( StaminaMod.Config.Data.ItemUseRate, "item use" );
					}
				}
				//Main.NewText("GatherPassiveStaminaDrains " + StaminaMod.Config.Data.ItemUseRate + ", " + this.ItemUseDuration);
			}
		}

		public void GatherActivityStaminaDrains() {
			// Is sprinting?
			if( !this.Player.mount.Active && this.Player.velocity.Y == 0f && this.Player.dashDelay >= 0 ) {
				float runMin = PlayerHelper.MinimumRunSpeed(this.Player);
				float acc = this.Player.accRunSpeed + 0.1f;
				float velX = this.Player.velocity.X;

				if( (this.Player.controlRight && velX > runMin && velX < acc) ||
					(this.Player.controlLeft && velX < -runMin && velX > -acc) ) {
//Main.NewText("runMin:"+ runMin+ ",acc:"+ acc+ ",velX:"+ velX+",maxRunSpeed:"+ this.Player.maxRunSpeed);
					this.DrainStamina( StaminaMod.Config.Data.SprintRate, "sprint" );
				}
			}

			// Is dashing?
			if( !this.ModPlayer.IsDashing ) {
				if( this.Player.dash != 0 && this.Player.dashDelay == -1 ) {
					this.DrainStamina( StaminaMod.Config.Data.DashRate, "dash" );
					this.ModPlayer.IsDashing = true;
				}
			} else if( this.Player.dashDelay != -1 ) {
				this.ModPlayer.IsDashing = false;
			}

			// Is (attempting) jump?
			if( this.Player.controlJump ) {
				if( !this.ModPlayer.IsJumping && !PlayerHelper.IsFlying(this.Player) ) {
					this.DrainStamina( StaminaMod.Config.Data.JumpBegin, "jump" );
					this.ModPlayer.IsJumping = true;
				}

				if( this.Player.jump > 0 || PlayerHelper.IsFlying( this.Player ) ) {
					this.DrainStamina( StaminaMod.Config.Data.JumpHoldRate, "jump hold" );
				}
			} else if( this.ModPlayer.IsJumping ) {
				this.ModPlayer.IsJumping = false;
			}
		}

		////////////////

		public void AddStamina( float amt ) {
			float add = amt * StaminaMod.Config.Data.ScaleAllStaminaRates;
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

		public void CommitStaminaDrains() {
			if( this.CurrentDrainCount == 0 ) {
				this.DrainingFX = false;
				return;
			}

			float drain = this.CurrentDrainMost + (this.CurrentDrain / ((float)this.CurrentDrainCount * 2f));

			if( this.Stamina == 0 ) {
				this.TiredTimer = (int)drain > this.TiredTimer ? 0d : this.TiredTimer - drain;
			} else {
				this.Stamina -= drain * StaminaMod.Config.Data.ScaleAllStaminaRates;

				// If newly exhausted, also add a bit to fatigue
				if( this.Stamina <= 0 ) {
					this.AddFatigue( StaminaMod.Config.Data.FatigueAmount );
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
