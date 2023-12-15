using MinimalApi.Domain.Superhero;

namespace MinimalApi.Features.Superheroes.GetSuperheroes
{
    public class GetSuperheroesEndPoint : RadEndpointWithoutRequest<GetSuperheroesResponse, GetSuperheroesMapper>
    {
        private readonly ISuperheroService _superheroService;

        public GetSuperheroesEndPoint(ISuperheroService superheroService)
        {
            _superheroService = superheroService;
        }

        public override void Configure()
        {
            Get("/superheroes")
                .Produces<GetSuperheroesResponse>(StatusCodes.Status200OK)
                .AddSwagger(tag: Constants.SuperheroesTag, desc: "Get all superheroes.");
        }

        public override async Task<IResult> Handle(CancellationToken c)
        {
            Logger.Log(LogLevel.Information, "This is an example log message.");
            var superheroes = await _superheroService.GetSuperheroes();
            Response = Map.FromEntity(superheroes);
            Response.Message = "Superheroes retrieved successfully";

            return Ok(Response);
        }
    }
}