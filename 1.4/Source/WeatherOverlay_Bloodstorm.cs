using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VPEHemosage
{
    public class WeatherOverlay_Bloodstorm : SkyOverlay
    {
        private HashSet<Faction> affectedFactions;

        public static Dictionary<Map, CachedResult<List<IntVec3>>> unroofedCells = new();
        public override void TickOverlay(Map map)
        {
            base.TickOverlay(map);
            if (map.weatherManager.CurWeatherPerceived == VPEH_DefOf.VPEH_Bloodstorm)
            {
                if (Rand.Chance(0.7f))
                {
                    if (!unroofedCells.TryGetValue(map, out var cache))
                    {
                        unroofedCells[map] = cache = new CachedResult<List<IntVec3>>(map.AllCells.Where(x => x.Roofed(map) is false).ToList(), 2500);
                    }
                    else if (cache.CacheExpired)
                    {
                        cache.Result = map.AllCells.Where(x => x.Roofed(map) is false).ToList();
                    }
                    if (cache.Result.TryRandomElement(out var cell) && cell.Roofed(map) is false)
                    {
                        FilthMaker.TryMakeFilth(cell, map, ThingDefOf.Filth_Blood);
                    }
                }

                foreach (var pawn in map.mapPawns.AllPawnsSpawned)
                {
                    if (pawn.Position.Roofed(map) is false && pawn.genes != null)
                    {
                        var gene = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
                        if (gene != null)
                        {
                            gene.ValuePercent += 0.01f / 60f;
                            pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtMaker.MakeThought(VPEH_DefOf.VPEH_SoakedInBlood, 1));
                        }
                        else
                        {
                            pawn.health.AddHediff(VPEH_DefOf.VPEH_Bloodmist);
                            AffectGoodwill(pawn.HomeFaction, pawn);
                            pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtMaker.MakeThought(VPEH_DefOf.VPEH_SoakedInBlood, 0));
                        }
                    }
                }

                if (map.weatherManager.curWeatherAge >= 6000)
                {
                    map.weatherManager.TransitionTo(WeatherDefOf.Clear);
                }
            }
        }

        private void AffectGoodwill(Faction faction, Pawn p)
        {
            affectedFactions ??= new HashSet<Faction>();
            if (faction != null && !faction.IsPlayer && !faction.HostileTo(Faction.OfPlayer) && (p == null || !p.IsSlaveOfColony) 
                && (!affectedFactions.Contains(faction)))
            {
                affectedFactions.Add(faction);
                Faction.OfPlayer.TryAffectGoodwillWith(faction, -35, canSendMessage: true, canSendHostilityLetter: true, HistoryEventDefOf.UsedHarmfulAbility);
            }
        }
    }
}
