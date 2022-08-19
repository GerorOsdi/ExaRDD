using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using ExaRDD.Model;
using ExaRDD.FireBase;
using Xamarin.Forms;
using System.Windows.Input;

namespace ExaRDD.ViewModel
{
    public class VMlstRecordatorios : Base
    {
        private ObservableCollection<recordatorios> ListRecordatorios;
        private recordatorios _Recodatorio;

        public ObservableCollection<recordatorios> GetListRecord
        {
            get { return ListRecordatorios; }
            set { ListRecordatorios = value; OnPropertyChange(); }
        }

        public recordatorios GetRecordatorio
        {
            get { return _Recodatorio; }
            set { _Recodatorio = value; OnPropertyChange(); }
        }


        public INavigation Navigation { set; get; }
        public ICommand DetallesCommand { set; get; }

        public VMlstRecordatorios(INavigation navigation)
        {
            Navigation = navigation;
            DetallesCommand = new Command<Type>(async (pageType) => await LlenarDetalle(pageType));
            GetListRecord = new ObservableCollection<recordatorios>();
            llenarLista();
        }

        async Task llenarLista()
        {
            ListRecordatorios = new ObservableCollection<recordatorios>();
            Consultas query = new Consultas();
            var lista = await query.getRecordatorios();

            foreach (var next in lista) {
                recordatorios dato =  new recordatorios();
                dato.ID = next.ID;
                dato.DESCR = next.DESCR;
                dato.FECHA = next.FECHA;
                dato.AUDIO = next.AUDIO;
                dato.IMAGE = next.IMAGE;


                ListRecordatorios.Add(dato);
            }
        }


        async Task LlenarDetalle(Type pageType)
        {

            if (GetRecordatorio != null)
            {
                var pagina = (Page)Activator.CreateInstance(pageType);

                pagina.BindingContext = new recordatorios()
                {
                    ID = GetRecordatorio.ID,
                    DESCR = GetRecordatorio.DESCR,
                    IMAGE = GetRecordatorio.IMAGE,
                    FECHA = GetRecordatorio.FECHA,
                    AUDIO = GetRecordatorio.AUDIO
                };

                await Navigation.PushAsync(pagina);
            }
        }

    }
}
