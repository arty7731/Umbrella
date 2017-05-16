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
	public class AccountControl : IAccountControl
	{
		public AccountProxy GetAccountById(int id)
		{
			using(var db = new DatabaseEntities())
			{
				return (AccountProxy)db.Accounts.FirstOrDefault(a => a.Id == id);
			}
		}

		public List<AccountProxy> GetAccountListByProjectId(int projectId)
		{
			using (var db = new DatabaseEntities())
			{
				return AccountProxy.GetAccountProxyList(db.AccountsProjects.Include(ap => ap.Account1).Where(ap => ap.Project == projectId).Select(ap => ap.Account1).ToList());
			}
		}

		public List<AccountProxy> GetAccountListByTaskId(int taskId)
		{
			using (var db = new DatabaseEntities())
			{
				return AccountProxy.GetAccountProxyList(db.AccountsTasks.Include(ap => ap.Account1).Where(ap => ap.Task == taskId).Select(ap => ap.Account1).ToList());
			}
		}

		public List<AccountProxy> GetAllAccounts()
		{
			using(var db = new DatabaseEntities())
			{
				return AccountProxy.GetAccountProxyList(db.Accounts.ToList());
			}
		}

		public void UpdateAccount(AccountProxy account)
		{
			using (var db = new DatabaseEntities())
			{
				var acc = db.Accounts.FirstOrDefault(a => a.Id == account.Id);
				acc.Password = account.Password;
				acc.FullName = account.FullName;
				acc.Email = account.Email;
				db.SaveChanges();
			}
		}
	}
}
