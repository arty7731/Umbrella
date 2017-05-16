using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.DataLevel.FacadeModel.Interface
{
	public interface IStatusControl
	{
		void CreateProjectStatus(int projectId, ProjectStatusProxy status);
		void CreateTaskStatus(int taskId, TaskStatusProxy status);

		void DeleteProjectStatusById(int statusId);
		void DeleteTaskStatusById(int statusId);

		void DeleteProjectStatusByProjectId(int projectId);
		void DeleteTaskStatusByProjectId(int projectId);

		List<ProjectStatusProxy> GetProjectStatusListByProjectId(int projectId);
		List<TaskStatusProxy> GetTaskStatusListByProjectId(int projectId);

	}
}
