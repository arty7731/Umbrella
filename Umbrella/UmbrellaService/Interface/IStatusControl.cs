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
	public interface IStatusControl
	{
		[OperationContract]
		void CreateProjectStatus(int accountId, int projectId, ProjectStatusProxy status);
		[OperationContract]
		void CreateTaskStatus(int accountId, int taskId, TaskStatusProxy status);

		[OperationContract]
		void DeleteProjectStatusById(int accountId, int statusId);
		[OperationContract]
		void DeleteTaskStatusById(int accountId, int statusId);

		[OperationContract]
		void DeleteProjectStatusByProjectId(int projectId);
		[OperationContract]
		void DeleteTaskStatusByProjectId(int projectId);

		[OperationContract]
		List<ProjectStatusProxy> GetProjectStatusListByProjectId(int projectId);
		[OperationContract]
		List<TaskStatusProxy> GetTaskStatusListByProjectId(int projectId);
	}
}
