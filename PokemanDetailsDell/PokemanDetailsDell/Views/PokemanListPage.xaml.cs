using System;
using System.Collections.Generic;
using PokemanDetailsDell.ViewModels;
using Xamarin.Forms;
using static PokemanDetailsDell.Models.Pokemon;

namespace PokemanDetailsDell.Views
{
    public partial class PokemanListPage : ContentPage
    {
        public PokemanListPage()
        {
            InitializeComponent();
            BindingContext = new PokemanListPageViewModel();

            var viewModel = BindingContext as PokemanListPageViewModel;
            viewModel.GetPokemonTypes();
            viewModel.GetPokemonList();
        }

        void ListView_ItemAppearing(System.Object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            var viewModel = BindingContext as PokemanListPageViewModel;
            if (e.ItemIndex == viewModel.PokemonDetailsList.Count - 1)
            {
                viewModel.LoadData();
            }
        }
    }
}
