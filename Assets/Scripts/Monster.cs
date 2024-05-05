using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor;
using State;

public class Monster : ActorBase
{
    public float frontWallDetectRayLength = 0.1f;
    public float playerDetectTime = 3f;
    public Detector detector;
    private Strategy.MonsterAI _strategy;
    private CapsuleCollider2D _capsuleCollider;
    private Vector2 _capsuleSize;
    private GameObject _player;
    private float _playerDetectTimer;
    protected bool _frontWall;
    protected bool _moveToRight = true;

    // Start is called before the first frame update
    public override void Start()
    {
        // Need refactor
        base.Start();
        _strategy = GetComponent<Strategy.MonsterAI>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _capsuleSize = _capsuleCollider.size;
        _player = null;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (_playerDetectTimer > 0.0f) {
            _playerDetectTimer -= Time.deltaTime;
        } else {
            _player = null;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _commandList.Clear();
    }

    protected override void PreparationBeforeFixedUpdate()
    {
        CollectState();
        if (_strategy != null) { _strategy.Decide(this); }
    }

    protected override void CollectState()
    {
        base.CollectState();
        _frontWall = DetectFrontWall() != null;
        // detect player
        if (detector != null && detector.IsDetected()) {
            _player = detector.GetDetectedObject();
            _playerDetectTimer = playerDetectTime;
        }
    }

    protected RaycastHit2D? DetectFrontWall()
    {
        LayerMask ground_mask = LayerMask.GetMask("Ground");
        Vector2 start_position = _rigidbody.position;
        Vector2 direction = Vector2.right * (GetMoveToRight() ? 1 : -1);
        start_position.y += _capsuleSize.y / 2;
        float ray_length = _capsuleSize.x / 2 + frontWallDetectRayLength;
        RaycastHit2D hit = Physics2D.Raycast(start_position, direction, ray_length, ground_mask);
        Debug.DrawRay(start_position, direction * ray_length, Color.red);
        return hit ? hit : null;
    }

    protected override BaseState InitialState() { return new MonsterOnLandState(); }

    public bool IsFrontWall() { return _frontWall; }
    public bool IsPlayerFound() { return _player != null; }
    public bool GetMoveToRight() { return _moveToRight; }
    public GameObject GetPlayer() { return _player; }
    public void TurnAround()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
        _moveToRight = !_moveToRight;
    }
}
