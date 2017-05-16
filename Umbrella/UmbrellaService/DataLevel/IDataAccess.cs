using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaService.DataLevel.FacadeModel.Interface;

namespace UmbrellaService.DataLevel
{
	public interface IDataAccess : 
		IAutentefication, IProjectControl, ITaskControl, ICommentControl, IAccountControl, IStatusControl, IHistoryControl
	{

	}
}
