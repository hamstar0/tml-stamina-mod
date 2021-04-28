using System.IO;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.TModLoader.Mods;


namespace Stamina {
	partial class StaminaMod : Mod {
		public static StaminaMod Instance { get; private set; }



		////////////////

		public StaminaConfig Config => ModContent.GetInstance<StaminaConfig>();



		////////////////

		public StaminaMod() {
			StaminaMod.Instance = this;
		}

		////////////////

		public override void Load() {
		}

		public override void Unload() {
			StaminaMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( StaminaAPI ), args );
		}


		////////////////

		public override void AddRecipeGroups() {
			var ninjaItem = new RecipeGroup( delegate () { return Lang.misc[37]+" martial arts master item"; },
				ItemID.Tabi, ItemID.BlackBelt );
			var greaterHook = new RecipeGroup( delegate () { return Lang.misc[37] + " greater biome hook"; },
				ItemID.IlluminantHook, ItemID.WormHook, ItemID.TendonHook, ItemID.ThornHook );

			RecipeGroup.RegisterGroup( "Stamina:MartialArtsMasterItems", ninjaItem );
			RecipeGroup.RegisterGroup( "Stamina:GreaterBiomeHook", greaterHook );
		}
	}
}
