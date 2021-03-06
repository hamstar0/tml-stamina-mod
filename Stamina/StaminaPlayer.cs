﻿using HamstarHelpers.Helpers.Debug;
using Stamina.Buffs;
using Stamina.Logic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Stamina {
	partial class StaminaPlayer : ModPlayer {
		public StaminaLogic Logic { get; internal set; }

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

		public override bool CloneNewInstances => false;



		////////////////
		
		public override void Initialize() {
			this.Logic = new StaminaLogic( -1, false );
		}

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (StaminaPlayer)clone;

			myclone.Logic = this.Logic.Clone();
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


		////////////////

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (StaminaMod)this.mod;

			if( Main.netMode == 0 ) {   // single
				this.OnSingleJoin();
			}
		}

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == 1 ) {
				if( newPlayer ) {
					this.OnClientJoin();
				}
			} else if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					this.OnServerJoin();
				}
			}
		}


		////

		public void OnSingleJoin() {
			//this.HasEnteredWorld = true;
		}

		public void OnClientJoin() {
			//this.HasEnteredWorld = true;
		}

		public void OnServerJoin() {
			//this.HasEnteredWorld = true;
		}


		////////////////

		public override void Load( TagCompound tags ) {
			this.Initialize();
			var mymod = (StaminaMod)this.mod;

			int max = mymod.Config.InitialStamina;
			bool has = true;
			if( tags.ContainsKey("max_stamina") && tags.ContainsKey("has_stamina") ) {
				max = tags.GetInt( "max_stamina" );
				has = tags.GetBool( "has_stamina" );
			}

			this.Logic = new StaminaLogic( max, has );
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

		private void ApplyDebuffs() {
			var mymod = (StaminaMod)this.mod;
			int buffid = this.mod.BuffType( "ExhaustionBuff" );

			int duration = mymod.Config.ExhaustionDuration - (int)this.Logic.TiredTimer;

			if( duration > 0 ) {
				this.player.AddBuff( buffid, duration );
			}
		}

		private void ApplyExhaustionEffect() {
			ExhaustionBuff.ApplyMovementExhaustion( this.player );
			this.player.dashDelay = 1;
			this.player.rocketTime = 0;
			this.player.wingTime = 0;
		}
	}
}
