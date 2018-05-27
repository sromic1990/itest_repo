using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;

public class GetCorrectSpriteByID : Singleton<GetCorrectSpriteByID>
{
	public List<SpriteIDHolder> SpriteHolders;

	public Sprite GetSpriteFromID (SpriteID ID)
	{
		Sprite sprite = GetSprite (ID);

		if (sprite == null) {
			sprite = GetSprite (SpriteID.DummySprite);
		}

		return sprite;
	}

	private Sprite GetSprite (SpriteID ID)
	{
		Sprite sprite = null;

		for (int i = 0; i < SpriteHolders.Count; i++) {
			if (ID == SpriteHolders [i].SpriteID) {
				sprite = SpriteHolders [i].Sprite;
				break;
			}
		}

		return sprite;
	}

	private void OnValidate ()
	{
		for (int i = 0; i < SpriteHolders.Count; i++) {
			SpriteHolders [i].ID = SpriteHolders [i].SpriteID.ToString ();
		}
	}
}

[Serializable]
public class SpriteIDHolder
{
	public string ID;
	public SpriteID SpriteID;
	public Sprite Sprite;
}
