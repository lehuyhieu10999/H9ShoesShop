using H9ShoesShopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Models.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Gets();
        T Create(T t);
        T Get(int id);
        T Edit(T t);
        bool Delete(int id);
    }
}
