using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpiderWebProjectile : MonoBehaviour
{
    public Vector2 moveDirection;
    public float speedDefault = 10;
    public float speedButterfly = 15;
    private Vector2 _velocity;
    private GameState _gameState;

    public int damage;
    public float knockBackStrength;
    public GameObject spiderWebPrefab;

    public Rigidbody2D rb;
    private GameState _gameObject;
    private TilemapCollider2D _tilemapCollider2D;
    private Tilemap _tilemap;

    private MusicManager _musicManager;
    public AudioClip createClip;
    public AudioClip deathClip;

    private void Awake()
    {
        _musicManager = FindObjectOfType<MusicManager>();
        _gameState = FindObjectOfType<GameState>();
        _gameObject = FindObjectOfType<GameState>();
        _tilemapCollider2D = FindObjectOfType<TilemapCollider2D>();

        _tilemap = _tilemapCollider2D.gameObject.GetComponent<Tilemap>();
    }

    private void Start()
    {
        _musicManager.CreateAudioClip(createClip);
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        float speed = speedDefault;
        if (_gameState.evolveState == GameState.EvolveState.Butterfly)
        {
            speed = speedButterfly;
        }

        Vector3 targetPos = new Vector3(moveDirection.x, moveDirection.y) * speed * Time.fixedDeltaTime;
        targetPos.z = pos.z;

        Vector3 worldTargetPos = transform.TransformPoint(targetPos);
        rb.MovePosition(worldTargetPos);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject gbo = col.gameObject;
        PlayerHealth health = ExtractPlayerHealthComponent(gbo);
        if (health != null)
        {
            health.TakeDamage(damage);
            health.KnockBackPlayer(transform.gameObject, knockBackStrength);
        }

        PlaceSpiderWeb();
        Destroy(gameObject);
    }

    [ContextMenu("PlaceSpiderWeb")]
    public void PlaceSpiderWeb()
    {
        _musicManager.CreateAudioClip(deathClip);
        Vector3Int worldPos = _tilemap.WorldToCell(transform.position);
        Vector3 tileCenter = _tilemap.GetCellCenterLocal(worldPos);

        GameObject web = Instantiate(spiderWebPrefab, tileCenter, Quaternion.Euler(Vector3.up));
        Vector3 pos = web.transform.position;
        pos.z = pos.z + 1;
        web.transform.position = pos;
    }


    private void OnDrawGizmos()
    {
        if (_tilemap != null)
        {
#if UNITY_EDITOR
            var worldPos = _tilemap.WorldToCell(transform.position);
            var tileCenter = _tilemap.GetCellCenterLocal(worldPos);
            Handles.Label(tileCenter, "X");
#endif
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
            if (segmentHealthHealth.damageAble)
            {
                return playerHealth;
            }
        }

        return null;
    }

    
}