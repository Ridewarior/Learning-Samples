namespace FactoryMethod.Levels;

public static class LevelFactory
{
    public static Level CreateLevel(int levelNum)
    {
        return levelNum switch
        {
            1 => new CaveLevel(),
            2 => new HauntedHouseLevel(),
            _ => throw new ArgumentException("Invalid Level Number", nameof(levelNum))
        };
    }
}