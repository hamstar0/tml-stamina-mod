using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.UI;
using System.Collections.Generic;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.Errors;


namespace Stamina {
	partial class StaminaMod : Mod {
		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			if( !this.Config.Enabled ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Resource Bars" ) );
			if( idx == -1 ) { return; }

			if( this.Config.ShowMainStaminaBar ) {
				var mainUiLayer = new LegacyGameInterfaceLayer( "Stamina: Main Meter", this.StaminaBarUIDraw, InterfaceScaleType.UI );
				layers.Insert( idx + 1, mainUiLayer );
			}

			if( this.Config.ShowMiniStaminaBar ) {
				var plrUiLayer = new LegacyGameInterfaceLayer( "Stamina: Player Meter", this.StaminaBarPlayerDraw, InterfaceScaleType.Game );
				layers.Insert( idx + 1, plrUiLayer );
			}
		}


		////////////////

		private bool StaminaBarUIDraw() {
			Player player = Main.LocalPlayer;
			StaminaPlayer myplayer = player.GetModPlayer<StaminaPlayer>();
			if( !myplayer.HasEnteredWorld ) { return true; }

			SpriteBatch sb = Main.spriteBatch;
			if( sb == null ) { return true; }

			try {
				int scrX = Main.screenWidth - 172;
				int scrY = 78;
				float alpha = myplayer.Logic.DrainingFX ? 1f : 0.65f;
				int stamina = (int)myplayer.Logic.Stamina;
				int maxStamina = myplayer.Logic.MaxStamina2;
				float fatigue = myplayer.Logic.Fatigue;
				bool isExercising = myplayer.Logic.IsExercising;
				int threshold = fatigue > 0 ? myplayer.Logic.GetStaminaLossAmountNeededForExercise() : -1;

				if( this.Config.CustomStaminaBarPositionX >= 0 ) {
					scrX = this.Config.CustomStaminaBarPositionX;
				}
				if( this.Config.CustomStaminaBarPositionY >= 0 ) {
					scrY = this.Config.CustomStaminaBarPositionY;
				}

				StaminaUI.DrawLongStaminaBar( sb, scrX, scrY, stamina, maxStamina, (int)fatigue, threshold, isExercising, alpha, 1f );
			} catch( Exception e ) {
				this.Logger.Info( e.ToString() );
			}

			if( this.Config.DebugModeInfoDrainers ) {
				this.PrintStaminaDrainers( sb, myplayer );
			}
			return true;
		}


		private bool StaminaBarPlayerDraw() {
			Player player = Main.LocalPlayer;
			if( player.dead ) { return true; }

			StaminaPlayer myplayer = player.GetModPlayer<StaminaPlayer>();
			if( !myplayer.HasEnteredWorld ) { return true; }

			SpriteBatch sb = Main.spriteBatch;
			if( sb == null ) { return true; }

			try {
				if( myplayer.Logic == null ) {
					throw new ModHelpersException( "Player logic failed to load." );
				}

				float alpha = myplayer.Logic.DrainingFX ? 1f : 0.65f;
				int stamina = (int)myplayer.Logic.Stamina;
				int maxStamina = myplayer.Logic.MaxStamina2;
				float fatigue = myplayer.Logic.Fatigue;
				bool isExercising = myplayer.Logic.IsExercising;
				int threshold = fatigue > 0 ? myplayer.Logic.GetStaminaLossAmountNeededForExercise() : -1;

				if( this.Config.ShowMiniStaminaBar ) {
					int plrX = (int)( player.position.X - Main.screenPosition.X ) + ( player.width / 2 );
					int plrY = (int)( player.position.Y - Main.screenPosition.Y ) + player.height;
					plrX += this.Config.PlayerStaminaBarOffsetX;
					plrY += this.Config.PlayerStaminaBarOffsetY;

					StaminaUI.DrawShortStaminaBar( sb, plrX, plrY, stamina, maxStamina, (int)fatigue, threshold, isExercising, alpha, 1f );
				}
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
			}
			return true;
		}


		////////////////

		private void PrintStaminaDrainers( SpriteBatch sb, StaminaPlayer myplayer ) {
			var dict = myplayer.Logic.CurrentDrainTypes;
			int i = 0;

			foreach( var kv in dict.ToList() ) {
				if( kv.Value == 0f ) { continue; }

				//string msg = kv.Key.ToString() + ":  " + kv.Value;
				//sb.DrawString( Main.fontMouseText, msg, new Vector2( 8, (Main.screenHeight - 32) - (i * 24) ), Color.White );
				DebugHelpers.Print( kv.Key.ToString(), "" + kv.Value, 30 );

				dict[kv.Key] = 0f;
				i++;
			}
		}
	}
}
