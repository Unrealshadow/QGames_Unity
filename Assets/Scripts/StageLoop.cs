using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stage main loop
/// </summary>

public enum GameState
{
	Playing,
	GameOver
}
public class StageLoop : MonoBehaviour
{
	#region static 
	static public StageLoop Instance { get; private set; }
	#endregion

	[Header("Components")]
	public TitleLoop m_title_loop;
	public GameOverLoop gameOverLoop;
	[Header("Layout")]
	public Transform m_stage_transform;
	public Text m_stage_score_text;

	[Header("Prefab")]
	public Player m_prefab_player;
	public EnemySpawner m_prefab_enemy_spawner;

	[Header("Params")]
	public GameState currentState;

	
	//
	int m_game_score = 0;


	//------------------------------------------------------------------------------
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
			return;
		}
	}

	#region loop
	public void StartStageLoop()
	{    currentState = GameState.Playing;

		StartCoroutine(StageCoroutine());
	}

	/// <summary>
	/// stage loop
	/// </summary>
	private IEnumerator StageCoroutine()
	{
		Debug.Log("Start StageCoroutine");

		SetupStage();

		while (currentState == GameState.Playing)
		{
			// Game loop code here
			yield return null;
		}

		if (currentState == GameState.GameOver)
		{
			gameOverLoop.StartGameOverLoop(m_game_score);
		}
	}

	#endregion


	void SetupStage()
	{
		Instance = this;
		
		m_game_score = 0;
		RefreshScore();
		AudioManager.Instance.PlayBGM();
		//create player
		{
			Player player = Instantiate(m_prefab_player, m_stage_transform);
			if (player)
			{
				player.transform.position = new Vector3(0, -4, 0);
				player.StartRunning();
			}
		}

		//create enemy spawner
		{
			{
				EnemySpawner spawner = Instantiate(m_prefab_enemy_spawner, m_stage_transform);
				if (spawner)
				{
					spawner.transform.position = new Vector3(-4, 4, 0);
					spawner.StartRunning();
				}
			}
			{
				EnemySpawner spawner = Instantiate(m_prefab_enemy_spawner, m_stage_transform);
				if (spawner)
				{
					spawner.transform.position = new Vector3(4, 4, 0);
					spawner.StartRunning();
				}
			}
		}
	}

	public void CleanupStage()
	{
		Time.timeScale = 1f;

		//delete all object in Stage
		{
			for (var n = 0; n < m_stage_transform.childCount; ++n)
			{
				Transform temp = m_stage_transform.GetChild(n);
				GameObject.Destroy(temp.gameObject);
			}
		}

		Instance = null;
	}

	//------------------------------------------------------------------------------

	public void AddScore(int a_value)
	{
		m_game_score += a_value;
		RefreshScore();
	}public void RemoveScore(int a_value)
	{
		if (m_game_score > 0)
		{
			m_game_score -= a_value;
			
		}
		else
		{
			m_game_score = 0;
		}
		RefreshScore();
	}

	void RefreshScore()
	{
		if (m_stage_score_text)
		{
			m_stage_score_text.text = $"Score {m_game_score:00000}";
		}
	}

	public int GetCurrentScore()
	{
		return m_game_score;
	}

}
