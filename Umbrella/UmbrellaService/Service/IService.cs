using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.Interface;

namespace UmbrellaService.Service
{
	//[ServiceContract]
	[ServiceContract(CallbackContract = typeof(IClientCallback), SessionMode = SessionMode.Required)]
	public interface IService : IAuthorization, IProjectControl, ITaskControl, ICommentControl, IAccountControl, IStatusControl, IHistoryControl
	{

	}
}
