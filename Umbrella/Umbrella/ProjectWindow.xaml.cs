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
	/// Interaction logic for ProjectWindow.xaml
	/// </summary>
	public partial class ProjectWindow : Window
	{
		private ServiceClient service;

		ProjectProxy currentProject;
		TaskProxy selectedTask;
		AccountProxy selectedParticipant;

		public ProjectWindow(ServiceClient service, ProjectProxy project, ViewType typeView)
		{
			InitializeComponent();
			this.service = service;
			this.currentProject = project;

			InitializeProject(typeView);

			btnTaskEdit.IsEnabled = false;
			btnTaskDelete.IsEnabled = false;
			btnParticipantDelete.IsEnabled = false;
			btnStatusEdit.IsEnabled = false;
			btnStatusDelete.IsEnabled = false;
		}

		private void InitializeProject(ViewType typeView)
		{
			var project = currentProject;

			tbProjectName.Text = project.Name;
			tbProjectDescription.Text = project.Description;
			dpProjectDateCreated.SelectedDate = project.DateStart;
			lblProjectOwner.Content = project.Owner.FullName;
			lvTaskList.ItemsSource = service.GetTasksByProjectId(project.Id);
			lvParticipantList.ItemsSource = service.GetAccountListByProjectId(project.Id);
			UpdateComboBoxStatus(project);
			lvProjectStatus.ItemsSource = service.GetProjectStatusListByProjectId(project.Id);
			lvTaskStatus.ItemsSource = service.GetTaskStatusListByProjectId(project.Id);
			switch (typeView)
			{
				case ViewType.View:
					InitializeControlForView();
					break;
				case ViewType.Edit:
					InitializeControlForEdit();
					break;
				case ViewType.Create:
					InitializeControlForCreate();
					break;
			}
		}
		private void InitializeControlForView()
		{
			tbProjectName.IsReadOnly = true;
			tbProjectDescription.IsReadOnly = true;
			dpProjectDateCreated.IsEnabled = false;
			cbProjectStatus.IsEnabled = false;

			wpButtonPanel.Visibility = Visibility.Collapsed;
		}
		private void InitializeControlForEdit()
		{
			tbProjectName.IsReadOnly = false;
			tbProjectDescription.IsReadOnly = false;
			dpProjectDateCreated.IsEnabled = true;
			cbProjectStatus.IsEnabled = true;

			wpButtonPanel.Visibility = Visibility.Visible;
		}
		private void InitializeControlForCreate()
		{
			tbProjectName.IsReadOnly = false;
			tbProjectDescription.IsReadOnly = false;
			dpProjectDateCreated.IsEnabled = true;
			cbProjectStatus.IsEnabled = true;

			wpButtonPanel.Visibility = Visibility.Visible;
		}
		#region Selection changed
		private void lvTaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (lvTaskList.SelectedItem != null)
			{
				selectedTask = lvTaskList.SelectedItem as TaskProxy;
				btnTaskEdit.IsEnabled = true;
				btnTaskDelete.IsEnabled = true;
			}
		}
		private void lvParticipantList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var participant = lvParticipantList.SelectedItem as AccountProxy;
			if (participant != null)
			{
				btnParticipantDelete.IsEnabled = currentProject.Owner.Id != participant.Id;
			}
		}
		private void lvProjectStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			lvTaskStatus.SelectedItem = null;
			var projectStatus = lvProjectStatus.SelectedItem as ProjectStatusProxy;
			if (projectStatus != null)
			{
				if (currentProject.ProjectStatus == null)
				{
					btnStatusDelete.IsEnabled = true;
					btnStatusEdit.IsEnabled = true;
					return;
				}

				btnStatusDelete.IsEnabled = currentProject.ProjectStatus.Id != projectStatus.Id;
				btnStatusEdit.IsEnabled = currentProject.ProjectStatus.Id != projectStatus.Id;
			}
		}
		private void lvTaskStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			lvProjectStatus.SelectedItem = null;
			if (lvTaskStatus.SelectedItem != null)
			{
				btnStatusDelete.IsEnabled = true;
				btnStatusEdit.IsEnabled = true;
			}
		}
		#endregion
		#region Button click
		private void btnTaskAdd_Click(object sender, RoutedEventArgs e)
		{
			var t = service.CreateTask(User.Instance.Account.Id, new TaskProxy(0, "", "", currentProject, service.GetTaskStatusListByProjectId(currentProject.Id).FirstOrDefault()));
			selectedTask = t;
			var task = new TaskWindow(service, selectedTask, ViewType.Edit);
			if (task.ShowDialog() == true)
			{
				UpdateTaskList(currentProject.Id);
			}
			else
			{
				service.DeleteTaskById(User.Instance.Account.Id, t.Id);
			}
		}
		private void btnTaskEdit_Click(object sender, RoutedEventArgs e)
		{
			var task = new TaskWindow(service, selectedTask, ViewType.Edit);
			if (task.ShowDialog() == true)
			{
				UpdateTaskList(currentProject.Id);
			}
		}
		private void btnParticipantDelete_Click(object sender, RoutedEventArgs e)
		{
			var participant = lvParticipantList.SelectedItem as AccountProxy;
			if (participant != null)
			{
				service.DeleteParticipant(User.Instance.Account.Id, participant.Id, currentProject.Id);
				UpdateParcitipantList(currentProject.Id);
			}
		}
		private void btnTaskDelete_Click(object sender, RoutedEventArgs e)
		{
			var task = lvTaskList.SelectedItem as TaskProxy;
			if (task != null)
			{
				service.DeleteTaskById(User.Instance.Account.Id, task.Id);
				UpdateTaskList(currentProject.Id);
			}
		}
		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			var name = tbProjectName.Text;
			var description = tbProjectDescription.Text;
			var status = cbProjectStatus.SelectedValue as ProjectStatusProxy;
			var dateStart = dpProjectDateCreated.SelectedDate;
			var project = currentProject;
			if (project == null)
			{
				project = new ProjectProxy();
				project.Name = name;
				project.Owner = User.Instance.Account;
				project.Description = description;
				project.ProjectStatus = status;
				project.DateStart = dateStart.Value;
				service.CreateProject(User.Instance.Account.Id, project);
			}
			else
			{
				project.Name = name;
				project.Description = description;
				project.ProjectStatus = status;
				project.DateStart = dateStart.Value;

				service.UpdateProject(User.Instance.Account.Id, project);
			}

			DialogResult = true;
		}
		private void btnParticipantAdd_Click(object sender, RoutedEventArgs e)
		{
			var accountList = service.GetAllAccounts().ToList();
			foreach (var acc in service.GetAccountListByProjectId(currentProject.Id))
			{
				var ac = accountList.FirstOrDefault(a => a.Id == acc.Id);
				accountList.Remove(ac);
			}
			var pl = new SelectAccountWindows(service, accountList);
			if (pl.ShowDialog() == true)
			{
				service.AddParticipant(User.Instance.Account.Id, pl.SelectAccountId, currentProject.Id);
				UpdateParcitipantList(currentProject.Id);
			}
		}
		private void btnStatusAdd_Click(object sender, RoutedEventArgs e)
		{
			var status = new CreateStatusWindow(service, currentProject);
			if (status.ShowDialog() == true)
			{
				UpdateListStatus(currentProject.Id);
			}
		}
		private void btnStatusDelete_Click(object sender, RoutedEventArgs e)
		{
			var pStatus = lvProjectStatus.SelectedItem as ProjectStatusProxy;
			var tStatus = lvTaskStatus.SelectedItem as TaskStatusProxy;
			if (pStatus != null)
			{
				service.DeleteProjectStatusById(User.Instance.Account.Id, pStatus.Id);
			}
			else if (tStatus != null)
			{
				service.DeleteTaskStatusById(User.Instance.Account.Id, tStatus.Id);
			}
			UpdateListStatus(currentProject.Id);
		}
		private void btnStatusEdit_Click(object sender, RoutedEventArgs e)
		{
			UpdateListStatus(currentProject.Id);
		}

		#endregion
		#region Update control element
		private void UpdateListStatus(int projectId)
		{
			Task.Run(() =>
			{
				Dispatcher.Invoke(() =>
				{
					lvProjectStatus.ItemsSource = service.GetProjectStatusListByProjectId(projectId);
					lvTaskStatus.ItemsSource = service.GetTaskStatusListByProjectId(projectId);
					UpdateComboBoxStatus(currentProject);
				});
			});
		}
		private void UpdateTaskList(int projectId)
		{
			Task.Run(() =>
			{
				lvTaskList.Dispatcher.Invoke(() =>
				{
					lvTaskList.ItemsSource = service.GetTasksByProjectId(projectId);
				});
			});
		}
		private void UpdateParcitipantList(int projectId)
		{
			Task.Run(() =>
			{
				lvParticipantList.Dispatcher.Invoke(() =>
				{
					lvParticipantList.ItemsSource = null;
					lvParticipantList.ItemsSource = service.GetAccountListByProjectId(projectId);
				});
			});
		}
		private void UpdateComboBoxStatus(ProjectProxy project)
		{
			Task.Run(() =>
			{
				cbProjectStatus.Dispatcher.Invoke(() =>
				{
					var statusList = service.GetProjectStatusListByProjectId(project.Id);
					cbProjectStatus.ItemsSource = statusList;
					if (project.ProjectStatus != null) cbProjectStatus.SelectedItem = statusList.FirstOrDefault(p => p.Id == project.ProjectStatus.Id);
				});
			});
		}
		#endregion
	}
}
