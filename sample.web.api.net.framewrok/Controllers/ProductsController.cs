using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sample.web.api.net.framewrok.Controllers
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductsController : ApiController
    {
        private static List<Product> products = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1", Category = "Category 1", Price = 10.99m },
        new Product { Id = 2, Name = "Product 2", Category = "Category 2", Price = 20.99m },
        new Product { Id = 3, Name = "Product 3", Category = "Category 3", Price = 30.99m },
    };

        // GET api/products
        public IEnumerable<Product> Get()
        {
            return products;
        }

        // GET api/products/1
        public IHttpActionResult Get(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST api/products
        public IHttpActionResult Post([FromBody] Product newProduct)
        {
            if (newProduct == null || string.IsNullOrEmpty(newProduct.Name))
            {
                return BadRequest("Invalid product data.");
            }

            newProduct.Id = products.Max(p => p.Id) + 1;
            products.Add(newProduct);
            return CreatedAtRoute("DefaultApi", new { id = newProduct.Id }, newProduct);
        }

        // PUT api/products/1
        public IHttpActionResult Put(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null || string.IsNullOrEmpty(updatedProduct.Name))
            {
                return BadRequest("Invalid product data.");
            }

            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Category = updatedProduct.Category;
            product.Price = updatedProduct.Price;

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE api/products/1
        public IHttpActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            products.Remove(product);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/products/createwithcategory
        [HttpPost]
        [Route("createwithcategory")]
        public IHttpActionResult CreateWithCategory([FromBody] Product newProduct, [FromUri] string category)
        {
            if (newProduct == null || string.IsNullOrEmpty(newProduct.Name) || string.IsNullOrEmpty(category))
            {
                return BadRequest("Invalid product data.");
            }

            newProduct.Id = products.Max(p => p.Id) + 1;
            newProduct.Category = category;
            products.Add(newProduct);
            return CreatedAtRoute("DefaultApi", new { id = newProduct.Id }, newProduct);
        }

        // POST api/products/{id}/duplicate
        [HttpPost]
        [Route("{id:int}/duplicate")]
        public IHttpActionResult DuplicateProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var duplicatedProduct = new Product
            {
                Id = products.Max(p => p.Id) + 1,
                Name = $"{product.Name} - Copy",
                Category = product.Category,
                Price = product.Price
            };

            products.Add(duplicatedProduct);
            return CreatedAtRoute("DefaultApi", new { id = duplicatedProduct.Id }, duplicatedProduct);
        }
    }

}
