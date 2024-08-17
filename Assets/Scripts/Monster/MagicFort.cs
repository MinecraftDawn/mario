using System;
using UnityEngine;
using State;
using Envrionment.Projectile;

namespace Monster
{

public class MagicFort : Monster
{
    public GameObject projectile;
    [SerializeField]
    private float _inactiveLatency = 1f;
    [SerializeField]
    private Color _inactiveColor;
    [SerializeField]
    private Color _activeColor;
    [SerializeField]
    private GameObject _weakness;
    private SpriteRenderer _renderer;
    private ProjectileManager _projectileManager;
    
    public override void Start()
    {
        base.Start();
        _renderer = GetComponent<SpriteRenderer>();
        _projectileManager = new ProjectileManager(projectile);
    }
    protected override void InitialState() { _stateManager.Init<FortActiveState>(); }
    public override void Damaged()
    {
        FortInActiveState state = StateTransition<FortInActiveState>();
        state.OnStateStart(this);
    }
    public float GetInactiveLatency() { return _inactiveLatency; }
    public void SetToInactiveColor() { _renderer.color = _inactiveColor; }
    public void SetToActiveColor() { _renderer.color = _activeColor; }
    public void InActiveWeakness() { _weakness.SetActive(false); }
    public void ActiveWeakness() { _weakness.SetActive(true); }
    public override GameObject GetNewProjectile() { return _projectileManager.GetInstance(); }
}

}