using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Title Screen Loop
/// </summary>
public class TitleLoop : MonoBehaviour
{
	public StageLoop m_stage_loop;

	[Header("Layout")]
	public Transform m_ui_title;

	public Transform howToPlayUI;
	public Button closehowToPlayBtn;
	//------------------------------------------------------------------------------

	private void Start()
	{
		//default start
		StartTitleLoop();
	}

	private void CloseHowToPlay()
	{
		howToPlayUI.gameObject.SetActive(false);
	}

	private void ShowHowToPlay()
	{
		howToPlayUI.gameObject.SetActive(true);
		
	}

	//
	#region loop
	public void StartTitleLoop()
	{
		StartCoroutine(TitleCoroutine());
	}

	/// <summary>
	/// Title loop
	/// </summary>
	private IEnumerator TitleCoroutine()
	{
		Debug.Log($"Start TitleCoroutine");

		SetupTitle();

		//waiting game start
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.H))
			{
				ShowHowToPlay();
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				CloseHowToPlay();
			}
			
			
			if (Input.GetKeyDown(KeyCode.Space) && !howToPlayUI.gameObject.activeSelf)
			{
				CleanupTitle();

				//Start StageLoop
				m_stage_loop.StartStageLoop();
				yield break;
			}
			yield return null;
		}
	}
	#endregion

	//
	void SetupTitle()
	{
		m_ui_title.gameObject.SetActive(true);
	}

	void CleanupTitle()
	{
		m_ui_title.gameObject.SetActive(false);
	}
}
