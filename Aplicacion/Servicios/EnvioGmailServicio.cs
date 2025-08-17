using System.Net.Mail;
using System.Net;
using Aplicacion.Configuraciones;
using Aplicacion.DTOs;

namespace Aplicacion.Servicios
{
    public class EnvioGmailServicio 
    {


        private readonly SmtpSettings _smtp;



        public EnvioGmailServicio()
        {


            _smtp = new SmtpSettings();



        }


        public async Task EnviarGmail(string nombre, string correo, ReservaCitasDTO reservaCitaModel)
        {


            if (string.IsNullOrEmpty(correo))
            {

                throw new ArgumentException("No se encontro el correo");
            }


            if (reservaCitaModel == null)
            {
                LoggerServicio.getInstancia().Error($"Fallo al enviar correo a {correo} la reserva no llego a registrarse");
                throw new Exception("La reserva aun no se registrado");
            }

            _smtp.Server = "smtp.gmail.com";
            _smtp.Port = 587;
            _smtp.UserName = "AgendaBookify@gmail.com";
            _smtp.SenderEmail = "AgendaBookify@gmail.com";
            _smtp.SenderName = "Bookify";
            _smtp.Password = "shss wuax lhan zgsb";






            string asunto = "Confirmación de cita";

            string cuerpo = $""""
                            
                            Estimado/a {nombre},

                            Hemos recibido su solicitud de cita en nuestro sistema y se encuentra en proceso de validación.  
                            Próximamente le enviaremos la confirmación con los detalles finales. Mientras tanto, le sugerimos mantener disponible la fecha y hora solicitadas.  

                            Ante cualquier duda o si desea modificar la reserva, puede contactarnos sin inconvenientes.  

                            Detalles de la cita:  
                            Fecha: {reservaCitaModel.Fecha}  
                            Hora: {reservaCitaModel.Hora}  
                            Estado: {reservaCitaModel.Estado}  

                            Gracias por confiar en nosotros.  

                            Atentamente,  
                            Bookify  
                            {_smtp.SenderEmail}  

                            """";




            using (var cliente = new SmtpClient(_smtp.Server, _smtp.Port))
            {
                cliente.Credentials = new NetworkCredential(_smtp.UserName, _smtp.Password);
                cliente.EnableSsl = true;

                var mensaje = new MailMessage(_smtp.SenderEmail, correo, asunto, cuerpo);

                LoggerServicio.getInstancia().Info($"Correo enviado sastifactoriamente a '{correo}' despues de reservar su cita");
                await cliente.SendMailAsync(mensaje);
            }
        }

    }
}
