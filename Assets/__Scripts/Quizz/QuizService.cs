using Cysharp.Threading.Tasks;
using Owlet;
using Owlet.UI;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class QuizService : SocketService
    {

        public const string EVENT_DEBUG = "debug";
        public const string EVENT_JOIN_ROOM = "joinRoom";
        public const string EVENT_LEAVE_ROOM = "leaveRoom";

        public const string EVENT_SEND_QUESTION = "sendQuestion";
        public const string EVENT_SEND_ANSWER = "sendAnswer";
        public const string EVENT_QUIZ_AUDIO = "audio";
        public const string EVENT_RESULT = "answerResult";
        public const string EVENT_END_QUIZ = "endQuiz";

        string currentRoom;

        protected override string GetURl()
        {
            return ServiceHelper.GetURL(Env.Routes.Quiz.Root);
        }

        public void JoinRoom(string roomID)
        {
            socket.Emit(EVENT_JOIN_ROOM, roomID);
            currentRoom = roomID;
        }

        public void AnswerQuestion(int answerIndex)
        {
            socket.Emit(EVENT_SEND_ANSWER, currentRoom, answerIndex);
        }
    }
}
