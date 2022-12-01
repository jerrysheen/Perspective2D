using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;

    public float detectRange = 5.0f;
    public float monsterSpeed = 2.0f;

    public Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInRange())
        {
            direction = player.transform.position - this.transform.position;
            Vector2 diff = Vector2.ClampMagnitude(direction, 1.0f) * monsterSpeed * Time.deltaTime;
            this.transform.position += new Vector3(diff.x, diff.y, 0.0f);
        }

    }

    private bool PlayerInRange()
    {
        if (Vector3.Magnitude(this.transform.position - player.transform.position) >= detectRange)
        {
            return false;
        }

        return true;
    }
}
