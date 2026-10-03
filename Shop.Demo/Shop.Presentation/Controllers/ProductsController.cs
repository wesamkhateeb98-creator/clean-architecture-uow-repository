using Microsoft.AspNetCore.Mvc;
using Shop.Application.Abstracts.Services;
using Shop.Application.Models;
using Shop.Presentation.Extensions.Mapper;
using Shop.Presentation.Models.Requests;

namespace Shop.Presentation.Controllers;

// Thin controller: HTTP in -> service -> HTTP out. No business logic, no DbContext.
public class ProductsController(IProductService productService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<ProductModel>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await productService.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductModel>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await productService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add(AddProductRequest request, CancellationToken cancellationToken)
    {
        var id = await productService.AddAsync(request.ToModel(), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        await productService.UpdateAsync(request.ToModel(id), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await productService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
