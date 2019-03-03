using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Stamina.Items.Consumables {
	class AthletePotionItem : ModItem {
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Athlete Potion" );
			this.Tooltip.SetDefault( "Add +100 maximum stamina"
				+'\n'+ "'Now legal in all sporting events!'" );
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
			this.item.buffType = this.mod.BuffType("AthleteBuff");
			this.item.buffTime = mymod.Config.AthletePotionDuration;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );
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

			player.AddBuff( this.mod.BuffType( "AthleteBuff" ), mymod.Config.AthletePotionDuration );

			return base.ConsumeItem( player );
		}

		public override void AddRecipes() {
			ModRecipe recipe = new AthletePotionItemRecipe( this );
			recipe.AddRecipe();
		}
	}




	class AthletePotionItemRecipe : ModRecipe {
		public AthletePotionItemRecipe( AthletePotionItem moditem ) : base( moditem.mod ) {
			this.AddIngredient( ItemID.BottledWater, 1 );
			this.AddIngredient( ItemID.Bone, 1 );
			this.AddRecipeGroup( "HamstarHelpers:StrangePlants", 1 );

			this.AddTile( TileID.Bottles );

			this.SetResult( moditem );
		}

		public override bool RecipeAvailable() {
			var mymod = (StaminaMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableAthletePotions;
		}
	}
}
