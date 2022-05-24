using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnterAjaTest.API.Repository
{
    public interface IRepository<T>
    {
        public Task<T> CreateAsync(T _object);

        public Task UpdateAsync(T _object);
        public Task<T> ExistAsync(string origin, string dest);

        public Task<List<T>> GetAllAsync();

        public Task<T> GetByIdAsync(int Id);

        public Task DeleteAsync(int id);
    }
}
