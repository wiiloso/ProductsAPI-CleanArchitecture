using ProductsAPI.Application.DTOs;
using ProductsAPI.Domain.Entities;
using ProductsAPI.Domain.Interfaces;

namespace ProductsAPI.Application.Commands
{
    public class CreateProductCommand
    {
        private readonly IProductRepository _repository;

        public CreateProductCommand(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> ExecuteAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                Category = createProductDto.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var createdProduct = await _repository.CreateAsync(product);

            return new ProductDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                Stock = createdProduct.Stock,
                Category = createdProduct.Category,
                CreatedAt = createdProduct.CreatedAt,
                UpdatedAt = createdProduct.UpdatedAt,
                IsActive = createdProduct.IsActive
            };
        }
    }
}