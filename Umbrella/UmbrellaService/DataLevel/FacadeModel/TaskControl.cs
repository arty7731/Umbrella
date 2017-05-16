using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.Database;
using UmbrellaService.DataLevel.FacadeModel.Interface;
using UmbrellaService.DataLevel.ProxyModel;
using System.Data.Entity;

namespace UmbrellaService.DataLevel.FacadeModel
{
	public class TaskControl : ITaskControl
	{
		public bool AddExecutor(int executorId, int taskId)
		{
			using(var db = new DatabaseEntities())
			{
				var at = db.AccountsTasks.Add(new AccountsTask() { Account = executorId, Task = taskId });
				db.SaveChanges();
				return at != null;
			}

		}

		public void ChangeTaskStatus(int accountId, int taskId, TaskStatusProxy status)
		{
			using (var db = new DatabaseEntities())
			{
				var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
				task.TaskStatusId = status.Id;
				task.CurrentExecutor = accountId;
				db.SaveChanges();
			}
		}

		public TaskProxy CreateTask(TaskProxy task)
		{
			using(var db = new DatabaseEntities())
			{
				var t = db.Tasks.Add(new Database.Task()
				{
					Name = task.Name,
					Description = task.Description,
					ProjectId = task.Project.Id,
					TaskStatusId = task.TaskStatus.Id
				});

				db.SaveChanges();

				return GetTaskById(t.Id);
			}
		}

		public void DeleteExecutor(int executorId, int taskId)
		{
			using (var db = new DatabaseEntities())
			{
				db.AccountsTasks.Remove(db.AccountsTasks.FirstOrDefault(a => a.Account == executorId && a.Task == taskId));
				db.SaveChanges();
			}
		}

		public void DeleteTaskById(int taskId)
		{
			using (var db = new DatabaseEntities())
			{
				db.AccountsTasks.RemoveRange(db.AccountsTasks.Where(a => a.Task == taskId));
				db.Tasks.Remove(db.Tasks.FirstOrDefault(t => t.Id == taskId));
				db.SaveChanges();
			}
		}

		public void DeleteTasksByProjectId(int projectId)
		{
			using(var db = new DatabaseEntities())
			{
				var tasks = db.Projects.Include(p => p.Tasks).FirstOrDefault(p => p.Id == projectId).Tasks;
				foreach (var task in tasks)
				{
					DeleteTaskById(task.Id);
				}
				db.SaveChanges();
			}
		}

		public TaskProxy GetTaskById(int id)
		{
			using(var db = new DatabaseEntities())
			{
				return (TaskProxy)db.Tasks.Include(t => t.Project).Include(t => t.Account).FirstOrDefault(t => t.Id == id);
			}
		}

		public List<TaskProxy> GetTasksAvailableById(int id)
		{
			using (var db = new DatabaseEntities())
			{
				var tasks = db.AccountsTasks.Where(at => at.Account == id).Include(t => t.Task1).Select(t => t.Task1).Include(t => t.Account).ToList();
				return TaskProxy.GetListTasksProxy(tasks);
			}
		}

		public List<TaskProxy> GetTasksByProjectId(int projectId)
		{
			using (var db = new DatabaseEntities())
			{
				var tasks = db.Tasks.Where(t => t.ProjectId == projectId).ToList();
				return TaskProxy.GetListTasksProxy(tasks);
			}
		}

		public List<TaskProxy> GetTasksByStatus(TaskStatusProxy status)
		{
			using (var db = new DatabaseEntities())
			{
				var tasks = db.Tasks.Where(t => t.TaskStatusId == status.Id).ToList();
				return TaskProxy.GetListTasksProxy(tasks);
			}
		}

		public void SetCurrentExecutor(int executorId)
		{
			throw new NotImplementedException();
		}

		public void UpdateTask(TaskProxy task)
		{
			using (var db = new DatabaseEntities())
			{
				var oldTask = db.Tasks.FirstOrDefault(t => t.Id == task.Id);
				if (oldTask == null) return;
				oldTask.Name = task.Name;
				oldTask.Description = task.Description;

				db.SaveChanges();
			}
		}
	}
}
