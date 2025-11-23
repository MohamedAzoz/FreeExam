using FreeExam.Application.Contracts.DTOs.Quetion;
using FreeExam.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService questionService;

        public QuestionController(IQuestionService _questionService)
        {
            questionService = _questionService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]CreateQuestionDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result =await questionService.CreateAsync(model);
            if (!result.IsSuccess)
            {
                int statusCode= result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Created();
        }
        [HttpPost]
        [Route("AddRange")]
        public async Task<IActionResult> AddRange([FromBody]List<CreateQuestionDto> model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await questionService.AddRangeAsync(model);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> getAll(int examId)
        {
            var result = await questionService.FindAllAsync(x=>x.ExamId==examId);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Ok(result);
        }

        [HttpPost("Test")]
        public async Task<IActionResult> Test([FromBody] TestDto testDto)
        {
            var result = await questionService.GetTestQuestionsAsync(testDto.ExamId,
                testDto.NumberOfQuestion);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await questionService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await questionService.Delete(id);
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Ok();
        }

        [HttpDelete]
        [Route("Clear")]
        public async Task<IActionResult> Clear()
        {
            var result = await questionService.ClearAsync();
            if (!result.IsSuccess)
            {
                int statusCode = result.StatusCode ?? 400;
                return StatusCode(statusCode, result.Message);
            }
            return Ok();
        }

    }
}
