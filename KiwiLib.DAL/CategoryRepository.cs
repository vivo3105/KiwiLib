using KiwiLib.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiLib.DAL
{
    public class CategoryRepository : IRepository<Category>
    {
        private ConcurrentDictionary<int, Category> categoryStore = new ConcurrentDictionary<int, Category>();
        public Task<int> AddAsync(Category item)
        {
            if (item.Id <= 0) // New Category which has no assigned Id yet
                item.Id = categoryStore.Keys.Max() + 1;

            if (categoryStore.ContainsKey(item.Id))
                return Task.FromResult(0);
            return Task.FromResult(categoryStore.TryAdd(item.Id, item) ? item.Id : 0);
        }

        public Task<List<Category>> GetAllAsync()
        {
            return Task.FromResult(categoryStore.Values.ToList());
        }

        public Task<Category?> GetAsync(int id)
        {
            categoryStore.TryGetValue(id, out var cat);
            return Task.FromResult(cat);
        }

        public Task<Category?> GetAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Task.FromResult<Category?>(null);

            var cat = categoryStore.FirstOrDefault(c => c.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Value;
            return Task.FromResult(cat);
        }

        /// <summary>
        /// [Vi] This method is intentionally not implemented, as it is not required for demo purposes.
        /// </summary>
        public Task<bool> UpdateAsync(Category item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [Vi] This method is intentionally not implemented, as it is not required for demo purposes.
        /// </summary>
        public Task<bool> DeleteAsync(Category item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [Vi] This method is intentionally not implemented, as it is not required for demo purposes.
        /// </summary>
        public Task<List<Category>> SearchAsync(string name)
        {
            throw new NotImplementedException();
        }

    }
}
