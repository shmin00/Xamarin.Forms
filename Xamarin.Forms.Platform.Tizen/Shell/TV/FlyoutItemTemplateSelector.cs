using System;
using System.Collections;
using Xamarin.Forms.Platform.Tizen.Native;

namespace Xamarin.Forms.Platform.Tizen.TV
{
	public class FlyoutItemTemplateSelector : DataTemplateSelector
	{
		public DataTemplate DefaultTemplate { get; private set; }
		public DataTemplate FlyoutItemTemplate { get; private set; }
		public DataTemplate MenuItemTemplate { get; private set; }

		public FlyoutItemTemplateSelector(INavigationView nv)
		{
			DefaultTemplate = new DataTemplate(() =>
			{
				var grid = new Grid
				{
					HeightRequest = nv.GetFlyoutItemHeight(),
				};

				ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();
				columnDefinitions.Add(new ColumnDefinition { Width = nv.GetFlyoutIconColumnSize() });
				columnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
				grid.ColumnDefinitions = columnDefinitions;

				var image = new Image
				{
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Center,
					HeightRequest = nv.GetFlyoutIconSize(),
					WidthRequest = nv.GetFlyoutIconSize(),
					Margin = new Thickness(nv.GetFlyoutMargin(), 0, 0, 0),
				};
				image.SetBinding(Image.SourceProperty, new Binding("FlyoutIcon"));
				grid.Children.Add(image);

				var label = new Label
				{
					FontSize = nv.GetFlyoutItemFontSize(),
					VerticalTextAlignment = TextAlignment.Center,
					Margin = new Thickness(nv.GetFlyoutMargin(), 0, 0, 0),
				};
				label.SetBinding(Label.TextProperty, new Binding("Title"));

				grid.Children.Add(label, 1, 0);

				var groups = new VisualStateGroupList();

				var commonGroup = new VisualStateGroup();
				commonGroup.Name = "CommonStates";
				groups.Add(commonGroup);

				var normalState = new VisualState();
				normalState.Name = "Normal";
				normalState.Setters.Add(new Setter
				{
					Property = VisualElement.BackgroundColorProperty,
					Value = nv.GetTvFlyoutItemDefaultColor()
				});
				;

				var focusedState = new VisualState();
				focusedState.Name = "Focused";
				focusedState.Setters.Add(new Setter
				{
					Property = VisualElement.BackgroundColorProperty,
					Value = nv.GetTvFlyoutItemFocusedColor()
				});

				commonGroup.States.Add(normalState);
				commonGroup.States.Add(focusedState);

				VisualStateManager.SetVisualStateGroups(grid, groups);
				return grid;
			});
		}

		public FlyoutItemTemplateSelector(INavigationView nv, DataTemplate flyoutItemTemplate, DataTemplate menuItemTemplate) : this(nv)
		{
			FlyoutItemTemplate = flyoutItemTemplate;
			MenuItemTemplate = menuItemTemplate;
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item is IMenuItemController)
				return (MenuItemTemplate != null) ? MenuItemTemplate : DefaultTemplate;
			else
				return (FlyoutItemTemplate != null) ? FlyoutItemTemplate : DefaultTemplate;
		}
	}
}
