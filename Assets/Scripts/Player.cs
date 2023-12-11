using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Character
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Prefab")] public PlayerBullet m_prefab_player_bullet;

    [Header("Parameter")] public float m_move_speed = 1;
    public Transform shootPoint;
    public AudioClip shootClip;
    [Header("Components")] public Animator animator;
    public AnimatedTexture animatedTexture;
    
    //------------------------------------------------------------------------------

    public void StartRunning()
    {
        StartCoroutine(MainCoroutine());
    }

    //
    private IEnumerator MainCoroutine()
    {
        while (true)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            if (movement.magnitude > 1)
            {
                movement.Normalize();
            }

            animator.SetFloat("Move X", movement.x);
            animator.SetFloat("Move Y", movement.y);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Shoot Triggered");
                AudioManager.Instance.PlaySoundEffect(shootClip);
                animatedTexture.StartAnimation();
                animator.SetTrigger("Shoot");
                animator.SetTrigger("Default");

                PlayerBullet bullet = Instantiate(m_prefab_player_bullet, transform.parent);
                bullet.transform.position = shootPoint.position;
            }

            transform.position += movement * m_move_speed * Time.deltaTime;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            animator.SetTrigger("Death");
            StageLoop.Instance.gameOverLoop.StartGameOverLoop(StageLoop.Instance.GetCurrentScore());
        }
    }
}