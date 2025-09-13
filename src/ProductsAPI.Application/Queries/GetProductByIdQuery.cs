using ProductsAPI.Application.DTOs;
using ProductsAPI.Domain.Interfaces;

namespace ProductsAPI.Application.Queries
{
    public class GetProductByIdQuery
    {
        private readonly IProductRepository _repository;

        public GetProductByIdQuery(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto?> ExecuteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                IsActive = product.IsActive
            };
        }
    }
}