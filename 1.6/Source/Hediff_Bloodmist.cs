using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VPEHemosage;

public class Hediff_Bloodmist : HediffWithComps
{
    public int nextVomitTick;
    public int nextFleckSpawnTick;
    private static readonly IntRange VomitTickInterval = new IntRange(120, 180);
    private static readonly IntRange FleckTickInterval = new IntRange(5, 10);
    private static readonly Vector3 BreathOffset = new Vector3(0f, 0f, -0.04f);
    public override bool ShouldRemove => base.ShouldRemove || pawn.IsHemogenic();

    public override void PostAdd(DamageInfo? dinfo)
    {
        base.PostAdd(dinfo);

        nextVomitTick = Find.TickManager.TicksGame + VomitTickInterval.RandomInRange;
        nextFleckSpawnTick  = Find.TickManager.TicksGame + FleckTickInterval.RandomInRange;
    }

    public override void TickInterval(int delta)
    {
        base.TickInterval(delta);

        if (Find.TickManager.TicksGame >= nextVomitTick)
        {
            nextVomitTick = Find.TickManager.TicksGame + VomitTickInterval.RandomInRange;
            if (pawn.Map != null)
            {
                Ability_CorpseExplosion.ThrowBloodSmoke(pawn.DrawPos, pawn.Map, 2f);
                if (pawn.CurJobDef != JobDefOf.Vomit)
                {
                    pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, resumeCurJobAfterwards: true);
                }
            }
        }
        if (Find.TickManager.TicksGame >= nextFleckSpawnTick && pawn.Map != null)
        {
            ThrowFleck(pawn.Drawer.DrawPos + pawn.Drawer.renderer.BaseHeadOffsetAt(pawn.Rotation) + pawn.Rotation.FacingCell.ToVector3() * 0.21f + BreathOffset
                , pawn.Map, pawn.Rotation.AsAngle, pawn.Drawer.tweener.LastTickTweenedVelocity);
            nextFleckSpawnTick = Find.TickManager.TicksGame + FleckTickInterval.RandomInRange;
        }
    }

    public void ThrowFleck(Vector3 loc, Map map, float throwAngle, Vector3 inheritVelocity)
    {
        if (loc.ToIntVec3().ShouldSpawnMotesAt(map))
        {
            FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + new Vector3(Rand.Range(-0.005f, 0.005f), 
                0f, Rand.Range(-0.005f, 0.005f)), map, FleckDefOf.AirPuff, Rand.Range(0.6f, 0.7f));
            dataStatic.rotationRate = Rand.RangeInclusive(-240, 240);
            dataStatic.velocityAngle = throwAngle + (float)Rand.Range(-10, 10);
            dataStatic.velocitySpeed = Rand.Range(0.1f, 0.8f);
            dataStatic.velocity = inheritVelocity * 0.5f;
            dataStatic.instanceColor = Color.red;
            dataStatic.scale = 0.9f;
            map.flecks.CreateFleck(dataStatic);
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref nextVomitTick, "nextVomitTick");
        Scribe_Values.Look(ref nextFleckSpawnTick, "nextFleckSpawnTick");
    }
}