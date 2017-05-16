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
	public interface ICommentControl
	{
		[OperationContract]
		CommentProxy CreateComment(int accountId, CommentProxy comment);
		[OperationContract]
		void DeleteCommentById(int id, int accountId);
		[OperationContract]
		void DeleteCommentsByTaskId(int taskId);
		[OperationContract]
		List<CommentProxy> GetCommentsByTaskId(int taskId);
	}
}
