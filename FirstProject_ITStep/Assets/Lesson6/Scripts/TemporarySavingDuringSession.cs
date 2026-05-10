using UnityEngine;

public class TemporarySavingDuringSession : MonoBehaviour
{
    private static TemporarySavingDuringSession instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
