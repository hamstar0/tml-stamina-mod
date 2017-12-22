using Microsoft.Xna.Framework;
using Stamina.Buffs;
using Stamina.Items.Accessories;
using Stamina.Logic;
using Stamina.NetProtocol;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Stamina {
	class StaminaPlayer : ModPlayer {
		public StaminaLogic Logic { get; private set; }
		public bool HasEnteredWorld { get; private set; }

		public bool WillApplyExhaustion = false;
		public bool IsDashing = false;
		public bool IsJumping = false;
		public bool IsFlying = false;
		public bool HasCheckedFlying = false;

		public bool IsWearingRageBandana = false;
		public bool IsUsingSupplements = false;
		public bool IsWearingMuscleBelt = false;
		public bool IsWearingJointBracer = false;
		public bool IsWearingLegSprings = false;
		public bool IsWearingExoskeleton = false;

		private int SweatDelay = 0;
		

		////////////////

		public override void Initialize() {
			this.Logic = null;
			this.HasEnteredWorld = false;
		}

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (StaminaPlayer)clone;

			myclone.Logic = this.Logic;
			myclone.WillApplyExhaustion = this.WillApplyExhaustion;
			myclone.IsDashing = this.IsDashing;
			myclone.IsJumping = this.IsJumping;
			myclone.IsFlying = this.IsFlying;
			myclone.HasCheckedFlying = this.HasCheckedFlying;
			myclone.SweatDelay = this.SweatDelay;
			myclone.IsWearingRageBandana = this.IsWearingRageBandana;
			myclone.IsUsingSupplements = this.IsUsingSupplements;
			myclone.IsWearingMuscleBelt = this.IsWearingMuscleBelt;
			myclone.IsWearingJointBracer = this.IsWearingJointBracer;
		}

		public override void OnEnterWorld( Player player ) {
			var mymod = (StaminaMod)this.mod;

			if( player.whoAmI == this.player.whoAmI ) { // Current player
				if( Main.netMode != 2 ) {   // Not server
					if( !mymod.Config.LoadFile() ) {
						mymod.Config.SaveFile();
					}
				}

				if( Main.netMode == 1 ) {   // Client
					ClientPacketHandlers.SendSettingsRequestFromClient( this.mod, player );
				} else {
					this.PostEnterWorld();
				}
			}
		}

		public void PostEnterWorld() {
			this.HasEnteredWorld = true;
		}

		public void OnReceiveServerSettings() {
			if( !this.HasEnteredWorld ) { this.PostEnterWorld(); }
		}

		////////////////

		public override void Load( TagCompound tags ) {
			this.Initialize();
			var mymod = (StaminaMod)this.mod;

			if( !mymod.Config.LoadFile() ) {
				mymod.Config.SaveFile();
			}

			int max = mymod.Config.Data.InitialStamina;
			bool has = true;
			if( tags.ContainsKey("max_stamina") && tags.ContainsKey("has_stamina") ) {
				max = tags.GetInt( "max_stamina" );
				has = tags.GetBool( "has_stamina" );
			}

			this.Logic = new StaminaLogic( mymod, max, has );
		}

		public override TagCompound Save() {
			if( this.Logic == null ) { return new TagCompound(); }

			return new TagCompound {
				{"has_saved", true},
				{"max_stamina", this.Logic.MaxStamina},
				{"has_stamina", this.Logic.HasStaminaSet}
			};
		}

		////////////////

		public override void PreUpdate() {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }
			if( this.Logic == null ) { return; }
			
			this.Logic.UpdateMaxStamina( mymod, this.player );

			if( this.Logic != null ) {
				if( !this.player.dead ) {
					this.Logic.PassiveFatigueRecover( mymod, this.player );
					this.Logic.PassiveStaminaRegen( mymod, this.player );
					this.Logic.GatherPassiveStaminaDrains( mymod, this.player );
					this.Logic.CommitStaminaDrains( mymod, this.player );

					if( this.Logic.Stamina == 0 ) {
						this.ApplyDebuffs();
					}
				}
			}

			if( StaminaItem.StarUseCooldown[this.player.whoAmI] > 0 ) {
				StaminaItem.StarUseCooldown[this.player.whoAmI]--;
			}
		}

		public override void PostUpdateRunSpeeds() {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }
			if( this.Logic == null ) { return; }

			if( !this.player.dead ) {
				this.Logic.GatherActivityStaminaDrains( mymod, this.player );

				if( this.WillApplyExhaustion ) {
					this.ApplyExhaustionEffect();
					this.WillApplyExhaustion = false;
				}
			}
		}

		////////////////

		public override bool PreHurt( bool pvp, bool quiet, ref int damage, ref int hit_direction, ref bool crit, ref bool custom_damage, ref bool play_sound, ref bool gen_gore, ref PlayerDeathReason damage_source ) {
			if( damage_source != null ) {
				string src_reason = damage_source.SourceCustomReason;
				string rage_reason = RageHeadbandItem.DamageType.SourceCustomReason;

				if( src_reason != null && src_reason.Equals( rage_reason ) ) {
					custom_damage = true;
					crit = false;
				}
			}
			return base.PreHurt( pvp, quiet, ref damage, ref hit_direction, ref crit, ref custom_damage, ref play_sound, ref gen_gore, ref damage_source );
		}


		public override void PostHurt( bool pvp, bool quiet, double damage, int hit_direction, bool crit ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }
			if( quiet ) { return; }

			float yike = (float)damage * mymod.Config.Data.PercentOfDamageAdrenalineBurst * (crit ? 2f : 1f);

			this.Logic.AddStamina( mymod, this.player, yike );
		}

		
		public override bool PreItemCheck() {
			bool prechecked = base.PreItemCheck();
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return prechecked; }
			if( this.Logic == null ) { return prechecked; }

			Item item = this.player.inventory[ this.player.selectedItem ];

			if( item.type != 0 && !this.player.noItems ) {
				/*if( (player.itemTime == 0 && player.controlUseItem && player.releaseUseItem) ||
					(player.itemTime == 1 && player.controlUseItem && item.autoReuse) ) {
Main.NewText("PreItemCheck "+ StaminaMod.Config.Data.SingularExertionRate * ((float)item.useTime / 30f));
					this.DrainStamina( StaminaMod.Config.Data.SingularExertionRate * ((float)item.useTime/30f) );
				}*/
				if( this.player.controlUseItem && this.player.itemTime <= 1 ) {
					this.Logic.SetItemUseHoldDurationForStaminaDrain( 36 );
				}
			}

			return prechecked;
		}

		////////////////

		public override void DrawEffects( PlayerDrawInfo draw_info, ref float r, ref float g, ref float b, ref float a, ref bool full_bright ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }
			if( this.Logic == null ) { return; }
			if( this.Logic.Stamina > 0 ) { return; }
			
			if( --this.SweatDelay <= 0 ) {
				this.SweatDelay = 3;

				int width = 32;
				int height = 16;

				var pos = new Vector2( this.player.position.X - (this.player.width / 2), this.player.position.Y );
				if( this.player.gravDir < 0 ) {
					pos.Y += this.player.height;
				}

				var vel = this.player.velocity;
				vel.X += 6f - (Main.rand.NextFloat() * 8f);
				vel.Y += Main.rand.NextFloat() * -8f;

				int dust = Dust.NewDust( pos, width, height, 52, vel.X, vel.Y );    // 33, 98-105, 154(-), 266
				//Main.dust[dust].alpha = 0;
			}
		}
		
		////////////////

		private void ApplyDebuffs() {
			var mymod = (StaminaMod)this.mod;
			int buffid = this.mod.BuffType( "ExhaustionBuff" );

			int duration = mymod.Config.Data.ExhaustionDuration - (int)this.Logic.TiredTimer;

			if( duration > 0 ) {
				this.player.AddBuff( buffid, duration );
			}
		}

		private void ApplyExhaustionEffect() {
			ExhaustionBuff.ApplyMovementExhaustion( (StaminaMod)this.mod, this.player );
			this.player.dashDelay = 1;
			this.player.rocketTime = 0;
			this.player.wingTime = 0;
		}
	}
}
