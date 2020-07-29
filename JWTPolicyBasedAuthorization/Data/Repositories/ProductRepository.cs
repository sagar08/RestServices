using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTPolicyBasedAuthorization.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext.Products
                        .Include(x => x.Brand)
                        .Include(c => c.Category)
                        .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _dbContext.Products
                        .Include(x => x.Brand)
                        .Include(c => c.Category)
                        .ToListAsync();
        }

        public async Task<List<Product>> GetProducts(Expression<Func<Product, bool>> expression)
        {
            return await FindByCondition(expression).ToListAsync();
        }

        public void CreateProduct(Product product)
        {
            Add(product);
        }

        public void UpdateProduct(Product product)
        {
            Update(product);
        }
        public void DeleteProduct(Product product)
        {
            Remove(product);
        }

    }
}