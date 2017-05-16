using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.Database;

namespace UmbrellaService.DataLevel.ProxyModel
{
	[DataContract]
	public class ProjectProxy
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public DateTime DateStart { get; set; }
		[DataMember]
		public ProjectStatusProxy ProjectStatus { get; set; }
		[DataMember]
		public AccountProxy Owner { get; set; }

		public ProjectProxy()
		{

		}

		public static List<ProjectProxy> GetListProjectProxy(List<Project> projects)
		{
			List<ProjectProxy> projectsProxy = new List<ProjectProxy>();
			foreach (var project in projects)
			{
				projectsProxy.Add((ProjectProxy)project);
			}
			return projectsProxy;
		}

		public ProjectProxy(int id, string name, string description, DateTime dateStart, ProjectStatusProxy projectStatus, AccountProxy owner)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
			this.DateStart = dateStart;
			this.ProjectStatus = projectStatus;
			this.Owner = owner;
		}

		public static explicit operator ProjectProxy(Project project)
		{
			if (project == null) return null;
			return new ProjectProxy(project.Id, project.Name, project.Description, project.DateStart, (ProjectStatusProxy)project.ProjectStatus.FirstOrDefault(p => p.Id == project.ProjectStatusId), (AccountProxy)project.Account);
		}

	}
}
