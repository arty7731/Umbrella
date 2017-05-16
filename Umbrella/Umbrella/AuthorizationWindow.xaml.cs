using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Umbrella
{
	/// <summary>
	/// Interaction logic for AuthorizationWindow.xaml
	/// </summary>
	public partial class AuthorizationWindow : Window
	{
		ServiceClient service { get; set; }
		public AuthorizationWindow(ServiceClient service)
		{
			InitializeComponent();
			if (service == null) MessageBox.Show("Service not found!");
			else this.service = service;
		}

		private void btnSignIn_Click(object sender, RoutedEventArgs e)
		{
			if (service != null)
			{

				var login = tbSignInLogin.Text;
				var pass = pbSignInPassword.Password;
				if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
				{
					MessageBox.Show("Login or password empty!");
					return;
				}
				else
				{
					btnSignIn.IsEnabled = false;
					ThreadPool.QueueUserWorkItem((s) =>
					{
						var acc = service.SignIn(login, pass);
						if (acc == null)
						{
							Dispatcher.Invoke(() =>
							{
								MessageBox.Show("Incorect login or password!");
								btnSignIn.IsEnabled = true;
							});
							return;
						}
						else
						{
							User.Instance.Account = acc;

							Dispatcher.Invoke(() =>
							{
								DialogResult = true;
							});
						}
					});
				}
			}
			else
			{
				DialogResult = false;
			}
		}

		private void btnSignInExit_Click(object sender, RoutedEventArgs e)
		{
			if (User.Instance.Account != null) service.LogOut(User.Instance.Account.Id);
			Owner.Close();
			Close();
			DialogResult = false;
		}

		private void btnSignUp_Click(object sender, RoutedEventArgs e)
		{
			if (service != null)
			{
				var login = tbSignUpLogin.Text;
				var pass = pbSignUpPassword.Password;
				var email = tbSignUpEmail.Text;
				var fullname = tbSignUpFullName.Text;
				if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
				{
					MessageBox.Show("Fill in all the fields!");
					return;
				}
				else
				{
					btnSignUp.IsEnabled = false;
					ThreadPool.QueueUserWorkItem((s) =>
					{
						User.Instance.Account = service.SignUp(login, pass, email, fullname, 0);
						if (User.Instance.Account == null)
						{
							Dispatcher.Invoke(() =>
							{
								btnSignUp.IsEnabled = true;
								MessageBox.Show("Login is busy!");
							});
							return;
						}
						else
						{
							Dispatcher.Invoke(() =>
							{
								DialogResult = true;
							});
						}
					});
				}
			}
			else
			{
				DialogResult = false;
			}
		}

		private void btnSignUpExit_Click(object sender, RoutedEventArgs e)
		{
			Owner.Close();
			Close();
			DialogResult = false;

		}
	}
}
