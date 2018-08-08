using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.UI;
using System.Collections.Generic;
using HamstarHelpers.Helpers.DebugHelpers;


namespace Stamina {
	partial class StaminaMod : Mod {
		private bool StaminaBarUIDraw() {
			Player player = Main.LocalPlayer;
			StaminaPlayer myplayer = player.GetModPlayer<StaminaPlayer>();
			if( !myplayer.HasEnteredWorld ) { return true; }

			SpriteBatch sb = Main.spriteBatch;
			if( sb == null ) { return true; }

			try {
				int scr_x = Main.screenWidth - 172;
				int scr_y = 78;
				float alpha = myplayer.Logic.DrainingFX ? 1f : 0.65f;
				int stamina = (int)myplayer.Logic.Stamina;
				int max_stamina = myplayer.Logic.MaxStamina2;
				float fatigue = myplayer.Logic.Fatigue;
				bool is_exercising = myplayer.Logic.IsExercising;
				int threshold = fatigue > 0 ? myplayer.Logic.GetStaminaLossAmountNeededForExercise( this ) : -1;

				if( this.Config.CustomStaminaBarPositionX >= 0 ) {
					scr_x = this.Config.CustomStaminaBarPositionX;
				}
				if( this.Config.CustomStaminaBarPositionY >= 0 ) {
					scr_y = this.Config.CustomStaminaBarPositionY;
				}

				StaminaUI.DrawLongStaminaBar( sb, scr_x, scr_y, stamina, max_stamina, (int)fatigue, threshold, is_exercising, alpha, 1f );
			} catch( Exception e ) { ErrorLogger.Log( e.ToString() ); }

			if( this.Config.DEBUG_VIEW_DRAINERS ) {
				this.PrintStaminaDrainers( sb, myplayer );
			}
			return true;
		}


		private bool StaminaBarPlayerDraw() {
			Player player = Main.LocalPlayer;
			StaminaPlayer modplayer = player.GetModPlayer<StaminaPlayer>();
			if( !modplayer.HasEnteredWorld ) { return true; }

			SpriteBatch sb = Main.spriteBatch;
			if( sb == null ) { return true; }

			try {
				float alpha = modplayer.Logic.DrainingFX ? 1f : 0.65f;
				int stamina = (int)modplayer.Logic.Stamina;
				int max_stamina = modplayer.Logic.MaxStamina2;
				float fatigue = modplayer.Logic.Fatigue;
				bool is_exercising = modplayer.Logic.IsExercising;
				int threshold = fatigue > 0 ? modplayer.Logic.GetStaminaLossAmountNeededForExercise( this ) : -1;

				if( this.Config.ShowMiniStaminaBar ) {
					int plr_x = (int)( player.position.X - Main.screenPosition.X ) + ( player.width / 2 );
					int plr_y = (int)( player.position.Y - Main.screenPosition.Y ) + player.height;
					plr_x += this.Config.PlayerStaminaBarOffsetX;
					plr_y += this.Config.PlayerStaminaBarOffsetY;

					StaminaUI.DrawShortStaminaBar( sb, plr_x, plr_y, stamina, max_stamina, (int)fatigue, threshold, is_exercising, alpha, 1f );
				}
			} catch( Exception e ) { ErrorLogger.Log( e.ToString() ); }
			return true;
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			if( !this.Config.Enabled ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Resource Bars" ) );
			if( idx == -1 ) { return; }

			if( this.Config.ShowMainStaminaBar ) {
				var main_ui_layer = new LegacyGameInterfaceLayer( "Stamina: Main Meter", this.StaminaBarUIDraw, InterfaceScaleType.UI );
				layers.Insert( idx + 1, main_ui_layer );
			}

			if( this.Config.ShowMiniStaminaBar ) {
				var plr_ui_layer = new LegacyGameInterfaceLayer( "Stamina: Player Meter", this.StaminaBarPlayerDraw, InterfaceScaleType.Game );
				layers.Insert( idx + 1, plr_ui_layer );
			}
		}


		private void PrintStaminaDrainers( SpriteBatch sb, StaminaPlayer modplayer ) {
			var dict = modplayer.Logic.CurrentDrainTypes;
			int i = 0;

			foreach( var kv in dict.ToList() ) {
				if( kv.Value == 0f ) { continue; }

				//string msg = kv.Key.ToString() + ":  " + kv.Value;
				//sb.DrawString( Main.fontMouseText, msg, new Vector2( 8, (Main.screenHeight - 32) - (i * 24) ), Color.White );
				DebugHelpers.Print( kv.Key.ToString(), "" + kv.Value, 30 );

				dict[ kv.Key ] = 0f;
				i++;
			}
		}
	}
}
