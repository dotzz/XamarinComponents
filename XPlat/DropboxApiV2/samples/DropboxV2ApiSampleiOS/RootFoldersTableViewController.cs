using Foundation;
using System;
using UIKit;
using Dropbox.Api;
using Xamarin.Auth;
using System.Threading.Tasks;
using DropboxV2ApiSampleiOS.Data;
using DropboxV2ApiSampleiOS.DataSources;
using DropboxV2ApiSampleiOS.Helpers;

namespace DropboxV2ApiSampleiOS
{
    public partial class RootFoldersTableViewController : UITableViewController
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="T:DropboxV2ApiSampleiOS.RootFoldersTableViewController"/> class.
		/// </summary>
		/// <param name="folder">Folder.</param>
		public RootFoldersTableViewController(DropBoxItem folder) : base()
		{
			this.TableView.Source = new DropBoxFolderSource(folder.Path)
			{
				HandleSelection = HandleSelection,
			};

			Title = folder.Name;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:DropboxV2ApiSampleiOS.RootFoldersTableViewController"/> class.
		/// </summary>
		/// <param name="handle">Handle.</param>
		public RootFoldersTableViewController(IntPtr handle) : base(handle)
		{
			this.TableView.Source = new DropBoxFolderSource()
			{
				HandleSelection = HandleSelection,
			};

			Title = "Dropbox";

		}

		public async override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			if (DropBoxHelper.IsAuthenticated)
			{
				await ReloadDataAsync();
			}
			else
			{
				Authenticate();
			}
		}

		public void Authenticate()
		{
			var authHelp = new DropBoxHelper(async () =>
			{
				await ReloadDataAsync();

			});

			authHelp.PresentAuthController(this);
			                    
		}

		public async Task ReloadDataAsync()
		{
			await ((DropBoxFolderSource)this.TableView.Source).FetchFolderItems();

			this.TableView.ReloadData();

		}

		public void HandleSelection(DropBoxItem item)
		{
			if (item.ItemType == DropBoxItemType.Folder)
			{
				//mavigate to sub-folder
				var newDbVc = new RootFoldersTableViewController(item);

				NavigationController.PushViewController(newDbVc, true);
			}

		}

	}
}