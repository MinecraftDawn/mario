using System.Collections;
using System.Collections.Generic;
using Envrionment.Projectile;
using Unity.VisualScripting;
using UnityEngine;
using utils;

namespace Strategy
{

public class AutoProjectile : MonsterAI
{
    [SerializeField]
    private Vector2 _firePosition;
    [SerializeField]
    private DelayTimer _fireInterval;
    private ProjectileManager _projectileManager;
    public GameObject projectile;

    void Start()
    {
        _projectileManager = new ProjectileManager(projectile);
    }

    public override void Decide(Monster monster)
    {
        GameObject player_obj = monster.GetDetectedPlayer();
        if (!player_obj || !_fireInterval.HasDelayPassed()) { return; }
        // fire projectile
        GameObject new_projectile = _projectileManager.GetInstance();
        new_projectile.transform.position = transform.position + (Vector3)_firePosition;
        Vector2 fire_direction = player_obj.transform.position - transform.position;
        Rigidbody2D projectile_rigid = new_projectile.GetComponent<Rigidbody2D>();
        projectile_rigid.velocity = fire_direction.normalized * 5f;
        _fireInterval.UpdateLastTime();
    }
}

}
