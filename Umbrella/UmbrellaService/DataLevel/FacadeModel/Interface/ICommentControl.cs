using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.ProxyModel;

namespace UmbrellaService.DataLevel.FacadeModel.Interface
{
	public interface ICommentControl
	{
		CommentProxy CreateComment(CommentProxy comment);
		void DeleteCommentById(int id, int accountId);
		void DeleteCommentsByTaskId(int taskId);
		List<CommentProxy> GetCommentsByTaskId(int taskId);


	}
}
