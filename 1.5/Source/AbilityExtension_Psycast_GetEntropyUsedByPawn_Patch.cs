using HarmonyLib;
using VanillaPsycastsExpanded;
using Verse;

namespace VPEHemosage
{
    [HarmonyPatch(typeof(AbilityExtension_Psycast), "GetEntropyUsedByPawn")]
    public static class AbilityExtension_Psycast_GetEntropyUsedByPawn_Patch
    {
        public static void Postfix(ref float __result, Pawn pawn)
        {
            if (pawn.Map != null && pawn.Map.weatherManager.CurWeatherPerceived == VPEH_DefOf.VPEH_Bloodstorm) 
            {
                var hediff = pawn.health.hediffSet.GetFirstHediff<Hediff_PsycastAbilities>();
                if (hediff != null)
                {
                    if (hediff.unlockedPaths.Contains(VPEH_DefOf.VPEH_Hemosage))
                    {
                        __result = 0;
                    }
                }
            }
        }
    }
}
