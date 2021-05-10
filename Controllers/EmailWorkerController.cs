using System;
using IPByEmail.Models;

namespace IPByEmail.Controllers
{
    public class EmailWorkerController
    {
        IEmailModel _model;
        public void PublicIPProcess()
        {
            _model = new PublicIPByEmailModel("imap.gmail.com", 993,
                true, "dimsonartex@gmail.com", "17890714");

            bool requestIsGot = _model.ProcessResults(_model.GetUnseenMessagesFromInbox());

            if (requestIsGot) _model.SendAnswerBySmtp();
        }
    }
}