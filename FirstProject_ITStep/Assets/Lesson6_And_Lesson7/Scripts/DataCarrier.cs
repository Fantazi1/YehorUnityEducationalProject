using UnityEngine;

public class DataCarrier : MonoBehaviour
{
    public static DataCarrier Instance;

    // —юди ми будемо записувати "зл≥пки" наших скрипт≥в у вигл€д≥ тексту
    public string healthJson;
    public string inventoryJson;
    public string statsJson;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
}