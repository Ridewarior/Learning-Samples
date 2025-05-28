namespace Minimap.Api;

public class PeopleService
{
    private readonly List<Person> _people =
    [
        new("Sam Smith"),
        new("John Jenks"),
        new("Abreham Austins")
    ];

    public IEnumerable<Person> Search(string searchTerm)
    {
        return _people.Where(person =>
            person.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}

public record Person(string FullName);