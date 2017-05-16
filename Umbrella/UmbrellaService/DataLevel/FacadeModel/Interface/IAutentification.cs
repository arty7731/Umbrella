using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.DataLevel.FacadeModel.Interface
{
	public interface IAutentefication
	{
		AccountProxy SignIn(string login, string password);
		AccountProxy SignUp(AccountProxy account);
		void SignOut(int id);

	}
}
