using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace Stamina {
	public static class StaminaUI {
		public static void DrawStaminaBar( SpriteBatch sb, float x, float y, int stamina, int max_stamina, int fatigue, int exercise_threshold, bool is_exercising, float alpha, float scale = 1f ) {
			Texture2D bar = Main.hbTexture1;
			Texture2D maxbar = Main.hbTexture2;
			int width = 256;
			int height = maxbar.Height;

			float ratio = (float)stamina / (float)max_stamina;
			if( ratio > 1f ) { ratio = 1f; }
			int stamina_bar_length = (int)((float)width * ratio);
			if( stamina_bar_length < 3 ) { stamina_bar_length = 3; }

			float offset_x = x - ((float)(width / 2) * scale);
			float offset_y = y;
			float depth = 1f;

			float r = 255f * alpha * 0.95f;
			float g = 255f * alpha * 0.95f * (ratio > 0.33f ? 1f : 0f);
			float b = 0f;
			float a = 255f * alpha * 0.95f;

			if( r < 0f ) { r = 0f; }
			if( r > 255f ) { r = 255f; }
			if( g < 0f ) { g = 0f; }
			if( g > 255f ) { g = 255f; }
			if( a < 0f ) { a = 0f; }
			if( a > 255f ) { a = 255f; }

			Color color = new Color( (int)((byte)r), (int)((byte)g), (int)((byte)b), (int)((byte)a) );

			//batch.DrawString(Main.fontMouseText, "stamina "+stamina+", max "+max_stamina+", ratio "+ratio+", ratio_scaled "+ratio_scaled, new Vector2(0, Main.screenHeight-32), Color.White);
			// Underneath bar (max stamina)
			{
				for( int i = 0; i < 16; i++ ) {
					int pos_x = 4 + (16 * i);
					if( pos_x >= width - 8 ) { break; }

					int wid_x = 16;
					if( wid_x > (width - 4) - pos_x ) {
						wid_x = (width - 4) - pos_x;
						if( wid_x <= 0 ) { break; }
					}

					// Mid
					var pos = new Vector2( pos_x + offset_x, offset_y );
					var rect = new Rectangle( 4, 0, wid_x, maxbar.Height );
					sb.Draw( maxbar, pos, new Rectangle?( rect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
				}

				// Start
				var s_pos = new Vector2( offset_x, offset_y );
				var s_rect = new Rectangle( 0, 0, 4, maxbar.Height );
				sb.Draw( maxbar, s_pos, new Rectangle?( s_rect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );

				// End
				var e_pos = new Vector2( offset_x + width - 4, offset_y );
				var e_rect = new Rectangle( maxbar.Width - 4, 0, 4, maxbar.Height );
				sb.Draw( maxbar, e_pos, new Rectangle?( e_rect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
			}

			// Overlay bar (stamina)
			{
				int wid_x = stamina_bar_length < 4 ? stamina_bar_length : 4;

				// Start cap
				var s_pos = new Vector2( offset_x, offset_y );
				var s_rect = new Rectangle( 0, 0, wid_x, bar.Height );
				sb.Draw( bar, s_pos, new Rectangle?( s_rect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );

				if( stamina_bar_length > 4 ) {
					int stamina_bar_length_nocap = stamina_bar_length < width - 4 ? stamina_bar_length : width - 4;

					for( int i = 0; i < 16; i++ ) {
						int pos_x = 4 + (16 * i);
						if( pos_x >= stamina_bar_length_nocap ) { break; }

						wid_x = 16;
						if( wid_x > stamina_bar_length_nocap - pos_x ) {
							wid_x = stamina_bar_length_nocap - pos_x;
							if( wid_x <= 0 ) { break; }
						}

						// Mid
						var pos = new Vector2( offset_x + pos_x, offset_y );
						var rect = new Rectangle( 4, 0, wid_x, bar.Height );
						sb.Draw( bar, pos, new Rectangle?( rect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
					}

					// End
					if( stamina_bar_length > width - 4 ) {
						var pos = new Vector2( offset_x + width - 4, offset_y );
						var rect = new Rectangle( bar.Width - 4, 0, 4 /*wid_x*/, bar.Height );
						sb.Draw( bar, pos, new Rectangle?( rect ), color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
					}
				}
			}

			// Fatigue bar (stamina)
			if( fatigue > 0 ) {
				Color fat_color = Color.Gray;
				if( is_exercising ) { fat_color = new Color(128, 255, 128); }
				float fat_ratio = (float)fatigue / (float)max_stamina;
				if( fat_ratio > 1f ) { fat_ratio = 1f; }
				int fat_bar_length = (int)( (float)width * fat_ratio );
				if( fat_bar_length < 3 ) { fat_bar_length = 3; }
				int fat_bar_length_nocap = fat_bar_length < width - 4 ? fat_bar_length : width - 4;

				// End cap
				var e_pos = new Vector2( offset_x + width - 4, offset_y );
				var e_rect = new Rectangle( bar.Width - 4, 0, 4, bar.Height );
				sb.Draw( bar, e_pos, new Rectangle?( e_rect ), fat_color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
				
				for( int i = 0; i < 16; i++ ) {
					int base_pos_x = (width - 20) - (i * 16);
					int rem_bar_len = fat_bar_length_nocap - (i * 16);
					int wid_x = rem_bar_len >= 16 ? 16 : rem_bar_len > 0 ? rem_bar_len : 0;
					if( wid_x <= 0 ) { break; }

					// Mid
					var pos = new Vector2( offset_x + base_pos_x + (16 - wid_x), offset_y );
					var rect = new Rectangle( 4, 0, wid_x, bar.Height );
					sb.Draw( bar, pos, new Rectangle?( rect ), fat_color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
				}

				// Start cap
				if( fat_bar_length >= 252 ) {
					var pos = new Vector2( offset_x, offset_y );
					var rect = new Rectangle( 0, 0, 4, bar.Height );
					sb.Draw( bar, pos, new Rectangle?( rect ), fat_color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
				}
			}

			// Exercise tick
			if( !is_exercising && exercise_threshold > 0 ) {
				float offset_exercise = ((float)(max_stamina - exercise_threshold) / (float)max_stamina) * (float)width;
				var pos = new Vector2( offset_x, offset_y );
				pos.X += offset_exercise - 2f;
				var rect = new Rectangle( 2, 0, 4, bar.Height );
				var ex_color = new Color( Main.DiscoR, Main.DiscoG, Main.DiscoB );
				
				sb.Draw( bar, pos, new Rectangle?(rect), ex_color, 0f, new Vector2(), scale, SpriteEffects.None, depth );
			}

			// Text overlay
			var txt = stamina + " / " + max_stamina;
			Vector2 t_rect = Main.fontMouseText.MeasureString( txt );
			float x_pos = offset_x + ((float)width / 2f) - (t_rect.X / 2f);
			var t_pos = new Vector2( x_pos, offset_y-1f );
			
			sb.DrawString( Main.fontMouseText, txt, t_pos, Color.White, 0, new Vector2(), new Vector2(1f, 0.6f), SpriteEffects.None, 1f );
		}
	}
}
