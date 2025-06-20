using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VPEHemosage
{
    public class Hediff_BloodRite : HediffWithComps
    {
        private Gene_Hemogen hemogen;
        private HediffStage curStage;
        private StatModifier statModifier;
        public override HediffStage CurStage
        {
            get
            {
                if (curStage is null)
                {
                    curStage = new HediffStage();
                    statModifier = new StatModifier();
                    statModifier.stat = StatDefOf.PsychicSensitivity;
                    curStage.statOffsets = new List<StatModifier> { statModifier };
                }

                if (hemogen is null)
                {
                    hemogen = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
                }
                statModifier.value = hemogen.Value;
                return curStage;
            }
        }
    }
}
