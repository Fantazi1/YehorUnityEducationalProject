using UnityEngine;

public class SphereCheckPlayerUnderfoot : MonoBehaviour
{
    [SerializeField] private float checkRadius = 0.3f; // Радіус кола перевірки
    [SerializeField] private Vector2 offset = new Vector2(0f, -0.5f); // Зміщення кола вниз під ноги

    void Update()
    {
        // Розраховуємо точку перевірки (позиція об'єкта + зміщення)
        Vector2 checkPosition = (Vector2)transform.position + offset;

        // Перевіряємо, які 2D-колайдери потрапили в наше коло
        Collider2D[] collidersInCircle = Physics2D.OverlapCircleAll(checkPosition, checkRadius);

        // Якщо знайшли хоча б один колайдер
        if (collidersInCircle.Length > 0)
        {
            // Перебираємо масив вручну, щоб переконатися, що це не наш власний колайдер
            for (int i = 0; i < collidersInCircle.Length; i++)
            {
                // Якщо знайдений колайдер належить ІНШОМУ об'єкту
                if (collidersInCircle[i].gameObject != this.gameObject)
                {
                    Debug.Log($"Під об'єктом є 2D колайдер: {collidersInCircle[i].name} на об'єкті {collidersInCircle[i].gameObject.name}");

                    // Зупиняємо цикл, бо ми вже знайшли те, що шукали
                    break;
                }
            }
        }
    }

    // Візуалізація кола в редакторі Unity (щоб бачити його розмір і налаштувати offset)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 checkPosition = transform.position + new Vector3(offset.x, offset.y, 0);
        Gizmos.DrawWireSphere(checkPosition, checkRadius);
    }
}