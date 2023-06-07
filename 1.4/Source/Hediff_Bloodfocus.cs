using RimWorld;
using Verse;

namespace VPEHemosage
{
    public class Hediff_Bloodfocus : HediffWithComps
    {
        public override bool ShouldRemove => Hemogen == null || Hemogen.Resource.ValuePercent <= 0.1f 
            || pawn.psychicEntropy.CurrentPsyfocus >= 1;
        private Gene_Hemogen Hemogen => pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
        public override void Tick()
        {
            base.Tick();
            if (Find.TickManager.TicksGame % 60 == 0)
            {
                Hemogen.Resource.ValuePercent -= 0.01f;
                pawn.psychicEntropy.OffsetPsyfocusDirectly(0.02f);
            }
        }
    }
}
