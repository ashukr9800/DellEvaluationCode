using System;
using PokemanDetailsDell.Styles;
using PokemanDetailsDell.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokemanDetailsDell
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            this.Resources = new DefaultStyle().Resources;

            MainPage = new NavigationPage(new PokemanListPage())
            {
                BarBackgroundColor = Color.Green,
                BarTextColor = Color.White,
            };
        }

        protected override void OnStart()
        {
            this.Resources = new DefaultStyle().Resources;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
