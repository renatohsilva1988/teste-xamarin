using MarvelApp.View.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarvelApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialView : BasePage
    {
        public InitialView()
        {
            InitializeComponent();
        }
    }
}