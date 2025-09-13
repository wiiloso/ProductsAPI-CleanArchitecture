using ProductsAPI.Application.DTOs;
using ProductsAPI.Domain.Interfaces;

namespace ProductsAPI.Application.Commands
{
    public class UpdateProductCommand
    {
        private readonly IProductRepository _repository;

        public UpdateProductCommand(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto?> ExecuteAsync(int id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null) return null;

            existingProduct.Name = updateProductDto.Name;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.Stock = updateProductDto.Stock;
            existingProduct.Category = updateProductDto.Category;
            existingProduct.IsActive = updateProductDto.IsActive;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            var updatedProduct = await _repository.UpdateAsync(existingProduct);

            return new ProductDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price,
                Stock = updatedProduct.Stock,
                Category = updatedProduct.Category,
                CreatedAt = updatedProduct.CreatedAt,
                UpdatedAt = updatedProduct.UpdatedAt,
                IsActive = updatedProduct.IsActive
            };
        }
    }
}