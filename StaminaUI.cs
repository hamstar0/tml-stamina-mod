using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;


namespace Stamina {
	public static class StaminaUI {
		public static void DrawShortStaminaBar( SpriteBatch sb, float x, float y, int stamina, int maxStamina, int fatigue, int exerciseThreshold, bool isExercising, float alpha, float scale = 1f ) {
			Texture2D bar = Main.hbTexture1;
			Texture2D maxbar = Main.hbTexture2;
			int texWidth = maxbar.Width;
			int texHeight = maxbar.Height;

			float ratio = (float)stamina / (float)maxStamina;
			if( ratio > 1f ) { ratio = 1f; }
			int staminaBarLength = (int)((float)texWidth * ratio);
			if( staminaBarLength < 3 ) { staminaBarLength = 3; }

			float fatRatio = (float)fatigue / (float)maxStamina;
			if( fatRatio > 1f ) { fatRatio = 1f; }

			float xFinal = x - ((float)(texWidth * 0.5f) * scale);
			float yFinal = y;
			float depth = 1f;

			float largestRatio = Math.Max( 0.1f, Math.Max( (1f - ratio), fatRatio ) );
			float alphaFinal = largestRatio * alpha;

			float r = 255f * alphaFinal * 0.95f;
			float g = 255f * alphaFinal * 0.95f * (ratio > 0.33f ? 1f : 0f);
			float b = 0f;
			float a = 255f * alphaFinal * 0.95f;

			if( r < 0f ) { r = 0f; } else if( r > 255f ) { r = 255f; }
			if( g < 0f ) { g = 0f; } else if( g > 255f ) { g = 255f; }
			if( a < 0f ) { a = 0f; } else if( a > 255f ) { a = 255f; }

			Color color = new Color( (byte)r, (byte)g, (byte)b, (byte)a );
			var pos = new Vector2( xFinal, yFinal );

			// Underneath bar
			var uRect = new Rectangle( 0, 0, texWidth, texHeight );
			sb.Draw( maxbar, pos, uRect, color, 0f, new Vector2(), scale, SpriteEffects.None, depth );

			// Overlay bar (stamina)
			var oRect = new Rectangle( 0, 0, (int)((float)texWidth * ratio), bar.Height );
			sb.Draw( bar, pos, oRect, color, 0f, new Vector2(), scale, SpriteEffects.None, depth );

			// Fatigue bar (stamina)
			if( fatigue > 0 ) {
				Color fatColor = Color.Gray;
				if( isExercising ) { fatColor = new Color( 128, 255, 128 ); }

				int fatBarLength = (int)((float)texWidth * fatRatio);

				var fPos = new Vector2( xFinal + (texWidth - fatBarLength), yFinal );
				var rect = new Rectangle( texWidth - fatBarLength, 0, fatBarLength, bar.Height );
				sb.Draw( bar, fPos, rect, fatColor, 0f, new Vector2(), scale, SpriteEffects.None, depth );
			}
		}


		public static void DrawLongStaminaBar( SpriteBatch sb, float x, float y, int stamina, int maxStamina, int fatigue, int exerciseThreshold, bool isExercising, float alpha, float scale = 1f ) {
			Texture2D bar = Main.hbTexture1;
			Texture2D maxbar = Main.hbTexture2;
			int width = 256;
			int height = maxbar.Height;

			float ratio = (float)stamina / (float)maxStamina;
			if( ratio > 1f ) { ratio = 1f; }
			int staminaBarLength = (int)((float)width * ratio);
			if( staminaBarLength < 3 ) { staminaBarLength = 3; }

			float offsetX = x - ((float)(width / 2) * scale);
			float offsetY = y;
			float depth = 1f;

			float r = 255f * alpha * 0.95f;
			float g = 255f * alpha * 0.95f * (ratio > 0.33f ? 1f : 0f);
			float b = 0f;
			float a = 255f * alpha * 0.95f;

			if( r < 0f ) { r = 0f; } else if( r > 255f ) { r = 255f; }
			if( g < 0f ) { g = 0f; } else if( g > 255f ) { g = 255f; }
			if( a < 0f ) { a = 0f; } else if( a > 255f ) { a = 255f; }

			Color color = new Color( (byte)r, (byte)g, (byte)b, (byte)a );

			//batch.DrawString(Main.fontMouseText, "stamina "+stamina+", max "+maxStamina+", ratio "+ratio+", ratioScaled "+ratioScaled, new Vector2(0, Main.screenHeight-32), Color.White);
			// Underneath bar (max stamina)
			{
				for( int i = 0; i < 16; i++ ) {
					int posX = 4 + (16 * i);
					if( posX >= width - 8 ) { break; }

					int widX = 16;
					if( widX > (width - 4) - posX ) {
						widX = (width - 4) - posX;
						if( widX <= 0 ) { break; }
					}

					// Mid
					var pos = new Vector2( posX + offsetX, offsetY );
					var rect = new Rectangle( 4, 0, widX, maxbar.Height );
					sb.Draw( maxbar, pos, new Rectangle?( rect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
				}

				// Start
				var sPos = new Vector2( offsetX, offsetY );
				var sRect = new Rectangle( 0, 0, 4, maxbar.Height );
				sb.Draw( maxbar, sPos, new Rectangle?( sRect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );

				// End
				var ePos = new Vector2( offsetX + width - 4, offsetY );
				var eRect = new Rectangle( maxbar.Width - 4, 0, 4, maxbar.Height );
				sb.Draw( maxbar, ePos, new Rectangle?( eRect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
			}

			// Overlay bar (stamina)
			{
				int widX = staminaBarLength < 4 ? staminaBarLength : 4;

				// Start cap
				var sPos = new Vector2( offsetX, offsetY );
				var sRect = new Rectangle( 0, 0, widX, bar.Height );
				sb.Draw( bar, sPos, new Rectangle?( sRect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );

				if( staminaBarLength > 4 ) {
					int staminaBarLengthNocap = staminaBarLength < width - 4 ? staminaBarLength : width - 4;

					for( int i = 0; i < 16; i++ ) {
						int posX = 4 + (16 * i);
						if( posX >= staminaBarLengthNocap ) { break; }

						widX = 16;
						if( widX > staminaBarLengthNocap - posX ) {
							widX = staminaBarLengthNocap - posX;
							if( widX <= 0 ) { break; }
						}

						// Mid
						var pos = new Vector2( offsetX + posX, offsetY );
						var rect = new Rectangle( 4, 0, widX, bar.Height );
						sb.Draw( bar, pos, new Rectangle?( rect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
					}

					// End
					if( staminaBarLength > width - 4 ) {
						var pos = new Vector2( offsetX + width - 4, offsetY );
						var rect = new Rectangle( bar.Width - 4, 0, 4 /*widX*/, bar.Height );
						sb.Draw( bar, pos, new Rectangle?( rect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
					}
				}
			}

			// Fatigue bar (stamina)
			if( fatigue > 0 ) {
				Color fatColor = Color.Gray;
				if( isExercising ) { fatColor = new Color(128, 255, 128); }
				float fatRatio = (float)fatigue / (float)maxStamina;
				if( fatRatio > 1f ) { fatRatio = 1f; }
				int fatBarLength = (int)( (float)width * fatRatio );
				if( fatBarLength < 3 ) { fatBarLength = 3; }
				int fatBarLengthNocap = fatBarLength < width - 4 ? fatBarLength : width - 4;

				// End cap
				var ePos = new Vector2( offsetX + width - 4, offsetY );
				var eRect = new Rectangle( bar.Width - 4, 0, 4, bar.Height );
				sb.Draw( bar, ePos, new Rectangle?( eRect ), fatColor, 0f, new Vector2(), scale, SpriteEffects.None, depth );
				
				for( int i = 0; i < 16; i++ ) {
					int basePosX = (width - 20) - (i * 16);
					int remBarLen = fatBarLengthNocap - (i * 16);
					int widX = remBarLen >= 16 ? 16 : remBarLen > 0 ? remBarLen : 0;
					if( widX <= 0 ) { break; }

					// Mid
					var pos = new Vector2( offsetX + basePosX + (16 - widX), offsetY );
					var rect = new Rectangle( 4, 0, widX, bar.Height );
					sb.Draw( bar, pos, new Rectangle?( rect ), fatColor, 0f, new Vector2(), scale, SpriteEffects.None, depth );
				}

				// Start cap
				if( fatBarLength >= 252 ) {
					var pos = new Vector2( offsetX, offsetY );
					var rect = new Rectangle( 0, 0, 4, bar.Height );
					sb.Draw( bar, pos, new Rectangle?( rect ), fatColor, 0f, new Vector2(), scale, SpriteEffects.None, depth );
				}
			}

			// Exercise tick
			if( !isExercising && exerciseThreshold > 0 ) {
				float offsetExercise = ((float)(maxStamina - exerciseThreshold) / (float)maxStamina) * (float)width;
				var pos = new Vector2( offsetX, offsetY );
				pos.X += offsetExercise - 2f;
				var rect = new Rectangle( 2, 0, 2, bar.Height );
				var exColor = new Color( Main.DiscoR, Main.DiscoG, Main.DiscoB );
				
				sb.Draw( bar, pos, new Rectangle?(rect), exColor, 0f, new Vector2(), scale, SpriteEffects.None, depth );
				pos.X += 2f;
				sb.Draw( bar, pos, new Rectangle?(rect), exColor, 0f, new Vector2(), scale, SpriteEffects.None, depth );
			}

			// Text overlay
			var txt = stamina + " / " + maxStamina;
			Vector2 tRect = Main.fontMouseText.MeasureString( txt );
			float xPos = offsetX + ((float)width / 2f) - (tRect.X / 2f);
			var tPos = new Vector2( xPos, offsetY-1f );
			
			sb.DrawString( Main.fontMouseText, txt, tPos, Color.White, 0, new Vector2(), new Vector2(1f, 0.6f), SpriteEffects.None, 1f );
		}
	}
}
