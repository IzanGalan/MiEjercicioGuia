﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics.Eventing.Reader;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        }

   
        private void button2_Click(object sender, EventArgs e)
        {
            private void button2_Click(object sender, EventArgs e)
            {
                if (Longitud.Checked)
                {
                    // Quiere saber la longitud
                    string mensaje = "1/" + nombre.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    byte[] msg2 = new byte[80]; // Se puede ajustar si se espera más de 80 caracteres
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    MessageBox.Show("La longitud de tu nombre es: " + mensaje);
                }
                else if (Bonito.Checked)
                {
                    // Quiere saber si el nombre es bonito
                    string mensaje = "2/" + nombre.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    byte[] msg2 = new byte[80]; // Se puede ajustar si se espera más de 80 caracteres
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                    if (mensaje == "SI")
                        MessageBox.Show("Tu nombre ES bonito.");
                    else
                        MessageBox.Show("Tu nombre NO es bonito. Lo siento.");
                }
                else if (Alto.Checked)
                {
                    // Quiere saber si el nombre es alto
                    string alturaText = Altura.Text;

                    // Verificar si la altura ingresada es un número
                    if (float.TryParse(alturaText, out float altura))
                    {
                        // Formato esperado: "3/nombre/altura"
                        string mensaje = "3/" + nombre.Text + "/" + alturaText;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        byte[] msg2 = new byte[80]; // Tamaño adecuado para la respuesta
                        server.Receive(msg2);
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                        MessageBox.Show(mensaje); // Muestra la respuesta recibida
                    }
                    else
                    {
                        MessageBox.Show("Por favor, ingresa una altura válida.");
                    }
                }
            }

        }

        private void Alto_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Conectar_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9080);
            
            //Creamos el Socket
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");


            }
            catch (SocketException ex)
            {
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }
        }

        private void Desconectar_Click(object sender, EventArgs e)
        {
            //Mensaje de Desocnexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Nos desconectamos
            this.BackColor = Color.Red;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }
}
