namespace FactoryMethod.Enemies;

public class Goblin : IEnemy
{
    public void Scream()
    {
        Console.WriteLine("The Goblin Screams!");
    }

    public void Attack()
    {
        Console.WriteLine("The Goblin Attacks!");
    }
}