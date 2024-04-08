using RimWorld;
using Verse;

namespace VPEHemosage
{
    public class FocusStrengthOffset_DeathrestBuilding : FocusStrengthOffset
    {
        public override string GetExplanation(Thing parent)
        {
            if (CanApply(parent))
            {
                return "VPEH.DeathrestBuilding".Translate() + ": " + GetOffset(parent).ToStringWithSign("0%");
            }
            return GetExplanationAbstract();
        }

        public override string GetExplanationAbstract(ThingDef def = null)
        {
            return "VPEH.DeathrestBuilding".Translate() + ": " + offset.ToStringWithSign("0%");
        }

        public override float GetOffset(Thing parent, Pawn user = null)
        {
            return offset;
        }

        public override bool CanApply(Thing parent, Pawn user = null)
        {
            if (user != null)
            {
                return user.Position.DistanceTo(parent.Position) <= 10;
            }
            return false;
        }
    }
}
