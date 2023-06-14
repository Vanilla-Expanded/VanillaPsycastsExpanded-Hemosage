using RimWorld;
using Verse;
using VFECore.Abilities;

namespace VPEHemosage
{
    public class BloodShard : AbilityProjectile
    {
        public override void Tick()
        {
            base.Tick();
            if (Map != null && Position.IsValid)
            {
                if (Find.TickManager.TicksGame % 4 == 0)
                {
                    FilthMaker.TryMakeFilth(Position, Map, ThingDefOf.Filth_Blood);
                }
            }
        }
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            FilthMaker.TryMakeFilth(Position, Map, ThingDefOf.Filth_Blood);
            base.Impact(hitThing, blockedByShield);
        }
    }
}
