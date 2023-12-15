namespace MinimalApi.Features.Superheroes.Dtos
{
    public class SuperheroDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SecretIdentity { get; set; } = string.Empty;
        public int Age { get; set; }
        public string PrimaryAbility { get; set; } = string.Empty;
        public string SecondaryAbility { get; set; } = string.Empty;
    }
}