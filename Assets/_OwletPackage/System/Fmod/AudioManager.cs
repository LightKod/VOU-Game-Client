using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet
{
    public enum AudioChannel
    {
        Master, Music, Ambience, SFX,
    }

    public class AudioManager : Singleton<AudioManager>
    {

        [Header("Default Volume")]
        [Range(0, 2)]
        public float masterVolume = 1;
        [Range(0, 1)]
        public float musicVolume = 1;
        [Range(0, 1)]
        public float ambienceVolume = 1;
        [Range(0, 1)]
        public float SFXVolume = 1;

        private List<EventInstance> eventInstances;
        private List<StudioEventEmitter> eventEmitters;

        private EventInstance ambienceEventInstance;
        private EventInstance musicEventInstance;

        private Bus masterBus;
        private Bus musicBus;
        private Bus ambienceBus;
        private Bus sfxBus;

        [SerializeField] EventReference music;

        protected override void Init()
        {
            base.Init();
            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
            sfxBus = RuntimeManager.GetBus("bus:/SFX");

            eventInstances = new List<EventInstance>();
            eventEmitters = new List<StudioEventEmitter>();
            SetupDefaultVolume();
            InitializeMusic(music);
        }

        private void OnDestroy()
        {
            if (!init) return;
            CleanUp();
        }

        [Button]
        public void SetVolume(AudioChannel channel, float value)
        {
            switch (channel)
            {
                case AudioChannel.Master:
                    masterBus.setVolume(value);
                    PlayerPrefs.SetFloat(playerPrefMaster, value);
                    break;
                case AudioChannel.Music:
                    musicBus.setVolume(value);
                    PlayerPrefs.SetFloat(playerPrefMusic, value);
                    break;
                case AudioChannel.Ambience:
                    ambienceBus.setVolume(value);
                    PlayerPrefs.SetFloat(playerPrefAmbience, value);
                    break;
                case AudioChannel.SFX:
                    sfxBus.setVolume(value);
                    PlayerPrefs.SetFloat(playerPrefSfx, value);
                    break;
                default:
                    break;
            }
        }

        public float GetVolume(AudioChannel channel)
        {
            float volume = 0;
            switch (channel)
            {
                case AudioChannel.Master:
                    masterBus.getVolume(out volume);
                    break;
                case AudioChannel.Music:
                    musicBus.getVolume(out volume);
                    break;
                case AudioChannel.Ambience:
                    ambienceBus.getVolume(out volume);
                    break;
                case AudioChannel.SFX:
                    sfxBus.getVolume(out volume);
                    break;
                default:
                    break;
            }

            return volume;
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPosition)
        {
            RuntimeManager.PlayOneShot(sound, worldPosition);
        }

        public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
        {
            StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
            emitter.EventReference = eventReference;
            eventEmitters.Add(emitter);
            return emitter;
        }

        public EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstances.Add(eventInstance);
            return eventInstance;
        }

        public void SetMusic(float value)
        {
            musicEventInstance.setParameterByName("BGMusic", value);
            musicEventInstance.start();
        }

        private void InitializeAmbience(EventReference ambienceEventReference)
        {
            ambienceEventInstance = CreateInstance(ambienceEventReference);
            ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        private void InitializeMusic(EventReference musicEventReference)
        {
            musicEventInstance = CreateInstance(musicEventReference);
            musicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        

        private void CleanUp()
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
            foreach (StudioEventEmitter emitter in eventEmitters)
            {
                emitter.Stop();
            }

            ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ambienceEventInstance.release();

            musicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicEventInstance.release();
        }


        const string playerPrefSfx = "Audio_SFX";
        const string playerPrefMusic = "Audio_Music";
        const string playerPrefAmbience = "Audio_Ambience";
        const string playerPrefMaster = "Audio_Master";
        void SetupDefaultVolume()
        {
            PlayerPrefs.SetFloat(playerPrefMaster, masterVolume);
            SetVolume(AudioChannel.Master, PlayerPrefs.GetFloat(playerPrefMaster, masterVolume));
            SetVolume(AudioChannel.Music, PlayerPrefs.GetFloat(playerPrefMusic, musicVolume));
            SetVolume(AudioChannel.Ambience, PlayerPrefs.GetFloat(playerPrefAmbience, ambienceVolume));
            SetVolume(AudioChannel.SFX, PlayerPrefs.GetFloat(playerPrefSfx, SFXVolume));
        }
      
    }
}
