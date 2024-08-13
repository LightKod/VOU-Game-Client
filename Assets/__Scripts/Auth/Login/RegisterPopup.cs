using Owlet.UI.Popups;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace VOU
{
    public class RegisterPopup : Popup
    {
        [SerializeField] Button btnConfirm;
        [SerializeField] InputFieldValidation inputEmail;
        [SerializeField] InputFieldValidation inputPassword;
        [SerializeField] InputFieldValidation inputConfirmPassword;

        private void Awake()
        {
            btnConfirm.onClick.AddListener(SendRegisterRequest);
        }

        protected override void OnEnableUI()
        {
            ToggleInteraction(true);
            base.OnEnableUI();
        }

        async void SendRegisterRequest()
        {
            string email = inputEmail.GetValue();
            string password = inputPassword.GetValue();
            string confirmPassword = inputConfirmPassword.GetValue();

            if (email.IsNullOrWhitespace())
            {
                ToastHandler.instance.Show("Please enter your email");
                return;
            }
            if(password.IsNullOrWhitespace())
            {
                ToastHandler.instance.Show("Please enter your password");
                return;
            }
            if(password != confirmPassword)
            {
                ToastHandler.instance.Show("Confirm password is not identical");
                return;
            }

            ToggleInteraction(false);


            Dictionary<string, string> form = new()
            {
                { "name", "AAA"},
                { "email", email },
                { "password", password },
            };

            await HttpClient.PostRequest(HttpClient.GetURL(Env.Routes.Auth.Register), form, OnRegisterSuccess, OnRegisterFailed);
        }

        void OnRegisterSuccess(string msg)
        {
            Debug.Log(msg);
            //TODO: a verification step?
            ToastHandler.instance.Show("Register Successful", ToastState.Success);
            SelfClosing();
        }

        void OnRegisterFailed(string msg)
        {
            Debug.Log(msg);
            ToggleInteraction(true);
        }

        void ToggleInteraction(bool active)
        {
            btnConfirm.enabled = active;
        }

    }
}
