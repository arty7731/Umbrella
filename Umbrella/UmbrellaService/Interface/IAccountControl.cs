using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;
using UmbrellaService.Service;

namespace UmbrellaService.Interface
{
	[ServiceContract]
	//[ServiceContract(CallbackContract = typeof(IClientCallback), SessionMode = SessionMode.Required)]
	public interface IAccountControl
	{
		[OperationContract]
		AccountProxy GetAccountById(int id);
		[OperationContract]
		List<AccountProxy> GetAllAccounts();
		[OperationContract]
		List<AccountProxy> GetAccountListByProjectId(int projectId);
		[OperationContract]
		List<AccountProxy> GetAccountListByTaskId(int taskId);
		[OperationContract]
		void UpdateAccount(AccountProxy account);
	}
}
