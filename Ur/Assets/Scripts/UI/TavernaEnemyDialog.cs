using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TavernaEnemyDialog : TavernaMiniGameDialog
{
	public Text enemyName;
	public Image enemyImage;
	private const string DefaultPortrait = "crew_portraits/phoenician_sailor";

	private CrewMember crew;

    void Start()
    {
        gm = GameObject.FindWithTag("Master").GetComponent<GameManager>();
		textBackground.SetActive(false);

        crew = gm.MasterCrewList.RandomElement();
        enemyName.text = crew.CrewName;
        enemyImage.sprite = crew.CrewPortrait;

        if (enemyImage.sprite == null)
        {
            enemyImage.sprite = Resources.Load<Sprite>(DefaultPortrait);
        }
	}

	//private void Update() {
	//	if (Input.GetKeyDown(KeyCode.Space)) {
	//		if (Random.Range(1, 3) % 2 == 0) {
	//			DisplayInsult();
	//		}
	//		else {
	//			DisplayBragging();
	//		}
	//	}
	//}
}
