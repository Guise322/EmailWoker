using System;
using IpByEmail.Models;
using IpByEmail.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace IpByEmail.Controllers
{
    public class EmailWorkerController
    {
        IEmailModel _model;
        
        public EmailWorkerController()
        {
            _model = new PublicIPByEmailModel();
        }

        public void PublicIPProcess()
        {
            string jsonString = File.ReadAllText("EmailCredentials.json");
            EmailCredentials credentials = JsonSerializer.Deserialize<EmailCredentials>(jsonString);
            
            _model.GetEmailCredentials(credentials);

            bool requestIsGot = _model.ProcessResults(_model.GetUnseenMessagesFromInbox());

            if (requestIsGot) _model.SendAnswerBySmtp();
        }
    }
}