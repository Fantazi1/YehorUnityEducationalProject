using UnityEngine;

public class GameSave : MonoBehaviour
{
    public static GameSave Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Це змусить Весь об'єкт і ВСІХ його дітей переїхати на нову сцену
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Якщо ми повернулися на стару сцену — видаляємо копію
            Destroy(gameObject);
        }
    }
}