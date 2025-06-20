using VEF.Abilities;
using Verse;

namespace VPEHemosage
{
    public class Ability_Bloodfocus : Ability
    {
        public override Gizmo GetGizmo()
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(VPEH_DefOf.VPEH_Bloodfocus);
            if (hediff != null)
                return new Command_Action
                {
                    defaultLabel = "VPEH.CancelBloodfocus".Translate(),
                    defaultDesc = "VPEH.CancelBloodfocusDesc".Translate(),
                    icon = def.icon,
                    action = delegate { pawn.health.RemoveHediff(hediff); },
                    Order = 10f + (def.requiredHediff?.hediffDef?.index ?? 0) + (def.requiredHediff?.minimumLevel ?? 0)
                };
            return base.GetGizmo();
        }
    }
}
