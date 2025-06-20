using RimWorld.Planet;
using VEF.Abilities;

namespace VPEHemosage
{
    public class Ability_Bloodstorm : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            pawn.Map.weatherManager.TransitionTo(VPEH_DefOf.VPEH_Bloodstorm);
        }
    }
}
