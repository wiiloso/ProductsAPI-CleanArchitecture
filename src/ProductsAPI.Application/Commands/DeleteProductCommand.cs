using ProductsAPI.Domain.Interfaces;

namespace ProductsAPI.Application.Commands
{
    public class DeleteProductCommand
    {
        private readonly IProductRepository _repository;

        public DeleteProductCommand(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists) return false;

            return await _repository.DeleteAsync(id);
        }
    }
}