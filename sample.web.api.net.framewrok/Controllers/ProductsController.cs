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
        
        using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Xml.Serialization;

        
        private static string SerializeObjectToXml<T>(T obj)
{
    var serializer = new XmlSerializer(typeof(T));
    using (var stringWriter = new StringWriter())
    {
        serializer.Serialize(stringWriter, obj);
        return stringWriter.ToString();
    }
    
    private static string ConvertXmlToSoapEnvelope(string xml, string soapNamespace, string operationName)
{
    // Create the SOAP Envelope
    var envelope = new XElement(XName.Get("Envelope", soapNamespace),
        new XAttribute(XNamespace.Xmlns + "soap", soapNamespace),
        new XElement(XName.Get("Body", soapNamespace),
            new XElement(XName.Get(operationName, soapNamespace), XElement.Parse(xml))
        )
    );

    return envelope.ToString();
}

[HttpPost]
public async Task<HttpResponseMessage> Post([FromBody] MyComplexClass myComplexObject)
{
    var soapEndpoint = "https://www.example.com/soap.asmx"; // Replace with the correct SOAP endpoint
    var soapAction = "http://www.example.com/soap/YourOperationName"; // Replace with the correct SOAP action
    var soapNamespace = "http://www.example.com/soap/";

    var serializedXml = SerializeObjectToXml(myComplexObject);
    var soapEnvelope = ConvertXmlToSoapEnvelope(serializedXml, soapNamespace, "YourOperationName");

    using (var httpClient = new HttpClient())
    {
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
        httpClient.DefaultRequestHeaders.Add("SOAPAction", soapAction);

        var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

        var response = await httpClient.PostAsync(soapEndpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            return Request.CreateErrorResponse(response.StatusCode, response.ReasonPhrase);
        }

        var result = await response.Content.ReadAsStringAsync();
        return Request.CreateResponse(HttpStatusCode.OK, result);
    }
}


}

    }

}
