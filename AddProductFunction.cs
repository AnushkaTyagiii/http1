using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace http1
{
    public class AddProductFunction
    {
        private readonly AppDbContext _dbContext;
        public AddProductFunction(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [FunctionName("AddProduct")]
        public async Task<IActionResult> Run( [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest req,ILogger log)
        {
            log.LogInformation("AddProduct HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(requestBody);

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult(product);
        }
    }
}
