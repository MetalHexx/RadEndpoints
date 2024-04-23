namespace RadEndpoints.Cli.Helpers
{
    public record MarkupColor(Color Color, string Name) 
    {
        public override string ToString()
        {
            return Name;
        }
    }
}