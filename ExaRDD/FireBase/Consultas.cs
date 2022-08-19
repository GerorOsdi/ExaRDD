using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database.Query;
using Firebase.Storage;
using System.IO;
using ExaRDD.Conexion;
using ExaRDD.Model;
using System.Threading.Tasks;
using System.Linq;

namespace ExaRDD.FireBase
{

    public class Consultas
    {
        private string idRecord;
        private string urlImage;
        private string urlAudio;
        public async Task<string> setRecordatorio(recordatorios datos)
        {
            //child agregar o poder utilizar una tabla y PostAsync es para insertat datos a la tabla
            var data = await dbFireBase.dbRecordatorios
                  .Child("tbRecordatorios")
                  .PostAsync(new recordatorios()
                  {
                      ID = datos.ID,
                      DESCR = datos.DESCR,
                      FECHA = datos.FECHA,
                      AUDIO = datos.AUDIO,
                      IMAGE = datos.IMAGE,

                  });

            idRecord = data.Key;
            return idRecord;

        }

        public async Task<string>  postImage(Stream ImagenStream, string Idusuarios)
        {
            var dataAlmacenamiento = await new FirebaseStorage("dbrecordatorios.appspot.com")
                .Child("imgRecordatorios")
                .Child(Idusuarios + ".jpg")
                .PutAsync(ImagenStream);
            urlImage = dataAlmacenamiento;
            return urlImage;
        }

        public async Task<string> postAudio(Stream ImagenStream, string Idusuarios)
        {
            var dataAlmacenamiento = await new FirebaseStorage("dbrecordatorios.appspot.com")
                .Child("imgRecordatorios")
                .Child(Idusuarios + ".mp3")
                .PutAsync(ImagenStream);
            urlAudio = dataAlmacenamiento;
            return urlAudio;
        }

        public async Task putRecordatorios(recordatorios parametros)
        {
            var obtenerData = (await dbFireBase.dbRecordatorios
                .Child("tbRecordatorios") //comparamos si es la misma key
                .OnceAsync<recordatorios>()).Where(a => a.Key == parametros.ID).FirstOrDefault();

            await dbFireBase.dbRecordatorios
                .Child("tbRecordatorios")
                .Child(obtenerData.Key)
                .PutAsync(new recordatorios()
                {
                    DESCR = parametros.DESCR,
                    IMAGE = parametros.IMAGE,
                    AUDIO = parametros.AUDIO,
                    FECHA = parametros.FECHA,
                });
        }

        public async Task<List<recordatorios>> getRecordatorios()
        {
            List<recordatorios> lstRecordatorios = new List<recordatorios>();

            var data = await dbFireBase.dbRecordatorios
                .Child("tbRecordatorios")
                .OrderByKey()
                .OnceAsync<recordatorios>();
            foreach (var next in data)
            {
                recordatorios datos = new recordatorios();
                datos.ID = next.Key;
                datos.DESCR = next.Object.DESCR;
                datos.FECHA = next.Object.FECHA;
                datos.AUDIO = next.Object.AUDIO;
                datos.IMAGE = next.Object.IMAGE;

                lstRecordatorios.Add(datos);
            }

            return lstRecordatorios;
        }
    }
}
