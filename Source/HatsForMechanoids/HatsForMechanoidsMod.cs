using System.IO;
using System.Reflection;
using HarmonyLib;
using HatsForMechanoids.Patches;
using Verse;

namespace HatsForMechanoids
{
    public class HatsForMechanoidsMod : Mod
    {
        public static string ModId => "winggar.hatsformechanoids";

        internal static string VersionDir =>
            Path.Combine(ModLister.GetActiveModWithIdentifier(ModId).RootDir.FullName, "Version.txt");

        public static string CurrentVersion { get; private set; }

        public HatsForMechanoidsMod(ModContentPack content) : base(content)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            if (Prefs.DevMode)
            {
                File.WriteAllText(VersionDir, CurrentVersion);
            }

            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            TargetingParameters_ForForceWear_Validator.Apply(harmony);
            DynamicPawnRenderNodeSetup_Apparel_ProcessApparel.Apply(harmony);
            PawnRenderNode_Apparel_GraphicsFor.Apply(harmony);
        }
    }
}