using Microsoft.AspNetCore.Mvc;
using Shop.Application.Abstracts.Services;
using Shop.Application.Models;
using Shop.Presentation.Extensions.Mapper;
using Shop.Presentation.Models.Requests;

namespace Shop.Presentation.Controllers;

public class CategoriesController(ICategoryService categoryService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryModel>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await categoryService.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryModel>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await categoryService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add(AddCategoryRequest request, CancellationToken cancellationToken)
    {
        var id = await categoryService.AddAsync(request.ToModel(), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        await categoryService.UpdateAsync(request.ToModel(id), cancellationToken);
        return NoContent();
    }

    // DELETE api/v1.0/categories/1?moveTo=2  -> moves products to 2, then deletes 1 (one transaction)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] int? moveTo, CancellationToken cancellationToken)
    {
        await categoryService.DeleteAsync(new DeleteCategoryModel(id, moveTo), cancellationToken);
        return NoContent();
    }
}
