using UnityEngine;
using System.Collections.Generic;

public class AudioMonitor : MonoBehaviour
{
    public List<AudioSource> allAudioSources = new List<AudioSource>();
    public GameObject obj;
    private void Start()
    {
        allAudioSources = FindAllAudioSources();
    }
    void Update()
    {
        // Находим все AudioSource в сцене
        var currentSources = FindObjectsOfType<AudioSource>();

        foreach (var source in currentSources)
        {
            if (source.isPlaying && !allAudioSources.Contains(source))
            {
                Debug.Log($"НОВОЕ ВОСПРОИЗВЕДЕНИЕ: Объект '{source.gameObject.name}' " + $"воспроизводит ");
                allAudioSources.Add(source);
            }
            else if (!source.isPlaying && allAudioSources.Contains(source))
            {
                Debug.Log($"ВОСПРОИЗВЕДЕНИЕ ЗАВЕРШЕНО: Объект '{source.gameObject.name}' ");
                allAudioSources.Remove(source);
            }
        }
    }
    // Метод для поиска всех AudioSource в сцене
    public static List<AudioSource> FindAllAudioSources()
    {
        List<AudioSource> audioSources = new List<AudioSource>();

        // Поиск всех активных объектов с компонентом AudioSource
        AudioSource[] foundSources = FindObjectsOfType<AudioSource>();
        audioSources.AddRange(foundSources);

        //Debug.Log($"Найдено AudioSource: {audioSources.Count}");
        return audioSources;
    }
}