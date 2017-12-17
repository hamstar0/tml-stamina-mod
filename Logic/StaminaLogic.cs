using HamstarHelpers.PlayerHelpers;
using Stamina.Buffs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace Stamina.Logic {
	partial class StaminaLogic {
		private float _stamina;
		public float Stamina {
			get {
				return this._stamina;
			}
			private set {
				float max = (float)this.MaxStamina2 - this.Fatigue;
				value = value < 0 ? 0 : value > max ? max : value;
				this._stamina = value;
			}
		}

		public float CurrentDrain { get; private set; }
		public float CurrentDrainMost { get; private set; }
		public int CurrentDrainCount { get; private set; }

		public int MaxStamina { get; private set; }
		public int MaxStamina2 { get; private set; }

		public float Fatigue { get; private set; }
		public int FatigueRecoverTimer { get; private set; }

		public bool DrainingFX { get; private set; }
		public IDictionary<string, float> CurrentDrainTypes = new Dictionary<string, float>();

		public bool HasStaminaSet { get; private set; }
		public double TiredTimer { get; private set; }

		public int ItemUseDrainDuration { get; private set; }
		public bool IsExercising { get; private set; }

		internal IList<StaminaChangeHook> StaminaChangeHooks = new List<StaminaChangeHook>();
		internal IList<FatigueChangeHook> FatigueChangeHooks = new List<FatigueChangeHook>();
		

		////////////////

		public StaminaLogic( StaminaMod mymod, int max_stamina, bool has_stamina ) {
			this.MaxStamina = max_stamina;
			this.MaxStamina2 = max_stamina;
			this.HasStaminaSet = has_stamina;

			if( !this.HasStaminaSet ) {
				this.MaxStamina = mymod.Config.Data.InitialStamina;
				this.MaxStamina2 = mymod.Config.Data.InitialStamina;
				this.HasStaminaSet = true;
			}

			this.Stamina = 1;
			this.TiredTimer = 0d;
		}

		////////////////

		public void UpdateMaxStamina( StaminaMod mymod, Player player ) {
			this.MaxStamina2 = this.MaxStamina;

			if( player.FindBuffIndex( mymod.BuffType<AthleteBuff>() ) != -1 ) {
				this.MaxStamina2 += AthleteBuff.MaxStaminaAdd;
			}
		}


		////////////////

		public void SetItemUseHoldDurationForStaminaDrain( int amt ) {
			this.ItemUseDrainDuration = amt;
		}

		////////////////

		public void PassiveStaminaRegen( StaminaMod mymod, Player player ) {
			//if( this.Player.suffocating || this.Player.breath <= 0 ) { return; }

			if( this.Stamina > 0 ) {
				this.TiredTimer = 0d;
				this.AddStamina( mymod, player, mymod.Config.Data.RechargeRate );
			} else {
				if( this.TiredTimer >= mymod.Config.Data.ExhaustionDuration ) {
					this.TiredTimer = 0d;
					this.Stamina = 0.0001f;

					this.AddStamina( mymod, player, mymod.Config.Data.ExhaustionRecover );
				} else {
					this.TiredTimer += 1d;
				}
			}
		}

		////////////////

		public void GatherPassiveStaminaDrains( StaminaMod mymod, Player player ) {
			// Is grappling?
			if( player.grappling[0] >= 0 ) {
				this.DrainStaminaViaGrappleHold( mymod, player );
			}

			// Is item in use?
			if( this.ItemUseDrainDuration > 0 ) {
				this.ItemUseDrainDuration--;

				Item curr_item = player.inventory[player.selectedItem];
				if( curr_item == null || curr_item.IsAir ) { return; }

				bool is_pewpew = curr_item.type == ItemID.SpaceGun || curr_item.type == ItemID.LaserRifle;
				bool is_spaceman = player.armor[0].type == ItemID.MeteorHelmet &&
					player.armor[1].type == ItemID.MeteorSuit &&
					player.armor[2].type == ItemID.MeteorLeggings;
				
				if( mymod.Config.Data.CustomItemUseRate.ContainsKey( curr_item.Name ) ) {
					float custom_rate = mymod.Config.Data.CustomItemUseRate[curr_item.Name];
					this.DrainStaminaViaCustomItemUse( mymod, player, curr_item.Name );
				} else {
					if( curr_item.magic && !(is_pewpew && is_spaceman) ) {
						this.DrainStaminaViaMagicItemUse( mymod, player );
					} else {
						this.DrainStaminaViaItemUse( mymod, player );
					}
				}
			}

			// Is using grav pot?
			if( player.FindBuffIndex( BuffID.Gravitation ) != -1 ) {
				this.DrainStaminaViaGravitationPotion( mymod, player );
			}
//Main.NewText("GatherPassiveStaminaDrains " + StaminaMod.Config.Data.ItemUseRate + ", " + this.ItemUseDuration);
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
					this.DrainStaminaViaSprint( mymod, player );
				}
			}

			// Is dashing?
			if( !modplayer.IsDashing ) {
				if( player.dash != 0 && player.dashDelay == -1 ) {
					this.DrainStaminaViaDash( mymod, player );
					modplayer.IsDashing = true;
				}
			} else if( player.dashDelay != -1 ) {
				modplayer.IsDashing = false;
			}

			// Is (attempting) jump?
			if( player.controlJump ) {
				if( !modplayer.IsJumping && !PlayerMovementHelpers.IsFlying( player ) ) {
					if( player.swimTime > 0 ) {
						this.DrainStaminaViaSwimBegin( mymod, player );
					} else {
						this.DrainStaminaViaJumpBegin( mymod, player );
					}
					modplayer.IsJumping = true;
				}

				if( player.jump > 0 || PlayerMovementHelpers.IsFlying( player ) ) {
					if( player.swimTime > 0 ) {
						this.DrainStaminaViaSwimHold( mymod, player );
					} else {
						this.DrainStaminaViaJumpHold( mymod, player );
					}
				}
			} else if( modplayer.IsJumping ) {
				modplayer.IsJumping = false;
			}
		}

		////////////////

		public void AddStamina( StaminaMod mymod, Player player, float amount ) {
			amount *= mymod.Config.Data.ScaleAllStaminaRates;

			if( this.Stamina == 0 ) {
				this.TiredTimer += amount / 2;
			} else {
				foreach( var hook in this.StaminaChangeHooks ) {
					amount = hook( player, StaminaDrainTypes.Recover, amount );
				}

				this.Stamina += amount;
			}
		}
	}
}
