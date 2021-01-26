using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using ElmSharp;
using EBox = ElmSharp.Box;

namespace Xamarin.Forms.Platform.Tizen
{
	public class TVShellSectionRenderer : IAppearanceObserver, IShellSectionRenderer
	{
		EBox _mainLayout = null;
		EBox _contentArea = null;

		EvasObject _currentContent = null;
		TVNavigationView _navigationView;

		Dictionary<ShellContent, EvasObject> _contentCache = new Dictionary<ShellContent, EvasObject>();

		bool _disposed = false;
		bool _drawerIsVisible => (ShellSection != null) ? (ShellSection.Items.Count > 1) : false;

		public TVShellSectionRenderer(ShellSection section)
		{
			ShellSection = section;
			ShellSection.PropertyChanged += OnSectionPropertyChanged;
			(ShellSection.Items as INotifyCollectionChanged).CollectionChanged += OnShellSectionCollectionChanged;

			_mainLayout = new EBox(Forms.NativeParent);
			_mainLayout.SetLayoutCallback(OnLayout);

			_navigationView = new TVNavigationView(Forms.NativeParent, section.Parent);
			_navigationView.SetAlignment(-1, -1);
			_navigationView.SetWeight(1, 1);
			_navigationView.Show();
			_mainLayout.PackEnd(_navigationView);

			_contentArea = new EBox(Forms.NativeParent);
			_contentArea.Show();

			_mainLayout.PackEnd(_contentArea);
			_mainLayout.AllowFocus(false);

			_navigationView.Focused += (s, e) =>
			{
				_contentArea.AllowTreeFocus = false;
			};

			_navigationView.Unfocused += (s, e) =>
			{
				_contentArea.AllowTreeFocus = true;
			};

			_navigationView.KeyDown += (s, e) =>
			{
				if (e.KeyName == "Right")
				{
					_contentArea.AllowTreeFocus = true;
					_contentArea.SetFocus(true);
				}
			};

			_navigationView.SelectedItemChanged += _navigationView_SelectedItemChanged;

			UpdateTabsItem();
			UpdateCurrentItem(ShellSection.CurrentItem);

			_contentArea.SetNextFocusObject(_navigationView, FocusDirection.Left);
			_navigationView.SetNextFocusObject(_contentArea, FocusDirection.Right);

			((IShellController)Shell.Current).AddAppearanceObserver(this, ShellSection);
		}

		private void _navigationView_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			var content = e.SelectedItem;
			if (ShellSection.CurrentItem != content)
			{
				ShellSection.SetValueFromRenderer(ShellSection.CurrentItemProperty, content);
			}
		}

		public ShellSection ShellSection { get; }

		~TVShellSectionRenderer()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				((IShellController)Shell.Current).RemoveAppearanceObserver(this);
				if (ShellSection != null)
				{
					(ShellSection as IShellSectionController).RemoveDisplayedPageObserver(this);
					ShellSection.PropertyChanged -= OnSectionPropertyChanged;
				}
				NativeView.Unrealize();
			}
			_disposed = true;
		}

		public EvasObject NativeView
		{
			get
			{
				return _mainLayout;
			}
		}

		void OnSectionPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CurrentItem")
			{
				UpdateCurrentItem(ShellSection.CurrentItem);
			}
		}

		void UpdateTabsItem()
		{
			if (!_drawerIsVisible)
			{
				return;
			}

			(_navigationView as TVNavigationView).BuildMenu(ShellSection.Items);
		}

		void UpdateCurrentItem(ShellContent content)
		{
			UpdateCurrentShellContent(content);
		}

		void UpdateCurrentShellContent(ShellContent content)
		{
			if (_currentContent != null)
			{
				_currentContent.Hide();
				_contentArea.UnPack(_currentContent);
				_currentContent = null;
			}

			if (content == null)
			{
				return;
			}

			if (!_contentCache.ContainsKey(content))
			{
				var native = CreateShellContent(content);
				native.SetAlignment(-1, -1);
				native.SetWeight(1, 1);
				_contentCache[content] = native;
			}
			_currentContent = _contentCache[content];
			_currentContent.Show();
			_contentArea.PackEnd(_currentContent);
			OnLayout();
		}

		EvasObject CreateShellContent(ShellContent content)
		{
			Page xpage = ((IShellContentController)content).GetOrCreateContent();
			return Platform.GetOrCreateRenderer(xpage).NativeView;
		}

		void OnLayout()
		{
			if (NativeView.Geometry.Width == 0 || NativeView.Geometry.Height == 0)
				return;

			var bound = NativeView.Geometry;
			var drawerWidth = _drawerIsVisible ? 400 : 0;
			var contentBound = bound;
			var drawerBound = bound;

			contentBound.X += drawerWidth;
			contentBound.Width -= drawerWidth;
			_contentArea.Geometry = contentBound;

			drawerBound.Width = drawerWidth;

			_navigationView.Geometry = drawerBound;
		}

		void OnShellSectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateTabsItem();
		}

		void IAppearanceObserver.OnAppearanceChanged(ShellAppearance appearance)
		{
		}
	}

}
