﻿namespace MinimalApi.Domain.Examples
{
    public interface IExampleService
    {
        Guid Id { get; } //Using to test scope
        Task<OneOf<None, NotFoundError>> DeleteExample(int id);
        Task<OneOf<Example, NotFoundError>> GetExample(int id);
        Task<OneOf<IEnumerable<Example>, NotFoundError>> GetExamples();
        Task<OneOf<IEnumerable<Example>, NotFoundError>> FindExamples(string? firstName, string? lastName);
        Task<OneOf<Example, ConflictError>> InsertExample(Example example);
        Task<OneOf<Example, NotFoundError, ConflictError>> UpdateExample(Example example);
        Task<OneOf<IEnumerable<Example>, NotFoundError>> SearchChildExample(int parentId, string? firstName, string? lastName);
        Task<OneOf<Example, NotFoundError, ConflictError>> PatchExample(int id, Example example);
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
            _examples.Add(new Example("Leia", "Skywalker", Id: 7, ParentId: 4));
        }
        public async Task<OneOf<IEnumerable<Example>, NotFoundError>> GetExamples()
        {
            await Task.CompletedTask;

            return _examples.Count == 0
                ? Problem.NotFound("No examples found")
                : _examples;
        }
        public async Task<OneOf<Example, NotFoundError>> GetExample(int id)
        {
            await Task.CompletedTask;

            var example = _examples.FirstOrDefault(e => e.Id == id);

            return example is null
                ? Problem.NotFound("Example not found")
                : example;
        }

        public async Task<OneOf<Example, ConflictError>> InsertExample(Example example)
        {
            await Task.CompletedTask;

            var id = _examples.Max(e => e.Id) + 1;

            if (IsDuplicateName(example))
            {
                return Problem.Conflict("An example with the same first and last name already exists");
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
            await Task.CompletedTask;

            var existingExample = _examples.FirstOrDefault(e => e.Id == example.Id);

            if (existingExample is null) return Problem.NotFound("Example not found");

            if (IsDuplicateName(example)) return Problem.Conflict("Example with the same first and last name already exists");

            var index = _examples.IndexOf(existingExample);
            _examples[index] = example;
            return example;
        }

        public async Task<OneOf<Example, NotFoundError, ConflictError>> PatchExample(int id, Example example)
        {
            await Task.CompletedTask;

            var existingExample = _examples.FirstOrDefault(e => e.Id == id);

            if (existingExample is null) return Problem.NotFound("Example not found");

            var update = string.IsNullOrWhiteSpace(example.FirstName)
                ? existingExample
                : existingExample with { FirstName = example.FirstName };

            update = string.IsNullOrWhiteSpace(example.LastName)
                ? existingExample
                : existingExample with { LastName = example.LastName };                

            if (IsDuplicateName(update)) return Problem.Conflict("Example with the same first and last name already exists");

            var index = _examples.IndexOf(existingExample);
            _examples[index] = example;
            return example;
        }

        public async Task<OneOf<None, NotFoundError>> DeleteExample(int id)
        {
            await Task.CompletedTask;

            var exampleToDelete = _examples.FirstOrDefault(e => e.Id == id);

            if (exampleToDelete is null) return Problem.NotFound("Example not found");
            
            var index = _examples.IndexOf(exampleToDelete);
            _examples.RemoveAt(index);
            
            return new None();
        }

        public async Task<OneOf<IEnumerable<Example>, NotFoundError>> FindExamples(string? firstName, string? lastName)
        {
            await Task.CompletedTask;

            var results = _examples.AsEnumerable();

            if (!string.IsNullOrEmpty(firstName))
            {
                results = results.Where(e => e.FirstName == firstName);
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                results = results.Where(e => e.LastName == lastName);
            }
            return results.Any()
                ? results.ToList()
                : Problem.NotFound("No examples found");
        }

        public async Task<OneOf<IEnumerable<Example>, NotFoundError>> SearchChildExample(int parentId, string? firstName, string? lastName)
        {
            await Task.CompletedTask;

            var results = _examples.Where(e => e.ParentId == parentId);

            if (!string.IsNullOrEmpty(firstName))
            {
                results = results.Where(e => e.FirstName == firstName);
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                results = results.Where(e => e.LastName == lastName);
            }

            return results.Any()
                ? results.ToList()
                : Problem.NotFound("No children found");
        }
    }
}