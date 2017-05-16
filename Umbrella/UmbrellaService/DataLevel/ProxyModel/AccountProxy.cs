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
	public class AccountProxy
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Login { get; set; }
		[DataMember]
		public string Password { get; set; }
		[DataMember]
		public string Email { get; set; }
		[DataMember]
		public string FullName { get; set; }
		[DataMember]
		public AccountStatus Status { get; set; }

		public AccountProxy()
		{

		}

		public AccountProxy(int id, string login, string password, AccountStatus status, string email, string fullName)
		{
			this.Id = id;
			this.Login = login;
			this.Password = password;
			this.Email = email;
			this.FullName = fullName;
			this.Status = status;
		}

		public static List<AccountProxy> GetAccountProxyList(List<Account> accounts)
		{
			var accountsProxy = new List<AccountProxy>();
			foreach (var account in accounts)
			{
				accountsProxy.Add((AccountProxy)account);
			}
			return accountsProxy;
		}
		public static explicit operator AccountProxy(Account account)
		{
			if (account == null) return null;
			return new AccountProxy(account.Id, account.Login, account.Password, (AccountStatus)account.Status, account.Email, account.FullName);
		}
	}

}
