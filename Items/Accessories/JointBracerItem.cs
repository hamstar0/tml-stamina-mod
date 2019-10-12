using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Hooks.ExtendedHooks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina.Items.Accessories {
	class JointBracerItem : ModItem {
		public static int Width = 30;
		public static int Height = 26;

		

		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Joint Bracer" );
			this.Tooltip.SetDefault( "Non-jump movements draw less stamina"
				+ '\n' + "Negates knockback"
				+ '\n' + "'For when push comes to shove.'" );

			ExtendedPlayerHooks.AddArmorEquipAction( "Stamina:JointBracerEquip", delegate ( Player player, int slot, Item myitem ) {
				if( myitem.type != ModContent.ItemType<JointBracerItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot(player, slot) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsWearingJointBracer = true;
			} );

			ExtendedPlayerHooks.AddArmorUnequipAction( "Stamina:JointBracerUnequip", delegate ( Player player, int slot, int itemType ) {
				if( itemType != ModContent.ItemType<JointBracerItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsWearingJointBracer = false;
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

		public override void UpdateAccessory( Player player, bool hideVisual ) {
			player.noKnockback = true;
		}

		////////////////

		public override void AddRecipes() {
			var recipe = new JointBracerItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class JointBracerItemRecipe : ModRecipe {
		public JointBracerItemRecipe( JointBracerItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.TinkerersWorkbench );

			this.AddIngredient( ItemID.DualHook, 1 );
			this.AddIngredient( ItemID.StaticHook, 1 );
			this.AddIngredient( ItemID.Cog, 25 );
			this.AddRecipeGroup( "Stamina:GreaterBiomeHook", 1 );
			this.AddRecipeGroup( "ModHelpers:WeightPressurePlates", 4 );
			
			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (StaminaMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableJointBracers;
		}
	}
}
