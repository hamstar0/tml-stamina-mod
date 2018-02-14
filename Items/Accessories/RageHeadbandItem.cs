using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.TmlHelpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina.Items.Accessories {
	[AutoloadEquip( EquipType.Face )]
	class RageHeadbandItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;
		public static readonly PlayerDeathReason DamageType = PlayerDeathReason.ByCustomReason( "over exertion" );


		////////////////

		public static int CalculateDamageFromStaminaDrain( StaminaMod mymod, float drain ) {
			return (int)(drain * mymod.Config.RageHeadbandDamageMultiplier);
		}

		public static void ApplyDamage( StaminaMod mymod, Player player, float drain ) {
			int damage = RageHeadbandItem.CalculateDamageFromStaminaDrain( mymod, drain );
			player.Hurt( RageHeadbandItem.DamageType, damage, 0 );
		}


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Rage Headband" );
			this.Tooltip.SetDefault( "Trades exhaustion for health loss"
				+'\n'+"'Feel the burn!'" );

			TmlPlayerHelpers.AddArmorEquipAction( "Stamina:RageHeadbandEquip", delegate ( Player player, int slot, Item myitem ) {
				if( myitem.type != this.mod.ItemType<RageHeadbandItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsWearingRageBandana = true;
			} );

			TmlPlayerHelpers.AddArmorUnequipAction( "Stamina:RageHeadbandUnequip", delegate ( Player player, int slot, int item_type ) {
				if( item_type != this.mod.ItemType<RageHeadbandItem>() ) { return; }
				if( !PlayerItemHelpers.IsAccessorySlot( player, slot ) ) { return; }

				var modplayer = player.GetModPlayer<StaminaPlayer>();
				modplayer.IsWearingRageBandana = false;
			} );
		}

		public override void SetDefaults() {
			this.item.width = RageHeadbandItem.Width;
			this.item.height = RageHeadbandItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
			this.item.rare = 4;
			//this.item.handOnSlot = 2;
			this.item.accessory = true;
		}


		////////////////
		
		public override void AddRecipes() {
			var recipe = new RageHeadbandItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class RageHeadbandItemRecipe : ModRecipe {
		public RageHeadbandItemRecipe( RageHeadbandItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.TinkerersWorkbench );
			this.AddIngredient( ItemID.MagmaStone, 1 );
			this.AddIngredient( ItemID.Hook, 4 );
			this.AddIngredient( ItemID.Silk, 10 );
			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (StaminaMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableRageHeadbands;
		}
	}
}
