using System;
using ElmSharp;
using Xamarin.Forms.Platform.Tizen.Native;
using NCollectionView = Xamarin.Forms.Platform.Tizen.Native.CollectionView;

namespace Xamarin.Forms.Platform.Tizen.TV
{
	public class CustomCollectionView : NCollectionView
	{
		public CustomCollectionView(EvasObject parent) : base(parent)
		{
		}

		protected override ViewHolder CreateViewHolder()
		{
			return new CustomViewHolder(this);
		}

		public class CustomViewHolder : ViewHolder
		{
			public CustomViewHolder(EvasObject parent) : base(parent)
			{
			}

			protected override void OnFocused(object sender, EventArgs e)
			{
				State = ViewHolderState.Focused;
			}

			protected override void OnUnfocused(object sender, EventArgs e)
			{
				State = ViewHolderState.Normal;
			}
		}
	}
}
