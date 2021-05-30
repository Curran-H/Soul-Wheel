using UnityEngine;
using UnityEditor;

namespace DungeonMaker.Editor
{
    public static class Menus
    {
        [MenuItem("Dungeon Maker/Editors/Dungeon Editor", priority = 1)]
        public static void OpenDungeonWindow()
        {
            DungeonWindow.OpenWindow();
        }

        [MenuItem("Dungeon Maker/Editors/Room Editor", priority = 2)]
        public static void OpenRoomWindow()
        {
            RoomWindow.OpenWindow();
        }

        [MenuItem("Dungeon Maker/Help")]
        public static void OpenHelpWindow()
        {
            HelpWindow.OpenWindow();
        }

        [MenuItem("GameObject/Dungeon Maker/Generator", false, 11)]
        public static void CreateGenerator()
        {
            GameObject generator = new GameObject("Generator");
            generator.AddComponent<Generator>();
        }
        
        [MenuItem("GameObject/Dungeon Maker/Framework", false, 12)]
        public static void CreateFramework()
        {
            GameObject framework = new GameObject("Framework");
            framework.AddComponent<Framework>();
        }

        [MenuItem("GameObject/Dungeon Maker/Rule", false, 13)]
        public static void CreateRule()
        {
            GameObject rule = new GameObject("Rule");
            rule.AddComponent<Rule>();
        }
        
        [MenuItem("GameObject/Dungeon Maker/Randomizer", false, 14)]
        public static void CreateRandomizer()
        {
            GameObject randomizer = new GameObject("Randomizer");
            randomizer.AddComponent<Randomizer>();
        }
    }
}