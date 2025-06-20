using RimWorld;
using Verse;

namespace VPEHemosage;

public class Hediff_Bloodfocus : HediffWithComps
{
    public override bool ShouldRemove => Hemogen == null || Hemogen.Resource.Value <= 0.1f 
                                                         || pawn.psychicEntropy.CurrentPsyfocus >= 1;
    private Gene_Hemogen Hemogen => pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
    public override void TickInterval(int delta)
    {
        base.TickInterval(delta);
        if (pawn.IsHashIntervalTick(60, delta))
        {
            Hemogen.Resource.Value -= 0.02f;
            pawn.psychicEntropy.OffsetPsyfocusDirectly(0.01f);
        }
    }
}