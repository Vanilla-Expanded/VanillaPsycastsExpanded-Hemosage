using RimWorld;
using System.Linq;
using UnityEngine;
using VanillaPsycastsExpanded.Graphics;
using Verse;

namespace VPEHemosage;

public class SanguineSpear : Projectile_Pointing
{
    public override int DamageAmount
    {
        get
        {
            var pawn = this.launcher as Pawn;
            var hemogen = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
            return (int)(hemogen.Value * 100);
        }
    }

    public override void TickInterval(int delta)
    {
        if (this.Map != null)
        {
            float num = ArcHeightFactor * GenMath.InverseParabola(DistanceCoveredFraction) * delta;
            Vector3 drawPos = DrawPos;
            Vector3 position = drawPos + new Vector3(0f, 0f, 1f) * num;
            Ability_CorpseExplosion.ThrowBloodSmoke(position, this.Map, 0.5f);
        }
        base.TickInterval(delta);
    }
    public override void Impact(Thing hitThing, bool blockedByShield = false)
    {
        foreach (var cell in GenRadial.RadialCellsAround(this.Position, 1.9f, true))
        {
            FilthMaker.TryMakeFilth(cell, this.Map, ThingDefOf.Filth_Blood, 2);
        }
        foreach (var pawn in GenRadial.RadialDistinctThingsAround(this.Position, this.Map, 1.9f, true).OfType<Pawn>())
        {
            if (pawn.IsHemogenic() is false && pawn.RaceProps.Humanlike)
            {
                var hediff = pawn.health.AddHediff(VPEH_DefOf.VPEH_Bloodmist);
                hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = GenDate.TicksPerHour * 2;
            }
        }
        for (var i = 0; i < 4; i++)
        {
            Ability_CorpseExplosion.ThrowBloodSmoke(DrawPos, this.Map, 2f);
        }
        MoteMaker.MakeAttachedOverlay(this, VPEH_DefOf.VPE_PsycastAreaEffect_CorpseExplosion, Vector3.zero);
        MoteMaker.MakeAttachedOverlay(this, VPEH_DefOf.VPEH_CorpseExplosion, Vector3.zero);
        base.Impact(hitThing, blockedByShield);
    }
}