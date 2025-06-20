using RimWorld.Planet;
using Verse;
using VEF.Abilities;

namespace VPEHemosage
{
    public class Ability_BloodSpew : Ability_ShootProjectile
    {
        protected override Projectile ShootProjectile(GlobalTargetInfo target)
        {
            var projectile = base.ShootProjectile(target) as BloodSpew;
            projectile.ability = this;
            return projectile;
        }
    }
}
