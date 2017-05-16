using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UmbrellaService.Service
{
	public class ServerClientCallback : IClientCallback
	{
		IClientCallback callback;

		public ServerClientCallback(IClientCallback callback)
		{
			this.callback = callback;
		}

		//public void UpdateComment(int taskId)
		//{
		//	callback.UpdateComment(taskId);
		//}

		//public void UpdateExecutor(int taskId)
		//{
		//	callback.UpdateExecutor(taskId);
		//}

		//public void UpdateParticipant(int projectId)
		//{
		//	callback.UpdateParticipant(projectId);
		//}
		//public void UpdateTask(int taskId)
		//{
		//	callback.UpdateTask(taskId);
		//}

		//[OperationContract(IsOneWay = true)]
		//public void UpdateProject(int projectId)
		//{
		//	callback.UpdateProject(projectId);
		//}

	}
}
