using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Owlet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VOU.Env.Routes;

namespace VOU
{
    public class FirebaseAuthManager : Singleton<FirebaseAuthManager>
    {
        Firebase.Auth.FirebaseAuth auth;

        public void SendPhoneVerificationRequest(string phoneNumber, Action onSuccess, Action onFail)
        {
            auth = FirebaseAuth.DefaultInstance;
            PhoneAuthProvider provider = PhoneAuthProvider.GetInstance(auth);
            Debug.Log($">>> Phone: {phoneNumber}");
            provider.VerifyPhoneNumber(
              new Firebase.Auth.PhoneAuthOptions
              {
                  PhoneNumber = phoneNumber,
                  TimeoutInMilliseconds = 100000,
                  ForceResendingToken = null
              },
              verificationCompleted: (credential) => {
                  Debug.Log(credential);
                  onSuccess?.Invoke();
                  // Auto-sms-retrieval or instant validation has succeeded (Android only).
                  // There is no need to input the verification code.
                  // `credential` can be used instead of calling GetCredential().
              },
              verificationFailed: (error) => {
                  onFail?.Invoke();
                  Debug.LogError($"Verification failed: {error}");
              },
              codeSent: (id, token) => {
                  Debug.Log($"Code sent ID: {id} | token: {token}");

                  // Verification code was successfully sent via SMS.
                  // `id` contains the verification id that will need to passed in with
                  // the code from the user when calling GetCredential().
                  // `token` can be used if the user requests the code be sent again, to
                  // tie the two requests together.
              },
              codeAutoRetrievalTimeOut: (id) => {
                  onFail?.Invoke();
                  Debug.Log($"Code timeout ID: {id}");

                  // Called when the auto-sms-retrieval has timed out, based on the given
                  // timeout parameter.
                  // `id` contains the verification id of the request that timed out.
              });
        }
    }
}
