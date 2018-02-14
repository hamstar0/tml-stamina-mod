using Stamina.Logic;
using System;
using Terraria;


namespace Stamina {
	public static partial class StaminaAPI {
		internal static object Call( string call_type, params object[] args ) {
			Player player;
			float amount;
			StaminaChangeHook sta_change_hook;
			FatigueChangeHook fat_change_hook;


			switch( call_type ) {
			case "GetModSettings":
				return StaminaAPI.GetModSettings();

			case "SaveModSettingsChanges":
				StaminaAPI.SaveModSettingsChanges();
				return null;

			case "IsShowingDrainingFX":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }
				
				return StaminaAPI.IsShowingDrainingFX( player );

			case "GetStamina":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }
				
				return StaminaAPI.GetStamina( player );

			case "GetMaxStamina":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return StaminaAPI.GetMaxStamina( player );

			case "GetMaxStamina2":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return StaminaAPI.GetMaxStamina2( player );

			case "GetFatigue":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return StaminaAPI.GetFatigue( player );

			case "GetExerciseThreshold":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return StaminaAPI.GetExerciseThreshold( player );

			case "IsExercising":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return StaminaAPI.IsExercising( player );

			case "DrainStamina":
				if( args.Length < 2 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				if( !( args[1] is float ) ) { throw new Exception( "Invalid parameter drain_type for API call " + call_type ); }
				int drain_type = (int)args[1];

				StaminaAPI.DrainStamina( player, (StaminaDrainTypes)drain_type );
				return null;

			case "DrainStaminaViaCustomItem":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				if( !( args[1] is string ) ) { throw new Exception( "Invalid parameter item_name for API call " + call_type ); }
				string item_name = (string)args[1];

				StaminaAPI.DrainStaminaViaCustomItem( player, item_name );
				return null;

			case "DrainCustomStaminaAmount":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				if( !( args[1] is float ) ) { throw new Exception( "Invalid parameter amount for API call " + call_type ); }
				amount = (float)args[1];

				if( !( args[2] is string ) ) { throw new Exception( "Invalid parameter custom_type for API call " + call_type ); }
				string custom_type = (string)args[2];

				StaminaAPI.DrainCustomStaminaAmount( player, amount, custom_type );
				return null;

			case "AddStamina":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				if( !( args[1] is float ) ) { throw new Exception( "Invalid parameter amount for API call " + call_type ); }
				amount = (float)args[1];

				StaminaAPI.AddStamina( player, amount );
				return null;

			case "AddFatigue":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				if( !( args[1] is float ) ) { throw new Exception( "Invalid parameter amount for API call " + call_type ); }
				amount = (float)args[1];

				StaminaAPI.AddFatigue( player, amount );
				return null;

			case "OnStaminaChange":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				sta_change_hook = args[1] as StaminaChangeHook;
				if( sta_change_hook == null ) { throw new Exception( "Invalid parameter sta_change_hook for API call " + call_type ); }

				StaminaAPI.OnStaminaChange( player, sta_change_hook );
				return null;

			case "OnFatigueChange":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }
				
				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				fat_change_hook = args[1] as FatigueChangeHook;
				if( fat_change_hook == null ) { throw new Exception( "Invalid parameter fat_change_hook for API call " + call_type ); }

				StaminaAPI.OnFatigueChange( player, fat_change_hook );
				return null;

			}
			throw new Exception( "No such api call " + call_type );
		}
	}
}
