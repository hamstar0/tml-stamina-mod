using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace Utils {
	class PlayerLabelText {
		public string Text;
		public Color Color;
		public int StartDuration;
		public int Duration;
		public bool Evaporates;
	}


	public static class UIHelper {
		private static IDictionary<int, PlayerLabelText> PlayerTexts = new Dictionary<int, PlayerLabelText>();

		
		public static void AddPlayerLabel( Player player, string text, Color color, int duration, bool evaporates ) {
			UIHelper.PlayerTexts[ player.whoAmI ] = new PlayerLabelText {
				Text = text,
				Color = color,
				StartDuration = duration,
				Duration = duration,
				Evaporates = evaporates
			};
		}

		public static void UpdatePlayerLabels() {	// Called from an Update function
			foreach( int i in UIHelper.PlayerTexts.Keys.ToArray() ) {
				Player player = Main.player[i];
				if( player == null || !player.active || player.dead ) {
					UIHelper.PlayerTexts.Remove( i );
					continue;
				}
				if( UIHelper.PlayerTexts[i].Duration <= 0 ) {
					UIHelper.PlayerTexts.Remove( i );
				} else {
					UIHelper.PlayerTexts[i].Duration--;
				}
			}
		}

		public static void DrawPlayerLabels( SpriteBatch sb ) {	// Called from a Draw function
			foreach( int i in UIHelper.PlayerTexts.Keys ) {
				Player player = Main.player[i];
				if( player == null || !player.active || player.dead ) { continue; }

				var txt = UIHelper.PlayerTexts[i];
				var pos = new Vector2( player.Center.X - Main.screenPosition.X, player.position.Y - Main.screenPosition.Y );
				var color = txt.Color;

				if( txt.Evaporates ) {
					pos.Y -= txt.StartDuration - txt.Duration;

					float scale = (float)txt.Duration / (float)txt.StartDuration;
					color.R = (byte)((float)color.R * scale);
					color.G = (byte)((float)color.G * scale);
					color.B = (byte)((float)color.B * scale);
					color.A = (byte)((float)color.A * scale);


				}
				pos.X -= (Main.fontItemStack.MeasureString( txt.Text ).X * 1.5f) / 2f;
				
//DebugHelper.Display["execise"] = pos.ToString()+" "+txt.Text+" "+ color.ToString();
				sb.DrawString( Main.fontItemStack, txt.Text, pos, color, 0f, new Vector2(), 1.5f, SpriteEffects.None, 1f );
			}
		}
	}
}
