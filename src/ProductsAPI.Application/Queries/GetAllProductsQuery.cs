using ProductsAPI.Application.DTOs;
using ProductsAPI.Domain.Interfaces;

namespace ProductsAPI.Application.Queries
{
    public class GetAllProductsQuery
    {
        private readonly IProductRepository _repository;

        public GetAllProductsQuery(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> ExecuteAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                IsActive = p.IsActive
            });
        }
    }
}