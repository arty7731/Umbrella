using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.FacadeModel;
using UmbrellaService.DataLevel.FacadeModel.Interface;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.DataLevel
{
	public class DataAccess : IDataAccess
	{
		IAutentefication autentification { get; set; }
		IProjectControl projectControl { get; set; }
		ITaskControl taskControl { get; set; }
		ICommentControl commentControl { get; set; }
		IAccountControl accountControl { get; set; }
		IStatusControl statusControl { get; set; }
		IHistoryControl historyControl { get; set; }

		public DataAccess()
		{
			autentification = new Autentefication();
			projectControl = new ProjectControl();
			taskControl = new TaskControl();
			commentControl = new CommentControl();
			accountControl = new AccountControl();
			statusControl = new StatusControl();
			historyControl = new HistoryControl();
		}

		#region Autentification
		public AccountProxy SignIn(string login, string password)
		{
			return autentification.SignIn(login, password);
		}
		public void SignOut(int id)
		{
			autentification.SignOut(id);
		}
		public AccountProxy SignUp(AccountProxy account)
		{
			return autentification.SignUp(account);
		}
		#endregion
		#region Project
		public ProjectProxy CreateProject(ProjectProxy project)
		{
			return projectControl.CreateProject(project);
		}
		public void DeleteProjectById(int accountId, int id)
		{
			DeleteTaskStatusByProjectId(id);
			DeleteTasksByProjectId(id);
			DeleteProjectStatusByProjectId(id);
			projectControl.DeleteProjectById(accountId, id);

		}
		public void UpdateProject(ProjectProxy project)
		{
			projectControl.UpdateProject(project);
		}
		public ProjectProxy GetProjectById(int id)
		{
			return projectControl.GetProjectById(id);
		}
		public List<ProjectProxy> GetProjectsByOwnerId(int ownerId)
		{
			return projectControl.GetProjectsByOwnerId(ownerId);
		}
		public List<ProjectProxy> GetProjectsByStatus(ProjectStatusProxy status)
		{
			return projectControl.GetProjectsByStatus(status);
		}
		public List<ProjectProxy> GetProjectsInWhichParticipatesId(int id)
		{
			return projectControl.GetProjectsInWhichParticipatesId(id);
		}
		public bool AddParticipant(int participantId, int projectId)
		{
			return projectControl.AddParticipant(participantId, projectId);
		}
		public void DeleteParticipant(int participantId, int projectId)
		{
			projectControl.DeleteParticipant(participantId, projectId);
		}
		#endregion
		#region Task
		public TaskProxy CreateTask(TaskProxy task)
		{
			return taskControl.CreateTask(task);
		}
		public void UpdateTask(TaskProxy task)
		{
			taskControl.UpdateTask(task);
		}
		public TaskProxy GetTaskById(int id)
		{
			return taskControl.GetTaskById(id);
		}
		public List<TaskProxy> GetTasksByProjectId(int projectId)
		{
			return taskControl.GetTasksByProjectId(projectId);
		}
		public List<TaskProxy> GetTasksByStatus(TaskStatusProxy status)
		{
			return taskControl.GetTasksByStatus(status);
		}
		public List<TaskProxy> GetTasksAvailableById(int id)
		{
			return taskControl.GetTasksAvailableById(id);
		}
		public bool AddExecutor(int executorId, int taskId)
		{
			return taskControl.AddExecutor(executorId, taskId);
		}
		public void DeleteTaskById(int taskId)
		{
			taskControl.DeleteTaskById(taskId);
		}
		public void DeleteExecutor(int executorId, int taskId)
		{
			taskControl.DeleteExecutor(executorId, taskId);
		}
		public void DeleteTasksByProjectId(int projectId)
		{
			taskControl.DeleteTasksByProjectId(projectId);
		}
		#endregion
		#region Comment
		public CommentProxy CreateComment(CommentProxy comment)
		{
			return commentControl.CreateComment(comment);
		}
		public void DeleteCommentById(int id, int accountId)
		{
			commentControl.DeleteCommentById(id, accountId);
		}
		public void DeleteCommentsByTaskId(int taskId)
		{
			commentControl.DeleteCommentsByTaskId(taskId);
		}
		public List<CommentProxy> GetCommentsByTaskId(int taskId)
		{
			return commentControl.GetCommentsByTaskId(taskId);
		}
		#endregion
		#region Account
		public AccountProxy GetAccountById(int id)
		{
			return accountControl.GetAccountById(id);	
		}
		public List<AccountProxy> GetAllAccounts()
		{
			return accountControl.GetAllAccounts();
		}
		public List<AccountProxy> GetAccountListByProjectId(int projectId)
		{
			return accountControl.GetAccountListByProjectId(projectId);
		}
		public List<AccountProxy> GetAccountListByTaskId(int taskId)
		{
			return accountControl.GetAccountListByTaskId(taskId);
		}
		public void UpdateAccount(AccountProxy account)
		{
			accountControl.UpdateAccount(account);
		}
		#endregion		
		#region Status
		public void ChangeTaskStatus(int accountId, int taskId, TaskStatusProxy status)
		{
			taskControl.ChangeTaskStatus(accountId, taskId, status);
		}
		public void ChangeProjectStatus(int projectId, ProjectStatusProxy status)
		{
			projectControl.ChangeProjectStatus(projectId, status);
		}
		public void CreateProjectStatus(int projectId, ProjectStatusProxy status)
		{
			statusControl.CreateProjectStatus(projectId, status);
		}
		public void CreateTaskStatus(int taskId, TaskStatusProxy status)
		{
			statusControl.CreateTaskStatus(taskId, status);
		}
		public void DeleteProjectStatusById(int statusId)
		{
			statusControl.DeleteProjectStatusById(statusId);
		}
		public void DeleteTaskStatusById(int statusId)
		{
			statusControl.DeleteTaskStatusById(statusId);
		}
		public void DeleteProjectStatusByProjectId(int projectId)
		{
			statusControl.DeleteProjectStatusByProjectId(projectId);
		}
		public void DeleteTaskStatusByProjectId(int projectId)
		{
			statusControl.DeleteTaskStatusByProjectId(projectId);

		}
		public List<ProjectStatusProxy> GetProjectStatusListByProjectId(int projectId)
		{
			return statusControl.GetProjectStatusListByProjectId(projectId);
		}
		public List<TaskStatusProxy> GetTaskStatusListByProjectId(int projectId)
		{
			return statusControl.GetTaskStatusListByProjectId(projectId);
		}

		public void AddHistory(HistoryProxy history, List<AccountProxy> readers)
		{
			historyControl.AddHistory(history, readers);
		}

		public void DeleteHistoryById(int id)
		{
			historyControl.DeleteHistoryById(id);
		}

		public HistoryProxy GetHistoryById(int id)
		{
			return historyControl.GetHistoryById(id);
		}

		public List<HistoryProxy> GetAvalibleHistoryById(int id)
		{
			return historyControl.GetAvalibleHistoryById(id);
		}


		#endregion
	}
}
