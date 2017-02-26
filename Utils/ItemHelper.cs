using Terraria;

namespace Stamina.Utils {
	public class ItemHelper {
		public static void DestroyItem( Item item ) {
			item.active = false;
			item.type = 0;
			item.name = "";
			item.stack = 0;
		}
	}
}
