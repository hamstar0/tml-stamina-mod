using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.TmlHelpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina.Items.Accessories {
	[AutoloadEquip( EquipType.Waist )]
	class ChampionBeltItem : ModItem {
		public static int Width = 22;
		public static int Height = 18;

		
		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Champion Belt" );
			this.Tooltip.SetDefault( "Attacks draw less stamina"
				+ '\n' + "'We are the champions!'" );

			TmlPlayerHelpers.AddArmorEquipAction( "Stamina:ChampionBeltEquip", delegate ( Player player, int slot, Item myitem ) {
				if( myitem.type != this.mod.ItemType<ChampionBeltItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsWearingMuscleBelt = true;
			} );

			TmlPlayerHelpers.AddArmorUnequipAction( "Stamina:ChampionBeltUnequip", delegate ( Player player, int slot, int item_type ) {
				if( item_type != this.mod.ItemType<ChampionBeltItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsWearingMuscleBelt = false;
			} );
		}

		public override void SetDefaults() {
			this.item.width = ChampionBeltItem.Width;
			this.item.height = ChampionBeltItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 10, 0, 0 );
			this.item.rare = 6;
			this.item.accessory = true;
		}


		////////////////
		
		public override void AddRecipes() {
			var recipe = new ChampionBeltItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class ChampionBeltItemRecipe : ModRecipe {
		public ChampionBeltItemRecipe( ChampionBeltItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.TinkerersWorkbench );

			this.AddIngredient( ItemID.TitanGlove, 1 );
			this.AddIngredient( ItemID.AvengerEmblem, 1 );
			this.AddIngredient( ItemID.Leather, 10 );

			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (StaminaMod)this.mod;
			return mymod.Config.Data.Enabled && mymod.Config.Data.CraftableChampionBelts;
		}
	}
}
