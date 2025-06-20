using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VPEHemosage;

[StaticConstructorOnStartup]
internal class MeditationFocusAutopatcher
{
    static MeditationFocusAutopatcher()
    {
        foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
        {
            if (def.GetCompProperties<CompProperties_DeathrestBindable>() != null)
            {
                def.comps.Add(new CompProperties_MeditationFocus
                {
                    statDef = StatDefOf.MeditationFocusStrength,
                    focusTypes = new List<MeditationFocusDef> { VPEH_DefOf.VPEH_Deathrest },
                    offsets = new List<FocusStrengthOffset>
                    {
                        new FocusStrengthOffset_DeathrestBuilding()
                    }
                });
                def.statBases ??= new List<StatModifier>();
                def.statBases.Add(
                    new StatModifier
                    {
                        stat = StatDefOf.MeditationFocusStrength,
                        value = 0.03f
                    }
                );
            }
        }
    }
}