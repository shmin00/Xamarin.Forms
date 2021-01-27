using System;
using ElmSharp;
using EBox = ElmSharp.Box;
using EColor = ElmSharp.Color;

namespace Xamarin.Forms.Platform.Tizen.TV
{
	public class TVNavigationDrawer : EBox, INavigationDrawer, IAnimatable
	{
		EBox _drawerBox;
		EBox _mainBox;
		EvasObject _main;
		TVNavigationView _drawer;

		DrawerBehavior _behavior;
		bool _isOpen;

		double _openRatio;
		EvasObject _lastfocusedobject;

		public TVNavigationDrawer(EvasObject parent) : base(parent)
		{
			Initialize(parent);
		}

		public event EventHandler Toggled;

		public EvasObject TargetView => this;

		public EvasObject NavigationView
		{
			get => _drawer;
			set => UpdateNavigationView(value);
		}

		public int DrawerWidth { get; set; }

		public int DrawerMinimumWidth { get; set; }

		public EvasObject Main
		{
			get => _main;
			set => UpdateMain(value);
		}

		public DrawerBehavior DrawerBehavior
		{
			get => _behavior;
			set => UpdateBehavior(value);
		}

		public bool IsOpen
		{
			get => _isOpen;
			set => UpdateOpenState(value);
		}

		void Initialize(EvasObject parent)
		{
			SetLayoutCallback(OnLayout);

			_drawerBox = new EBox(parent);
			_drawerBox.Show();
			PackEnd(_drawerBox);

			_drawerBox.Focused += (s, e) =>
			{
				IsOpen = true;
				_lastfocusedobject = _drawerBox;
			};

			_mainBox = new EBox(parent);
			_mainBox.BackgroundColor = EColor.Black;
			_mainBox.Show();
			PackEnd(_mainBox);

			_drawerBox.KeyUp += (s, e) =>
			{
				// Workaroud to prevent losing focus to invisible object using remote controller.
				if (e.KeyName == "Return")
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						_mainBox.SetFocus(true);
						if (_mainBox.IsFocusedObjectExist())
							_drawerBox.AllowTreeFocus = false;
					});
				}
			};

			_mainBox.KeyDown += (s, e) =>
			{
				// Restore AllowTreeFocus when an any key is pressed.
				if (!_drawerBox.AllowTreeFocus)
					_drawerBox.AllowTreeFocus = true;

				// Workaroud to prevent losing focus to invisible object using remote controller.
				if (e.KeyName == "Left")
				{
					var geo = _mainBox.GetFocusedObjectGeometry();

					if ((geo.Width * geo.Height) <= 0)
					{
						_drawerBox.AllowTreeFocus = true;
						_drawerBox.SetFocus(true);
					};
				}
			};
		}

		void UpdateNavigationView(EvasObject navigationView)
		{
			if (_drawer != null)
			{
				_drawer.ItemFocused -= OnNavigationViewItemFocused;
				_drawer.ItemUnfocused -= OnNavigationViewItemUnfocused;
				_drawerBox.UnPack(_drawer);
				_drawer.Hide();
			}

			_drawer = navigationView as TVNavigationView;

			if (_drawer != null)
			{
				_drawer.SetAlignment(-1, -1);
				_drawer.SetWeight(1, 1);
				_drawer.Show();
				_drawerBox.PackEnd(_drawer);

				if (_drawer is TVNavigationView nv)
				{
					nv.ItemFocused += OnNavigationViewItemFocused;
					nv.ItemUnfocused += OnNavigationViewItemUnfocused;
				};
			}
		}

		void OnNavigationViewItemFocused(object sender, EventArgs args)
		{
			IsOpen = true;
		}

		void OnNavigationViewItemUnfocused(object sender, EventArgs args)
		{
			IsOpen = false;
		}

		void UpdateMain(EvasObject main)
		{
			if (_main != null)
			{
				_mainBox.UnPack(_main);
				_main.Hide();
			}

			_main = main;

			if (_main != null)
			{
				_main.SetAlignment(-1, -1);
				_main.SetWeight(1, 1);
				_main.Show();

				_mainBox.PackEnd(_main);
			}
		}

		void UpdateBehavior(DrawerBehavior behavior)
		{
			_behavior = behavior;
			var open = false;

			if (_behavior == DrawerBehavior.Locked)
				open = true;
			else if (_behavior == DrawerBehavior.Disabled)
				open = false;
			else
				open = _drawerBox.IsFocused;

			UpdateOpenState(open);
		}

		void OnLayout()
		{
			if (Geometry.Width == 0 || Geometry.Height == 0)
				return;

			var bound = Geometry;

			var drawerWidthMax = this.GetFlyoutWidth();
			var drawerWidthMin = this.GetFlyoutCollapseWidth();

			var drawerWidthOutBound = (int)((drawerWidthMax - drawerWidthMin) * (1 - _openRatio));
			var drawerWidthInBound = drawerWidthMax - drawerWidthOutBound;

			var drawerGeometry = bound;
			drawerGeometry.Width = drawerWidthInBound;
			_drawerBox.Geometry = drawerGeometry;

			var containerGeometry = bound;
			containerGeometry.X = drawerWidthInBound;
			containerGeometry.Width = (_behavior == DrawerBehavior.Locked) ? (bound.Width - drawerWidthInBound) : (bound.Width - drawerWidthMin);
			_mainBox.Geometry = containerGeometry;
		}

		void UpdateOpenState(bool isOpen)
		{
			if (_behavior == DrawerBehavior.Locked && !isOpen)
				return;

			double endState = ((_behavior != DrawerBehavior.Disabled) && isOpen) ? 1 : 0;
			new Animation((r) =>
			{
				_openRatio = r;
				OnLayout();
			}, _openRatio, endState, Easing.SinIn).Commit(this, "DrawerMove", length: (uint)(250 * Math.Abs(endState - _openRatio)), finished: (f, aborted) =>
			{
				if (!aborted)
				{
					if (_isOpen != isOpen)
					{
						_isOpen = isOpen;
						Toggled?.Invoke(this, EventArgs.Empty);
					}
				}
			});
		}

		void IAnimatable.BatchBegin()
		{
		}

		void IAnimatable.BatchCommit()
		{
		}
	}
}