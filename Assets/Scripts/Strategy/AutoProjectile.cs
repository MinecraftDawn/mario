using System.Collections;
using System.Collections.Generic;
using Command;
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

    public override void Decide(Monster.Monster monster)
    {
        GameObject player_obj = monster.GetDetectedPlayer();
        if (!player_obj || !_fireInterval.HasDelayPassed()) { return; }
        // fire projectile
        FireProjectileCommand fire_command = monster.GenerateCommand<FireProjectileCommand>();
        fire_command.SetProjectile(_projectileManager.GetInstance());
        fire_command.SetFirePosition(transform.position + (Vector3)_firePosition);
        fire_command.SetDirection(player_obj.transform.position - transform.position);
        fire_command.SetSpeed(5f);
        monster.ReceiveCommands(fire_command);
        _fireInterval.UpdateLastTime();
    }
}

}
