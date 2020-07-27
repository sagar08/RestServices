using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Models;

namespace JWTPolicyBasedAuthorization.Data.Contracts
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<Product> GetProductById(int id);
        Task<List<Product>> GetProducts();
        Task<List<Product>> GetProducts(Expression<Func<Product, bool>> expression);

        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);


    }
}