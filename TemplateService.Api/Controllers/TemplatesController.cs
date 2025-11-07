using Microsoft.AspNetCore.Mvc;
using XXXXMedia.Shared.Persistence.Entities;
using XXXXMedia.Shared.Persistence.Repositories.Interfaces;

namespace TemplateService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly IRepository<Template> _repo;
    public TemplatesController(IRepository<Template> repo) { _repo = repo; }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Template template)
    {
        var saved = await _repo.AddAsync(template);
        return Ok(new { id = saved.Id });
    }
}
