using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MonPDLib.General
{
    public class MailHelper
    {
        public static void SendMail(bool html, string judulemail, string pesan, byte[]? file)
        {
            string emailtujuan = "iwe.zakaria22@gmail.com";
            string SystemMail = "infobapenda@surabaya.go.id";
            string MailHost = "mail.surabaya.go.id";
            int MailPort = 587;
            string MailPass = "B4p3nd4Dihati.2024";
            bool ssl = true;

            SmtpClient smpt = new SmtpClient();
            MailMessage mail = new MailMessage();


            smpt.Credentials = new System.Net.NetworkCredential(SystemMail, MailPass);
            smpt.Port = MailPort;
            smpt.Host = MailHost;
            smpt.EnableSsl = ssl;
            mail.From = new MailAddress(SystemMail);
            mail.To.Add(emailtujuan);
            mail.IsBodyHtml = html;
            mail.Subject = judulemail;
            mail.Body = pesan;

            if (file != null)
            {
                Attachment fileAttach = new Attachment(new MemoryStream(file), "file.pdf", MediaTypeNames.Application.Pdf);
                mail.Attachments.Add(fileAttach);
            }

            smpt.Send(mail);
            mail.Dispose();
        }
    }
}
