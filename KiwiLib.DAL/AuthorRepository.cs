using KiwiLib.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiLib.DAL
{
    public class AuthorRepository : IRepository<Author>
    {
        private ConcurrentDictionary<int, Author> authorStore = new ConcurrentDictionary<int, Author>();
        public Task<int> AddAsync(Author item)
        {
            if (item.Id <= 0) // New author which has no assigned Id yet
                item.Id = authorStore.Keys.Max() + 1;

            if (authorStore.ContainsKey(item.Id))
                return Task.FromResult(0);
            return Task.FromResult(authorStore.TryAdd(item.Id, item) ? item.Id : 0);
        }

        public Task<List<Author>> GetAllAsync()
        {
            return Task.FromResult(authorStore.Values.ToList());
        }

        public Task<Author?> GetAsync(int id)
        {
            authorStore.TryGetValue(id, out var author);
            return Task.FromResult(author);
        }
        public Task<Author?> GetAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Task.FromResult<Author?>(null);

            var author = authorStore.FirstOrDefault(c => c.Value.FullName.Equals(name, StringComparison.OrdinalIgnoreCase)).Value;
            return Task.FromResult(author);
        }


        /// <summary>
        /// [Vi] This method is intentionally not implemented, as it is not required for demo purposes.
        /// </summary>
        public Task<bool> UpdateAsync(Author item) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [Vi] This method is intentionally not implemented, as it is not required for demo purposes.
        /// </summary>
        public Task<bool> DeleteAsync(Author item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [Vi] This method is intentionally not implemented, as it is not required for demo purposes.
        /// </summary>
        public Task<List<Author>> SearchAsync(string name)
        {
            throw new NotImplementedException();
        }

    }
}
