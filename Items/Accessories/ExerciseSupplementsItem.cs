using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.Players;
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

			PlayerState.AddArmorEquipAction( "Stamina:ExerciseSupplementsEquip", delegate ( Player player, int slot, Item myitem ) {
				if( myitem.type != this.mod.ItemType<ExerciseSupplementsItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var myplayer = player.GetModPlayer<StaminaPlayer>();
				myplayer.IsUsingSupplements = true;
			} );

			PlayerState.AddArmorUnequipAction( "Stamina:ExerciseSupplementsUnequip", delegate ( Player player, int slot, int itemType ) {
				if( itemType != this.mod.ItemType<ExerciseSupplementsItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var myplayer = player.GetModPlayer<StaminaPlayer>();
				myplayer.IsUsingSupplements = false;
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
			return mymod.Config.Enabled && mymod.Config.CraftableExerciseSupplements;
		}
	}
}
