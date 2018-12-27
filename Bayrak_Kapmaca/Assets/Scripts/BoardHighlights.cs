using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Seçilen taş için mümkün hareket yerlerinin beyaz renkle gösterilmesini sağlayan sınıftır.
public class BoardHighlights : MonoBehaviour 
{
	public static BoardHighlights Instance{ set; get;}

	public GameObject highlightPrefab;
	private List<GameObject> highlights;

	//Start() fonksiyonu oyun başladığı anda yapılması gerekenler için kullanılır. 
	private void Start()
	{
		Instance = this;
		highlights = new List<GameObject> ();

	}
	// Highlightprefab objesini setler.
	private GameObject GetHighLightObject()
	{
		GameObject go = highlights.Find (g => !g.activeSelf);

		if (go == null) 
		{
			go = Instantiate (highlightPrefab);
			highlights.Add (go);
		}

		return go;
	}
	// Mümkün hareketleri göstermek için oluşturulan beyaz karelerin pozisyonlarını setler.
	public void HighlightAllowedMoves(bool[,] moves)
	{
		for (int i = 0; i < 7; i++) 
		{
			for (int j = 0; j < 9; j++) 
			{
				if (moves [i, j]) 
				{
					GameObject go = GetHighLightObject ();
					go.SetActive (true);
					go.transform.position = new Vector3 (i + 0.58f, 0.12f, j+0.65f);
				}
			}
		}
	}
	// Aktif olan beyaz kareleri saklar.
	public void Hidehighlights()
	{
		foreach (GameObject go in highlights)
			go.SetActive (false);
	}

}
