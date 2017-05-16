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
	class StatusControl : IStatusControl
	{
		public void CreateProjectStatus(int projectId, ProjectStatusProxy status)
		{
			using(var db = new DatabaseEntities())
			{
				db.ProjectStatus.Add(new ProjectStatu()
				{
					Name = status.Name,
					Project = projectId
				});
				db.SaveChanges();
			}
		}

		public void CreateTaskStatus(int projectId, TaskStatusProxy status)
		{
			using (var db = new DatabaseEntities())
			{
				db.TaskStatus.Add(new TaskStatu()
				{
					Name = status.Name,
					Project = projectId
				});
				db.SaveChanges();
			}
		}

		public void DeleteProjectStatusById(int statusId)
		{
			using(var db = new DatabaseEntities())
			{
				db.ProjectStatus.Remove(db.ProjectStatus.FirstOrDefault(ps => ps.Id == statusId));
				db.SaveChanges();
			}
		}

		public void DeleteProjectStatusByProjectId(int projectId)
		{
			using (var db = new DatabaseEntities())
			{
				var statusList = db.Projects.Include(p => p.ProjectStatus).FirstOrDefault(p => p.Id == projectId).ProjectStatus;
				db.ProjectStatus.RemoveRange(statusList);
				db.SaveChanges();
			}
		}

		public void DeleteTaskStatusById(int statusId)
		{
			using (var db = new DatabaseEntities())
			{
				db.TaskStatus.Remove(db.TaskStatus.FirstOrDefault(ts => ts.Id == statusId));
				db.SaveChanges();
			}
		}

		public void DeleteTaskStatusByProjectId(int projectId)
		{
			using (var db = new DatabaseEntities())
			{
				var statusList = db.Projects.Include(p => p.TaskStatus).FirstOrDefault(p => p.Id == projectId).TaskStatus;
				db.TaskStatus.RemoveRange(statusList);
				db.SaveChanges();
			}
		}

		public List<ProjectStatusProxy> GetProjectStatusListByProjectId(int projectId)
		{
			using (var db = new DatabaseEntities())
			{
				var statusList = db.Projects.Include(p => p.ProjectStatus).FirstOrDefault(p => p.Id == projectId).ProjectStatus.ToList();
				return ProjectStatusProxy.GetProjectStatusList(statusList);
			}
		}

		public List<TaskStatusProxy> GetTaskStatusListByProjectId(int projectId)
		{
			using (var db = new DatabaseEntities())
			{
				var statusList = db.Projects.Include(p => p.TaskStatus).FirstOrDefault(p => p.Id == projectId).TaskStatus.ToList();
				return TaskStatusProxy.GetTaskStatusList(statusList);
			}
		}
	}
}
