using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;

namespace ExaRDD.Conexion
{
    public class dbFireBase
    {
        public static FirebaseClient dbRecordatorios = new FirebaseClient("https://dbrecordatorios-default-rtdb.firebaseio.com/");
    }
}
