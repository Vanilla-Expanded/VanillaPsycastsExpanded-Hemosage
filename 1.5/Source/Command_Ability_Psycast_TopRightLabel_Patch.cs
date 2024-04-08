using HarmonyLib;
using RimWorld;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;

namespace VPEHemosage
{
    [HarmonyPatch(typeof(Command_Ability_Psycast), "TopRightLabel", MethodType.Getter)]
    public static class Command_Ability_Psycast_TopRightLabel_Patch
    {
        public static void Postfix(ref string __result, Command_Ability_Psycast __instance)
        {
            if (__result != null)
            {
                var extension = __instance.ability.def.GetModExtension<AbilityExtension_HemogenCost>();
                if (extension != null && extension.hemogenCost > float.Epsilon)
                {
                    if (__result.Length > 1)
                    {
                        __result += "\n" + "VPEH.HemogenLetter".Translate() + ": " + Mathf.RoundToInt(extension.hemogenCost * 100f);
                    }
                    else
                    {
                        __result += "VPEH.HemogenLetter".Translate() + ": " + Mathf.RoundToInt(extension.hemogenCost * 100f);
                    }
                }
            }
        }
    }
}
