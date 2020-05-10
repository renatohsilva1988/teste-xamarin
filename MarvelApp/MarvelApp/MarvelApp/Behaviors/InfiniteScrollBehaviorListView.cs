using System;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace MarvelApp.Behaviors
{
    public class InfiniteScrollBehaviorListView : BehaviorBase<ListView>
    {
        public static readonly BindableProperty LoadMoreCommandProperty =
            BindableProperty.Create(
                nameof(LoadMoreCommand),
                typeof(ICommand),
                typeof(InfiniteScrollBehaviorListView),
                null);

        public static readonly BindableProperty IsLoadingMoreProperty =
            BindableProperty.Create(
                nameof(IsLoadingMore),
                typeof(bool),
                typeof(InfiniteScrollBehaviorListView),
                default(bool),
                BindingMode.OneWayToSource);

        public ICommand LoadMoreCommand
        {
            get => (ICommand)GetValue(LoadMoreCommandProperty);
            set => SetValue(LoadMoreCommandProperty, value);
        }

        public bool IsLoadingMore
        {
            get => (bool)GetValue(IsLoadingMoreProperty);
            set => SetValue(IsLoadingMoreProperty, value);
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
            bindable.ItemAppearing += InfiniteListView_ItemAppearing;
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
            bindable.ItemAppearing -= InfiniteListView_ItemAppearing;
        }

        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (AssociatedObject.ItemsSource is IList)
            {
                if (IsLoadingMore)
                {
                    return;
                }

                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null) && ShouldLoadMore(e.Item))
                {
                    LoadMoreCommand.Execute(null);
                }
            }
        }

        private bool ShouldLoadMore(object item)
        {
            if (AssociatedObject.ItemsSource is IList list)
            {
                if (list.Count == 0)
                {
                    return true;
                }

                var lastItem = list[list.Count - 1];
                if (AssociatedObject.IsGroupingEnabled && lastItem is IList group)
                {
                    return group.Count == 0 || group[group.Count - 1] == item;
                }

                return lastItem == item;
            }
            return false;
        }
    }
}
