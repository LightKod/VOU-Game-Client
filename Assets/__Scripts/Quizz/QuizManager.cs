using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Owlet;
using Owlet.UI;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace VOU
{
    //TODO: Upgrade this to use Socket
    public class QuizManager : Singleton<QuizManager>
    {
        QuizService service;

        public Action<int> onAnswerSelected;
        public Action<QuestionObject> onQuestionReceive;
        public Action<AudioClip> onMCDataReceieve;

        string roomID = "20";

        protected override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            SetupConnection();

            QuizTimer.onCounterFinish += ClosePopup;
        }

        private void OnDestroy()
        {
            QuizTimer.onCounterFinish -= ClosePopup;

            service?.Dispose();
        }

        public async void SetupConnection()
        {
            service = new();
            await service.CreateConnection();
            service.JoinRoom(roomID);

            service.On(QuizService.EVENT_SEND_QUESTION, OnQuesionReceived);
            service.On(QuizService.EVENT_QUIZ_AUDIO, OnAudioReceived);
            service.On(QuizService.EVENT_SEND_ANSWER, OnAnswerReceievd);
        }

        public void SelectAnswer(int index)
        {
            service.AnswerQuestion(index);
            onAnswerSelected.Invoke(index);
        }

        async void OnQuesionReceived(SocketIOResponse res)
        {
            string questionStr = res.GetValue<string>(0);
            QuestionObject questionObject = JsonConvert.DeserializeObject<QuestionObject>(questionStr);

            QuizzAnswerSelector answerSelector = await PopupManager.instance.OpenUI<QuizzAnswerSelector>(Keys.Popup.QuizzAnswerSelector, 1);
            answerSelector.SetupUI(questionObject);

            onQuestionReceive?.Invoke(questionObject);
        }

        async void OnAudioReceived(SocketIOResponse res)
        {
            string audioStr = res.GetValue<string>(0);
            Debug.Log($"Audio: {audioStr}");
            AudioClip clip = await HandleAudio(audioStr);
            onMCDataReceieve?.Invoke(clip);
        }

        public async UniTask<AudioClip> HandleAudio(string audioStr)
        {
            var audioBytes = Convert.FromBase64String(audioStr);
            var tempPath = Path.Combine(Application.persistentDataPath, "tmpMP3Base64.mp3");

            // Write bytes to a temporary file
            await File.WriteAllBytesAsync(tempPath, audioBytes);
            Debug.Log($"Audio byte length: {audioBytes.Length}");
            Debug.Log($"Temp path: {tempPath}");

            // Use UnityWebRequest to load the audio file into an AudioClip
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + tempPath, AudioType.MPEG))
            {
                ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
                await www.SendWebRequest();

                Debug.Log(www.result);

                DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)www.downloadHandler;

                if (dlHandler.isDone)
                {
                    AudioClip audioClip = dlHandler.audioClip;

                    if (audioClip != null)
                    {
                        var _audioClip = DownloadHandlerAudioClip.GetContent(www);
                        Debug.Log("Playing song using Audio Source!");
                        return _audioClip;

                    }
                    else
                    {
                        Debug.Log("Couldn't find a valid AudioClip :(");
                    }
                }
                else
                {
                    Debug.Log("The download process is not completely finished.");
                }
            }

            return null;
        }

        async void OnAnswerReceievd(SocketIOResponse res)
        {
            string ansStr = res.GetValue<string>(1);
            AnswerObject answerObject = JsonConvert.DeserializeObject<AnswerObject>(ansStr);

            string qstStr = res.GetValue<string>(0);
            QuestionObject questionObject = JsonConvert.DeserializeObject<QuestionObject>(qstStr);

            QuizzAnswerSelector answerSelector = await PopupManager.instance.OpenUI<QuizzAnswerSelector>(Keys.Popup.QuizzAnswerSelector, 1);
            answerSelector.SetupUIResult(questionObject, answerObject);

            await UniTask.Delay(TimeSpan.FromSeconds(10), ignoreTimeScale: false);
            ClosePopup();
        }

        private void ClosePopup()
        {
            PopupManager.instance.CloseUI(Keys.Popup.QuizzAnswerSelector);
        }
    }
}
