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
    public AudioClip death;
    public GameObject deathVFX;
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
        while (true && StageLoop.Instance.currentState == GameState.Playing)
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

            Vector3 newPosition = transform.position + movement * m_move_speed * Time.deltaTime;
            newPosition = ClampPositionToScreen(newPosition);

            transform.position = newPosition;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {        StageLoop.Instance.currentState = GameState.GameOver;

            animator.SetTrigger("Death");
            Instantiate(deathVFX,transform.position,Quaternion.identity);
            AudioManager.Instance.PlaySoundEffect(death);
            Time.timeScale = 0.5f;
            StageLoop.Instance.gameOverLoop.StartGameOverLoop(StageLoop.Instance.GetCurrentScore());
        }
    }
    
    private Vector3 ClampPositionToScreen(Vector3 position)
    {
        float colliderRadius = GetComponent<SphereCollider>().radius;

        float minX = CameraDimensions.Instance.BottomLeft.x + colliderRadius;
        float maxX = CameraDimensions.Instance.BottomRight.x - colliderRadius;
        float minY = CameraDimensions.Instance.BottomLeft.y + colliderRadius;
        float maxY = CameraDimensions.Instance.TopLeft.y - colliderRadius;

        // Clamp the position to be within the camera's view
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }

}