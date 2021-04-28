using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Hooks.ExtendedHooks;
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

			ExtendedPlayerHooks.AddArmorEquipAction( "Stamina:ChampionBeltEquip", delegate ( Player player, int slot, Item myitem ) {
				if( myitem.type != ModContent.ItemType<ChampionBeltItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var myplayer = player.GetModPlayer<StaminaPlayer>();
				myplayer.IsWearingMuscleBelt = true;
			} );

			ExtendedPlayerHooks.AddArmorUnequipAction( "Stamina:ChampionBeltUnequip", delegate ( Player player, int slot, int itemType ) {
				if( itemType != ModContent.ItemType<ChampionBeltItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var myplayer = player.GetModPlayer<StaminaPlayer>();
				myplayer.IsWearingMuscleBelt = false;
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
			return mymod.Config.Enabled && mymod.Config.CraftableChampionBelts;
		}
	}
}
