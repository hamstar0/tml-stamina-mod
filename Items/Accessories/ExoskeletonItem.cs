using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.RecipeHelpers;
using HamstarHelpers.TmlHelpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina.Items.Accessories {
	[AutoloadEquip( EquipType.Shoes, EquipType.Neck, EquipType.Back, EquipType.Waist )]
	class ExoskeletonItem : ModItem {
		public static int Width = 30;
		public static int Height = 30;

		
		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Powered Exoskeleton Frame" );
			this.Tooltip.SetDefault( "Attacks, movement, and jumping draw less stamina"
				+ '\n' + "Negates fall damage and knockback"
				+ '\n' + "'Oil Can not included.'" );

			TmlPlayerHelpers.AddArmorEquipAction( "Stamina:ExoskeletonEquip", delegate ( Player player, int slot, Item myitem ) {
				if( myitem.type != this.mod.ItemType<ExoskeletonItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsWearingExoskeleton = true;
			} );

			TmlPlayerHelpers.AddArmorUnequipAction( "Stamina:ExoskeletonUnequip", delegate ( Player player, int slot, int item_type ) {
				if( item_type != this.mod.ItemType<ExoskeletonItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsWearingExoskeleton = false;
			} );
		}

		public override void SetDefaults() {
			this.item.width = ExoskeletonItem.Width;
			this.item.height = ExoskeletonItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 20, 0, 0 );
			this.item.rare = 7;
			this.item.defense = 6;
			this.item.accessory = true;
		}
		

		////////////////

		public override void UpdateAccessory( Player player, bool hideVisual ) {
			player.noKnockback = true;
			player.noFallDmg = true;
		}

		////////////////

		public override void AddRecipes() {
			var recipe = new ExoskeletonItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class ExoskeletonItemRecipe : ModRecipe {
		public ExoskeletonItemRecipe( ExoskeletonItem myitem ) : base( myitem.mod ) {
			var mymod = (StaminaMod)this.mod;

			this.AddTile( TileID.TinkerersWorkbench );

			if( mymod.Config.CraftableMuscleBelts ) {
				this.AddIngredient( this.mod.ItemType<ChampionBeltItem>(), 1 );
			}
			if( mymod.Config.CraftableJointBracers ) {
				this.AddIngredient( this.mod.ItemType<JointBracerItem>(), 1 );
			}
			if( mymod.Config.CraftableLegSprings ) {
				this.AddIngredient( this.mod.ItemType<LegSpringItem>(), 1 );
			}
			this.AddIngredient( ItemID.LihzahrdPowerCell, 2 );
			this.AddRecipeGroup( RecipeHelpers.ConveyorBelts.Key, 50 );

			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (StaminaMod)this.mod;
			var data = mymod.Config;
			
			return data.Enabled && data.CraftableExoskeletons
				&& (data.CraftableMuscleBelts || data.CraftableJointBracers || data.CraftableLegSprings );
		}
	}
}
