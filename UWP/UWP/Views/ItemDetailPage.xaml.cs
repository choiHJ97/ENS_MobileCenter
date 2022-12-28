using System.ComponentModel;
using UWP.ViewModels;
using Xamarin.Forms;

namespace UWP.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}