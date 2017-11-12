using Microsoft.Xna.Framework;
using Stamina.Buffs;
using Stamina.NetProtocol;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Stamina {
	public class MyPlayer : ModPlayer {
		private StaminaLogic Logic;
		public bool IsInitialized { get; private set; }

		public bool WillApplyExhaustion = false;
		public bool IsDashing = false;
		public bool IsJumping = false;
		public bool IsFlying = false;
		public bool HasCheckedFlying = false;

		private int SweatDelay = 0;


		////////////////

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (MyPlayer)clone;

			myclone.Logic = this.Logic;
			myclone.WillApplyExhaustion = this.WillApplyExhaustion;
			myclone.IsDashing = this.IsDashing;
			myclone.IsJumping = this.IsJumping;
			myclone.IsFlying = this.IsFlying;
			myclone.HasCheckedFlying = this.HasCheckedFlying;
			myclone.SweatDelay = this.SweatDelay;
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
				}
			}
		}

		////////////////

		public override void LoadLegacy( BinaryReader reader ) {
			this.Initialize();

			int max = reader.ReadInt32();
			bool has = reader.ReadBoolean();

			this.Logic = new StaminaLogic( (StaminaMod)this.mod, max, has );
			this.IsInitialized = true;
		}

		public override void Load( TagCompound tags ) {
			this.Initialize();
			var mymod = (StaminaMod)this.mod;

			if( !mymod.Config.LoadFile() ) {
				mymod.Config.SaveFile();
			}

			int max = mymod.Config.Data.InitialStamina;
			bool has = true;
			if( tags.ContainsKey("max_stamina") && tags.ContainsKey( "has_stamina") ) {
				max = tags.GetInt( "max_stamina" );
				has = tags.GetBool( "has_stamina" );
			}

			this.Logic = new StaminaLogic( mymod, max, has );
			this.IsInitialized = true;
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
			
			if( this.Logic != null ) {
				if( !this.player.dead ) {
					this.Logic.PassiveFatigueRecover( mymod, this.player );
					this.Logic.PassiveStaminaRegen( mymod, this.player );
					this.Logic.GatherPassiveStaminaDrains( mymod, this.player );
					this.Logic.CommitStaminaDrains( mymod );
					if( this.Logic.Stamina == 0 ) {
						this.ApplyDebuffs();
					}
				}
			}

			if( MyItem.StarUseCooldown[this.player.whoAmI] > 0 ) {
				MyItem.StarUseCooldown[this.player.whoAmI]--;
			}
		}

		public override void PostUpdateRunSpeeds() {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			if( this.Logic == null ) { return; }

			if( !this.player.dead ) {
				this.Logic.GatherActivityStaminaDrains( (StaminaMod)this.mod, this.player );

				if( this.WillApplyExhaustion ) {
					this.ApplyExhaustion();
					this.WillApplyExhaustion = false;
				}
			}
		}

		////////////////

		public override void PostHurt( bool pvp, bool quiet, double damage, int hit_direction, bool crit ) {
			var mymod = (StaminaMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }
			if( quiet ) { return; }

			float yike = (float)damage * mymod.Config.Data.PercentOfDamageAdrenalineBurst * (crit ? 2f : 1f);

			this.Logic.AddStamina( mymod, yike );
		}


		public override bool PreItemCheck() {
			var mymod = (StaminaMod)this.mod;
			bool prechecked = base.PreItemCheck();
			if( !mymod.Config.Data.Enabled ) { return prechecked; }
			if( this.Logic == null ) { return prechecked; }

			Item item = this.player.inventory[this.player.selectedItem ];

			if( item.type != 0 && !this.player.noItems ) {
				/*if( (player.itemTime == 0 && player.controlUseItem && player.releaseUseItem) ||
					(player.itemTime == 1 && player.controlUseItem && item.autoReuse) ) {
Main.NewText("PreItemCheck "+ StaminaMod.Config.Data.SingularExertionRate * ((float)item.useTime / 30f));
					this.DrainStamina( StaminaMod.Config.Data.SingularExertionRate * ((float)item.useTime/30f) );
				}*/
				if( this.player.controlUseItem && this.player.itemTime <= 1 ) {
					this.Logic.SetItemUseDuration( 36 );
				}
			}

			return prechecked;
		}

		////////////////

		public override void DrawEffects( PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright ) {
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

			this.player.AddBuff( buffid, (int)(mymod.Config.Data.ExhaustionDuration - this.Logic.TiredTimer) );
		}

		private void ApplyExhaustion() {
			ExhaustionBuff.ApplyMovementExhaustion( (StaminaMod)this.mod, this.player );
			this.player.dashDelay = 1;
			this.player.rocketTime = 0;
			this.player.wingTime = 0;
		}

		////////////////

		public bool GetDrainingFX() {
			if( this.Logic == null ) { throw new Exception("Logic not available."); }
			return this.Logic.DrainingFX;
		}
		public float GetStamina() {
			if( this.Logic == null ) { throw new Exception( "Logic not available." ); }
			return this.Logic.Stamina;
		}
		public int GetMaxStamina() {
			if( this.Logic == null ) { throw new Exception( "Logic not available." ); }
			return this.Logic.MaxStamina;
		}
		public float GetFatigue() {
			if( this.Logic == null ) { throw new Exception( "Logic not available." ); }
			return this.Logic.Fatigue;
		}
		public IDictionary<string, float> GetCurrentDrainTypes() {
			if( this.Logic == null ) { throw new Exception( "Logic not available." ); }
			return this.Logic.CurrentDrainTypes;
		}
		public int GetExerciseThreshold() {
			if( this.Logic == null ) { throw new Exception( "Logic not available." ); }
			return this.Logic.GetExerciseThreshold( (StaminaMod)this.mod );
		}
		public bool IsExercising() {
			if( this.Logic == null ) { throw new Exception( "Logic not available." ); }
			return this.Logic.IsExercising;
		}

		public void DrainStamina( float amt, string type ) {
			if( this.Logic == null ) { throw new Exception( "Logic not available." ); }
			this.Logic.DrainStamina( amt, type );
		}
		public void AddStamina( float amt ) {
			if( this.Logic == null ) { throw new Exception( "Logic not available." ); }
			this.Logic.AddStamina( (StaminaMod)this.mod, amt );
		}
		public void AddFatigue( float amt ) {
			if( this.Logic == null ) { throw new Exception( "Logic not available." ); }
			this.Logic.AddFatigue( amt );
		}
	}
}
