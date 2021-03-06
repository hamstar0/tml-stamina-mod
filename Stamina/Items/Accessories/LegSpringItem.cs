﻿using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Hooks.ExtendedHooks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina.Items.Accessories {
	[AutoloadEquip( EquipType.Shoes )]
	class LegSpringItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;

		

		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Leg Spring" );
			this.Tooltip.SetDefault( "Jumps draw less stamina"
				+ '\n' + "Negates fall damage"
				+ '\n' + "'Puts a spring in your step.'" );

			ExtendedPlayerHooks.AddArmorEquipAction( "Stamina:LegSprintEquip", delegate ( Player player, int slot, Item myitem ) {
				if( myitem.type != ModContent.ItemType<LegSpringItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var myplayer = player.GetModPlayer<StaminaPlayer>();
				myplayer.IsWearingLegSprings = true;
			} );

			ExtendedPlayerHooks.AddArmorUnequipAction( "Stamina:LegSprintUnequip", delegate ( Player player, int slot, int itemType ) {
				if( itemType != ModContent.ItemType<LegSpringItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var myplayer = player.GetModPlayer<StaminaPlayer>();
				myplayer.IsWearingLegSprings = false;
			} );
		}

		public override void SetDefaults() {
			this.item.width = LegSpringItem.Width;
			this.item.height = LegSpringItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 10, 0, 0 );
			this.item.rare = 6;
			this.item.accessory = true;
		}


		////////////////

		public override void UpdateAccessory( Player player, bool hideVisual ) {
			player.noFallDmg = true;
		}

		////////////////

		public override void AddRecipes() {
			var recipe = new LegSpringItemRecipe( this );
			recipe.AddRecipe();
		}
	}




	class LegSpringItemRecipe : ModRecipe {
		public LegSpringItemRecipe( LegSpringItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.TinkerersWorkbench );

			this.AddRecipeGroup( "Stamina:MartialArtsMasterItems", 1 );
			this.AddIngredient( ItemID.FrogLeg, 1 );
			this.AddIngredient( ItemID.IronBar, 10 );

			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (StaminaMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableLegSprings;
		}
	}
}
