using BepInEx.Logging;
using ExamplePlugin;
using R2API;
using Rewired;
using RoR2;
using UnityEngine;

namespace JDContent.Scripts.Difficulty
{
    public class StormIncursion
    {
        public static DifficultyDef ModDiffDef;

        public static DifficultyIndex ModDifIndex;

        public void AddDifficulty()
        {
            ModDiffDef = new(4.25f, "DIFFICULTY_NAME", "DIFFICULTY_ICON", "DIFFICULTY_DESC", new Color32(200, 10, 50, 255), "ed", true);
            ModDiffDef.iconSprite = null;
            ModDiffDef.foundIconSprite = true;
            ModDifIndex = DifficultyAPI.AddDifficulty(ModDiffDef);

            Run.onRunStartGlobal += (Run run) =>
            {
                if (run.selectedDifficulty == ModDifIndex)
                {
                    TeleporterDirectorBonus.ExtraCredits = true;
                } else
                {
                    TeleporterDirectorBonus.ExtraCredits = false;
                }
            };
        }

        public void FillTokens()
        {
            LanguageAPI.Add("DIFFICULTY_NAME", "Storm Incursion");
            LanguageAPI.Add("DIFFICULTY_DESC", "You will fall under storm's pressure.\n\n" +
            "<style=cStack>>Difficulty Scaling: </style><style=cIsHealth>+125%</style>\n" +
            "<style=cStack>>Teleporter: </style><style=cIsHealth>much more enemies after killing boss.</style>\n" +
            "<style=cStack>>Time: </style><style=cIsHealth>spending more than 4 minutes on a single stage permanently improves enemies' attack speed and skill cooldowns.</style>\n");
        }
    }

}