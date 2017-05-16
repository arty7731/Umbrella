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
	/// Interaction logic for TaskWindow.xaml
	/// </summary>
	public partial class TaskWindow : Window
	{
		private ServiceClient service;
		TaskProxy currentTask;
		public TaskWindow(ServiceClient service, TaskProxy task, ViewType typeView)
		{
			InitializeComponent();
			this.service = service;
			this.currentTask = task;
			InitializeTask(typeView);
		}

		private void InitializeTask(ViewType typeView)
		{
			var task = currentTask;

			tbTaskName.Text = task.Name;
			tbTaskDescription.Text = task.Description;
			UpdateComboBoxStatus(task);
			UpdateCommentList(task.Id);
			lvExecutorList.ItemsSource = service.GetAccountListByTaskId(task.Id);
			if (task.Executor != null)
			{
				lblExecutor.Content = task.Executor.FullName;
				gbExecutors.IsEnabled = false;
			}
			else
			{
				gbExecutors.IsEnabled = true;
			}

			switch (typeView)
			{
				case ViewType.View:
					InitializeControlForView();
					break;
				case ViewType.Edit:
					InitializeControlForEdit();
					break;
			}
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

		private void InitializeControlForView()
		{
			tbTaskDescription.IsReadOnly = true;
			tbTaskName.IsReadOnly = true;
		}
		private void InitializeControlForEdit()
		{
			tbTaskDescription.IsReadOnly = false;
			tbTaskName.IsReadOnly = false;
			cbTaskStatus.IsEnabled = true;
		}


		private void lvExecutorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var executor = lvExecutorList.SelectedItem as AccountProxy;
			if (executor != null)
			{
				btnExecutorAdd.IsEnabled = true;
			}
		}

		private void btnExecutorAdd_Click(object sender, RoutedEventArgs e)
		{
			var accountList = service.GetAccountListByProjectId(currentTask.Project.Id).ToList();
			foreach (var acc in service.GetAccountListByTaskId(currentTask.Id))
			{
				var ac = accountList.FirstOrDefault(a => a.Id == acc.Id);
				accountList.Remove(ac);
			}
			var pl = new SelectAccountWindows(service, accountList);
			if (pl.ShowDialog() == true)
			{
				service.AddExecutor(User.Instance.Account.Id, pl.SelectAccountId, currentTask.Id);
				UpdateExecutorList(currentTask.Id);
			}
		}

		private void btnExecutorDelete_Click(object sender, RoutedEventArgs e)
		{
			var executor = lvExecutorList.SelectedItem as AccountProxy;
			if (executor != null)
			{
				service.DeleteExecutor(User.Instance.Account.Id, executor.Id, currentTask.Id);
				UpdateExecutorList(currentTask.Id);
				lvExecutorList.SelectedItem = null;
			}
		}

		private void UpdateExecutorList(int taskId)
		{
			Task.Run(() =>
			{
				Dispatcher.Invoke(() =>
				{
					lvExecutorList.ItemsSource = service.GetAccountListByTaskId(taskId);
				});
			});
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			var name = tbTaskName.Text;
			var description = tbTaskDescription.Text;

			var task = currentTask;

			task.Name = name;
			task.Description = description;

			service.UpdateTask(User.Instance.Account.Id, task);

			DialogResult = true;
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

		private void btnCommentAdd_Click(object sender, RoutedEventArgs e)
		{
			service.CreateComment(User.Instance.Account.Id, new CommentProxy(0, tbCurrentComment.Text, User.Instance.Account, currentTask, DateTime.Now));
			UpdateCommentList(currentTask.Id);
			tbCurrentComment.Text = "";
			btnCommentAdd.IsEnabled = false;
		}

		private void tbCurrentComment_KeyDown(object sender, KeyEventArgs e)
		{
			btnCommentAdd.IsEnabled = !string.IsNullOrEmpty(tbCurrentComment.Text);
		}

		private void cbTaskStatus_DropDownClosed(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				cbTaskStatus.Dispatcher.Invoke(() =>
				{
					var status = cbTaskStatus.SelectedItem as TaskStatusProxy;
					if (status == null) return;
					if (status.Id == currentTask.Id) return;
					service.ChangeTaskStatus(User.Instance.Account.Id, currentTask.Id, status);
				});
			});

		}
	}
}
