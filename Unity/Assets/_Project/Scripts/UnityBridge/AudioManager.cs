using UnityEngine;
using UnityEngine.Audio;

namespace Legacy.UnityBridge
{
    public sealed class AudioManager : MonoBehaviour
    {
        private const float MutedDecibels = -80f;

        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AudioMixerGroup _masterGroup;
        [SerializeField] private AudioMixerGroup _musicGroup;
        [SerializeField] private AudioMixerGroup _ambienceGroup;
        [SerializeField] private AudioMixerGroup _sfxGroup;
        [SerializeField] private AudioMixerGroup _uiGroup;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _ambienceSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _uiSource;

        public AudioMixer Mixer => _mixer;
        public AudioMixerGroup MasterGroup => _masterGroup;
        public AudioMixerGroup MusicGroup => _musicGroup;
        public AudioMixerGroup AmbienceGroup => _ambienceGroup;
        public AudioMixerGroup SfxGroup => _sfxGroup;
        public AudioMixerGroup UiGroup => _uiGroup;

        private void Awake()
        {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureSources();
        }

        private void OnDestroy()
        {
            if (Instance == this) {
                Instance = null;
            }
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            PlayLooping(_musicSource, clip, loop);
        }

        public void PlayAmbience(AudioClip clip, bool loop = true)
        {
            PlayLooping(_ambienceSource, clip, loop);
        }

        public void PlaySfx(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip == null) {
                return;
            }

            if (_sfxGroup == null) {
                AudioSource.PlayClipAtPoint(clip, position, Mathf.Clamp01(volume));
                return;
            }

            var oneShotObject = new GameObject("SfxOneShot");
            oneShotObject.transform.position = position;
            AudioSource source = oneShotObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _sfxGroup;
            source.clip = clip;
            source.volume = Mathf.Clamp01(volume);
            source.Play();
            Destroy(oneShotObject, clip.length);
        }

        public void PlayUi(AudioClip clip, float volume = 1f)
        {
            if (clip == null) {
                return;
            }

            _uiSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }

        public void SetMixerVolume(string exposedParameter, float normalizedVolume)
        {
            if (_mixer == null || string.IsNullOrWhiteSpace(exposedParameter)) {
                return;
            }

            float decibels = normalizedVolume <= 0f
                ? MutedDecibels
                : Mathf.Log10(Mathf.Clamp01(normalizedVolume)) * 20f;
            _mixer.SetFloat(exposedParameter, decibels);
        }

        private void EnsureSources()
        {
            if (_musicSource == null) {
                _musicSource = CreateSource("MusicSource", _musicGroup);
            }

            if (_ambienceSource == null) {
                _ambienceSource = CreateSource("AmbienceSource", _ambienceGroup);
            }

            if (_sfxSource == null) {
                _sfxSource = CreateSource("SfxSource", _sfxGroup);
                _sfxSource.loop = false;
            }

            if (_uiSource == null) {
                _uiSource = CreateSource("UiSource", _uiGroup);
                _uiSource.loop = false;
            }
        }

        private AudioSource CreateSource(string sourceName, AudioMixerGroup mixerGroup)
        {
            var sourceObject = new GameObject(sourceName);
            sourceObject.transform.SetParent(transform);
            AudioSource source = sourceObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = true;
            source.outputAudioMixerGroup = mixerGroup;
            return source;
        }

        private static void PlayLooping(AudioSource source, AudioClip clip, bool loop)
        {
            if (source == null) {
                return;
            }

            if (clip == null) {
                source.Stop();
                source.clip = null;
                return;
            }

            source.clip = clip;
            source.loop = loop;
            source.Play();
        }
    }
}
