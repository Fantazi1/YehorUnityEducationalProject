using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WarriorProducer : MonoBehaviour
{
    [Tooltip("якого юн≥та виробл€Ї буд≥вл€")]
    [SerializeField] private UnitsType unitsType = UnitsType.Warrior;

    [Tooltip("—к≥льки додавати за один т≥к")]
    [SerializeField] private int amountPerTick = 1;

    [Tooltip("≤нтервал м≥ж т≥ками в секундах")]
    [SerializeField] private float secondsPerTick = 2f;

    [SerializeField] private GameObject unitPrefub;
    [SerializeField] private int maxUnitCount = 5;

    private int randTransformXMax = 1;
    private int randTransformYMax = 1;

    private float _timer;
    private Vector2 randTransform;

    private List<GameObject> unitsList = new List<GameObject>{};
    private GameObject createdUnit;

    private void Update()
    {
        if (ResourceManager.Instance == null) return;

        randTransform.x = Random.Range(transform.position.x, randTransformXMax);
        randTransform.y = Random.Range(transform.position.y, randTransformYMax);

        _timer += Time.deltaTime;
        while (_timer >= secondsPerTick)
        {
            _timer -= secondsPerTick;

            if (unitsList.Count >= maxUnitCount) {
                return;
            }

            createdUnit = Instantiate(unitPrefub, randTransform, Quaternion.identity);
            unitsList.Add(createdUnit);
        }
    }
}