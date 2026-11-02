using Backend.Api.Data;
using Backend.Api.DTOs;
using Backend.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/v1/products")]

public sealed class ProductController : ControllerBase
{
  private readonly InMemoryProductCatalog _catalog;

  public ProductController(InMemoryProductCatalog catalog)
  {
    _catalog = catalog;
  }

  [HttpGet]
  [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
  public ActionResult<IReadOnlyList<Product>> GetAll()
  {
    var products = _catalog.GetAll().Select(ProductResponse.FromModel).ToArray();

    return Ok(products);
  }

  [HttpGet("{productCode}")]
  [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public ActionResult<IReadOnlyList<Product>> GetByCode([FromRoute] string productCode)
  {
    var product = _catalog.GetByCode(productCode);

    if (product is null)
    {
      return Problem(
        statusCode: StatusCodes.Status404NotFound,
        title: "Product not found",
        detail: $"No product was found for code {productCode}"
      );
    }

    return Ok(ProductResponse.FromModel(product));
  }

  [HttpPost]
  [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
  public ActionResult<ProductResponse> CreateProduct([FromBody] ProductRequest req)
  {
    var newProduct = new Product(req.ProductCode, req.Name, req.Price, req.Stock, req.Category);

    _catalog.Create(newProduct);

    return CreatedAtAction(actionName: nameof(GetByCode), routeValues: new { productCode = newProduct.ProductCode }, value: ProductResponse.FromModel(newProduct));
  }

  [HttpDelete("{productCode}")]
  [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public ActionResult DeleteProduct([FromRoute] string productCode)
  {
    try
    {
      _catalog.Delete(productCode);
    }
    catch (ArgumentNullException e)
    {
      return Problem(
             statusCode: StatusCodes.Status404NotFound,
             title: "Product not found",
             detail: $"Error: {e}"
           );
    }

    return Ok();
  }
}