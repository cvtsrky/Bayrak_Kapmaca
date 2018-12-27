using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Oyun yönetimini sağlayan temel sınıftır.
public class BoardManager : MonoBehaviour 
{	// Oyun tahtasının durumu için kulanılan nesne.
	public static BoardManager Instance{set;get;}
	//İzinli hareketler için kullanılan değişken.
	private bool[,] allowedMoves{ set; get;}
	//Seçilen taşın konumu ve tipini tutmak için gereken, Flagman sınıfından oluşturulmuş nesneler.
	public Flagman[,] Flagmans{set;get;}
	private Flagman selectedFlagman;

	// X, Y ve Z koordinatlarının kesin olarak belirlenmesi için gereken sabit değerler.
	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.6f;
	private const float TILE_HEIGH = 0.22f;
	// Seçilecek koordinatları tutacak değişkenler.
	private int selectionX = -1;
	private int selectionY = -1;
	//Kazanılan beyaz ve siyah bayrakların sayısını tutan değişkenler.
	private int whitecount = 0;
	private int blackcount = 0;
	// Taşların prefabının (Taşların model, boyut, renk vb. tüm özelliklerinin tamamlanmış hali) liste olarak alınması.
	public List<GameObject> flagmanPrefabs;
	// Oyunda aktif olan objelerin listeye alınması.
	private List<GameObject> activeFlagman;
	// Oyun sonu için gereken kazanım paneli ve yazısı için gerekli objeler.
	public GameObject winPanel;
	public Text winText;
	// Oyunun ilk hamlesinin beyaz tarafından yapılmasını sağlayan bool değişkeni.
	public bool isWhiteTurn = true;
	// Taşların dönüşümü için gerekli transform kodu.
	private Quaternion orientation = Quaternion.Euler(0,180,0);

	//Start() fonksiyonu oyun başladığı anda yapılması gerekenler için kullanılır. 
	private void Start()
	{
		Instance = this; // Oyun tahtasının tüm notlarını setler.
		winPanel.SetActive (false); // Kazanım panelini oyun başı saklar.
		SpawnAllFlagman (); // Tüm taşları ortaya çıkarır (Spawn eder).
	}

	private void Update ()
	{
		UpdateSelection (); // Seçimi günceller.
		DrawGameboard (); //Oyun tahtasını çizer.
		//Mouse ile seçilen koordinat sınırlar içerisindeyse işlemleri gerçekleştirir.
		//Eğer o noktada taş yok veya taş seçili değilse taş seçme işlemi yapılır.
		//Eğer seçili taş varsa onun hareketi gerçekleştirilir.
		if (Input.GetMouseButtonDown(0)) 
		{
			if (selectionX >= 0 && selectionY >= 0) 
			{
				if (selectedFlagman == null) 
				{
					//Taş seçer.
					SelectFlagman(selectionX,selectionY);
				} 
				else 
				{
					//Taşın hareketi sağlanır.
					MoveFlagman(selectionX, selectionY);
				}
			}
		}

		//Sıra kimdeyse ona göre bakış açısı değişen kameranın kodları.
		/*if (isWhiteTurn) 
		{
			Camera.main.transform.position = new Vector3 (3.5f, 7.75f, -0.95f);
			Camera.main.transform.rotation = Quaternion.Euler (60, 0, 0); 
		} 
		else 
		{
			Camera.main.transform.position = new Vector3 (3.5f, 7.75f, 10.0f);
			Camera.main.transform.rotation = Quaternion.Euler (60, 180, 0);
		}*/
			
	}
	//Seçilen taşın mümkün hareket yerlerini beyaz seçili alanlar ile gösterir.
	private void SelectFlagman(int x, int y)
	{	//Eğer seçili alanda taş yoksa dön.
		if (Flagmans [x, y] == null)
			return;
		// Eğer seçili alandaki taş beyaz ve beyazın sırası değilse dön.
		if (Flagmans [x, y].isWhite != isWhiteTurn)
			return;
		// Seçilen taşın mümkün hareketlerini setle.
		allowedMoves = Flagmans [x, y].PossibleMove ();
		//Seçilen taşı setle.
		selectedFlagman = Flagmans [x, y];
		//Mümkün hareketlerin olduğu alanları beyaz renkle göster.
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);

	}
	// Taş hareketlerinin, bayrak kazanılması ve taşların yenilmesi olaylarının yönetildiği sınıf.
	private void MoveFlagman(int x, int y)
	{
		
			
		if (allowedMoves[x,y]) 
		{	//Seçili taşı Flagman sınıfından türetilen nesneye setler.
			Flagman c = Flagmans [x, y];
			// Burada rakibin taşının yendiği durumda aktif taş listesinden kaldırılması ve objenin yok edilmesini sağlanır.
			if (c != null && c.isWhite != isWhiteTurn) 
			{
				// Yenilen taş bayrak ise;
				if (c.GetType () == typeof(Flag)) 
				{	//Bayrağı kazanan beyaz ise;
					if (c == isWhiteTurn) 
					{	
						whitecount++; // kazanılan bayrak sayısını artır.
						Score.wscore = whitecount; // Beyazın skorunu setle.
						// Eğer kazanılan bayrak sayısı 2 ise oyun sonuna git.
						if (whitecount == 2) 
						{
							EndGame ();
							return;
						}
					}
					//Bayrağı kazanan siyah ise;
					else 
					{
						blackcount++; // kazanılan bayrak sayısını artır.
						Score.bscore = blackcount; //Siyahın skorunu setle.
						// Eğer kazanılan bayrak sayısı 2 ise oyun sonuna git.
						if (blackcount == 2) 
						{
							EndGame ();
							return;
						}
					}
				}
				activeFlagman.Remove(c.gameObject);
				Destroy (c.gameObject);
			}
			//Eğer seçilen taş O ise;
			if (selectedFlagman.GetType () == typeof(MarkO)) 
			{	// Beyaz için, eğer rakibin en son satırına geldiyse bu taşı X yap. 
				if (y == 8) 
				{
					
					activeFlagman.Remove (selectedFlagman.gameObject); // Seçilen taşı aktif taşlar listesinden kaldır.
					Destroy (selectedFlagman.gameObject); // Seçili taşı oyundan yok et.
					SpawnFlagman (1, x, y); // Bulunduğu yerde bir X taşı oluştur.
					selectedFlagman = Flagmans [x, y]; // X taşının koordinatlarını buna setle.
				}
				// Siyah için, eğer rakibin en son satırına geldiyse bu taşı X yap.
				else if(y == 0)
				{
					
					activeFlagman.Remove (selectedFlagman.gameObject);
					Destroy (selectedFlagman.gameObject);
					SpawnFlagman (4, x, y);
					selectedFlagman = Flagmans [x, y];
				}
			}
			// Hareket edeceği noktanın koordinatlarının setlenmesi sağlanmıştır.
			Flagmans [selectedFlagman.CurrentX, selectedFlagman.CurrentY] = null;
			selectedFlagman.transform.position = GetTileCenter (x, y);
			selectedFlagman.SetPosition (x, y);
			Flagmans [x, y] = selectedFlagman;
			isWhiteTurn = !isWhiteTurn;
		}
		//Hareket sonrası beyaz kareleri saklar.
		BoardHighlights.Instance.Hidehighlights ();
		selectedFlagman = null; // Taş seçimini boşa setler.

	}
	//Burada kameranın bakış açısı ve oyun tahtası (FlagPlane) zeminini sınır baz alınarak mouse tıklaması sayesinde,
	// bulunulan yerin 'X' ve 'Z' koordinatlarının alınmasını sağlar.
	private void UpdateSelection()
	{
		if (!Camera.main)
			return;
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("FlagPlane"))) 
		{
			selectionX = (int)hit.point.x;
			selectionY = (int)hit.point.z;
		} 
	}
	// Taşların doğacak (Spawn edilecek) yerlerinin kesin olarak belirlenmesi için gereken fonksiyon.
	// İndex olarak taşın türünü, x olarak 'X' koordinatını ve y olarakta 'Z' koordinatını almaktadır.
	private void SpawnFlagman(int index,int x,int y)
	{	
		GameObject go = Instantiate (flagmanPrefabs [index], GetTileCenter(x,y), orientation) as GameObject;
		go.transform.SetParent (transform);
		Flagmans [x, y] = go.GetComponent<Flagman> ();
		Flagmans [x, y].SetPosition (x, y);
		activeFlagman.Add (go);
	}
	// Tüm taşların doğduğu (Spawn edildiği) koordinatların belirtildiği fonksiyon.
	private void SpawnAllFlagman()
	{
		// Oyun tahtasına yerleştirelecek objelerin (Taşları) listesini alır.
		activeFlagman = new List<GameObject> ();
		//
		Flagmans = new Flagman[7, 9];
		// Beyaz (1. Oyuncunun) taşların koordinatları.
		//Bayrakların koordinatlarının kesin olarak belirtilmesi için SpawnFlagman() fonksiyonuna gönderilmesi.
		// Burada 1. değer index yani taşın türünü belirler. 2. değer 'X' koordinatını, 3. değer ise 'Z koordinatını belirler.'
		SpawnFlagman (0, 0,0);
		SpawnFlagman (0, 3,0);
		SpawnFlagman (0, 6,0);

		//X taşının koordinatlarının kesin olarak belirtilmesi için SpawnFlagman() fonksiyonuna gönderilmesi.
		for(int i=0; i<7; i++)
			SpawnFlagman (1, i,1);

		//O taşının koordinatlarının kesin olarak belirtilmesi için SpawnFlagman() fonksiyonuna gönderilmesi.
		for(int j=0; j<7; j++)
			SpawnFlagman (2, j,2);


		//Siyah (2. Oyuncunun) taşların koordinatları.
		//Bayrakların koordinatları.
		SpawnFlagman (3, 0,8);
		SpawnFlagman (3, 3,8);
		SpawnFlagman (3, 6,8);

		//X taşının koordinatları.
		for(int i=0; i<7; i++)
			SpawnFlagman (4, i,7);

		//O taşının Koordinatları.
		for(int j=0; j<7; j++)
			SpawnFlagman (5, j,6);
	}

	// Taşların, oyun tahtası üzerinde doğru pozisyonda bulunması için gereken fonksiyon.
	private Vector3 GetTileCenter (int x, int y)
	{
		Vector3 origin = Vector3.zero; //X,y ve z si 0 olan bir vektör.
		// X koordinatı için; 
		//TILE_SIZE ('X' koordinatı için satırlarda gezinmesini sağlar) * spawn edileceği 'X' koordinatı + TILE_OFFSET (Taşın X koordinatındaki tam yeri için gerekli).
		origin.x += (TILE_SIZE * x) + TILE_OFFSET;
		// Taşların yükseklğini yani 'Y' koordinatını setler.
		origin.y += TILE_HEIGH;
		// Z koordinatı için; 
		//TILE_SIZE ('Z' koordinatı için sütunlarda gezinmesini sağlar) * spawn edileceği 'Z' koordinatı + TILE_OFFSET (Taşın Z koordinatındaki tam yeri için gerekli).
		origin.z += (TILE_SIZE * y) + TILE_OFFSET;
		//Taşın tam pozisyonunu geri döndürür.
		return origin;
	}

	//7 satır, 9 sütunluk oyun tahtasının vektörel olarak çizilmesi.
	//Bu koordinatlar sayesinde oyunun hareket noktaları belirlenmektedir.
	private void DrawGameboard()
	{
		Vector3 withLine = Vector3.right * 7;
		Vector3 heightLine = Vector3.forward * 9;

		for (int i = 0; i <= 9; i++) 
		{
			Vector3 start = Vector3.forward * i;
			Debug.DrawLine (start, start + withLine);
			for(int j=0; j<=7; j++)
			{
				start = Vector3.right * j;
				Debug.DrawLine (start, start + heightLine);
			}
		}
		//Ayrımları çizer.
		if (selectionX >= 0 && selectionY >= 0)
		{	
			Debug.DrawLine (
				Vector3.forward * selectionY + Vector3.right * selectionX,
				Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

			Debug.DrawLine (
				Vector3.forward * (selectionY + 1 )+ Vector3.right * selectionX,
				Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
		}

	}
	//Aynı oyuncularla oyunu tekrar oynamak için gereken fonksiyon.
	public void RestartGame ()
	{	//Kazanım panelini saklar.
		winPanel.SetActive (false);
		//Kapılan bayrak sayılarını sıfırlar.
		blackcount = 0;
		whitecount = 0;
		//Skorları sıfırlar.
		Score.bscore = 0;
		Score.wscore = 0;
		//Taşları tekrar yükler.
		SpawnAllFlagman ();
	}
	// Oyun sahnesinden, menü kısmına dönmek için gereken fonksiyon.
	public void BackMenu()
	{	// MainMenu ismindeki sahneyi yükler.
		SceneManager.LoadScene ("MainMenu");
	}


	//Oyun bitişi yapılması gereken şeyler için bir fonksiyon.
	private void EndGame()
	{
		//Beyaz (1. Oyuncu) kazanırsa.
		if (isWhiteTurn) 
		{   //Kazandı panelini aktif hale getirir ve 1. Oyuncunun ismini ekrana basar. 
			winPanel.SetActive (true);
			winText.text = Score.playernamestr1 +"\n Kazandı!";
		}
		//Siyah (2. Oyuncu) kazanırsa.
		else 
		{   //Kazandı panelini aktif hale getirir ve 2. Oyuncunun ismini ekrana basar.
			winPanel.SetActive (true);
			winText.text= Score.playernamestr2 +"\n Kazandı!";
		}
		//Oyun sonu sahnede aktif olarak bulunan tüm taşları (objeleri) foreach döngüsü ile yok eder.
		foreach (GameObject go in activeFlagman)
			Destroy (go);
		//Oyun sonu sırayı beyaza setler.
		isWhiteTurn = true;
		//Oyun sonu mümkün hareket yerini gösteren beyaz ışıklandırmayı görünmez yapar.
		BoardHighlights.Instance.Hidehighlights ();
	}
}
