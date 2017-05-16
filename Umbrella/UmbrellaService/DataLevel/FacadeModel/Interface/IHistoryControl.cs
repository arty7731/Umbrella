using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.DataLevel.FacadeModel.Interface
{
	public interface IHistoryControl
	{
		void AddHistory(HistoryProxy history, List<AccountProxy> readers);
		void DeleteHistoryById(int id);
		HistoryProxy GetHistoryById(int id);
		List<HistoryProxy> GetAvalibleHistoryById(int id);
	}
}
