using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.AudioRecorder;
using Xamarin.Essentials;
using System.IO;

namespace ExaRDD
{
    public partial class MainPage : ContentPage
    {
        private AudioRecorderService audio = new AudioRecorderService();
        private AudioPlayer playAudio = new AudioPlayer();
        private FireBase.Consultas consultas = new FireBase.Consultas();
        Stream urlAudio1;
        string urlAudio;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnGrabar_Clicked(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
                return;

            if (audio.IsRecording)
            {
               await audio.StopRecording();
                playAudio.Play(audio.GetAudioFilePath());
                urlAudio1 = audio.GetAudioFileStream();

                urlAudio = await consultas.postAudio(urlAudio1, "Prueba");

            }
            else
            {
               await audio.StartRecording();
            }
        }
    }
}
