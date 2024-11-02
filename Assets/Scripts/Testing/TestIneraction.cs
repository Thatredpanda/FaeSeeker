using System.Collections;
using UnityEngine;

public class TestIneraction : Interactable
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float amplitude = 0.2f;
    [SerializeField] private float collectSpeed = 1f;
    private Collider coll;
    private float defaultYPos;
    private float timer;

    private Outline outline;

    private void Start()
    {
        defaultYPos = transform.position.y;
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    private void Update()
    {
        Levitation();
    }

    public override void OnFocus()
    {
        Debug.Log("Focus on " + gameObject.name);
        outline.enabled = true;
    }

    public override void OnInteract()
    {
        if (coll == null)
        {
            coll = GetComponent<Collider>();
            coll.enabled = false;
            StartCoroutine(Collect());
        }
    }

    public override void OnLoseFocus()
    {
        Debug.Log("Lose focus from " + gameObject.name);
        outline.enabled = false;
    }

    private void Levitation()
    {
        timer += Time.deltaTime;

        transform.Rotate(0, 10f * speed * Time.deltaTime, 0);

        if (coll != null) return;

        transform.position = new Vector3(transform.position.x, defaultYPos + Mathf.PingPong(timer * amplitude, amplitude), transform.position.z);
    }

    private IEnumerator Collect()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        
        while (Vector3.Distance(player.position + Vector3.up, transform.position) > 0.1f)
        {
            collectSpeed += collectSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position + Vector3.up, collectSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }
}