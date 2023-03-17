using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpiderWebProjectile : MonoBehaviour
{
    public Vector2 moveDirection;
    public float speed = 5;
    private Vector2 _velocity;

    public int damage;
    public float knockBackStrength;
    public GameObject spiderWebPrefab;

    public Rigidbody2D rb;
    private GameState _gameObject;
    private TilemapCollider2D _tilemapCollider2D;
    [SerializeField] private Tilemap _tilemap;

    private void Awake()
    {
        _gameObject = FindObjectOfType<GameState>();
        _tilemapCollider2D = FindObjectOfType<TilemapCollider2D>();

        _tilemap = _tilemapCollider2D.gameObject.GetComponent<Tilemap>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
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
        Vector3Int worldPos = _tilemap.WorldToCell(transform.position);
        Vector3 tileCenter = _tilemap.GetCellCenterLocal(worldPos);

        GameObject web = Instantiate(spiderWebPrefab, tileCenter, Quaternion.Euler(Vector3.up));
        Vector3 pos = web.transform.position;
        pos.z = pos.z + 1;
        web.transform.position = pos;
    }


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var worldPos = _tilemap.WorldToCell(transform.position);
        var tileCenter = _tilemap.GetCellCenterLocal(worldPos);
        Handles.Label(tileCenter, "X");
#endif
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