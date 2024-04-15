using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioEvent
{
    BOOK_DROP,
    SHOE_DROP,
    TRUNK_CLOSE,
    WINE,
    OTHER,
    SWEEP
}

public enum MusicType
{
    MAIN
}

public class AudioManager : MonoBehaviour
{
    public delegate void OnMusicLoaded();
    public OnMusicLoaded onLoad;

    public bool hasLoaded;

    Dictionary<AudioEvent, AudioClip> sfxClips;
    Dictionary<MusicType, AudioClip> musicClips;

    private AudioSource _audioSFX;
    private AudioSource _audioMusic;

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        if (Instance != this) Destroy(gameObject); 

        DontDestroyOnLoad(gameObject);

        _audioSFX = gameObject.AddComponent<AudioSource>();
        _audioMusic = gameObject.AddComponent<AudioSource>();
        _audioMusic.volume = 0.7f;
        _audioSFX.volume = 0.7f;

        sfxClips = new Dictionary<AudioEvent, AudioClip>();

        sfxClips.Add(AudioEvent.BOOK_DROP, loadClip("Book1"));
        sfxClips.Add(AudioEvent.SHOE_DROP, loadClip("Snoedrop"));
        sfxClips.Add(AudioEvent.TRUNK_CLOSE, loadClip("Close_Trunk"));
        sfxClips.Add(AudioEvent.SWEEP, loadClip("Sweep"));
        sfxClips.Add(AudioEvent.WINE, loadClip("Wine"));
        sfxClips.Add(AudioEvent.OTHER, loadClip("Other"));

        musicClips = new Dictionary<MusicType, AudioClip>();

        musicClips.Add(MusicType.MAIN, loadClip("basicSong1"));

        Debug.Log("~~ Audio files loaded! ~~");
        hasLoaded = true;

        onLoad?.Invoke();
    }

    public void PlayOneShot(AudioEvent ev)
    {
        AudioClip clip;
        if (!sfxClips.TryGetValue(ev, out clip))
        {
            Debug.Log("Clip not loaded for event: " + ev.ToString());
            return;
        }

        _audioSFX.PlayOneShot(clip);
    }

    public void PlayMusic(MusicType music)
    {
        AudioClip clip;
        if (!musicClips.TryGetValue(music, out clip))
        {
            Debug.Log("Clip not loaded for music: " + music.ToString());
            return;
        }

        _audioMusic.clip = clip;
        _audioMusic.volume = 0.35f;
        _audioMusic.loop = true;
        _audioMusic.Play();
    }

    public void StopAll()
    {
        _audioSFX.Stop();
        _audioMusic.Stop();
    }

    private AudioClip loadClip(string name)
    {
        return (AudioClip)Resources.Load("Audio/" + name);
    }

}
