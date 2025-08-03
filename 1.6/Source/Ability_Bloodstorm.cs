using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Ability = VEF.Abilities.Ability;

namespace VPEHemosage;

public class Ability_Bloodstorm : Ability
{
    public Map affectedMap;
    public WeatherOverlay_Bloodstorm currentOverlay;
    public HashSet<Faction> affectedFactions;

    public override void Cast(params GlobalTargetInfo[] targets)
    {
        base.Cast(targets);
        pawn.Map.weatherManager.TransitionTo(VPEH_DefOf.VPEH_Bloodstorm);
        affectedMap = pawn.Map;
        currentOverlay = VPEH_DefOf.VPEH_Bloodstorm.Worker.overlays.OfType<WeatherOverlay_Bloodstorm>().FirstOrDefault();
        if (currentOverlay != null)
            currentOverlay.currentAbilities[affectedMap] = this;
    }

    public override void TickInterval(int delta) => CheckIfDataIsValid();

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref affectedMap, "affectedMap");
        Scribe_Collections.Look(ref affectedFactions, "affectedFactions", LookMode.Reference);

        switch (Scribe.mode)
        {
            // Check data on saving to make sure we don't save useless stuff
            case LoadSaveMode.Saving:
                CheckIfDataIsValid();
                break;
            // If no map after loading, try to clear affected factions
            case LoadSaveMode.PostLoadInit when affectedMap == null:
                affectedFactions?.Clear();
                break;
            // If we have a map after loading, setup the overlay and set its ability to this one
            case LoadSaveMode.PostLoadInit:
                currentOverlay = VPEH_DefOf.VPEH_Bloodstorm.Worker.overlays.OfType<WeatherOverlay_Bloodstorm>().FirstOrDefault();
                if (currentOverlay != null)
                    currentOverlay.currentAbilities[affectedMap] = this;
                break;
        }
    }

    private void CheckIfDataIsValid()
    {
        if (affectedMap != null)
        {
            // If a different pawn cast the ability, they took over handling it
            if (currentOverlay != null)
            {
                // If the overlay doesn't have the current map, or the ability is for a different pawn - this one is inactive.
                if (!currentOverlay.currentAbilities.TryGetValue(affectedMap, out var ability) || ability != this)
                {
                    currentOverlay = null;
                }
                // If ability is the same, but the weather isn't bloodstorm - it became inactive, clear data
                else if (affectedMap.weatherManager.curWeather != VPEH_DefOf.VPEH_Bloodstorm && affectedMap.weatherManager.CurWeatherPerceived != VPEH_DefOf.VPEH_Bloodstorm)
                {
                    currentOverlay.currentAbilities.Remove(affectedMap);
                    currentOverlay = null;
                }
            }

            // If we have a map with no overlay, the ability is inactive - clear data
            if (currentOverlay == null)
            {
                affectedMap = null;
                affectedFactions?.Clear();
            }
        }
    }
}