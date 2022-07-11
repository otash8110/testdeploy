using FluentValidation;
using Application.Interfaces.IServices;
using Application.Interfaces.IDataBase;
using Application.Interfaces.IMappers;
using Domain.Entities;
using Application.DTO;
using Application.Interfaces.ITasksAdditional;
using Domain.Exceptions;

namespace Application.Services
{
    public class TasksService : ITasksService
    {
        private readonly IGenericRepository<ProjectTask> _taskRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenerateDescription _descriptionGenerator;
        private readonly IProjectTaskMapper _taskMapper;
        private readonly AbstractValidator<ProjectTaskDto> _projectValidator;

        public TasksService(IGenericRepository<ProjectTask> taskRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Role> roleRepository,
            IGenerateDescription descriptionGenerator,
            IProjectTaskMapper taskMapper, AbstractValidator<ProjectTaskDto> projectValidator)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _descriptionGenerator = descriptionGenerator;
            _taskMapper = taskMapper;
            _projectValidator = projectValidator;
        }

        public async Task CreateTaskAsync(string name, string description, Statuses status, int creatorId, int performerId = 0)
        {
            ProjectTask task = new ProjectTask()
            {
                Name = name,
                Description = description,
                Status = status,
                CreatorId = creatorId,
                PerformerId = performerId
            };

            await _taskRepository.CreateAsync(task);
        }

        public async Task CreateTaskAsync(ProjectTaskDto taskDto)
        {
            var description = await _descriptionGenerator
                .GenerateDescription(_taskMapper.MapDtoToProjectTask(taskDto));
            taskDto.Description = description;

            var objectValidated = _projectValidator.Validate(taskDto);
            if (!objectValidated.IsValid)
            {
                throw new AppException(objectValidated.Errors[0].ErrorMessage);
            }
            ProjectTask task = new ProjectTask();
            if (taskDto.PerformerId != 0)
            {
                task = await ValidateRolesHierarchyForTask(taskDto);
            } else task = _taskMapper.MapDtoToProjectTask(taskDto);

            await _taskRepository.CreateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            ProjectTask task = await _taskRepository.GetByIdAsync(id);
            await _taskRepository.DeleteAsync(task);
        }

        public async Task<List<ProjectTaskDto>> GetAllTasksAsync()
        {
            var allTasks = await _taskRepository.GetAllAsync();
            if (!allTasks.Any()) throw new KeyNotFoundException("No any tasks");
            var mappedDtoTasks = MapDtoToProjectTasksList(allTasks);
            return mappedDtoTasks;
        }

        public async Task<ProjectTaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) throw new KeyNotFoundException("No task is found with provided ID");
            var mappedTask = _taskMapper.MapProjectTaskToDto(task);
            return mappedTask;
        }

        public Task UpdateTaskAsync(string name, string description, Statuses status, int creatorId, int performerId = 0)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTaskAsync(ProjectTaskDto projectTaskDto)
        {
            if (projectTaskDto.Id == 0) throw new AppException("ID for task is not provided");
            else
            {
                ProjectTask task = _taskMapper.MapDtoToProjectTask(projectTaskDto);
                await _taskRepository.UpdateAsync(task);
            }
        }

        public List<ProjectTaskDto> MapDtoToProjectTasksList(List<ProjectTask> tasks)
        {
            List<ProjectTaskDto> dtosList = new List<ProjectTaskDto>();
            foreach (var task in tasks)
            {
                var taskDto = _taskMapper.MapProjectTaskToDto(task);
                dtosList.Add(taskDto);
            }
            return dtosList;
        }

        public async Task<ProjectTask> ValidateRolesHierarchyForTask(ProjectTaskDto projectTaskDto)
        {
            ProjectTask task = _taskMapper.MapDtoToProjectTask(projectTaskDto);
            var populatedTask = await PopulateProjectTask(task);

            if (populatedTask.Creator.Role.Name == "TeamLead"
                || (populatedTask.Creator.Role.Name == "Senior" && populatedTask.Performer.Role.Name == "Middle")
                || (populatedTask.Creator.Role.Name == "Senior" && populatedTask.Performer.Role.Name == "Junior")
                || (populatedTask.Creator.Role.Name == "Middle" && populatedTask.Performer.Role.Name == "Junior"))
            {
                return populatedTask;
            }
            else
            {
                if (populatedTask.Creator.Role.Name == "Junior")
                    throw new AppException("Juniors cannot create tasks!");
                else throw new AppException("Creator's level is lower than the performer's");
            }

        }

        public async Task<ProjectTask> PopulateProjectTask(ProjectTask task)
        {
            task.Creator = await _userRepository.GetAsync(u => u.Id == task.CreatorId);
            task.Performer = await _userRepository.GetAsync(u => u.Id == task.PerformerId);

            task.Creator.Role = await _roleRepository.GetAsync(r => r.Id == task.Creator.RoleId);
            task.Performer.Role = await _roleRepository.GetAsync(r => r.Id == task.Performer.RoleId);
            return task;
        }
    }
}
