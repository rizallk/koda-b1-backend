using Backend.Api.Models;

namespace Backend.Api.DTOs;

public sealed record ProductRequest(string ProductCode, string Name, decimal Price, int Stock, string Category);