namespace MinimalApi.Domain.Examples
{
    public interface IExampleService
    {
        Guid Id { get; } //Using to test scope
        Task<OneOf<None, NotFoundError>> DeleteExample(int id);
        Task<Example?> GetExample(int id);
        Task<IEnumerable<Example>> GetExamples();
        Task<IEnumerable<Example>> FindExamples(string? firstName, string? lastName);
        Task<Example?> InsertExample(Example example);
        Task<OneOf<Example, NotFoundError, ConflictError>> UpdateExample(Example example);
        Task<IEnumerable<Example>> SearchChildExample(int parentId, string? firstName, string? lastName);
    }

    public class ExampleService : IExampleService
    {
        private List<Example> _examples = new();
        public Guid Id { get; } = Guid.NewGuid();

        public ExampleService()
        {
            _examples.Add(new Example("James", "Smith", Id: 1));
            _examples.Add(new Example("Maria", "Garcia", Id: 2, ParentId: 1));
            _examples.Add(new Example("David", "Johnson", Id: 3, ParentId: 2));
            _examples.Add(new Example("Anakin", "Skywalker", Id: 4, ParentId: 2));
            _examples.Add(new Example("Michael", "Brown", Id: 5, ParentId: 3));
            _examples.Add(new Example("Luke", "Skywalker", Id: 6, ParentId: 4));
        }
        public async Task<IEnumerable<Example>> GetExamples()
        {
            await Task.Delay(1);
            return _examples;
        }
        public async Task<Example?> GetExample(int id)
        {
            await Task.Delay(1);            
            return _examples.FirstOrDefault(e => e.Id == id);
        }

        public async Task<Example?> InsertExample(Example example)
        {
            await Task.Delay(1);

            var id = _examples.Max(e => e.Id) + 1;

            var isDuplicateName = _examples.Any(record =>
                record.FirstName == example.FirstName
                && record.LastName == example.LastName);

            if (isDuplicateName)
            {
                return null;
            }
            var newExample = example with { Id = id };

            _examples.Add(newExample);

            return newExample;
        }

        private bool IsDuplicateName(Example example) => _examples.Any(record =>
            record.FirstName == example.FirstName
            && record.LastName == example.LastName);

        public async Task<OneOf<Example, NotFoundError, ConflictError>> UpdateExample(Example example)
        {
            await Task.Delay(1);

            var existingExample = _examples.FirstOrDefault(e => e.Id == example.Id);

            if (existingExample is null) return Problem.NotFound("Example not found");

            if (IsDuplicateName(example)) return Problem.Conflict("Example with the same first and last name already exists");

            var index = _examples.IndexOf(existingExample);
            _examples[index] = example;
            return example;
        }

        public async Task<OneOf<None, NotFoundError>> DeleteExample(int id)
        {
            await Task.Delay(1);

            var exampleToDelete = _examples.FirstOrDefault(e => e.Id == id);

            if (exampleToDelete is null) return Problem.NotFound("Example not found");
            
            var index = _examples.IndexOf(exampleToDelete);
            _examples.RemoveAt(index);
            
            return new None();
        }

        public async Task<IEnumerable<Example>> FindExamples(string? firstName, string? lastName)
        {
            await Task.Delay(1);

            var results = _examples.AsEnumerable();

            if (firstName != null)
            {
                results = results.Where(e => e.FirstName == firstName);
            }

            if (lastName != null)
            {
                results = results.Where(e => e.LastName == lastName);
            }
            return results;
        }

        public async Task<IEnumerable<Example>> SearchChildExample(int parentId, string? firstName, string? lastName)
        {
            await Task.Delay(1);

            var results = _examples.Where(e => e.ParentId == parentId);

            if (firstName != null)
            {
                results = results.Where(e => e.FirstName == firstName);
            }

            if (lastName != null)
            {
                results = results.Where(e => e.LastName == lastName);
            }
            return results;
        }
    }
}