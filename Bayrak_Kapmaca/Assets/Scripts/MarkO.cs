using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// O taşı için mümkün hareketlerin bulunduğu, Flagman sınıfından miras alan bir sınıftır.
public class MarkO : Flagman 
{
	public override bool[,] PossibleMove()
	{
		
		bool[,] r = new bool[7, 9];
		//Flagman sınıfından oluşturulan nesne.
		Flagman c;
		// 1. Oyuncunun Mark O taşı için mümkün hamle pozisyonları.
		if (isWhite) 
		{
			//Sol çapraz hareket.
			if (CurrentX != 0 && CurrentY != 8) 
			{
				c = BoardManager.Instance.Flagmans [CurrentX - 1, CurrentY + 1];
				if (c == null || !c.isWhite)
					r [CurrentX - 1, CurrentY + 1] = true;
			}	
			//Sağ çapraz hareket.
			if (CurrentX != 6 && CurrentY != 8) 
			{
				c = BoardManager.Instance.Flagmans [CurrentX + 1, CurrentY + 1];
				if (c == null || !c.isWhite)
					r [CurrentX + 1, CurrentY + 1] = true;
			}	

			//Orta kısım için hareket.
			if (CurrentY != 8) 
			{
				c = BoardManager.Instance.Flagmans [CurrentX, CurrentY + 1];
				if (c == null || !c.isWhite)
					r [CurrentX, CurrentY + 1] = true;
			}
		}
		// 2. Oyuncunun Mark O taşı için mümkün hamle pozisyonları.
		else 
		{
			//Sol çapraz hareket.
			if (CurrentX != 0 && CurrentY != 0) 
			{
				c = BoardManager.Instance.Flagmans [CurrentX - 1, CurrentY - 1];
				if (c == null || c.isWhite)
					r [CurrentX - 1, CurrentY - 1] = true;
			}	
			//Sağ çapraz hareket.
			if (CurrentX != 6 && CurrentY != 0) 
			{
				c = BoardManager.Instance.Flagmans [CurrentX + 1, CurrentY - 1];
				if (c == null || c.isWhite)
					r [CurrentX + 1, CurrentY - 1] = true;
			}	

			//Orta kısım için hareket.
			if (CurrentY != 0) 
			{
				c = BoardManager.Instance.Flagmans [CurrentX, CurrentY - 1];
				if (c == null || c.isWhite)
					r [CurrentX, CurrentY - 1] = true;
			}
		}

		return r; // Geçerli koordinatı dönderir.

	}

}
