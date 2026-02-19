using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    public GameObject heartPrefab;
    public Health Health;
    List<HealthHeart> hearts = new List<HealthHeart>();

    private void OnEnable()
    {
        Health.OnDamageTaken.AddListener(DrawHearts);
    }


    private void Start()
    {
        int bull = 0;
        DrawHearts(bull);
    }


    public void DrawHearts(int use)
    {
        ClearHearts();
        for (int i = 0; i < Health.MaxHealth; i++)
        {
           EmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++)
        {
            int heartStatus = Mathf.Clamp(Health.CurrentHealth - i, 0, 1);
            hearts[i].SetHeartStatus((HeartStatus)heartStatus);
        }
    }
    public void EmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartStatus(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }
    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }


}
