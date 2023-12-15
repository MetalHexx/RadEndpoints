namespace MinimalApi.Domain.Superhero

{
  public interface ISuperheroService
  {
    Task<IEnumerable<Superhero>> GetSuperheroes();
  }

  public class SuperheroService : ISuperheroService
  {

    private List<Superhero> _superheroes = new();

    public SuperheroService()
    {
      _superheroes.Add(new Superhero("Batman", "Bruce Wayne", 32, "Rich", "Martial Arts", 1));
      _superheroes.Add(new Superhero("Superman", "Clark Kent", 32, "Super Strength", "Flight", 2));
      _superheroes.Add(new Superhero("Spiderman", "Peter Parker", 33, "Web-slinging", "Super Strength", 3));
    }

    public async Task<IEnumerable<Superhero>> GetSuperheroes()
    {
      await Task.Delay(1);
      return _superheroes;
    }

  }



}