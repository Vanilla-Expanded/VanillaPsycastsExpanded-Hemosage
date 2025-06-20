using RimWorld;
using VanillaPsycastsExpanded;
using Verse;

namespace VPEHemosage;

public class Bloodmist : MoteBetween
{
    public Ability_Hemodrain ability;
    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        var hemogen = ability.pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
        hemogen.Value += 0.2f;
        base.Destroy(mode);
    }
}