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
	public class ProjectStatusProxy
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }

		public ProjectStatusProxy()
		{

		}

		public ProjectStatusProxy(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
		public static List<ProjectStatusProxy> GetProjectStatusList(List<ProjectStatu> statusList)
		{
			var statusListProxy = new List<ProjectStatusProxy>();
			foreach (var status in statusList)
			{
				statusListProxy.Add((ProjectStatusProxy)status);
			}
			return statusListProxy;
		}

		public static explicit operator ProjectStatusProxy(ProjectStatu status)
		{
			if (status == null) return null;
			return new ProjectStatusProxy(status.Id, status.Name);
		}


	}
}
