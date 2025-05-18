using FactoryMethod.Enemies;

namespace FactoryMethod.Levels;

public class CaveLevel : Level
{
    public override IEnemy CreateEnemy()
    {
        return new Goblin();
    }
}