using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;
using UmbrellaService.Service;

namespace UmbrellaService.Interface
{
	[ServiceContract]
	//[ServiceContract(CallbackContract = typeof(IClientCallback), SessionMode = SessionMode.Required)]
	public interface ITaskControl
	{
		[OperationContract]
		TaskProxy CreateTask(int accountId, TaskProxy task);
		[OperationContract]
		void UpdateTask(int accountId, TaskProxy task);
		[OperationContract]
		void ChangeTaskStatus(int accountId, int taskId, TaskStatusProxy status);
		[OperationContract]
		void DeleteTaskById(int accountId, int taskId);
		[OperationContract]
		void DeleteTasksByProjectId(int projectId);
		[OperationContract]
		TaskProxy GetTaskById(int id);
		[OperationContract]
		List<TaskProxy> GetTasksByProjectId(int projectId);
		[OperationContract]
		List<TaskProxy> GetTasksByStatus(TaskStatusProxy status);
		[OperationContract]
		bool AddExecutor(int accountId, int executorId, int taskId);
		[OperationContract]
		void DeleteExecutor(int accountId, int executorId, int taskId);
		[OperationContract]
		List<TaskProxy> GetTasksAvailableById(int id);
	}
}
