using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Umbrella.Core;
using Umbrella.ServiceReference;
using UmbrellaService.DataLevel.ProxyModel;

namespace Umbrella
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ServiceClient service { get; set; }

		ProjectProxy selectedProject;
		TaskProxy selectedPrjTask;
		TaskProxy selectedTsTask;

		public MainWindow()
		{
			InitializeComponent();
			var callback = new ClientCallback();
			service = new ServiceClient();
			service.Open();

			this.Closed += (e, s) =>
			{
				if (User.Instance.Account != null) service.LogOut(User.Instance.Account.Id);
				service.Close();
			};

			this.Loaded += (e, s) =>
			{
				if (ShowAuthorizationWindow())
				{
					if (User.Instance.Account != null) lblInfoFullname.Content = User.Instance.Account.FullName;
					RefreshPrj();
					RefreshTs();
					RefreshHist();

				}
			};

		}
		#region Control event
		private void btnInfoLogOut_Click(object sender, RoutedEventArgs e)
		{
			ClearPrj();
			if (User.Instance.Account != null) service.LogOut(User.Instance.Account.Id);

			if (ShowAuthorizationWindow())
			{
				if (User.Instance.Account != null) lblInfoFullname.Content = User.Instance.Account.FullName;
				RefreshPrj();
				RefreshTs();
				RefreshHist();
			}
		}
		private void btnPrjCreateProject_Click(object sender, RoutedEventArgs e)
		{
			var project = service.CreateProject(User.Instance.Account.Id, new ProjectProxy(0, "", "", DateTime.Now, null, User.Instance.Account));
			if (ShowCreateProject(ViewType.Create, project))
			{
				RefreshPrj();
			}
			else
			{
				service.DeleteProjectById(User.Instance.Account.Id, project.Id);
			}
		}
		private void btnPrjUpdateProject_Click(object sender, RoutedEventArgs e)
		{
			if (ShowCreateProject(ViewType.Edit, selectedProject))
			{
				RefreshPrj();
			}
		}
		private void btnPrjDeleteProject_Click(object sender, RoutedEventArgs e)
		{
			service.DeleteProjectById(User.Instance.Account.Id, selectedProject.Id);
			UpdatePrjProjectList(service.GetProjectsInWhichParticipatesId(User.Instance.Account.Id));
		}
		private void lvPrjProjectList_Selected(object sender, RoutedEventArgs e)
		{
			selectedProject = (lvPrjProjectList.SelectedItem as ProjectProxy);
			if (selectedProject == null) return;
			UpdatePrjParticipantList(service.GetAccountListByProjectId(selectedProject.Id));
			UpdatePrjTaskList(service.GetTasksByProjectId(selectedProject.Id));
		}
		private void lvPrjTaskList_Selected(object sender, RoutedEventArgs e)
		{
			selectedPrjTask = (lvPrjTaskList.SelectedItem as TaskProxy);
		}
		private void btnPrjCreateTask_Click(object sender, RoutedEventArgs e)
		{
			var t = service.CreateTask(User.Instance.Account.Id, new TaskProxy(0, "", "", selectedProject, service.GetTaskStatusListByProjectId(selectedProject.Id).FirstOrDefault()));
			selectedPrjTask = t;
			if (ShowCreateTask(ViewType.Edit, selectedPrjTask))
			{
				if (selectedProject != null) UpdatePrjTaskList(service.GetTasksByProjectId(selectedProject.Id));
			}
			else
			{
				service.DeleteTaskById(User.Instance.Account.Id, t.Id);
			}
		}
		private void btnPrjUpdateTask_Click(object sender, RoutedEventArgs e)
		{
			ShowCreateTask(ViewType.Edit, selectedPrjTask);
			if (selectedProject != null) UpdatePrjTaskList(service.GetTasksByProjectId(selectedProject.Id));
		}

		private void btnPrjDeleteTask_Click(object sender, RoutedEventArgs e)
		{
			service.DeleteTaskById(User.Instance.Account.Id, selectedPrjTask.Id);
			if (selectedProject != null) UpdatePrjTaskList(service.GetTasksByProjectId(selectedProject.Id));
		}
		private void lvPrjProjectList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ShowCreateProject(ViewType.View, selectedProject);
		}
		#endregion
		#region Update methods
		private void UpdatePrjProjectList(ProjectProxy[] projects)
		{
			Task.Run(() =>
			{
				lvPrjProjectList.Dispatcher.Invoke(() =>
				{
					lvPrjProjectList.ItemsSource = projects;
				});
			});
		}
		private void UpdatePrjTaskList(TaskProxy[] tasks)
		{
			Task.Run(() =>
			{
				lvPrjTaskList.Dispatcher.Invoke(() =>
				{
					lvPrjTaskList.ItemsSource = tasks;
				});
			});
		}
		private void UpdatePrjParticipantList(AccountProxy[] accounts)
		{
			Task.Run(() =>
			{
				lvPrjPartticipantList.Dispatcher.Invoke(() =>
				{
					lvPrjPartticipantList.ItemsSource = accounts;
				});
			});
		}
		private void RefreshPrj()
		{
			if (User.Instance.Account == null) return;
			var projects = service.GetProjectsInWhichParticipatesId(User.Instance.Account.Id);
			UpdatePrjProjectList(projects);

			if (selectedProject == null)
			{
				return;
			}
			var tasks = service.GetTasksByProjectId(selectedProject.Id);
			var accounts = service.GetAccountListByProjectId(selectedProject.Id);
			var taskStatusList = service.GetTaskStatusListByProjectId(selectedProject.Id);
			UpdatePrjTaskList(tasks);
			UpdatePrjParticipantList(accounts);
		}
		private void RefreshTs()
		{
			var tasks = service.GetTasksAvailableById(User.Instance.Account.Id);
			UpdateTsTaskList(tasks);
		}

		private void ClearPrj()
		{
			User.Instance.Clean();

			lvPrjPartticipantList.ItemsSource = null;
			lvPrjProjectList.ItemsSource = null;
			lvPrjTaskList.ItemsSource = null;


			lvPrjPartticipantList.Items.Clear();
			lvPrjProjectList.Items.Clear();
			lvPrjTaskList.Items.Clear();
		}
		#endregion
		#region Show window methods
		private bool ShowAuthorizationWindow()
		{
			if (User.Instance.Account != null) service.LogOut(User.Instance.Account.Id);
			User.Instance.Account = null;
			AuthorizationWindow auth = new AuthorizationWindow(service);
			auth.Owner = this;
			return auth.ShowDialog().Value;
		}
		private bool ShowCreateProject(ViewType typeView, ProjectProxy project)
		{
			var proj = new ProjectWindow(service, project, typeView);
			proj.Owner = this;
			return proj.ShowDialog().Value;
		}
		private bool ShowCreateTask(ViewType typeView, TaskProxy task)
		{
			var taskWindow = new TaskWindow(service, task, typeView);
			taskWindow.Owner = this;
			return taskWindow.ShowDialog().Value;
		}


		#endregion

		private void lvPrjTaskList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ShowCreateTask(ViewType.View, selectedPrjTask);
			if (selectedProject != null) UpdatePrjTaskList(service.GetTasksByProjectId(selectedProject.Id));
		}

		//##########################################
		//###########Tab item "my task"#############
		//##########################################

		private void InitializeTaskView(TaskProxy task)
		{
			tbTaskName.IsEnabled = false;
			tbTaskDescription.IsEnabled = false;

			tbTaskName.Text = task.Name;
			tbTaskDescription.Text = task.Description;
			if (task.Executor != null) lblExecutor.Content = task.Executor.FullName;
			UpdateComboBoxStatus(task);
			UpdateCommentList(task.Id);
		}

		private void UpdateCommentList(int taskId)
		{
			Task.Run(() =>
			{
				Dispatcher.Invoke(() =>
				{
					var commentList = service.GetCommentsByTaskId(taskId).ToList();
					lbTaskComment.ItemsSource = GetUIItemsComment(commentList);
					lbTaskComment.SelectedIndex = lbTaskComment.Items.Count - 1;
					lbTaskComment.ScrollIntoView(lbTaskComment.SelectedItem);
				});
			});
		}

		private List<UIElement> GetUIItemsComment(List<CommentProxy> commentList)
		{
			List<UIElement> lbItems = new List<UIElement>();
			foreach (var comment in commentList)
			{
				StackPanel stack = new StackPanel();
				stack.Orientation = Orientation.Horizontal;
				stack.Margin = new Thickness(1);

				StackPanel author = new StackPanel();
				author.Children.Add(new Label() { Content = comment.Owner.FullName });
				author.Children.Add(new Label() { Content = comment.DatePublic.ToString("g") });

				stack.Children.Add(new Separator());
				stack.Children.Add(author);
				stack.Children.Add(new Label() { Content = comment.Text });

				lbItems.Add(new Separator() { Margin = new Thickness(1) });
				lbItems.Add(stack);
			}
			return lbItems;
		}

		private void UpdateComboBoxStatus(TaskProxy task)
		{
			Task.Run(() =>
			{
				cbTaskStatus.Dispatcher.Invoke(() =>
				{
					var statusList = service.GetTaskStatusListByProjectId(task.Project.Id);
					cbTaskStatus.ItemsSource = statusList;
					if (task.TaskStatus != null) cbTaskStatus.SelectedItem = statusList.FirstOrDefault(s => s.Id == task.TaskStatus.Id);
				});
			});
		}

		private void cbTaskStatus_DropDownClosed(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				cbTaskStatus.Dispatcher.Invoke(() =>
				{
					var status = cbTaskStatus.SelectedItem as TaskStatusProxy;
					if (status == null) return;
					if (status.Id == selectedTsTask.Id) return;
					service.ChangeTaskStatus(User.Instance.Account.Id, selectedTsTask.Id, status);
				});
			});
		}

		private void btnCommentAdd_Click(object sender, RoutedEventArgs e)
		{
			service.CreateComment(User.Instance.Account.Id, new CommentProxy(0, tbCurrentComment.Text, User.Instance.Account, selectedTsTask, DateTime.Now));
			UpdateCommentList(selectedTsTask.Id);
			tbCurrentComment.Text = "";
			btnCommentAdd.IsEnabled = false;
		}

		private void tbCurrentComment_KeyDown(object sender, KeyEventArgs e)
		{
			btnCommentAdd.IsEnabled = !string.IsNullOrEmpty(tbCurrentComment.Text);
		}

		private void UpdateTsTaskList(TaskProxy[] tasks)
		{
			Task.Run(() =>
			{
				lvTsTaskList.Dispatcher.Invoke(() =>
				{
					lvTsTaskList.ItemsSource = tasks;
				});
			});
		}

		private void lvTsTaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var task = lvTsTaskList.SelectedItem as TaskProxy;
			if (task == null) return;
			selectedTsTask = task;
			InitializeTaskView(task);
		}

		private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var tab = (TypeTab)Enum.Parse(typeof(TypeTab), (tcMenu.SelectedItem as TabItem).Tag.ToString());

			if (tab == User.Instance.CurrentTab) return;
			else User.Instance.CurrentTab = tab;
			switch (tab)
			{
				case TypeTab.History:
					RefreshHist();
					break;
				case TypeTab.Project:
					RefreshPrj();
					break;
				case TypeTab.Task:
					RefreshTs();
					break;
			}
		}

		//################################################################
		//
		//#################################################################

		void RefreshHist()
		{
			UpdateTbHistories();
		}

		void UpdateTbHistories()
		{
			Task.Run(() =>
			{
				Dispatcher.Invoke(() =>
				{
					if (User.Instance.Account != null)
					{
						var historyList = service.GetAvalibleHistoryById(User.Instance.Account.Id).ToList();
						tbHistories.ItemsSource = GetUIItemsHistory(historyList);
					}
				});
			});
		}

		private List<UIElement> GetUIItemsHistory(List<HistoryProxy> historyList)
		{
			List<UIElement> lbItems = new List<UIElement>();
			foreach (var history in historyList)
			{
				StackPanel stack = new StackPanel();
				stack.Orientation = Orientation.Horizontal;
				stack.Margin = new Thickness(1);


				stack.Children.Add(new Label() { Content = history.Date });
				stack.Children.Add(new Separator());
				stack.Children.Add(new Label() { Content = history.Action });

				lbItems.Add(new Separator() { Margin = new Thickness(1) });
				lbItems.Add(stack);
			}
			return lbItems;
		}

		private void lvPrjProjectList_Selected(object sender, SelectionChangedEventArgs e)
		{
			selectedProject = lvPrjProjectList.SelectedItem as ProjectProxy;
			btnPrjDeleteProject.IsEnabled = selectedProject != null;
			btnPrjUpdateProject.IsEnabled = selectedProject != null;

			if (selectedProject != null)
			{
				UpdatePrjTaskList(service.GetTasksByProjectId(selectedProject.Id));
				UpdatePrjParticipantList(service.GetAccountListByProjectId(selectedProject.Id));
			}
		}

		private void lvPrjTaskList_Selected(object sender, SelectionChangedEventArgs e)
		{
			selectedPrjTask = lvPrjTaskList.SelectedItem as TaskProxy;
			btnPrjUpdateTask.IsEnabled = selectedPrjTask != null;
			btnPrjDeleteTask.IsEnabled = selectedPrjTask != null;
		}
	}
}
