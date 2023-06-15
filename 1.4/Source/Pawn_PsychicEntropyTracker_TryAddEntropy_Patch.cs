using HarmonyLib;
using RimWorld;
using VanillaPsycastsExpanded;

namespace VPEHemosage
{
    [HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), "TryAddEntropy")]
    public static class Pawn_PsychicEntropyTracker_TryAddEntropy_Patch
    {
        public static bool Prefix(Pawn_PsychicEntropyTracker __instance)
        {
            var pawn = __instance.pawn;
            if (pawn.Map != null && pawn.Map.weatherManager.CurWeatherPerceived == VPEH_DefOf.VPEH_Bloodstorm)
            {
                var hediff = pawn.health.hediffSet.GetFirstHediff<Hediff_PsycastAbilities>();
                if (hediff != null)
                {
                    if (hediff.unlockedPaths.Contains(VPEH_DefOf.VPEH_Hemosage))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
