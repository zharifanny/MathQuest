using UnityEngine;
using UnityEngine.Audio; // Untuk menggunakan Audio Mixer

public class AudioController : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup masterMixerGroup; // Referensi ke Audio Mixer Group

    void Start()
    {
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }*/

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch;

            // Hubungkan AudioSource ke Master Mixer Group
            if (masterMixerGroup != null)
            {
                s.source.outputAudioMixerGroup = masterMixerGroup;
            }
        }
    }

    public void PlaySound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Play();
                return;
            }
        }
    }
}
