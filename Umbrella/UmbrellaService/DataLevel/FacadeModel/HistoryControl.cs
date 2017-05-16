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
	public class HistoryControl : IHistoryControl
	{
		public void AddHistory(HistoryProxy history, List<AccountProxy> readers)
		{
			using(var db = new DatabaseEntities())
			{
				var h = db.Histories.Add(new History()
				{
					Date = history.Date,
					Action = history.Action
				});
				db.SaveChanges();

				foreach (var reader in readers)
				{
					db.AccountHistories.Add(new AccountHistory() { Account = reader.Id, History = h.Id });
				}
				db.SaveChanges();
			}
		}

		public void DeleteHistoryById(int id)
		{
			using (var db = new DatabaseEntities())
			{
				var ahList = db.AccountHistories.Where(ah => ah.History == id);
				db.AccountHistories.RemoveRange(ahList);

				db.SaveChanges();

				db.Histories.Remove(db.Histories.FirstOrDefault(h => h.Id == id));

				db.SaveChanges();
			}
		}

		public List<HistoryProxy> GetAvalibleHistoryById(int id)
		{
			using (var db = new DatabaseEntities())
			{
				List<HistoryProxy> historyList = HistoryProxy.GetHistoryProxyList(db.AccountHistories.Include(ah => ah.History1).Where(ah => ah.Account == id).Select(ah => ah.History1).ToList());
				historyList.Reverse();
				return historyList;
			}
		}

		public HistoryProxy GetHistoryById(int id)
		{
			using (var db = new DatabaseEntities())
			{
				return (HistoryProxy)db.Histories.FirstOrDefault(h => h.Id == id);
			}
		}
	}
}
