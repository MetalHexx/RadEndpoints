using MinimalApi.Features.Examples.Common.Models;

namespace MinimalApi.Features.Examples.Common.Services
{
    public interface IExampleService
    {
        Task DeleteExample(int id);
        Task<Example?> GetExample(int id);
        Task<IEnumerable<Example>> GetExamples();
        Task InsertExample(Example example);
        Task<Example?> UpdateExample(Example example);
    }

    public class ExampleService : IExampleService
    {
        private Dictionary<int, Example> _examples = new();

        public ExampleService()
        {
            _examples.Add(1, new Example("James", "Smith", 1));
            _examples.Add(2, new Example("Maria", "Garcia", 2));
            _examples.Add(3, new Example("David", "Johnson", 3));
            _examples.Add(4, new Example("Emma", "Williams", 4));
            _examples.Add(5, new Example("Michael", "Brown", 5));
        }
        public Task<IEnumerable<Example>> GetExamples()
        {
            return Task.FromResult(_examples.Values.AsEnumerable());
        }
        public Task<Example?> GetExample(int id)
        {
            _examples.TryGetValue(id, out var example);
            return Task.FromResult(example);
        }

        public Task InsertExample(Example example)
        {
            var id = _examples.Keys.Max() + 1;
            _examples.Add(id, example);
            return Task.CompletedTask;
        }

        public Task<Example?> UpdateExample(Example example)
        {
            if (_examples.ContainsKey(example.Id))
            {
                _examples[example.Id] = example;
                return Task.FromResult(example with { });
            }
            return Task.FromResult((Example?)null);
        }

        public Task DeleteExample(int id)
        {
            if (_examples.ContainsKey(id))
            {
                _examples.Remove(id);
            }
            return Task.CompletedTask;
        }
    }
}
