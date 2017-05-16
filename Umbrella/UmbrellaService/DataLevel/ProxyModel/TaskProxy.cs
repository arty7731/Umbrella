using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UmbrellaService.DataLevel.ProxyModel
{
	[DataContract]
	public class TaskProxy
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public AccountProxy Executor { get; set; }
		[DataMember]
		public ProjectProxy Project { get; set; }
		[DataMember]
		public TaskStatusProxy TaskStatus { get; set; }

		public TaskProxy()
		{

		}

		public static List<TaskProxy> GetListTasksProxy(List<Database.Task> tasks)
		{
			List<TaskProxy> tasksProxy = new List<TaskProxy>();
			foreach (var task in tasks)
			{
				tasksProxy.Add((TaskProxy)task);
			}
			return tasksProxy;
		}

		public TaskProxy(int id, string name, string description, ProjectProxy project, TaskStatusProxy taskStatus)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
			this.Project = project;
			this.TaskStatus = taskStatus;		  
		}

		public TaskProxy(int id, string name, string description, AccountProxy executor, ProjectProxy project, TaskStatusProxy taskStatus)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
			this.Executor = executor;
			this.Project = project;
			this.TaskStatus = taskStatus;
		}


		public static explicit operator TaskProxy(Database.Task task)
		{
			if (task == null) return null;
			return new TaskProxy(task.Id, task.Name, task.Description, (AccountProxy)task.Account, (ProjectProxy)task.Project, (TaskStatusProxy)task.TaskStatu);
		}
	}
}
