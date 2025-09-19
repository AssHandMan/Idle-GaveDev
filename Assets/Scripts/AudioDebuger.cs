using UnityEngine;
using System.Collections.Generic;

public class GlobalAudioMonitor : MonoBehaviour
{
    public List<AudioSource> allAudioSources = new List<AudioSource>();
    void Update()
    {
        // Находим все AudioSource в сцене
        var currentSources = FindObjectsOfType<AudioSource>();

        foreach (var source in currentSources)
        {
            if (source.isPlaying && !allAudioSources.Contains(source))
            {
                Debug.Log($"НОВОЕ ВОСПРОИЗВЕДЕНИЕ: Объект '{source.gameObject.name}' " +
                         $"воспроизводит '{source.clip?.name}'");
                allAudioSources.Add(source);
            }
            else if (!source.isPlaying && allAudioSources.Contains(source))
            {
                Debug.Log($"ВОСПРОИЗВЕДЕНИЕ ЗАВЕРШЕНО: Объект '{source.gameObject.name}'");
                allAudioSources.Remove(source);
            }
        }
    }
}