using FactoryMethod.Enemies;

namespace FactoryMethod.Levels;

public class HauntedHouseLevel : Level
{
    public override IEnemy CreateEnemy()
    {
        return new Ghost();
    }
}