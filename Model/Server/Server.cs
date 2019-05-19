﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Model;
using Newtonsoft.Json;

namespace Model
{
    public class Server
    {
        internal static Thread _ListenTh;
        internal static bool _isListening = true;

        internal void StartServer()       
        {
            //Préparation et démarrage du thread en charge d'écouter.
            _ListenTh = new Thread(new ThreadStart(Listen));
            _ListenTh.Start();
        }

        internal static void Listen()
        {

            UdpClient server = new UdpClient(5035);


            //Création d'une boucle infinie qui aura pour tâche d'écouter.
            while (_isListening)
            {
                //On crée le serveur en lui spécifiant le port sur lequel il devra écouter.

                //Création d'un objet IPEndPoint qui recevra les données du Socket distant.
                IPEndPoint client = null;
                //On écoute jusqu'à recevoir un message.
                byte[] data = server.Receive(ref client);
                string message = Encoding.Default.GetString(data);
                KeyPress keyPress = JsonConvert.DeserializeObject<KeyPress>(message);
                keyPress._game._controls.Update(keyPress._key);
                Console.WriteLine(message);
            }
        }
    }
}