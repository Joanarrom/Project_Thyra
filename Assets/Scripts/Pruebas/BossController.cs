using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject impactMarkerPrefab;
    public float attackInterval = 5f;
    public float markerDuration = 1.5f;
    public float markerPreFireTime = 0.3f; // Tiempo entre marcador y fireball
    public Transform target;
    public GameObject boss1;
    public GameObject boss2;
    public Animator animatorBoss1;
    public Animator animatorBoss2;
    public LayerMask groundLayer;
    public float markerYOffset = 0.2f;

    private float attackCooldown;
    private float timer;
    private bool isAnimating = false;

    void Start()
    {
        InitializeComponents();
        attackCooldown = attackInterval;

        // Suscribirse al evento que notifica la destrucción de PowerSources
        PowerSourceManager.Instance.OnAllPowerSourcesDestroyed += Die;
    }

    void InitializeComponents()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        boss1.SetActive(true);
        boss2.SetActive(false);
        animatorBoss1.Play("Idle");
        animatorBoss2.Play("Idle");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown && !isAnimating)
        {
            StartCoroutine(AttackSequence());
            timer = 0f;
        }
    }

    IEnumerator AttackSequence()
    {
        isAnimating = true;

        ShowBossPhase1();
        GameObject marker = CreateMarker();
        StartCoroutine(BlinkMarker(marker));

        // Esperar solo el tiempo pre-fire
        yield return new WaitForSeconds(markerPreFireTime);

        // Lanzar ataque mientras el marcador sigue activo
        yield return PerformAttack();

        // Esperar tiempo restante del marcador
        float remainingMarkerTime = markerDuration - markerPreFireTime;
        if (remainingMarkerTime > 0)
            yield return new WaitForSeconds(remainingMarkerTime);

        if (marker != null) Destroy(marker);

        ShowBossPhase1();
        isAnimating = false;
    }

    void ShowBossPhase1()
    {
        boss1.SetActive(true);
        boss2.SetActive(false);
        animatorBoss1.Play("Idle", 0, 0f);
    }

    GameObject CreateMarker()
    {
        Vector3 markerPos = GetGroundPosition();
        return Instantiate(
            impactMarkerPrefab,
            markerPos,
            impactMarkerPrefab.transform.rotation
        );
    }

    Vector3 GetGroundPosition()
    {
        RaycastHit hit;
        Vector3 rayOrigin = target.position + Vector3.up * 2f;
        float rayDistance = 10f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance, groundLayer))
        {
            return hit.point + Vector3.up * markerYOffset;
        }
        return new Vector3(target.position.x, markerYOffset, target.position.z);
    }

    IEnumerator PerformAttack()
    {
        boss1.SetActive(false);
        boss2.SetActive(true);
        animatorBoss2.Play("Attack", 0, 0f);

        // Reducir el tiempo de espera antes de lanzar
        yield return new WaitForSeconds(0.5f);

        Vector3 spawnPos = target.position + Vector3.up * 15f;
        Instantiate(fireballPrefab, spawnPos, Quaternion.identity);

        attackCooldown = attackInterval;
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator BlinkMarker(GameObject marker)
    {
        if (marker == null) yield break;

        Renderer markerRenderer = marker.GetComponent<Renderer>();
        if (markerRenderer == null) yield break;

        float timer = 0f;
        bool isVisible = true;

        while (timer < markerDuration && marker != null)
        {
            markerRenderer.enabled = isVisible;
            isVisible = !isVisible;
            yield return new WaitForSeconds(0.15f); // Parpadeo más rápido
            timer += 0.15f;
        }

        if (marker != null) markerRenderer.enabled = true;
    }

    void Die()
    {
     Debug.Log("Boss derrotado!");
     Destroy(gameObject);

     // Esperar un momento antes de cargar la nueva escena
      SceneManager.LoadScene("ScoreFinal");
    }

    
}