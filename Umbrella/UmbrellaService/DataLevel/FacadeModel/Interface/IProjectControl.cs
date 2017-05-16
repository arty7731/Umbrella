using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.DataLevel.FacadeModel.Interface
{
	public interface IProjectControl
	{
		ProjectProxy CreateProject(ProjectProxy project);
		void DeleteProjectById(int accountId, int id);
		bool AddParticipant(int participantId, int projectId);
		void DeleteParticipant(int participantId, int projectId);
		void UpdateProject(ProjectProxy project);
		void ChangeProjectStatus(int projectId, ProjectStatusProxy status);
		ProjectProxy GetProjectById(int id);
		List<ProjectProxy> GetProjectsByOwnerId(int ownerId);
		List<ProjectProxy> GetProjectsByStatus(ProjectStatusProxy status);
		List<ProjectProxy> GetProjectsInWhichParticipatesId(int id);


	}
}
