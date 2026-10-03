using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs;
using Shop.Application.Services;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(CategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll(CancellationToken ct) =>
        Ok(await categoryService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id, CancellationToken ct) =>
        Ok(await categoryService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken ct)
    {
        var id = await categoryService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryRequest request, CancellationToken ct)
    {
        await categoryService.UpdateAsync(id, request, ct);
        return NoContent();
    }

    // DELETE api/categories/1?moveTo=2  -> moves products to 2, then deletes 1 (one transaction)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] int? moveTo, CancellationToken ct)
    {
        await categoryService.DeleteAsync(id, moveTo, ct);
        return NoContent();
    }
}
