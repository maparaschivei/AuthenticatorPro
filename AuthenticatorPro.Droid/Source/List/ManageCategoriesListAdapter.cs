// Copyright (C) 2021 jmh
// SPDX-License-Identifier: GPL-3.0-only

using System;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using AuthenticatorPro.Droid.Data.Source;
using AuthenticatorPro.Droid.Util;

namespace AuthenticatorPro.Droid.List
{
    internal sealed class ManageCategoriesListAdapter : RecyclerView.Adapter, IReorderableListAdapter
    {
        public event EventHandler<int> MenuClick;
        public string DefaultId { get; set; }
        
        private readonly CategorySource _source;

        public override int ItemCount => _source.GetView().Count;

        public ManageCategoriesListAdapter(CategorySource source)
        {
            _source = source;
        }

        public void MoveItemView(int oldPosition, int newPosition)
        {
            _source.Swap(oldPosition, newPosition);
            NotifyItemMoved(oldPosition, newPosition);
        }

        public async void NotifyMovementFinished()
        {
            try
            {
                await _source.CommitRanking();
            }
            catch(Exception e)
            {
                // Cannot revert, keep going
                Logger.Error(e);
            }
        }

        public void NotifyMovementStarted() { }

        public override long GetItemId(int position)
        {
            return _source.Get(position).Id.GetHashCode();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var category = _source.Get(position);

            if(category == null)
                return;
            
            var holder = (ManageCategoriesListHolder) viewHolder;
            holder.Name.Text = category.Name;
            holder.DefaultImage.Visibility = DefaultId == category.Id
                ? ViewStates.Visible
                : ViewStates.Invisible;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.listItemManageCategory, parent, false);

            var holder = new ManageCategoriesListHolder(itemView);
            holder.MenuButton.Click += delegate { MenuClick(this, holder.AdapterPosition); };

            return holder;
        }
    }
}