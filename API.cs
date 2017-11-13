namespace Stamina {
	public static class StaminaAPI {
		public static StaminaConfigData GetModSettings() {
			if( StaminaMod.Instance == null ) { return null; }
			return StaminaMod.Instance.Config.Data;
		}
	}
}
