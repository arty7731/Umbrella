using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Umbrella.Core;
using Umbrella.ServiceReference;
using UmbrellaService.DataLevel.ProxyModel;

namespace Umbrella
{
	/// <summary>
	/// Interaction logic for CreateStatusWindow.xaml
	/// </summary>
	public partial class CreateStatusWindow : Window
	{
		private ServiceClient service;
		ProjectProxy currentProject;

		public CreateStatusWindow(ServiceClient service, ProjectProxy project)
		{
			InitializeComponent();
			this.service = service;
			this.currentProject = project;
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			var projectId = currentProject.Id;
			btnSave.IsEnabled = false;
			if (rbprojectStatus.IsChecked.Value)
			{
				Task.Run(() => 
				{
					Dispatcher.Invoke(() => 
					{
						var statusList = service.GetProjectStatusListByProjectId(projectId);
						if (statusList.Any(s => s.Name == tbStatusName.Text)) return;
						service.CreateProjectStatus(User.Instance.Account.Id, projectId, new ProjectStatusProxy(0, tbStatusName.Text));
					});
				});
				DialogResult = true;
			}
			else if (rbTaskStatus.IsChecked.Value)
			{
				Task.Run(() =>
				{
					Dispatcher.Invoke(() =>
					{
						var statusList = service.GetTaskStatusListByProjectId(projectId);
						if (statusList.Any(s => s.Name == tbStatusName.Text)) return;
						service.CreateTaskStatus(User.Instance.Account.Id, projectId, new TaskStatusProxy(0, tbStatusName.Text));
					});
				});
				DialogResult = true;
			}
		}

		private void tbStatusName_KeyDown(object sender, KeyEventArgs e)
		{
			btnSave.IsEnabled = !string.IsNullOrEmpty(tbStatusName.Text);
		}
	}
}
