using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly AuthorsService _authorsService;

    public AuthorsController(AuthorsService authorsService) =>
        _authorsService = authorsService;

    [HttpGet]
    public async Task<List<Authors>> Get() =>
        await _authorsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Authors>> Get(string id)
    {
        var authors = await _authorsService.GetAsync(id);

        if (authors is null)
        {
            return NotFound();
        }

        return authors;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Authors newAuthors)
    {
        await _authorsService.CreateAsync(newAuthors);

        return CreatedAtAction(nameof(Get), new { id = newAuthors.Id }, newAuthors);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Authors updatedAuthors)
    {
        var authors = await _authorsService.GetAsync(id);

        if (authors is null)
        {
            return NotFound();
        }

        updatedAuthors.Id = authors.Id;

        await _authorsService.UpdateAsync(id, updatedAuthors);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var authors = await _authorsService.GetAsync(id);

        if (authors is null)
        {
            return NotFound();
        }

        await _authorsService.RemoveAsync(id);

        return NoContent();
    }
}