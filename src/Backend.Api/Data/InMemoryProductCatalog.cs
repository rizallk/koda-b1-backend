using Backend.Api.Models;

namespace Backend.Api.Data;

public sealed class InMemoryProductCatalog
{
  private readonly List<Product> _products = [];

  public IReadOnlyList<Product> GetAll()
  {
    return _products.ToArray();
  }

  public Product? GetByCode(string productCode)
  {
    return _products.FirstOrDefault(product => string.Equals(product.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));
  }

  public void Create(Product data)
  {
    _products.AddRange(data);
  }

  public void Update(string productCode, Product data)
  {
    var product = _products.FirstOrDefault(product => string.Equals(product.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));

    if (product is not null)
    {
      product.Name = data.Name;
      product.Price = data.Price;
      product.Stock = data.Stock;
      product.Category = data.Category;
    }
    else
    {
      throw new ArgumentNullException("Produk tidak ditemukan");

    }
  }

  public void Delete(string productCode)
  {
    var product = _products.FirstOrDefault(product => string.Equals(product.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));

    if (product is not null)
    {
      _products.Remove(product);
    }
    else
    {
      throw new ArgumentNullException("Produk tidak ditemukan");

    }
  }
}