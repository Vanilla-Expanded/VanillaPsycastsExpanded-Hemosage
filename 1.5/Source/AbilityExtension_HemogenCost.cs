﻿using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using VFECore.Abilities;
using Ability = VFECore.Abilities.Ability;

namespace VPEHemosage
{
    public class AbilityExtension_HemogenCost : AbilityExtension_AbilityMod
    {
        public float hemogenCost;
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            var hemogen = ability.pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
            hemogen.Resource.Value -= hemogenCost;
        }

        public override string GetDescription(Ability ability)
        {
            return (("AbilityHemogenCost".Translate() + ": ") + Mathf.RoundToInt(hemogenCost * 100f)).ToString().Colorize(Color.cyan);
        }
    }
}
