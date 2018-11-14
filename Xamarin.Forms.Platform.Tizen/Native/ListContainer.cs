using System;
using System.Collections.Generic;
using ElmSharp;
using System.Linq;
using EList = ElmSharp.List;

namespace Xamarin.Forms.Platform.Tizen.Native
{
	public class ListContainer : EList, IContainable<ListItem>
	{
		IList<ListItem> _items = new List<ListItem>();

		IList<ListItem> IContainable<ListItem>.Children => _items;

		public ListContainer(EvasObject parent) : base(parent)
		{
		}

		public new ListItem Append(string label)
		{
			Log.Debug(" ++++++++++++++++++++++++++++ new add");
			return Append(label, null, null);
		}

		public new ListItem Append(string label, EvasObject leftIcon, EvasObject rightIcon)
		{
			var item = base.Append(label, leftIcon, rightIcon);
			item.Deleted += ItemDeleted;
			_items.Add(item);
			Log.Debug(" ++++++++++++++++++++++++++++ new add text: _" + item.Text + "_");
			return item;
		}

		public new ListItem Prepend(string label, EvasObject leftIcon, EvasObject rightIcon)
		{
			Log.Debug(" ++++++++++++++++++++++++++++ new add");
			var item = base.Prepend(label, leftIcon, rightIcon);
			item.Deleted += ItemDeleted;
			_items.Add(item);
			return item;
		}

		public new ListItem Prepend(string label)
		{
			return Prepend(label, null, null);
		}

		public new void Clear()
		{
			_items.Clear();
			base.Clear();
		}

		void ItemDeleted(object sender, EventArgs e)
		{
			_items.Remove((ListItem)sender);
		}
	}
}
