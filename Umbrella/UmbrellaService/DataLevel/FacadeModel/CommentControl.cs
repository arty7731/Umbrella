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
	public class CommentControl : ICommentControl
	{
		public CommentProxy CreateComment(CommentProxy comment)
		{
			using(var db = new DatabaseEntities())
			{
				var com = db.Comments.Add(new Comment()
				{
					Text = comment.Text,
					Owner = comment.Owner.Id,
					TaskId = comment.Task.Id,
					DatePublic = comment.DatePublic
				});

				db.SaveChanges();
				return (CommentProxy)com;
			}
		}

		public void DeleteCommentById(int id, int accountId)
		{
			using(var db = new DatabaseEntities())
			{
				db.Comments.Remove(db.Comments.FirstOrDefault(c => c.Id == id && c.Owner == accountId));
				db.SaveChanges();
			}
		}

		public void DeleteCommentsByTaskId(int taskId)
		{
			using(var db = new DatabaseEntities())
			{
				db.Comments.RemoveRange(db.Tasks.Include(t => t.Comments).FirstOrDefault(t => t.Id == taskId).Comments);
				db.SaveChanges();
			}
		}

		public List<CommentProxy> GetCommentsByTaskId(int taskId)
		{
			using (var db = new DatabaseEntities())
			{
				return CommentProxy.GetListCommentsProxy(db.Tasks.Include(t => t.Comments).FirstOrDefault(t => t.Id == taskId).Comments.ToList());
			}
		}
	}
}
