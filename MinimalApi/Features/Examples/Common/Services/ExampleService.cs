using MinimalApi.Features.Examples.Common.Models;

namespace MinimalApi.Features.Examples.Common.Services
{
    public interface IExampleService
    {
        Task DeleteExample(int id);
        Task<Example?> GetExample(int id);
        Task<IEnumerable<Example>> GetExamples();
        Task<Example?> InsertExample(Example example);
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
        public async Task<IEnumerable<Example>> GetExamples()
        {
            await Task.Delay(1);
            return _examples.Values.AsEnumerable();
        }
        public async Task<Example?> GetExample(int id)
        {
            await Task.Delay(1);
            _examples.TryGetValue(id, out var example);            
            return example;
        }

        public async Task<Example?> InsertExample(Example example)
        {
            await Task.Delay(1);
            var id = _examples.Keys.Max() + 1;
            var successs = _examples.TryAdd(id, example);

            if (!successs)
            {
                return null;
            }
            return example;
        }

        public async Task<Example?> UpdateExample(Example example)
        {
            await Task.Delay(1);

            if (_examples.ContainsKey(example.Id))
            {
                _examples[example.Id] = example;
                return example with { };
            }
            return null;
        }

        public async Task DeleteExample(int id)
        {
            await Task.Delay(1);

            if (_examples.ContainsKey(id))
            {
                _examples.Remove(id);
            }
        }
    }
}
