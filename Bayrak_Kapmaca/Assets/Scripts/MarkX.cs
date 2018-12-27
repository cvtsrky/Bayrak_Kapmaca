using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// X taşının soyut Flagman sınıfından miras alarak mümkün hareketlerinin sağlandığı sınıftır.
public class MarkX : Flagman 
{
	public override bool[,] PossibleMove ()
	{	// 7 satır, 9 sütunluk hareket için gereken değişken.
		bool[,] r = new bool[7, 9];
		// Flagman sınıfından türetilen bir nesne.
		Flagman c;
		// For döngüleri için kullanılan değişkenler.
		int i,j,k;

		// Sağ tarafa hareket.
		i = CurrentX; // İ'ye şimdiki X koordinatı setlenir.
		for (j=0; j<2;j++)
		{
			i++;
			if(i>=7)// Oyun tahtasının sınırını aşarsa sağ tarafa hareket edemez.
				break;
			c = BoardManager.Instance.Flagmans [i,CurrentY]; // Seçili İ ve Y koordinatlarında obje varsa veya yoksa bunu c nesnesine setler.
			if(c == null) // Eğer o kısımda taş yok ise oraya hareketin mümkün olduğunu doğrular. Ayrıca beyaz X için hareketi sağlar.
				r [i,CurrentY] = true;
			else
			{
				if(c.isWhite != isWhite) // Siyah X için hareketi sağlar.
					r [i,CurrentY] = true;
				break;
			}
		}

		// Sol tarafa hareket.
		i = CurrentX;
		for (j=0; j<2;j++)
		{
			i--;
			if(i<0)
				break;
			c = BoardManager.Instance.Flagmans [i,CurrentY];
			if(c == null)
				r [i,CurrentY] = true;
			else
			{
				if(c.isWhite != isWhite)
					r [i,CurrentY] = true;
				break;
			}
		}

		// Yukarı tarafa hareket.
		i = CurrentY;
		for (j=0; j<2;j++)
		{
			i++;
			if(i>=9)
				break;
			c = BoardManager.Instance.Flagmans [CurrentX,i];
			if(c == null)
				r [CurrentX,i] = true;
			else
			{
				if(c.isWhite != isWhite)
					r [CurrentX,i] = true;
				break;
			}
		}

		// Aşağı tarafa hareket.
		i = CurrentY;
		for (j=0; j<2;j++)
		{
			i--;
			if(i<0)
				break;
			c = BoardManager.Instance.Flagmans [CurrentX,i];
			if(c == null)
				r [CurrentX,i] = true;
			else
			{
				if(c.isWhite != isWhite)
					r [CurrentX,i] = true;
				break;
			}
		}
		// Sol üst çapraza hareket.
		i = CurrentX;
		j = CurrentY;
		for (k=0; k<2;k++)
		{
			i--;
			j++;
			if(i<0 || j>=9)
				break;
			c = BoardManager.Instance.Flagmans [i,j];
			if(c == null)
				r [i,j] = true;
			else
			{
				if(c.isWhite != isWhite)
					r [i,j] = true;
				break;
			}
		}

		// Sağ üst çapraza hareket.
		i = CurrentX;
		j = CurrentY;
		for (k=0; k<2;k++)
		{
			i++;
			j++;
			if(i>=7 || j>=9)
				break;
			c = BoardManager.Instance.Flagmans [i,j];
			if(c == null)
				r [i,j] = true;
			else
			{
				if(c.isWhite != isWhite)
					r [i,j] = true;
				break;
			}
		}
		// Sol aşağı çapraza hareket.
		i = CurrentX;
		j = CurrentY;
		for (k=0; k<2;k++)
		{
			i--;
			j--;
			if(i<0 || j<0)
				break;
			c = BoardManager.Instance.Flagmans [i,j];
			if(c == null)
				r [i,j] = true;
			else
			{
				if(c.isWhite != isWhite)
					r [i,j] = true;
				break;
			}
		}

		// Sağ aşağı çapraza hareket.
		i = CurrentX;
		j = CurrentY;
		for (k=0; k<2;k++)
		{
			i++;
			j--;
			if(i>=7 || j<0)
				break;
			c = BoardManager.Instance.Flagmans [i,j];
			if(c == null)
				r [i,j] = true;
			else
			{
				if(c.isWhite != isWhite)
					r [i,j] = true;
				break;
			}
		}
		return r; //Geçerli koordinatı dönderir.
	}

}
