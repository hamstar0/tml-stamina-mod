using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.TmlHelpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina.Items.Accessories {
	class ExerciseSupplementsItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;

		
		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Exercise Supplements" );
			this.Tooltip.SetDefault( "Makes exercise easier" );

			TmlPlayerHelpers.AddArmorEquipAction( "Stamina:ExerciseSupplementsEquip", delegate ( Player player, int slot, Item myitem ) {
				if( myitem.type != this.mod.ItemType<ExerciseSupplementsItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsUsingSupplements = true;
			} );

			TmlPlayerHelpers.AddArmorUnequipAction( "Stamina:ExerciseSupplementsUnequip", delegate ( Player player, int slot, int item_type ) {
				if( item_type != this.mod.ItemType<ExerciseSupplementsItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsUsingSupplements = false;
			} );
		}

		public override void SetDefaults() {
			this.item.width = ExerciseSupplementsItem.Width;
			this.item.height = ExerciseSupplementsItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
			this.item.rare = 5;
			//this.item.handOnSlot = 2;
			this.item.accessory = true;
		}


		////////////////
		
		public override void AddRecipes() {
			var recipe = new ExerciseSupplementsItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class ExerciseSupplementsItemRecipe : ModRecipe {
		public ExerciseSupplementsItemRecipe( ExerciseSupplementsItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.Bottles );
			this.AddIngredient( ItemID.Vitamins, 1 );
			this.AddIngredient( ItemID.PinkGel, 15 );
			this.AddIngredient( ItemID.Stinger, 15 );
			this.AddIngredient( ItemID.PumpkinPie, 5 );
			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (StaminaMod)this.mod;
			return mymod.Config.Data.Enabled && mymod.Config.Data.CraftableExerciseSupplements;
		}
	}
}
