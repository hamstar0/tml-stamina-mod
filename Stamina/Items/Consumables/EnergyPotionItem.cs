using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina.Items.Consumables {
	class EnergyPotionItem : ModItem {
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Energy Potion" );
			this.Tooltip.SetDefault( "Gives stamina regeneration" +'\n'
				+ "'It's got electrolytes!'" );
		}

		public override void SetDefaults() {
			var mymod = (StaminaMod)this.mod;

			this.item.UseSound = SoundID.Item3;
			this.item.useStyle = 2;
			this.item.useTurn = true;
			this.item.useAnimation = 17;
			this.item.useTime = 17;
			this.item.maxStack = 30;
			this.item.consumable = true;
			this.item.width = 14;
			this.item.height = 24;
			//item.potion = true;
			this.item.buffType = this.mod.BuffType("EnergizedBuff");
			this.item.buffTime = mymod.Config.EnergyPotionDuration;
			this.item.value = Item.buyPrice( 0, 0, 10, 0 );
			this.item.rare = 1;
		}
		
		public override bool UseItem( Player player ) {
			if( player.itemAnimation > 0 && player.itemTime == 0 ) {
				player.itemTime = this.item.useTime;
				return true;
			}
			return base.UseItem( player );
		}

		public override bool ConsumeItem( Player player ) {
			var mymod = (StaminaMod)this.mod;
			StaminaPlayer info = player.GetModPlayer<StaminaPlayer>();
			info.Logic.AddStamina( player, 100 );

			player.AddBuff( this.mod.BuffType("EnergizedBuff"), mymod.Config.EnergyPotionDuration );

			return base.ConsumeItem( player );
		}

		public override void AddRecipes() {
			ModRecipe recipe = new EnergyPotionItemRecipe( this );
			recipe.AddRecipe();
		}
	}




	class EnergyPotionItemRecipe : ModRecipe {
		public EnergyPotionItemRecipe( EnergyPotionItem moditem ) : base( moditem.mod ) {
			this.AddIngredient( ItemID.BottledHoney, 1 );
			this.AddIngredient( ItemID.SwiftnessPotion, 1 );
			//this.AddIngredient( ItemID.JungleGrassSeeds, 1 );
			//this.AddIngredient( ItemID.PinkGel, 1 );
			//this.AddIngredient( ItemID.JungleSpore, 1 );
			this.AddIngredient( ItemID.Daybloom, 1 );
			//this.AddIngredient( ItemID.Deathweed, 1 );
			this.AddIngredient( ItemID.FallenStar, 1 );
			//this.AddIngredient( ItemID.Firefly, 1 );
			//this.AddIngredient( ItemID.MushroomGrassSeeds, 1 );
			this.AddIngredient( ItemID.PurificationPowder, 1 );

			this.AddTile( TileID.Bottles );

			this.SetResult(moditem);
		}

		public override bool RecipeAvailable() {
			var mymod = (StaminaMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableEnergyDrinks;
		}
	}
}
