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
	public interface IProjectControl
	{
		[OperationContract]
		ProjectProxy CreateProject(int accountId, ProjectProxy project);
		[OperationContract]
		void DeleteProjectById(int accountId, int id);
		[OperationContract]
		void UpdateProject(int accountId, ProjectProxy project);
		[OperationContract]
		void ChangeProjectStatus(int accountId, int projectId, ProjectStatusProxy status);
		[OperationContract]
		bool AddParticipant(int accountId, int participantId, int projectId);
		[OperationContract]
		void DeleteParticipant(int accountId, int participantId, int projectId);
		[OperationContract]
		ProjectProxy GetProjectById(int id);
		[OperationContract]
		List<ProjectProxy> GetProjectsByOwnerId(int ownerId);
		[OperationContract]
		List<ProjectProxy> GetProjectsByStatus(ProjectStatusProxy status);
		[OperationContract]
		List<ProjectProxy> GetProjectsInWhichParticipatesId(int id);
	}
}
