using HarmonyLib;
using RimWorld;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;

namespace VPEHemosage
{
    [HarmonyPatch(typeof(GeneGizmo_ResourceHemogen), "GizmoOnGUI")]
    public static class GeneGizmo_ResourceHemogen_GizmoOnGUI_Patch
    {
        public static void Postfix(GeneGizmo_ResourceHemogen __instance, Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            float num = Mathf.Repeat(Time.time, 0.85f);
            float num2 = 1f;
            if (num < 0.1f)
            {
                num2 = num / 0.1f;
            }
            else if (num >= 0.25f)
            {
                num2 = 1f - (num - 0.25f) / 0.6f;
            }
            var command_Ability = ((MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow)?.LastMouseoverGizmo as Command_Ability_Psycast;
            if (command_Ability != null && __instance.gene.Max != 0f)
            {
                var extension = command_Ability.ability.def.GetModExtension<AbilityExtension_HemogenCost>();
                if (extension != null  && extension.hemogenCost > float.Epsilon)
                {
                    Rect rect = __instance.barRect.ContractedBy(3f);
                    float width = rect.width;
                    float num3 = __instance.gene.Value / __instance.gene.Max;
                    rect.xMax = rect.xMin + width * num3;
                    float num4 = Mathf.Min(extension.hemogenCost / __instance.gene.Max, 1f);
                    rect.xMin = Mathf.Max(rect.xMin, rect.xMax - width * num4);
                    GUI.color = new Color(1f, 1f, 1f, num2 * 0.7f);
                    GenUI.DrawTextureWithMaterial(rect, GeneGizmo_ResourceHemogen.HemogenCostTex, null);
                    GUI.color = Color.white;
                }
            }
        }
    }
}
