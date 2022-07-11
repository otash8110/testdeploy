using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.IServices;
using Application.DTO;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITasksService _tasksService;

        public TaskController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }
        /// <summary>
        /// Create Task
        /// </summary>
        /// <param name="taskDto"></param>
        /// <returns>Message of successfull creation</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /CreateTask
        ///     {
        ///         "name": "init project",
        ///         "description": "some description of the task",
        ///         "status": 0, //0 - Not started, 1 - Completed
        ///         "creatorId": 1,
        ///         "performerId": 2
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Message of successfully created Task</response>
        /// <response code="400">If error occurs during creation or validation</response>
        [HttpPost("[action]")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTask(ProjectTaskDto taskDto)
        {
                await _tasksService.CreateTaskAsync(taskDto);
                Response.StatusCode = StatusCodes.Status200OK;
                return new JsonResult(new { Message = "Successfully Added" });
        }

        /// <summary>
        /// Get all Tasks
        /// </summary>
        /// <returns>All tasks available</returns>
        /// <response code="200">All Tasks in JSON are returned</response>
        /// <response code="400">Probably there is no any task</response>
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetTasks()
        {
                var allTasks = await _tasksService.GetAllTasksAsync();
                Response.StatusCode = StatusCodes.Status200OK;
                return new JsonResult(new { Result = allTasks });
        }

        /// <summary>
        /// Get one Task by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task by id</returns>
        /// <response code="200">Task in JSON is returned</response>
        /// <response code="400">Probably there is no task with such ID</response>
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetTaskById(int id)
        {
                var task = await _tasksService.GetTaskByIdAsync(id);
                return new JsonResult(new { Result = task });
        }

        /// <summary>
        /// Update Task
        /// </summary>
        /// <param name="taskDto"></param>
        /// <returns>Message of successfull task update</returns>
        /// <response code="200">Task is updated successfully</response>
        /// <response code="400">Something wrong happend during update</response>
        [HttpPut("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTask(ProjectTaskDto taskDto)
        {
                await _tasksService.UpdateTaskAsync(taskDto);
                return new JsonResult(new { Message = "Updated!" });
        }

        [HttpDelete("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteTask(int id)
        {
                await _tasksService.DeleteTaskAsync(id);
                return new JsonResult(new { Message = "Deleted" });
        }

    }
}
