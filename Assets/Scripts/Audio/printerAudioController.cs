using UnityEditor;
using UnityEngine;

public class printerAudioController : MonoBehaviour
{
    public BoxCollider _collider;
    public AudioSource _audio;
    public string _name;
    public ArcadeMachine _machine;
    public bool isDance = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_name == "Arcade")
            {
                Debug.Log("Arcade");
                if (_machine.isPlaying)
                {
                    _audio.Play();
                    Debug.Log("??");
                }

            }
            else
            {
                if (_name == "Printer")
                    _audio.Play();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _audio.Stop();
        }
       
    }


}
