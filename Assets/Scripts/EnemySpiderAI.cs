using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpiderAI : MonoBehaviour
{
    private GameState _gameState;

    [Header("AI")] public float visionRange = 6.9f;
    public float projectileShotDelay = 2f;
    public float projectileShootAnimTime = 1f;
    private GameObject _aimLock;
    private float _projectileShotDelayTimer = 0;

    [Header("Visuals")] public Sprite spriteDefault;
    public Sprite spriteAim;
    public SpriteRenderer mySpriteRenderer;

    [Header("Data")] public GameObject stringPrefab;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSeePlayer();
        if (CanSeePlayer())
        {
            _projectileShotDelayTimer = _projectileShotDelayTimer + Time.deltaTime;
            if (_projectileShotDelayTimer >= projectileShotDelay)
            {
                _projectileShotDelayTimer = 0;
                Debug.DrawLine(transform.position, _aimLock.transform.position, Color.red);
                ShootString();
            }
        }
        else
        {
            _projectileShotDelayTimer = 0;
        }

        mySpriteRenderer.sprite = spriteDefault;
        if (_projectileShotDelayTimer >= projectileShootAnimTime)
        {
            mySpriteRenderer.sprite = spriteAim;
        }
    }

    public void ShootString()
    {
        _projectileShotDelayTimer = 0;
        print("shoot");

        GameObject projectile = Instantiate(stringPrefab, transform.position, Quaternion.Euler(Vector3.up));
        Vector2 direction = _aimLock.transform.position - transform.position;
        SpiderWebProjectile webProjectile = projectile.GetComponent<SpiderWebProjectile>();

        Vector3 pos = transform.position;
        pos.z = pos.z + 0.15f;
        webProjectile.transform.position = pos;

        direction = direction.normalized;
        webProjectile.moveDirection = direction;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Vector3 wireOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        Handles.DrawWireDisc(wireOrigin, Vector3.forward, visionRange);
        Handles.Label(wireOrigin, "Has Target: " + CanSeePlayer());
#endif
    }

    public bool CanSeePlayer()
    {
        return _aimLock != null;
    }

    public void UpdateSeePlayer()
    {
        _aimLock = null;
        GameObject ob = _gameState.Camera.Follow.gameObject;
        if (ob == null)
        {
            return;
        }

        Vector2 direction = ob.transform.position - transform.position;
        ArrayList allHits = new ArrayList();

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, visionRange);
        allHits.AddRange(hits);

        int mask = LayerMask.NameToLayer("CaterpillarSegment");
        hits = Physics2D.RaycastAll(transform.position, direction, visionRange, mask);
        allHits.AddRange(hits);

        mask = LayerMask.NameToLayer("Worm");
        hits = Physics2D.RaycastAll(transform.position, direction, visionRange, mask);
        allHits.AddRange(hits);

        mask = LayerMask.NameToLayer("WormEnd");
        hits = Physics2D.RaycastAll(transform.position, direction, visionRange, mask);
        allHits.AddRange(hits);

        Debug.DrawRay(transform.position, direction);
        float hitDist = float.MaxValue;
        float tilemapDist = float.MaxValue;
        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObj = hit.transform.gameObject;

            PlayerHealth health = ExtractPlayerHealthComponent(hitObj);
            var dist = hit.distance;

            if (health != null)
            {
                _aimLock = hitObj;
                hitDist = dist;
            }

            if (hitObj.GetComponent<BendyLine>() != null)
            {
                _aimLock = hitObj;
                hitDist = dist;
            }

            if (hitObj.GetComponent<Better_Worm_Movement>() != null)
            {
                _aimLock = hitObj;
                hitDist = dist;
            }

            if (hitObj.GetComponent<WormSegmentHealth>() != null)
            {
                _aimLock = hitObj;
                hitDist = dist;
            }

            if (hitObj.GetComponent<WormEndController>() != null)
            {
                _aimLock = hitObj;
                hitDist = dist;
            }

            if (hitObj.GetComponent<TilemapCollider2D>() != null)
            {
                tilemapDist = dist;
            }
        }

        if (tilemapDist < hitDist)
        {
            // Hit interrupted
            _aimLock = null;
        }
    }

    private PlayerHealth ExtractPlayerHealthComponent(GameObject obj)
    {
        PlayerHealth playerHealth = obj.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            return playerHealth;
        }

        var segmentHealthHealth = obj.GetComponent<WormSegmentHealth>();
        if (segmentHealthHealth != null)
        {
            playerHealth = segmentHealthHealth.playerHealth;
            return playerHealth;
        }

        return null;
    }
}