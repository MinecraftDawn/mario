using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI {

public class HealthBarController : MonoBehaviour
{
    public float interval = 90.0f;
    public float firstInterval = 50.0f;
    public float hight = 50.0f;
    public GameObject _healthItemPrefab;
    private List<GameObject> _healthItems;

    // Start is called before the first frame update
    void Start()
    {
        _healthItems = new List<GameObject>();
    }

    public void Init(int max_health)
    {
        float next_interval = firstInterval;
        for (int i = 0; i < max_health; i++) {
            GameObject health_item = Instantiate(_healthItemPrefab, gameObject.transform);
            RectTransform rect_transform = health_item.GetComponent<RectTransform>();
            rect_transform.anchoredPosition = new Vector2(next_interval, hight);
            _healthItems.Add(health_item);
            next_interval += interval;
        }
    }

    public void DecreaseHealth(int new_health)
    {
        if (new_health < 0) { return; }
        GameObject target_health_item = _healthItems[new_health];
        GameObject bubble = target_health_item.transform.Find("Bubble").gameObject;
        GameObject empty = target_health_item.transform.Find("Empty").gameObject;
        bubble.SetActive(false);
        empty.SetActive(true);
    }

    public void IncreaseHealth(int new_health)
    {
        if (new_health >= _healthItems.Count) { return; }
        GameObject target_health_item = _healthItems[new_health - 1];
        GameObject bubble = target_health_item.transform.Find("Bubble").gameObject;
        GameObject empty = target_health_item.transform.Find("Empty").gameObject;
        bubble.SetActive(true);
        empty.SetActive(false);
    }
}

}
