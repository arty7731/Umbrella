using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.DataLevel.FacadeModel.Interface
{
	public interface IAccountControl
	{
		AccountProxy GetAccountById(int id);
		List<AccountProxy> GetAllAccounts();
		List<AccountProxy> GetAccountListByProjectId(int projectId);
		List<AccountProxy> GetAccountListByTaskId(int taskId);
		void UpdateAccount(AccountProxy account);
	}
}
