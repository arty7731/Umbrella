using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.DataLevel.FacadeModel.Interface
{
	public interface ITaskControl
	{
		TaskProxy CreateTask(TaskProxy task);
		void UpdateTask(TaskProxy task);
		void DeleteTaskById(int taskId);
		//void SetCurrentExecutor(int executorId);
		void DeleteTasksByProjectId(int projectId);
		bool AddExecutor(int executorId, int taskId);
		void DeleteExecutor(int executorId, int taskId);

		void ChangeTaskStatus(int accountId, int taskId, TaskStatusProxy status);
		TaskProxy GetTaskById(int id);
		List<TaskProxy> GetTasksByProjectId(int projectId);

		List<TaskProxy> GetTasksByStatus(TaskStatusProxy status);
		List<TaskProxy> GetTasksAvailableById(int id);
	}
}
