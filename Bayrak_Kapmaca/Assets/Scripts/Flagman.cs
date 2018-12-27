using System.Collections;
using UnityEngine;

// Taşların mümkün hareketleri ve pozisyonları için gereken temel fonksiyonların bulunduğu,
// taşların miras aldığı soyut temel sınıftır.
public abstract class Flagman : MonoBehaviour 
{
	public int CurrentX{set;get;}
	public int CurrentY{set;get;}
	// Taşların ayrım yapılması için gereken bool değişkeni.
	public bool isWhite;

	//Anlık pozisyonları yeni değişkenlere atar.
	public void SetPosition(int x, int y)
	{
		CurrentX = x;
		CurrentY = y;
	}

	// 7 satır, 9 sütunluk mümkün hareket notları için dizi.
	public virtual bool[,] PossibleMove()
	{
		return new bool[7,9];
	}
}
