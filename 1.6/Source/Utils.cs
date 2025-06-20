using RimWorld;
using Verse;

namespace VPEHemosage
{
    public static class Utils
    {
        public static bool IsHemogenic(this Pawn pawn) => pawn.genes?.GetFirstGeneOfType<Gene_Hemogen>() != null;
    }
}
