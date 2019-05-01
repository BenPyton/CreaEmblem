using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;

public class AudioManager {
    
    private List<AudioEvent> m_audioList = new List<AudioEvent>();

    // Get an audio event by name
    // if doesn't exist create it
    // You can't assign it directly
    public AudioEvent this[string _name]
    {
        get {
            AudioEvent audio = GetAudioByName(_name);
            if(audio == null)
            {
                audio = CreateAudioWithName(_name);
            }
            return audio;
        }
    }

    // Return false if event doesn't exist
    public bool ReleaseAudio(string _name)
    {
        bool success = false;
        AudioEvent audio = GetAudioByName(_name);
        if(audio != null)
        {
            audio.Release();
            m_audioList.Remove(audio);
            success = true;
        }
        return success;
    }
    
    // Return false if event doesn't exist
    public bool ReleaseAudio(AudioEvent _audio)
    {
        bool success = false;
        if (_audio != null)
        {
            _audio.Release();
            m_audioList.Remove(_audio);
            success = true;
        }
        return success;
    }

    public void Clear()
    {
        for(int i = 0; i < m_audioList.Count; i++)
        {
            ReleaseAudio(m_audioList[i]);
        }
        m_audioList.Clear();
    }

    private AudioEvent GetAudioByName(string _name)
    {
        AudioEvent audio = null;

        for(int i = 0; i < m_audioList.Count; i++)
        {
            if(m_audioList[i].name == _name)
            {
                audio = m_audioList[i];
            }
        }

        return audio;
    }

    private AudioEvent CreateAudioWithName(string _name)
    {
        AudioEvent audio = new AudioEvent(_name);
        audio.Init();
        m_audioList.Add(audio);
        return audio;
    }

    public void PauseAll(bool _pause)
    {
        //RuntimeManager.PauseAllEvents(_pause);
        foreach(AudioEvent audio in m_audioList)
        {
            audio.Pause(_pause);
        }
    }

    public void SetVCAVolume(string _name, float _volume)
    {
        VCA vca = RuntimeManager.GetVCA("vca:/" + _name);
        if (vca.isValid())
        {
            vca.setVolume(_volume);
        }
    }
}


public class AudioEvent
{
    public enum State {
        None        = 0x00000,
        Starting    = 0x00001,
        Playing     = 0x00010,
        Stopping    = 0x00100,
        Stopped     = 0x01000,
        Sustaining  = 0x10000
    };


    // name of audio event in FMOD
    public string name { get { return m_name; } }


    private string m_name;
    private EventInstance m_audioEvent;
    

    public AudioEvent(string _name)
    {
        m_name = _name;
    }

    // Try to get event from FMOD
    // return false if event doesn't exists in FMOD project
    public bool Init()
    {
        bool success = false;
        try
        {
            m_audioEvent = RuntimeManager.CreateInstance("event:/" + name);
            success = true;
        }
        catch
        {
            Debug.LogError("Can't create Audio Event \"" + name + "\"");
            success = false;
        }
        
        return success;
    }


    // Play the audio
    // Return false if not initialized or already playing
    public bool Play()
    {
        bool success = false;
        if (m_audioEvent.isValid())
        {
            m_audioEvent.start();
            success = true;
        }

        return success;
    }

    // Stop the audio
    // return false if not initialized
    public bool Stop(bool _immediate = true)
    {
        bool success = false;
        if (m_audioEvent.isValid())
        {
            m_audioEvent.stop(_immediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            success = true;
        }

        return success;
    }

    // Pause the sound
    // return false if not initialized
    public bool Pause(bool _paused)
    {
        bool success = false;
        if (m_audioEvent.isValid())
        {
            m_audioEvent.setPaused(_paused);
            success = true;
        }

        return success;
    }

    // Set a FMOD parameter
    // Return false if not initialized
    public bool Set(string _name, float _value)
    {
        bool success = false;
        if (m_audioEvent.isValid())
        {
            m_audioEvent.setParameterValue(_name, _value);
            success = true;
        }

        return success;
    }

    // Get a FMOD parameter
    public float Get(string _name)
    {
        float value = 0.0f;
        float finalValue = 0.0f;
        if (m_audioEvent.isValid())
        {
            // value is the last value from user setParameterValue
            // finalValue is the real value used by FMOD (after automation, modulation, smoothing, etc)
            m_audioEvent.getParameterValue(_name, out value, out finalValue);
        }
        return finalValue;
    }

    // Return none if audio not initialized
    public State GetState()
    {
        PLAYBACK_STATE state;
        State returnState = State.None;

        if (m_audioEvent.isValid())
        {
            m_audioEvent.getPlaybackState(out state);

            switch (state)
            {
                case PLAYBACK_STATE.STARTING:
                    returnState = State.Starting;
                    break;
                case PLAYBACK_STATE.PLAYING:
                    returnState = State.Playing;
                    break;
                case PLAYBACK_STATE.STOPPING:
                    returnState = State.Stopping;
                    break;
                case PLAYBACK_STATE.STOPPED:
                    returnState = State.Stopped;
                    break;
                case PLAYBACK_STATE.SUSTAINING:
                    returnState = State.Sustaining;
                    break;
            }
        }


        return returnState;
    }
    

    // return true if audio is in one of the states 
    public bool IsState(params State[] _states)
    {
        bool validState = false;

        State currentState = GetState();

        for (int i = 0; i < _states.Length; i++)
        {
            if (_states[i] == currentState)
            {
                validState = true;
                i = _states.Length;
            }
        }

        return validState;
    }


    public void Release()
    {
        m_audioEvent.release();
        m_audioEvent.clearHandle();
    }
    
}