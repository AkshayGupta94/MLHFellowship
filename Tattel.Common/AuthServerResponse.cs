using System;
using System.Collections.Generic;
using System.Text;

namespace Tattel.Common
{
    public class AuthServerResponse
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}

/*
var response = AuthHelper.Authenticate(AppSettings.SecurityService.Uri, "proctor", "secret", txtUsername.Text, txtPassword.Text,
                    "openid profile role webapi");

                if (response.ResponseCode > 0)
                {
                    viewModel.IsBusy = false;
                    await DisplayAlert(AppVariables.AppName, response.Message, "OK");
                    return;
                }
*/

