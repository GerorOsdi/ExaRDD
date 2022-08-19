using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ExaRDD.ViewModel;
using ExaRDD.FireBase;

namespace ExaRDD.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VLstRecord : ContentPage
    {
        public VLstRecord()
        {
            InitializeComponent();
            BindingContext = new VMlstRecordatorios(Navigation);
            //Lista();
        }

        private async Task Lista()
        {
            Consultas data = new Consultas();
            lstRecordatorios.ItemsSource = await data.getRecordatorios();
        }
    }

}