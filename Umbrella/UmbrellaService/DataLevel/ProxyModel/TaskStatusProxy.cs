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
	public class TaskStatusProxy
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }

		public TaskStatusProxy()
		{

		}

		public TaskStatusProxy(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public static List<TaskStatusProxy> GetTaskStatusList(List<TaskStatu> statusList)
		{
			var statusListProxy = new List<TaskStatusProxy>();
			foreach (var status in statusList)
			{
				statusListProxy.Add((TaskStatusProxy)status);
			}
			return statusListProxy;
		}

		public override string ToString()
		{
			return Name;
		}

		public static explicit operator TaskStatusProxy(TaskStatu status)
		{
			if (status == null) return null;
			return new TaskStatusProxy(status.Id, status.Name);
		}
	}
}
