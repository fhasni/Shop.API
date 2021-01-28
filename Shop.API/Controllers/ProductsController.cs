using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.API.Common;
using Shop.API.Database;
using Shop.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ShopContext _context;

        public ProductsController(ShopContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();
        }
        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParameters queryParameters)
        {

            IQueryable<Product> products = _context.Products;

            if (queryParameters.MinPrice != null) {
                products = products.Where(p => p.Price >= queryParameters.MinPrice);
            }

            if (queryParameters.MaxPrice != null)
            {
                products = products.Where(p => p.Price <= queryParameters.MaxPrice);
            }

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products.Where(p => p.Sku == queryParameters.Sku);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy) && typeof(Product).GetProperty(queryParameters.SortBy) != null)
            {
                products = products.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
            }

            products = products.Skip(queryParameters.Size * (queryParameters.Page - 1))
                                .Take(queryParameters.Size);

            return Ok(await products.ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducts(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product) {

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetProducts",
                new  { id = product.Id },
                product
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product) {
            if (id != product.Id) {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {

                if(_context.Products.Find(id) == null) {
                    return NotFound();
                }

                throw;

            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            _context.Entry(product).State = EntityState.Deleted;

            await _context.SaveChangesAsync();

            return product;
        }
    }
}
