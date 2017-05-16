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
	public class HistoryProxy
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public DateTime Date { get; set; }
		[DataMember]
		public string Action { get; set; }

		public HistoryProxy()
		{

		}

		public HistoryProxy(int id, DateTime date, string action)
		{
			this.Id = id;
			this.Date = date;
			this.Action = action;
		}

		public static List<HistoryProxy> GetHistoryProxyList(List<History> histories)
		{
			var historiesProxy = new List<HistoryProxy>();
			foreach (var history in histories)
			{
				historiesProxy.Add((HistoryProxy)history);
			}
			return historiesProxy;
		}
		public static explicit operator HistoryProxy(History history)
		{
			if (history == null) return null;
			return new HistoryProxy(history.Id, history.Date, history.Action);
		}
	}
}
