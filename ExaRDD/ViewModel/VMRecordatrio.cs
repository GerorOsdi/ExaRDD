using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ExaRDD.FireBase;
using ExaRDD.Model;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Diagnostics;
using Plugin.AudioRecorder;
using Xamarin.Essentials;
using System.IO;

namespace ExaRDD.ViewModel
{
    public class VMRecordatrio : Base
    {
            private string _id;
            private string _Descripcion;
            private double _Fecha;
            private string _image;
            private string _audio;

            //id del recordatorio guardado en firebase
            private string idRecord;

            //Variableas para obtener imagen
            private string urlImage;
            private MediaFile imgStream;
            private ImageSource streamImageSource;

            //Variables de audio
            private AudioRecorderService audio = new AudioRecorderService();
            private Stream audioStream;
            string urlAudio = "-";
            private AudioPlayer playAudio = new AudioPlayer();

            //Varialbes para trabajar las fechas.
            public DateTime fecha;
            public DateTime ahora = DateTime.Now.Date;


            public string id {
                get { return _id; }
                set { this._id = value; OnPropertyChange(); }
            }

            public string Descripcion
            {
                get { return _Descripcion; }
                set { this._Descripcion = value; OnPropertyChange(); }
            }

            public double Fecha
            {
                get { return _Fecha; }
                set { this._Fecha = value; OnPropertyChange(); }
            }

            public string Image
            {
                get { return _image; }
                set { this._image = value; OnPropertyChange(); }
            }

            public string Audio
            {
                get { return _audio; }
                set { this._audio = value; OnPropertyChange(); }
            }

            public DateTime selectFecha {
            get { return fecha; }
            set
            {
                fecha = value; OnPropertyChange("selectFecha");
            }
            }

            public DateTime nowFecha
            {
                get { return ahora; }
                set { this.ahora = value; OnPropertyChange(); }
            }

        public ICommand CleanCommand { get; set; }
            public ICommand setImageCommand { get; set; }
            public ICommand SaveCommand { get; set; }
            public ICommand SetGrabar { get; set; }
            public ICommand AsynAudioCommand { get; set; }

        void limpiar()
            {
                id = String.Empty;
                Descripcion = String.Empty;
                Fecha = 0.0;
                Image = String.Empty;
                Audio = String.Empty;
            }


            async void SaveRecord()
            {
            
            Fecha = Convert.ToDouble(fecha.ToOADate()); 
            Consultas fbSAve = new Consultas();
            recordatorios datos = new recordatorios();
            datos.DESCR = Descripcion;
            datos.IMAGE = "-";
            datos.AUDIO = urlAudio;
            datos.FECHA = Fecha;


            idRecord = await fbSAve.setRecordatorio(datos);
            await setImageStorage();
            //Subir audio a fire base
            urlAudio = await fbSAve.postAudio(audioStream, idRecord);
            //Subir la imagen a firebase
            await putRecordatorio();
            }

            public VMRecordatrio()
            {
                CleanCommand = new Command(() => limpiar());
                SaveCommand = new Command(() => SaveRecord());
                setImageCommand = new Command(() => setImage());
                SetGrabar = new Command(() => GrabarAudio());
                AsynAudioCommand = new Command(() => ReproducirAudio());
        }

        private async Task setImageStorage()
        {
            Consultas funcion = new Consultas();
            urlImage = await funcion.postImage(imgStream.GetStream(), idRecord);

        }

        private async void setImage()
        {

            await CrossMedia.Current.Initialize();
            try
            {
                imgStream = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (imgStream == null)
                {
                    return;
                }
                else
                {
                    streamImageSource = ImageSource.FromStream(() =>
                    {
                        var rutaImagen = imgStream.GetStream();
                        return rutaImagen;
                    });

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }


        private async Task putRecordatorio()
        {
                Consultas consultas = new Consultas();
                recordatorios datos = new  recordatorios();
                datos.ID = idRecord;
                datos.IMAGE = urlImage;
                datos.DESCR = Descripcion;
                datos.AUDIO = urlAudio;
                datos.FECHA = Fecha;

                await consultas.putRecordatorios(datos);
        }
        private async void GrabarAudio()
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
                return;
            
            if (audio.IsRecording)
            {
                await audio.StopRecording();
                playAudio.Play(audio.GetAudioFilePath());
                audioStream = audio.GetAudioFileStream();

            }
            else {
                await audio.StartRecording();
            }
            
        }

        private async void ReproducirAudio(){
            if (audio.IsRecording){
                //await     
            }
        }
    }
}
