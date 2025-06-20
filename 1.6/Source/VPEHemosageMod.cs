using HarmonyLib;
using Verse;

namespace VPEHemosage;

public class VPEHemosageMod : Mod
{
    public VPEHemosageMod(ModContentPack pack) : base(pack)
    {
        new Harmony("VPEHemosageMod").PatchAll();
    }
}