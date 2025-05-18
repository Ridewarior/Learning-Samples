namespace FactoryMethod.Enemies;

public class Ghost : IEnemy
{
    public void Scream()
    {
        Console.WriteLine("The Ghost Screams!");
    }

    public void Attack()
    {
        Console.WriteLine("The Ghost Attacks!");
    }
}