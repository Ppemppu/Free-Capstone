using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public int maxSimultaneousSounds = 1;

    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // AudioSource 초기화
        for (int i = 0; i < maxSimultaneousSounds; i++)
        {
            GameObject soundObject = new GameObject("PooledSound");
            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            soundObject.SetActive(false);
            audioSources.Add(source);
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        // 현재 재생 중인 사운드 수 체크
        int playingSoundCount = audioSources.FindAll(source => source.isPlaying).Count;

        // 최대 동시 사운드 수를 초과하면 무시
        if (playingSoundCount >= maxSimultaneousSounds) return;

        AudioSource source = GetAvailableAudioSource();
        if (source == null) return;

        source.clip = clip;
        source.volume = volume;
        source.gameObject.SetActive(true);
        source.Play();

        StartCoroutine(DeactivateAfterPlaying(source));
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying) return source;
        }
        return null;
    }

    private IEnumerator DeactivateAfterPlaying(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        source.gameObject.SetActive(false);
    }
}