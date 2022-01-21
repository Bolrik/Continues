using Levels;

namespace GameManagement
{
    public static class PlayerPrefKeys
    {
        public static string MouseSensitivity { get; } = "MOUSE_SENSITIVITY";

        public static string GetLevelTimeKey(LevelData levelData)
        {
            return $"LL_{levelData.Chapter:D2}_{levelData.Level:D3}_TIME";
        }
    }
}