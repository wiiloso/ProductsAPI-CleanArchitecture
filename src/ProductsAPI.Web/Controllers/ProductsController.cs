using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Application.Commands;
using ProductsAPI.Application.DTOs;
using ProductsAPI.Application.Queries;

namespace ProductsAPI.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly GetAllProductsQuery _getAllProductsQuery;
        private readonly GetProductByIdQuery _getProductByIdQuery;
        private readonly CreateProductCommand _createProductCommand;
        private readonly UpdateProductCommand _updateProductCommand;
        private readonly DeleteProductCommand _deleteProductCommand;

        public ProductsController(
            GetAllProductsQuery getAllProductsQuery,
            GetProductByIdQuery getProductByIdQuery,
            CreateProductCommand createProductCommand,
            UpdateProductCommand updateProductCommand,
            DeleteProductCommand deleteProductCommand)
        {
            _getAllProductsQuery = getAllProductsQuery;
            _getProductByIdQuery = getProductByIdQuery;
            _createProductCommand = createProductCommand;
            _updateProductCommand = updateProductCommand;
            _deleteProductCommand = deleteProductCommand;
        }

        /// <summary>
        /// Obtiene todos los productos activos
        /// </summary>
        /// <returns>Lista de productos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            try
            {
                var products = await _getAllProductsQuery.ExecuteAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "El ID debe ser mayor a 0" });

                var product = await _getProductByIdQuery.ExecuteAsync(id);
                
                if (product == null)
                    return NotFound(new { message = $"Producto con ID {id} no encontrado" });

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="createProductDto">Datos del producto a crear</param>
        /// <returns>Producto creado</returns>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdProduct = await _createProductCommand.ExecuteAsync(createProductDto);
                
                return CreatedAtAction(
                    nameof(GetProductById), 
                    new { id = createdProduct.Id }, 
                    createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <param name="updateProductDto">Datos actualizados del producto</param>
        /// <returns>Producto actualizado</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "El ID debe ser mayor a 0" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedProduct = await _updateProductCommand.ExecuteAsync(id, updateProductDto);
                
                if (updatedProduct == null)
                    return NotFound(new { message = $"Producto con ID {id} no encontrado" });

                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un producto (soft delete)
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "El ID debe ser mayor a 0" });

                var result = await _deleteProductCommand.ExecuteAsync(id);
                
                if (!result)
                    return NotFound(new { message = $"Producto con ID {id} no encontrado" });

                return Ok(new { message = "Producto eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }
    }
}