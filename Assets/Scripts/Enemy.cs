using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("Parameter")] public float m_move_speed = 5;
    public float m_rotation_speed = 200;
    public float m_life_time = 5;
    public int m_score = 100;
    public int health = 1;
    public List<GameObject> hitEffects = new List<GameObject>();
    public List<AudioClip> hitSfx = new List<AudioClip>();
    [Header("VFX")] public GameObject enemyDeath;

    [Header("Components")] public Animator ZAnimator;

    public Slider healthBar;
    //------------------------------------------------------------------------------

    private void Start()
    {
        healthBar.minValue = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
        StartCoroutine(MainCoroutine());
    }

    private void DeleteObject()
    {
        health--;
        healthBar.value = health;
        Instantiate(hitEffects[Random.Range(0, hitEffects.Count)], transform.position, Quaternion.identity);
        Instantiate(enemyDeath, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            transform.GetComponentInChildren<Collider>().enabled = false;
            m_move_speed = 0;

            ZAnimator.SetTrigger("Z_Death");
            GameObject.Destroy(gameObject, 1f);
        }
    }

    //
    private IEnumerator MainCoroutine()
    {
        while (true)
        {
            //move
            transform.position += new Vector3(0, -1, 0) * m_move_speed * Time.deltaTime;

            //animation
            transform.rotation *= Quaternion.AngleAxis(m_rotation_speed * Time.deltaTime, new Vector3(1, 1, 0));


            //lifetime
            m_life_time -= Time.deltaTime;
            if (m_life_time <= 0)
            {
                DeleteObject();
                yield break;
            }

            yield return null;
        }
    }

    //------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        PlayerBullet player_bullet = other.transform.GetComponent<PlayerBullet>();
        if (player_bullet)
        {
            AudioManager.Instance.PlaySoundEffect(hitSfx[Random.Range(0, hitSfx.Count)]);
            DestroyByPlayer(player_bullet);
        }
    }


    void DestroyByPlayer(PlayerBullet a_player_bullet)
    {
        //add score
        if (StageLoop.Instance)
        {
            StageLoop.Instance.AddScore(m_score);
        }

        //delete bullet
        if (a_player_bullet)
        {
            a_player_bullet.DeleteObject();
        }

        //delete self
        DeleteObject();
    }
}