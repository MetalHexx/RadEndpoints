using MinimalApi.Features.Superheroes.Dtos;
using MinimalApi.Domain.Superhero;


namespace MinimalApi.Features.Superheroes.GetSuperheroes
{
    public class GetSuperheroesMapper : RadMapper<GetSuperheroesResponse, IEnumerable<Superhero>>
    {
        public override GetSuperheroesResponse FromEntity(IEnumerable<Superhero> e) => new()
        {
            Data = e.Select(e => new SuperheroDto
            {
                Id = e.Id,
                Name = e.Name,
                SecretIdentity = e.SecretIdentity,
                Age = e.Age,
                PrimaryAbility = e.PrimaryAbility,
                SecondaryAbility = e.SecondaryAbility
            })
        };
    }
}