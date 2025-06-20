using RimWorld;
using RimWorld.Planet;
using System.Linq;
using UnityEngine;
using Verse;
using Ability = VEF.Abilities.Ability;

namespace VPEHemosage
{
    public class Ability_CorpseExplosion : Ability
    {
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            var corpse = target.Thing as Corpse;
            if (corpse is null)
            {
                if (showMessages)
                {
                    Messages.Message("VPE.MustBeCorpse".Translate(), corpse, MessageTypeDefOf.CautionInput);
                }
                return false;
            }
            else if (!corpse.InnerPawn.RaceProps.Humanlike)
            {
                if (showMessages)
                {
                    Messages.Message("VPE.MustBeCorpseHumanlike".Translate(), corpse, MessageTypeDefOf.CautionInput);
                }
                return false;
            }
            else if (corpse.GetRotStage() == RotStage.Dessicated)
            {
                if (showMessages)
                {
                    Messages.Message("VPEH.MustBeNotDessicated".Translate(), corpse, MessageTypeDefOf.CautionInput);
                }
                return false;
            }
            return base.ValidateTarget(target, showMessages);
        }

        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            var target = targets[0].Thing as Corpse;
            foreach (var cell in GenRadial.RadialCellsAround(target.Position, this.def.radius, true))
            {
                FilthMaker.TryMakeFilth(cell, target.Map, ThingDefOf.Filth_Blood, 2);
            }
            foreach (var pawn in GenRadial.RadialDistinctThingsAround(target.Position, target.Map, this.def.radius, true).OfType<Pawn>())
            {
                if (pawn.IsHemogenic() is false && pawn.RaceProps.Humanlike)
                {
                    pawn.health.AddHediff(VPEH_DefOf.VPEH_Bloodmist);
                }
            }
            for (var i = 0; i < 4; i++)
            {
                ThrowBloodSmoke(target.DrawPos, pawn.Map, 2f);
            }
            MoteMaker.MakeAttachedOverlay(target, VPEH_DefOf.VPE_PsycastAreaEffect_CorpseExplosion, Vector3.zero);
            MoteMaker.MakeAttachedOverlay(target, VPEH_DefOf.VPEH_CorpseExplosion, Vector3.zero);
            target.Destroy();
        }

        public static void ThrowBloodSmoke(Vector3 loc, Map map, float size)
        {
            FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Smoke, Rand.Range(1.5f, 2.5f) * size);
            dataStatic.rotationRate = Rand.Range(-30f, 30f);
            dataStatic.velocityAngle = Rand.Range(0, 360);
            dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
            dataStatic.instanceColor = Color.red;
            map.flecks.CreateFleck(dataStatic);
        }
    }


}
