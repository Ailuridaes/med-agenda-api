using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;

namespace MedAgenda.API.HelperFunctions
{
    public class TwilioSmsHelper
    {

        public static string SendSms(string telephone, string messageContents)
        {
            string smsOutcome = "Sms sent successfully";

            var accountSid = "AC5d8b2507e705c81e779763db62d3504e";
            var authToken = "e6841974aa429fd007d375e2f58e5ff2";
            var twilioNumber = "+18588779197";

            var twilio = new TwilioRestClient(accountSid, authToken);
            var message = twilio.SendMessage(
                twilioNumber,
                telephone,
                messageContents
                );

            return smsOutcome;
        }
    }
}