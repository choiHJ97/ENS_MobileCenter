using System;
using System.Collections.Generic;
using System.ComponentModel;
using UWP.Models;
using UWP.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UWP.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}