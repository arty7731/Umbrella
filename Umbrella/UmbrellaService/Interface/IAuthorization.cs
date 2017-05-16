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
	public interface IAuthorization
	{
		[OperationContract]
		AccountProxy SignIn(string login, string password);
		[OperationContract]
		AccountProxy SignUp(string login, string password, string email, string fullName, int age);
		[OperationContract]
		void LogOut(int id);
	}
}
