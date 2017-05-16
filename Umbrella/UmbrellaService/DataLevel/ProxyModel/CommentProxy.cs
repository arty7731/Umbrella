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
	public class CommentProxy
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Text { get; set; }
		[DataMember]
		public AccountProxy Owner { get; set; }
		[DataMember]
		public TaskProxy Task { get; set; }
		[DataMember]
		public DateTime DatePublic { get; set; }

		public CommentProxy()
		{
			
		}

		public CommentProxy(int id, string text, AccountProxy owner, TaskProxy task, DateTime datePublic)
		{
			this.Id = id;
			this.Text = text;
			this.Owner = owner;
			this.Task = task;
			this.DatePublic = datePublic;
		}

		public static List<CommentProxy> GetListCommentsProxy(List<Comment> comments)
		{
			var commentsProxy = new List<CommentProxy>();
			foreach (var comment in comments)
			{
				commentsProxy.Add((CommentProxy)comment);
			}
			return commentsProxy;
		}

		public static explicit operator CommentProxy(Comment comment)
		{
			if (comment == null) return null;
			return new CommentProxy(comment.Id, comment.Text, (AccountProxy)comment.Account, (TaskProxy)comment.Task, comment.DatePublic);
		}
	}
}
