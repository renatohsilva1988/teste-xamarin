using MarvelApp.ViewModel.Base;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace MarvelApp.View.Base
{
    public abstract class BasePage : ContentPage, IPage
    {
        private BaseViewModel ViewModel => BindingContext as BaseViewModel;

        private bool DesabilitaBackButton { get; }

        public BasePage() : this(false, false)
        {

        }

        public BasePage(bool desabilitaBackButton, bool hasNavigationBar = false)
        {
            DesabilitaBackButton = desabilitaBackButton;
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, hasNavigationBar);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel == null)
            {
                return;
            }

            if (ViewModel is IHandleViewAppearing viewAware)
            {
                await viewAware.OnViewAppearingAsync(this);
            }

            Title = ViewModel.Title;
            IconImageSource = ViewModel.IconImageSource;
            ViewModel.PropertyChanged += LocalPropertyChanged;
        }

        protected override async void OnDisappearing()
        {
            if (ViewModel is IHandleViewDisappearing viewAware)
            {
                await viewAware.OnViewDisappearingAsync(this);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (DesabilitaBackButton)
            {
                return true;
            }

            base.OnBackButtonPressed();
            return false;
        }

        public void LocalPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Title))
            {
                Title = ViewModel.Title;
                return;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals(obj as IPage);
        }

        public bool Equals(IPage obj)
        {
            return Equals(obj.Title, Title) && Equals(obj.AutomationId, AutomationId) && Equals(obj.IconImageSource, IconImageSource);
        }

        public override int GetHashCode()
        {
            // It does not metter int overflow
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HASHING_BASE = (int)2166136261;
                const int HASHING_MULTIPLIER = 16777619;

                int hash = HASHING_BASE;
                hash *= HASHING_MULTIPLIER + ((Title is null) ? 0 : Title.GetHashCode());
                hash *= HASHING_MULTIPLIER + ((AutomationId is null) ? 0 : AutomationId.GetHashCode());
                hash *= HASHING_MULTIPLIER + ((IconImageSource is null) ? 0 : IconImageSource.GetHashCode());
                return hash;
            }
        }
    }
}
