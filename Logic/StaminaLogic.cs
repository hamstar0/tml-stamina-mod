using HamstarHelpers.Helpers.Players;
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

		////

		internal IList<StaminaChangeHook> StaminaChangeHooks = new List<StaminaChangeHook>();
		internal IList<FatigueChangeHook> FatigueChangeHooks = new List<FatigueChangeHook>();
		


		////////////////

		public StaminaLogic( int maxStamina, bool hasStamina ) {
			var mymod = StaminaMod.Instance;

			this.MaxStamina = maxStamina;
			this.MaxStamina2 = maxStamina;
			this.HasStaminaSet = hasStamina;
			this.Fatigue = 0;
			this.FatigueRecoverTimer = 0;
			this.CurrentDrain = 0;
			this.CurrentDrainMost = 0;
			this.CurrentDrainCount = 0;
			this.DrainingFX = false;
			this.ItemUseDrainDuration = 0;
			this.IsExercising = false;

			if( !hasStamina ) {
				this.MaxStamina = mymod.Config.InitialStamina;
				this.MaxStamina2 = mymod.Config.InitialStamina;
				this.HasStaminaSet = true;
			}

			this.Stamina = 1;
			this.TiredTimer = 0d;
		}

		////////////////

		public void UpdateMaxStamina2( Player player ) {
			var mymod = StaminaMod.Instance;

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

		public void PassiveStaminaRegen( Player player ) {
			//if( this.Player.suffocating || this.Player.breath <= 0 ) { return; }
			var mymod = StaminaMod.Instance;

			if( this.Stamina > 0 ) {
				this.TiredTimer = 0d;
				this.AddStamina( player, mymod.Config.RechargeRate );
			} else {
				if( this.TiredTimer >= mymod.Config.ExhaustionDuration ) {
					this.TiredTimer = 0d;
					this.Stamina = 0.0001f;

					this.AddStamina( player, mymod.Config.ExhaustionRecover );
				} else {
					this.TiredTimer += 1d;
				}
			}
		}

		////////////////

		public void GatherPassiveStaminaDrains( Player player ) {
			var mymod = StaminaMod.Instance;

			// Is grappling?
			if( player.grappling[0] >= 0 ) {
				this.DrainStaminaViaGrappleHold( player );
			}

			// Is item in use?
			if( this.ItemUseDrainDuration > 0 ) {
				this.ItemUseDrainDuration--;

				Item currItem = player.inventory[player.selectedItem];
				if( currItem == null || currItem.IsAir ) { return; }

				bool isPewpew = currItem.type == ItemID.SpaceGun || currItem.type == ItemID.LaserRifle;
				bool isSpaceman = player.armor[0].type == ItemID.MeteorHelmet &&
					player.armor[1].type == ItemID.MeteorSuit &&
					player.armor[2].type == ItemID.MeteorLeggings;
				
				if( mymod.Config.CustomItemUseRate.ContainsKey( currItem.Name ) ) {
					float customRate = mymod.Config.CustomItemUseRate[currItem.Name];
					this.DrainStaminaViaCustomItemUse( player, currItem.Name );
				} else {
					if( currItem.magic && !(isPewpew && isSpaceman) ) {
						this.DrainStaminaViaMagicItemUse( player );
					} else {
						this.DrainStaminaViaItemUse( player );
					}
				}
			}

			// Is using grav pot?
			if( player.FindBuffIndex( BuffID.Gravitation ) != -1 ) {
				this.DrainStaminaViaGravitationPotion( player );
			}
//Main.NewText("GatherPassiveStaminaDrains " + StaminaMod.Config.Data.ItemUseRate + ", " + this.ItemUseDuration);
		}

		public void GatherActivityStaminaDrains( Player player ) {
			var mymod = StaminaMod.Instance;
			var myplayer = player.GetModPlayer<StaminaPlayer>();

			// Is sprinting?
			if( !player.mount.Active && player.velocity.Y == 0f && player.dashDelay >= 0 ) {
				float runMin = PlayerMovementHelpers.MinimumRunSpeed( player );
				float acc = player.accRunSpeed + 0.1f;
				float velX = player.velocity.X;

				if( (player.controlRight && velX > runMin && velX < acc) ||
					(player.controlLeft && velX < -runMin && velX > -acc) ) {
//Main.NewText("runMin:"+ runMin+ ",acc:"+ acc+ ",velX:"+ velX+",maxRunSpeed:"+ this.Player.maxRunSpeed);
					this.DrainStaminaViaSprint( player );
				}
			}

			// Is dashing?
			if( !myplayer.IsDashing ) {
				if( player.dash != 0 && player.dashDelay == -1 ) {
					this.DrainStaminaViaDash( player );
					myplayer.IsDashing = true;
				}
			} else if( player.dashDelay != -1 ) {
				myplayer.IsDashing = false;
			}

			// Is (attempting) jump?
			if( player.controlJump ) {
				if( !myplayer.IsJumping && !PlayerMovementHelpers.IsFlying( player ) ) {
					if( player.swimTime > 0 ) {
						this.DrainStaminaViaSwimBegin( player );
					} else {
						if( player.velocity.Y == 0 || player.sliding ||
								player.jumpAgainBlizzard || player.jumpAgainCloud || player.jumpAgainFart || player.jumpAgainSandstorm ) {
							this.DrainStaminaViaJumpBegin( player );
						}
					}
					myplayer.IsJumping = true;
				}

				if( player.jump > 0 || PlayerMovementHelpers.IsFlying( player ) ) {
					if( player.swimTime > 0 ) {
						this.DrainStaminaViaSwimHold( player );
					} else {
						this.DrainStaminaViaJumpHold( player );
					}
				}
			} else if( myplayer.IsJumping ) {
				myplayer.IsJumping = false;
			}
		}
	}
}
