using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using task_api.Models;
using task_api.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using task_api.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Diagnostics;

namespace task_api.Controllers;

[Route("api/tasks")]
[ApiController]
[Authorize]
public class WorkController : ControllerBase
{
    private readonly IWorkRepository _workrepository;

    public WorkController(IWorkRepository workrepository)
    {
        _workrepository = workrepository;
    }

    public async Task<List<Work>> GetAllWorks()
    {
        Console.WriteLine("Si entra");
        return await _workrepository.GetAsync();
    }
    

    [HttpPost]
    public async Task<ActionResult> AddTask(Work work)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var resp = Jwt.getTokenBody(identity);

        var userId = resp.result;

        try
        {
            work.user = userId;
            await _workrepository.CreateAsync(work);
            return CreatedAtAction(nameof(GetAllWorks), new { id = work.Id }, work);
        }
        catch (System.Exception)
        {
            return StatusCode(500, new { status = 500, error = "Error durante la creación de tarea" });
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var workid = await _workrepository.GetWorkByIdAsync(id);
        if (workid == null)
        {
            return StatusCode(404, new { status = 404, error = "No se ha encontrado el elemento que desea eliminar" });
        }

        var vt = await validatingToken(id);
        if (!vt.success)
        {
            return StatusCode(403, new { status = 403, error = "No tiene autorización para realizar esta acción" });
        }
        else
        {
            await _workrepository.DeleteAsync(id);
            return StatusCode(200, new { status = 200, error = "Tarea eliminada con éxito" });
        }

        return NoContent();
    }


    private async Task<dynamic> validatingToken(string workId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var resTB = Jwt.getTokenBody(identity);

        if (!resTB.success) return new { success = false, result = "" };

        var userDB = await _workrepository.ComprobateUserByTaskIdAsync(resTB.result, workId);

        if (userDB == null) return new { success = false, result = "" };

        return new
        {
            success = true,
            result = userDB
        };
    }


    [Route("/error-development")]
    public IActionResult HandleErrorDevelopment(
        [FromServices] IHostEnvironment hostEnvironment)
    {
        if (!hostEnvironment.IsDevelopment())
        {
            return NotFound();
        }

        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        return Problem(
            detail: exceptionHandlerFeature.Error.StackTrace,
            title: exceptionHandlerFeature.Error.Message);
    }

    [Route("/error")]
    public IActionResult HandleError() =>
        Problem();

}