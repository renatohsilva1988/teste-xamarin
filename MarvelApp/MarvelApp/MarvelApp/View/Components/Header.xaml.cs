using MarvelApp.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarvelApp.View.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Header : ContentView
    {
        public static readonly BindableProperty BackCommandProperty =
            BindableProperty.Create(
                nameof(BackCommand),
                typeof(Command),
                typeof(Header)
            );

        public static readonly BindableProperty LogoutCommandProperty =
            BindableProperty.Create(
                nameof(LogoutCommand),
                typeof(Command),
                typeof(Header)
            );


        public static readonly BindableProperty HeaderTitleProperty =
            BindableProperty.Create(
                nameof(HeaderTitle),
                typeof(string),
                typeof(Header)
            );

        public static readonly BindableProperty HeaderTitleComplementProperty =
            BindableProperty.Create(
                nameof(HeaderTitleComplement),
                typeof(string),
                typeof(Header)
            );

        public static readonly BindableProperty ShowBackIconProperty =
           BindableProperty.Create(
               nameof(ShowBackIcon),
               typeof(bool),
               typeof(Header),
               defaultValue: true
           );

        public static readonly BindableProperty ShowLogoutIconProperty =
            BindableProperty.Create(
                nameof(ShowLogoutIcon),
                typeof(bool),
                typeof(Header),
                defaultValue: true
            );

        public static readonly BindableProperty ShowDisconnectedProperty =
            BindableProperty.Create(
                nameof(ShowDisconnected),
                typeof(bool),
                typeof(Header),
                defaultValue: true
            );

        public static readonly BindableProperty DisconnectedProperty =
            BindableProperty.Create(
                nameof(Disconnected),
                typeof(string),
                typeof(Header)
            );

        public bool ShowBackIcon
        {
            get => (bool)GetValue(ShowBackIconProperty);
            set => SetValue(ShowBackIconProperty, value);
        }

        public bool ShowLogoutIcon
        {
            get => (bool)GetValue(ShowLogoutIconProperty);
            set => SetValue(ShowLogoutIconProperty, value);
        }

        public Command BackCommand
        {
            get => (Command)GetValue(BackCommandProperty);
            set => SetValue(BackCommandProperty, value);
        }

        public Command LogoutCommand
        {
            get => (Command)GetValue(BackCommandProperty);
            set => SetValue(BackCommandProperty, value);
        }

        public string HeaderTitle
        {
            get => (string)GetValue(HeaderTitleProperty);
            set => SetValue(HeaderTitleProperty, value);
        }

        public string HeaderTitleComplement
        {
            get => (string)GetValue(HeaderTitleComplementProperty);
            set => SetValue(HeaderTitleComplementProperty, value);
        }        

        public bool ShowDisconnected
        {
            get => (bool)GetValue(ShowDisconnectedProperty);
            set => SetValue(ShowDisconnectedProperty, value);
        }

        public string Disconnected
        {
            get => (string)GetValue(DisconnectedProperty);
            set => SetValue(DisconnectedProperty, value);
        }

        public Header()
        {
            InitializeComponent();
        }
    }
}