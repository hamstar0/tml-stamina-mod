using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Stamina.Items {
	public class EnergyPotionItem : ModItem {
		public override void SetDefaults() {
			this.item.name = "Energy Potion";
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
			this.item.buffTime = 30 * 60;
			this.item.toolTip = "It's got electrolytes!";
			this.item.value = 1000;
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
			StaminaPlayer info = player.GetModPlayer<StaminaPlayer>( this.mod );
			info.AddStamina( 1000 );

			player.AddBuff( this.mod.BuffType("EnergizedBuff"), 30 * 60 );

			return base.ConsumeItem( player );
		}

		public override void AddRecipes() {
			ModRecipe recipe = new EnergyPotionRecipe( this );
			recipe.AddRecipe();
		}
	}



	class EnergyPotionRecipe : ModRecipe {
		public EnergyPotionRecipe( EnergyPotionItem moditem ) : base( moditem.mod ) {
			this.AddIngredient( "Bottled Honey", 1 );
			this.AddIngredient( "Swiftness Potion", 1 );
			//this.AddIngredient( "Jungle Grass Seeds", 1 );
			//this.AddIngredient( "Pink Gel", 1 );
			//this.AddIngredient( "Jungle Spore", 1 );
			this.AddIngredient( "Daybloom", 1 );
			//this.AddIngredient( "Deathweed", 1 );
			this.AddIngredient( "Fallen Star", 1 );
			//this.AddIngredient( "Firefly", 1 );
			//this.AddIngredient( "Mushroom Grass Seeds", 1 );
			this.AddIngredient( "Purification Powder", 1 );

			this.AddTile(13);	// Bottle

			this.SetResult(moditem);
		}

		public override bool RecipeAvailable() {
			var mymod = (StaminaMod)this.mod;
			return mymod.Config.Data.CraftableEnergyDrinks;
		}
	}
}
