using UnityEditor;
using UnityEngine;

public class printerAudioController : MonoBehaviour
{
    public BoxCollider _collider;
    public AudioSource _audio;
    public string _name;
    public ArcadeMachine _machine;


    private void OnTriggerEnter(Collider other)
    {
        if (_name == "Arcade" && (other.CompareTag("Player") && _machine.isPlaying)){
            Debug.Log("****");
            _audio.Play(); }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (_name == "Arcade" && (other.CompareTag("Player") && _machine.isPlaying))
        {
            _audio.Stop();
        }
       
    }


}
