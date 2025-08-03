using RimWorld;
using System.Collections.Generic;
using System.Linq;
using VEF.CacheClearing;
using Verse;

namespace VPEHemosage;

public class WeatherOverlay_Bloodstorm : WeatherOverlayDualPanner
{
    public Dictionary<Map, Ability_Bloodstorm> currentAbilities = [];

    public static Dictionary<Map, CachedResult<List<IntVec3>>> unroofedCells = new();

    static WeatherOverlay_Bloodstorm()
    {
        ClearCaches.clearCacheTypes.Add(typeof(WeatherOverlay_Bloodstorm));
    }

    public override void TickOverlay(Map map, float lerpFactor)
    {
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
                if (pawn.genes != null && pawn.Position.Roofed(map) is false)
                {
                    var gene = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
                    if (gene != null)
                    {
                        gene.Value += 0.01f / 60f;
                        pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtMaker.MakeThought(VPEH_DefOf.VPEH_SoakedInBlood, 1));
                    }
                    else
                    {
                        if (pawn.health.hediffSet.GetFirstHediffOfDef(VPEH_DefOf.VPEH_Bloodmist) is null)
                        {
                            pawn.health.AddHediff(VPEH_DefOf.VPEH_Bloodmist);
                        }
                        if (currentAbilities.TryGetValue(map, out var ability))
                            AffectGoodwill(pawn.HomeFaction, pawn, ability);
                        pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtMaker.MakeThought(VPEH_DefOf.VPEH_SoakedInBlood, 0));
                    }
                }
            }

            if (map.weatherManager.curWeatherAge >= 6000)
            {
                map.weatherManager.TransitionTo(WeatherDefOf.Clear);
                if (currentAbilities.TryGetValue(map, out var ability))
                {
                    ability.currentOverlay = null;
                    currentAbilities.Remove(map);
                }
            }
        }
    }

    private void AffectGoodwill(Faction faction, Pawn p, Ability_Bloodstorm ability)
    {
        ability.affectedFactions ??= [];
        if (faction is { IsPlayer: false } && !faction.HostileTo(Faction.OfPlayer) && p is not { IsSlaveOfColony: true } && ability.affectedFactions.Add(faction))
        {
            Faction.OfPlayer.TryAffectGoodwillWith(faction, -35, canSendMessage: true, canSendHostilityLetter: true, HistoryEventDefOf.UsedHarmfulAbility);
        }
    }
}