namespace MinimalApi.Domain.Superhero
{
    public record Superhero(string Name, string SecretIdentity, int Age, string PrimaryAbility, string SecondaryAbility, int Id = 0);
}