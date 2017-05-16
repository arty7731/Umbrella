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
	public class ProjectControl : IProjectControl
	{
		public bool AddParticipant(int participantId, int projectId)
		{
			using(var db = new DatabaseEntities())
			{
				var ap = db.AccountsProjects.Add(new AccountsProject()
				{
					Account = participantId,
					Project = projectId
				});
				db.SaveChanges();
				return ap != null;
			}
		}

		public void ChangeProjectStatus(int projectId, ProjectStatusProxy status)
		{
			using(var db = new DatabaseEntities())
			{
				var proj = db.Projects.FirstOrDefault(p => p.Id == projectId);
				if (proj != null) proj.ProjectStatusId = status.Id;
				db.SaveChanges();
			}
		}

		public ProjectProxy CreateProject(ProjectProxy project)
		{
			using(var db = new DatabaseEntities())
			{
				var proj = new Project()
				{
					Name = project.Name,
					Description = project.Description,
					DateStart = project.DateStart
				};
				if (project.ProjectStatus != null) proj.ProjectStatusId = project.ProjectStatus.Id;
				if (project.Owner != null) proj.Owner = project.Owner.Id;

				var p = db.Projects.Add(proj);

				db.SaveChanges();

				db.AccountsProjects.Add(new AccountsProject() { Account = project.Owner.Id, Project = p.Id });
				var s = db.ProjectStatus.Add(new ProjectStatu() { Name = "Added", Project = p.Id });
				db.SaveChanges();
				proj.ProjectStatusId = s.Id;
				db.TaskStatus.Add(new TaskStatu() { Name = "TODO:", Project = p.Id });
				db.SaveChanges();
				return GetProjectById(p.Id);

			}
		}

		public void DeleteParticipant(int participantId, int projectId)
		{
			using (var db = new DatabaseEntities())
			{
				db.AccountsProjects.Remove(db.AccountsProjects.FirstOrDefault(p => p.Project == projectId && p.Account == participantId));
				db.SaveChanges();
			}
		}

		public void DeleteProjectById(int accountId, int id)
		{
			using(var db = new DatabaseEntities())
			{
				var project = db.Projects.FirstOrDefault(p => p.Id == id && p.Owner == accountId);
				db.AccountsProjects.RemoveRange(db.AccountsProjects.Where(ap => ap.Project == project.Id));
				db.Projects.Remove(project);

				db.SaveChanges();
			}
		}

		public ProjectProxy GetProjectById(int id)
		{
			using(var db = new DatabaseEntities())
			{
				return (ProjectProxy)db.Projects.Include(p => p.ProjectStatus).FirstOrDefault(pr => pr.Id == id);
			}
		}

		public List<ProjectProxy> GetProjectsByOwnerId(int ownerId)
		{
			using (var db = new DatabaseEntities())
			{
				var projects = db.Projects.Where(p => p.Owner == ownerId).ToList();
				return ProjectProxy.GetListProjectProxy(projects);
			}
		}

		public List<ProjectProxy> GetProjectsByStatus(ProjectStatusProxy status)
		{
			using (var db = new DatabaseEntities())
			{
				var projects = db.Projects.Where(p => p.ProjectStatusId == status.Id).ToList();
				return ProjectProxy.GetListProjectProxy(projects);
			}
		}

		public List<ProjectProxy> GetProjectsInWhichParticipatesId(int id)
		{
			using (var db = new DatabaseEntities())
			{
				var accountsProjects = db.AccountsProjects.Where(p => p.Account == id).Include(ap => ap.Project1);
				var projects = new List<ProjectProxy>();
				foreach (var project in accountsProjects)
				{
					projects.Add((ProjectProxy)project.Project1);
				}
				return projects;
			}
		}

		public void UpdateProject(ProjectProxy project)
		{
			using(var db = new DatabaseEntities())
			{
				var proj = db.Projects.FirstOrDefault(p => p.Id == project.Id);
				if (proj == null) return;
				proj.Name = project.Name;
				proj.Description = project.Description;
				if(project.ProjectStatus != null) proj.ProjectStatusId = project.ProjectStatus.Id;
				proj.DateStart = project.DateStart;

				db.SaveChanges();
			}
		}
	}
}
