using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UmbrellaService.DataLevel;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.Service
{
	[ServiceBehavior(IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
	public class Service : IService
	{
		IDataAccess dataAccess { get; set; }
		private Dictionary<int, IClientCallback> callbackList = new Dictionary<int, IClientCallback>();

		public Service()
		{
			dataAccess = new DataAccess();
		}
		public void LogOut(int id)
		{
			callbackList.Remove(id);
			dataAccess.SignOut(id);
		}

		public AccountProxy SignIn(string login, string password)
		{
			var callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
			var acc = dataAccess.SignIn(login, password);
			if (acc != null)
			{
				callbackList.Add(acc.Id, callback);
			}
			return acc;
		}

		public AccountProxy SignUp(string login, string password, string email, string fullName, int age)
		{
			var callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
			var acc = dataAccess.SignUp(new AccountProxy(0, login, password, AccountStatus.Online, email, fullName));
			if (acc != null)
			{
				callbackList.Add(acc.Id, callback);
			}
			return acc;
		}

		public void DeleteProjectById(int accountId, int id)
		{
			dataAccess.DeleteTasksByProjectId(id);
			dataAccess.DeleteProjectById(accountId, id);
		}

		public ProjectProxy GetProjectById(int id)
		{
			return dataAccess.GetProjectById(id);
		}

		public List<ProjectProxy> GetProjectsByOwnerId(int ownerId)
		{
			return dataAccess.GetProjectsByOwnerId(ownerId);
		}

		public List<ProjectProxy> GetProjectsByStatus(ProjectStatusProxy status)
		{
			return dataAccess.GetProjectsByStatus(status);
		}

		public List<ProjectProxy> GetProjectsInWhichParticipatesId(int id)
		{
			return dataAccess.GetProjectsInWhichParticipatesId(id);
		}

		public TaskProxy GetTaskById(int id)
		{
			return dataAccess.GetTaskById(id);
		}

		public List<TaskProxy> GetTasksByProjectId(int projectId)
		{
			return dataAccess.GetTasksByProjectId(projectId);
		}

		public List<TaskProxy> GetTasksByStatus(TaskStatusProxy status)
		{
			return dataAccess.GetTasksByStatus(status);
		}

		public List<TaskProxy> GetTasksAvailableById(int id)
		{
			return dataAccess.GetTasksAvailableById(id);
		}

		public void DeleteTasksByProjectId(int projectId)
		{
			dataAccess.DeleteTasksByProjectId(projectId);
		}

		public void DeleteCommentById(int id, int accountId)
		{
			dataAccess.DeleteCommentById(id, accountId);
		}

		public void DeleteCommentsByTaskId(int taskId)
		{
			dataAccess.DeleteCommentsByTaskId(taskId);
		}

		public List<CommentProxy> GetCommentsByTaskId(int taskId)
		{
			return dataAccess.GetCommentsByTaskId(taskId);
		}

		public AccountProxy GetAccountById(int id)
		{
			return dataAccess.GetAccountById(id);
		}

		public List<AccountProxy> GetAccountListByProjectId(int projectId)
		{
			return dataAccess.GetAccountListByProjectId(projectId);
		}

		public List<AccountProxy> GetAccountListByTaskId(int taskId)
		{
			return dataAccess.GetAccountListByTaskId(taskId);
		}

		public void UpdateAccount(AccountProxy account)
		{
			dataAccess.UpdateAccount(account);
		}

		public void ChangeTaskStatus(int accountId, int taskId, TaskStatusProxy status)
		{
			dataAccess.ChangeTaskStatus(accountId, taskId, status);
		}

		public void DeleteProjectStatusByProjectId(int projectId)
		{
			dataAccess.DeleteProjectStatusByProjectId(projectId);
		}

		public void DeleteTaskStatusByProjectId(int projectId)
		{
			dataAccess.DeleteTaskStatusByProjectId(projectId);
		}

		public List<ProjectStatusProxy> GetProjectStatusListByProjectId(int projectId)
		{
			return dataAccess.GetProjectStatusListByProjectId(projectId);
		}

		public List<TaskStatusProxy> GetTaskStatusListByProjectId(int projectId)
		{
			return dataAccess.GetTaskStatusListByProjectId(projectId);
		}

		public List<AccountProxy> GetAllAccounts()
		{
			return dataAccess.GetAllAccounts();
		}

		public void AddHistory(HistoryProxy history, List<AccountProxy> readers)
		{
			dataAccess.AddHistory(history, readers);
		}

		public void DeleteHistoryById(int id)
		{
			dataAccess.DeleteHistoryById(id);
		}

		public HistoryProxy GetHistoryById(int id)
		{
			return dataAccess.GetHistoryById(id);
		}

		public List<HistoryProxy> GetAvalibleHistoryById(int id)
		{
			return dataAccess.GetAvalibleHistoryById(id);
		}

		//###########################################################
		//#################Method with callback######################
		//############################################################
		public ProjectProxy CreateProject(int accountId, ProjectProxy project)
		{
			var proj = dataAccess.CreateProject(project);
			return proj;
		}

		public void UpdateProject(int accountId, ProjectProxy project)
		{
			dataAccess.UpdateProject(project);
			dataAccess.AddHistory(
				new HistoryProxy(
					0,
					DateTime.Now,
					string.Format("User {0} edited project {1}", dataAccess.GetAccountById(accountId).FullName, project.Name)),
					dataAccess.GetAccountListByProjectId(project.Id)
					);
		}

		public void ChangeProjectStatus(int accountId, int projectId, ProjectStatusProxy status)
		{
			dataAccess.ChangeProjectStatus(projectId, status);
			dataAccess.AddHistory(
				new HistoryProxy(
					0,
					DateTime.Now,
					string.Format("User {0} edited project status {1} to {2}", dataAccess.GetAccountById(accountId).FullName, dataAccess.GetProjectById(projectId).Name, status.Name)),
					dataAccess.GetAccountListByProjectId(projectId)
					);
		}

		public bool AddParticipant(int accountId, int participantId, int projectId)
		{
			var par = dataAccess.AddParticipant(participantId, projectId);
			dataAccess.AddHistory(
				new HistoryProxy(
					0,
					DateTime.Now,
					string.Format("User {0} added participant {1}, to project {2}", dataAccess.GetAccountById(accountId).FullName, dataAccess.GetProjectById(projectId).Name, dataAccess.GetAccountById(participantId).FullName)),
					dataAccess.GetAccountListByProjectId(projectId)
					);
			return par;
		}

		public void DeleteParticipant(int accountId, int participantId, int projectId)
		{
			dataAccess.DeleteParticipant(participantId, projectId);
			dataAccess.AddHistory(
				new HistoryProxy(
					0,
					DateTime.Now,
					string.Format("User {0} deleted participant {1}, from project {2}", dataAccess.GetAccountById(accountId).FullName, dataAccess.GetProjectById(projectId).Name, dataAccess.GetAccountById(participantId).FullName)),
					dataAccess.GetAccountListByProjectId(projectId)
					);
		}

		public TaskProxy CreateTask(int accountId, TaskProxy task)
		{
			var t = dataAccess.CreateTask(task);
			return t;
		}

		public void UpdateTask(int accountId, TaskProxy task)
		{
			dataAccess.UpdateTask(task);
			dataAccess.AddHistory(
				new HistoryProxy(
					0,
					DateTime.Now,
					string.Format("User {0} updated task {1}, to project {2}", dataAccess.GetAccountById(accountId).FullName, task.Name, dataAccess.GetProjectById(task.Project.Id).Name)),
					dataAccess.GetAccountListByProjectId(task.Project.Id)
					);
		}

		public void DeleteTaskById(int accountId, int taskId)
		{
			var task = dataAccess.GetTaskById(taskId);
			dataAccess.AddHistory(
			new HistoryProxy(
				0,
				DateTime.Now,
				string.Format("User {0} deleted task {1}, to project {2}", dataAccess.GetAccountById(accountId).FullName, task.Name, dataAccess.GetProjectById(task.Project.Id).Name)),
				dataAccess.GetAccountListByProjectId(task.Project.Id)
				);
			dataAccess.DeleteTaskById(taskId);
		}

		public bool AddExecutor(int accountId, int executorId, int taskId)
		{
			var task = dataAccess.GetTaskById(taskId);
			dataAccess.AddHistory(
				new HistoryProxy(
					0,
					DateTime.Now,
					string.Format("User {0} added executor {1}, to task {2}", dataAccess.GetAccountById(accountId).FullName, dataAccess.GetAccountById(executorId).FullName, task.Name)),
					dataAccess.GetAccountListByProjectId(task.Project.Id)
					);
			var exec = dataAccess.AddExecutor(executorId, taskId);
			return exec;
		}

		public void DeleteExecutor(int accountId, int executorId, int taskId)
		{
			var task = dataAccess.GetTaskById(taskId);

			dataAccess.AddHistory(
				new HistoryProxy(
					0,
					DateTime.Now,
					string.Format("User {0} deleted executor {1}, from task {2}", dataAccess.GetAccountById(accountId).FullName, dataAccess.GetAccountById(executorId).FullName, task.Name)),
					dataAccess.GetAccountListByProjectId(task.Project.Id)
					);
			dataAccess.DeleteExecutor(executorId, taskId);
		}

		public CommentProxy CreateComment(int accountId, CommentProxy comment)
		{
			var comm = dataAccess.CreateComment(comment);
			return comm;
		}

		public void CreateProjectStatus(int accountId, int projectId, ProjectStatusProxy status)
		{
			dataAccess.CreateProjectStatus(projectId, status);
		}

		public void CreateTaskStatus(int accountId, int projectId, TaskStatusProxy status)
		{
			dataAccess.CreateTaskStatus(projectId, status);
		}

		public void DeleteProjectStatusById(int accountId, int statusId)
		{
			dataAccess.DeleteProjectStatusById(statusId);
		}

		public void DeleteTaskStatusById(int accountId, int statusId)
		{
			dataAccess.DeleteTaskStatusById(statusId);
		}
	}
}
