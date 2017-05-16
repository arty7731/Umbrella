using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbrella.ServiceReference;
using UmbrellaService.DataLevel.ProxyModel;

namespace Umbrella.Core
{
	public class User
	{
		static User instance = new User();

		public static User Instance {
			get
			{
				return instance;
			}
		}

		private User()
		{

		}

		public AccountProxy Account { get; set; }
		public TypeTab CurrentTab { get; set; }

		internal void Clean()
		{
			CurrentTab = TypeTab.History;
			Account = null;
		}
	}
}
