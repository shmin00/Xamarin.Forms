using System;
using ElmSharp;
using EBox = ElmSharp.Box;
using EColor = ElmSharp.Color;

namespace Xamarin.Forms.Platform.Tizen.TV
{
	public class TVNavigationDrawer : EBox, INavigationDrawer, IAnimatable
	{
		EvasObject _navigationView;
		EBox _drawerBox;
		EvasObject _main;
		EBox _mainContainer;

		DrawerBehavior _behavior;
		bool _isOpen;

		double _openRatio;

		public TVNavigationDrawer(EvasObject parent) : base(parent)
		{
			Initialize(parent);
		}

		public event EventHandler Toggled;

		public EvasObject TargetView => this;

		public EvasObject NavigationView
		{
			get => _navigationView;
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
			};

			_drawerBox.KeyDown += (s, e) =>
			{
				if (e.KeyName == "Right" && _behavior == DrawerBehavior.Default)
				{
					IsOpen = false;
				}
			};

			_mainContainer = new EBox(parent);
			_mainContainer.BackgroundColor = EColor.Black;
			_mainContainer.Show();
			PackEnd(_mainContainer);

			_mainContainer.KeyDown += (s, e) =>
			{
				// It is workaround of managing focus, to prevent losing the focus because of invisible objects.
				Device.BeginInvokeOnMainThread(() =>
				{
					// 1. focus can be returned to _drawerbox if there is no object to get the focus.
					if(_drawerBox.IsFocusedObjectExist())
					{
						IsOpen = true;
					}

					// 2. to prevent to move the focus to an invisible object of _main.
					var focused = _main.GetFocusedObjectGeometry();
					var isVisible = _main.IsFocusedObjectVisible();
					if (focused.Width * focused.Height <= 0 || !isVisible)
					{
						if (_behavior == DrawerBehavior.Default)
						{
							IsOpen = true;
						}
						else if (_behavior == DrawerBehavior.Locked)
						{
							UpdateFocusPolicy();
						}
					}
				});
			};

			_drawerBox.AllowFocus(true);
			_mainContainer.SetNextFocusObject(_drawerBox, FocusDirection.Left);
		}

		void UpdateNavigationView(EvasObject navigationView)
		{
			if (_navigationView != null)
			{
				_drawerBox.UnPack(_navigationView);
				_navigationView.Hide();
			}

			_navigationView = navigationView;

			if (_navigationView != null)
			{
				_navigationView.SetAlignment(-1, -1);
				_navigationView.SetWeight(1, 1);
				_navigationView.Show();
				_drawerBox.PackEnd(_navigationView);

				UpdateFocusPolicy();
			}
		}

		void UpdateMain(EvasObject main)
		{
			if (_main != null)
			{
				_mainContainer.UnPack(_main);
				_main.Hide();
			}

			_main = main;

			if (_main != null)
			{
				_main.SetAlignment(-1, -1);
				_main.SetWeight(1, 1);
				_main.Show();

				_mainContainer.PackEnd(_main);

				UpdateFocusPolicy();
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

			var ratio = this.GetFlyoutRatio(Geometry.Width, Geometry.Height);
			var minimumRatio = (_behavior == DrawerBehavior.Disabled) ? 0 : this.GetFlyoutCollapseRatio();
			var drawerWidthMax = (int)(bound.Width * ratio);
			var drawerWidthMin = (int)(bound.Width * minimumRatio);

			var drawerWidthOutBound = (int)((drawerWidthMax - drawerWidthMin) * (1 - _openRatio));
			var drawerWidthInBound = drawerWidthMax - drawerWidthOutBound;

			var drawerGeometry = bound;
			drawerGeometry.Width = drawerWidthMax;
			_drawerBox.Geometry = drawerGeometry;

			var containerGeometry = bound;
			containerGeometry.X = drawerWidthInBound;
			containerGeometry.Width = (_behavior == DrawerBehavior.Locked) ? (bound.Width - drawerWidthInBound) : (bound.Width - drawerWidthMin);
			_mainContainer.Geometry = containerGeometry;
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
						UpdateFocusPolicy();
					}
				}
			});
		}

		void UpdateFocusPolicy()
		{
			var drawer = _navigationView as Widget;
			var main = _main as Widget;

			if (_behavior == DrawerBehavior.Locked)
			{
				if (drawer != null)
				{
					drawer.AllowTreeFocus = true;
					drawer.SetFocus(true);
				}
				if (main != null)
				{
					main.AllowTreeFocus = true;
				}
				return;
			}

			if (_isOpen)
			{
				if (drawer != null)
				{
					drawer.AllowTreeFocus = true;
					drawer.SetFocus(true);
				}
				if (main != null)
				{
					main.AllowTreeFocus = false;
				}
			}
			else
			{
				if (drawer != null)
				{
					drawer.AllowTreeFocus = false;
				}
				if (main != null)
				{
					main.AllowTreeFocus = true;
					main.SetFocus(true);

					// It is workaround of managing focus, to prevent losing the focus because of invisible objects.
					// If there is no object to get focus in main box, the focus will be returned to drawer.
					Device.BeginInvokeOnMainThread(() =>
					{
						if(_drawerBox.IsFocusedObjectExist())
						{
							IsOpen = true;
						}
					});
				}
			}
		}

		void IAnimatable.BatchBegin()
		{
		}

		void IAnimatable.BatchCommit()
		{
		}
	}
}