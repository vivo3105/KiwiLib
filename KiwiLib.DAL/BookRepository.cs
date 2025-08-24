using KiwiLib.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiLib.DAL
{
    public class BookRepository : IRepository<Book>
    {
        private ConcurrentDictionary<int, Book> _bookStore = new ConcurrentDictionary<int, Book>();
        public Task<int> AddAsync(Book item)
        {
            if (item.Id <= 0) // New Category which has no assigned Id yet
            {
                if (_bookStore.Count == 0)
                    item.Id = 1;
                else
                    item.Id = _bookStore.Keys.Max() + 1;
            }                

            if (_bookStore.ContainsKey(item.Id))
                return Task.FromResult(0);
            return Task.FromResult(_bookStore.TryAdd(item.Id, item) ? item.Id : 0);
        }

        public Task<bool> UpdateAsync(Book item)
        {
            return Task.FromResult(_bookStore.TryUpdate(item.Id, item, _bookStore[item.Id]));
        }

        public Task<bool> DeleteAsync(Book item)
        {
            return Task.FromResult(_bookStore.TryRemove(item.Id, out _));
        }

        public Task<List<Book>> GetAllAsync()
        {
            return Task.FromResult(_bookStore.Values.ToList());
        }

        public Task<Book?> GetAsync(int id)
        {
            _bookStore.TryGetValue(id, out var book);
            return Task.FromResult(book);
        }

        public Task<List<Book>> SearchAsync(string name)
        {
            throw new NotImplementedException();
        }

    }
}
