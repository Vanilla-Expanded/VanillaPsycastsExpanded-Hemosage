using RimWorld;
using System.Linq;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;

namespace VPEHemosage
{
    public class StatPart_NearbyDeathrest : StatPart_Focus
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!this.ApplyOn(req) || req.Thing.Map == null) return;
            val += Mathf.Min(0.6f, val);
        }
        public override string ExplanationPart(StatRequest req)
        {
            if (!this.ApplyOn(req) || req.Thing.Map == null) return "";

            var nearbyDeathrestBuildings = GenRadialCached.RadialDistinctThingsAround(req.Thing.Position, req.Thing.Map, 10f, true)
                .Where(x => x.TryGetComp<CompDeathrestBindable>() != null).ToList();
            var bonus = Mathf.Min(0.6f, nearbyDeathrestBuildings.Count * 0.03f);

            return "VPEH.DeathrestBuildingsNearby".Translate(nearbyDeathrestBuildings.Count) + ": " +
                   this.parentStat.Worker.ValueToString(bonus, true, ToStringNumberSense.Offset);
        }
    }
}
