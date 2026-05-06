using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour 
{
    [SerializeField] private string sceneText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(sceneText);
        }
    }
}
