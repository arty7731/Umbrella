using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.Interface
{
	[ServiceContract]
	public interface IHistoryControl
	{
		[OperationContract]
		void AddHistory(HistoryProxy history, List<AccountProxy> readers);
		[OperationContract]
		void DeleteHistoryById(int id);
		[OperationContract]
		HistoryProxy GetHistoryById(int id);
		[OperationContract]
		List<HistoryProxy> GetAvalibleHistoryById(int id);
	}
}
