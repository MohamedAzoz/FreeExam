using FreeExam.Application.Contracts.DTOs.Exam;
using FreeExam.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService examService;

        public ExamController(IExamService _examService)
        {
            examService = _examService;
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateExamDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await examService.CreateAsync(model);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Created();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await examService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await examService.GetAllAsync();
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Ok(result);
        }

        [HttpGet("GetAll{id:int}")]
        public async Task<IActionResult> GetAll(int id)
        {
            var result = await examService.FindAllAsync(x=>x.SubjectId==id);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Ok(result);
        }

    }
}
