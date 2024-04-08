using RimWorld;
using RimWorld.Planet;
using System.Linq;
using Verse;
using VFECore.Abilities;
using Ability = VFECore.Abilities.Ability;

namespace VPEHemosage
{
    public class Ability_CloseWounds : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            var injuries = pawn.health.hediffSet.hediffs.OfType<Hediff_Injury>().Where(x => x.BleedRate > 0).ToList();
            var hemogen = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
            foreach (var injury in injuries)
            {
                if (hemogen.Value > 0.11f)
                {
                    injury.ageTicks = injury.AgeTicksToStopBleeding;
                    pawn.health.Notify_HediffChanged(injury);
                    hemogen.Value -= 0.01f;
                }
            }
        }
    }
}
