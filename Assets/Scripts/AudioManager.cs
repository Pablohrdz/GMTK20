using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Text.RegularExpressions;
using System.Linq;

public enum AudioEvent
{
    Stop = 0,
    Play = 1
}

public class AudioEventArgs
{
    public string sampleId;
    public bool loop;
    public float volume = 1.0f;
    public float delaySeconds = 0.0f;
    public string mixerChannelName = "FX";
    public float throttleSeconds = 0.0f;
    public bool restart = false;
}

public interface Sample
{
    void play(AudioSource source, AudioMixerGroup mixerGroup, AudioEventArgs args);
    void stop(AudioEventArgs args);
}

public class RandomSampleVariation : Sample
{
    public List<AudioClip> clips;
    private bool playing = false;
    private bool dfdfdfrestoreState = false;
    private Dictionary<string, AudioSource> previousSourceStates = new Dictionary<string, AudioSource>();
    // private AudioSource currentSource;

    public void play(AudioSource source, AudioMixerGroup mixerGroup, AudioEventArgs args)
    {
        var idx = UnityEngine.Random.Range(0, clips.Count);
        var clip = clips[idx];

        if (args.loop)
        {
            // TODO to save previous states
            // var previousSourceState = UnityEngine.Object.Instantiate(source);
            if (args.restart || !source.isPlaying)
            {

              
                source.Stop();

                source.loop = args.loop;
                source.clip = clip;
                source.volume = args.volume;
                source.outputAudioMixerGroup = mixerGroup;
                source.PlayDelayed(args.delaySeconds);
            }
        }
        else
        {
            source.PlayOneShot(clip, args.volume);
        }
        previousSourceStates.AddOrUpdate(args.sampleId, source);
    }
    public void stop(AudioEventArgs args)
    {

        if (previousSourceStates.ContainsKey(args.sampleId))
        {
            
            var previousSourceState = previousSourceStates[args.sampleId];
            previousSourceState.Stop();
            // previousSourceState.clip = previousSourceState.clip;
            previousSourceState.volume = args.volume;
            previousSourceState.loop = false;
            // currentSource.loop = previousSourceState.loop;
            // currentSource.clip = previousSourceState.clip;
            // currentSource.volume = previousSourceState.volume;
        }

        // playing = currentSource.isPlaying;
        previousSourceStates.Remove(args.sampleId);
    }
}

// Utility
public static class Extensions
{
    public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key].Add(value);
        }
        else
        {
            dictionary.Add(key, new List<TValue> { value });
        }
    }

    public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey,TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }
}

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;

    public AudioClip[] clips;
    private Dictionary<string, Sample> samples;
    private Dictionary<string, float> throttleTimers = new Dictionary<string, float>();
    // pseudo singleton, without all the problems
    public static AudioManager instance
    {
        get
        {
            return GameObject.FindWithTag("GameController").GetComponent<AudioManager>();
        }
    }

    public bool sendAudioEvent(AudioEvent type, AudioEventArgs args)
    {
        return sendAudioEvent(type, this.GetComponent<AudioSource>(), args);
    }

    public bool sendAudioEvent(AudioEvent type, AudioSource source, AudioEventArgs args)
    {
        var groups = mixer.FindMatchingGroups(args.mixerChannelName);
        if (groups.Length > 0)
        {
            var mixerGroup = groups[0];
            if (samples.ContainsKey(args.sampleId))
            {
                var sample = samples[args.sampleId];
                switch (type)
                {
                    case AudioEvent.Play:
                        var time = Time.time;
                       
                        if (time - throttleTimers[args.sampleId] < args.throttleSeconds)
                        {
                            break;
                        }
                        throttleTimers[args.sampleId] = time;
                        if (args.delaySeconds > 0.0)
                        {
                            StartCoroutine(delayedFunc(() => sample.play(source, mixerGroup, args), args.delaySeconds));
                        }
                        else
                        {
                            sample.play(source, mixerGroup, args);
                        }
                        break;
                    case AudioEvent.Stop:
                        sample.stop(args);
                        break;
                }
                return true;
            }
            else
            {
                Debug.LogError("Sample unavaliable: '" + args.sampleId + "' make sure to add it to the 'clips' array");
            }
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        var time = Time.time;
        Regex rx = new Regex(@"(.*)[\-]([0-9]+)$",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var dict = new Dictionary<string, List<AudioClip>>();

        foreach (var clip in clips)
        {
            var match = rx.Match(clip.name);
            if (match.Groups.Count > 1)
            {
                var key = match.Groups[1].Value;
                // TODO use index correctly, not needed for random sample variation
                // var index = match.Groups[2];
                // Debug.Log("MATCH " + key);
                dict.AddOrUpdate(key, clip);
                throttleTimers.AddOrUpdate(key, time);
            }
            else
            {
                // Debug.LogError("Malformed audio clip name: \"" + clip.name + "\"");
                dict.AddOrUpdate(clip.name, clip);
                throttleTimers.AddOrUpdate(clip.name, time);
            }
        }

        this.samples = dict.ToDictionary(
            pair => pair.Key,
            pair => new RandomSampleVariation() { clips = pair.Value } as Sample
        );

        StartCoroutine(startAmbiance(8.0f, 17.0f));
    }

    // Update is called once per frame
    void Update()
    {


    }

    private IEnumerator startAmbiance(float rangeStartSeconds, float rangeEndSeconds)
    {
        var wait = Random.Range(rangeEndSeconds, rangeEndSeconds);
        yield return new WaitForSeconds(wait);
        var sampNames = new string[] { "whale-communication-fx", "radar" };
        var idx = Random.Range(0, sampNames.Length);
        this.sendAudioEvent(AudioEvent.Play, this.GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = sampNames[idx], volume = 0.9f, mixerChannelName = "Ambiance" });
        StartCoroutine(startAmbiance(rangeEndSeconds, rangeEndSeconds));
    }

    private IEnumerator delayedFunc(System.Action func, float waitTimeSeconds)
    {
        yield return new WaitForSeconds(waitTimeSeconds);
        func();
    }

    public IEnumerator StartFade(string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            mixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}

