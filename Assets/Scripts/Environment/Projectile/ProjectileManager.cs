using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Envrionment
{
namespace Projectile
{

[Serializable]
public class ProjectileManager
{
    [SerializeField]
    private int _numProjectile = 10;
    private List<GameObject> _pool;
    private int nextIdx = 0;

    public ProjectileManager(GameObject projectile_prefab)
    {
        _pool = new List<GameObject>();
        for (int i = 0; i < _numProjectile; i++) {
            GameObject projectile = GameObject.Instantiate(projectile_prefab);
            projectile.SetActive(false);
            _pool.Add(projectile);
        }
    }

    public GameObject GetInstance()
    {
        GameObject projectile = null;
        if (!_pool[nextIdx].activeSelf) {
            projectile = _pool[nextIdx];
            nextIdx = (nextIdx + 1) % _pool.Count;
            projectile.SetActive(true);
            return projectile;
        }
        for (int i = 0; i < _pool.Count; i++) {
            Debug.Log(_pool[i].activeSelf);
            if (_pool[i].activeSelf) { continue; }
            projectile = _pool[i];
            nextIdx = (i + 1) % _pool.Count;
            projectile.SetActive(true);
            return projectile;
        }
        Debug.LogError("[Error] no enough projectile");
        return projectile;
    }
}

}
}