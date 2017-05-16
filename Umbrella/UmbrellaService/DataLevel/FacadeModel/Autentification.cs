using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.Database;
using UmbrellaService.DataLevel.FacadeModel.Interface;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.DataLevel.FacadeModel
{
	public class Autentefication : IAutentefication
	{
		public AccountProxy SignIn(string login, string password)
		{
			using (var db = new DatabaseEntities())
			{
				var acc = db.Accounts.FirstOrDefault(a => a.Login == login && a.Password == password);
				if (acc != null) acc.Status = (int)AccountStatus.Online;
				db.SaveChanges();
				return (AccountProxy)acc;
			}
		}

		public void SignOut(int id)
		{
			try
			{
				using(var db = new DatabaseEntities())
				{
					var acc = db.Accounts.FirstOrDefault(a => a.Id == id);
					if (acc == null) throw new Exception("Account not found");
					else acc.Status = (int)AccountStatus.Offline;
					db.SaveChanges();
				}
			}
			catch (Exception err)
			{

				throw err;
			}
		}

		public AccountProxy SignUp(AccountProxy account)
		{
			try
			{
				using (var db = new DatabaseEntities())
				{
					if(db.Accounts.Count() != 0 )
					if (db.Accounts.Any(a => a.Login == account.Login)) return null;
					var acc = db.Accounts.Add(new Account()
					{
						Login = account.Login,
						Password = account.Password,
						Email = account.Email,
						FullName = account.FullName,
						Status = (int)AccountStatus.Online
					});

					db.SaveChanges();
					return (AccountProxy)acc;
				}
			}
			catch (Exception err)
			{
				throw err;
			}
		}
	}
}
