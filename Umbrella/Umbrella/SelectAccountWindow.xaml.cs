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
using Umbrella.ServiceReference;
using UmbrellaService.DataLevel.ProxyModel;

namespace Umbrella
{
	/// <summary>
	/// Interaction logic for SelectAccountWindows.xaml
	/// </summary>
	public partial class SelectAccountWindows : Window
	{
		private ServiceClient service;
		public int SelectAccountId { get; private set; }
		public SelectAccountWindows(ServiceClient service, List<AccountProxy> accounts)
		{
			InitializeComponent();
			this.service = service;

			lvAccountList.ItemsSource = accounts;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			var acc = lvAccountList.SelectedItem as AccountProxy;
			if (acc != null)
			{
				SelectAccountId = acc.Id;
				DialogResult = true;
			}
			else
			{
				DialogResult = false;
			}
		}
	}
}
