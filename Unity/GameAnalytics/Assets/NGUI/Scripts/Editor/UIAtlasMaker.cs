//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Atlas maker lets you create atlases from a bunch of small textures. It's an alternative to using the external Texture Packer.
/// </summary>

public class UIAtlasMaker : EditorWindow
{
	class SpriteEntry
	{
		public Texture2D tex;	// Sprite texture -- original texture or a temporary texture
		public Rect rect;		// Sprite's outer rectangle within the generated texture atlas
		public int minX = 0;	// Padding, if any (set if the sprite is trimmed)
		public int maxX = 0;
		public int minY = 0;
		public int maxY = 0;
		public bool temporaryTexture = false;	// Whether the texture is temporary and should be deleted
	}

	Vector2 mScroll = Vector2.zero;
	string mDelName = null;

	/// <summary>
	/// Atlas selection callback.
	/// </summary>

	void OnSelectAtlas (MonoBehaviour obj)
	{
		UISettings.atlas = obj as UIAtlas;
		Repaint();
	}

	/// <summary>
	/// Refresh the window on selection.
	/// </summary>

	void OnSelectionChange () { mDelName = null; Repaint(); }

	/// <summary>
	/// Helper function that retrieves the list of currently selected textures.
	/// </summary>

	List<Texture> GetSelectedTextures ()
	{
		List<Texture> textures = new List<Texture>();

		if (Selection.objects != null && Selection.objects.Length > 0)
		{
			Object[] objects = EditorUtility.CollectDependencies(Selection.objects);

			foreach (Object o in objects)
			{
				Texture tex = o as Texture;
				if (tex != null && (UISettings.atlas == null || UISettings.atlas.texture != tex)) textures.Add(tex);
			}
		}
		return textures;
	}

	/// <summary>
	/// Load the specified list of textures as Texture2Ds, fixing their import properties as necessary.
	/// </summary>

	static List<Texture2D> LoadTextures (List<Texture> textures)
	{
		List<Texture2D> list = new List<Texture2D>();

		foreach (Texture tex in textures)
		{
			Texture2D t2 = NGUIEditorTools.ImportTexture(tex, true, false);
			if (t2 != null) list.Add(t2);
		}
		return list;
	}

	/// <summary>
	/// Pack all of the specified sprites into a single texture, updating the outer and inner rects of the sprites as needed.
	/// </summary>

	static void PackTextures (Texture2D tex, List<SpriteEntry> sprites)
	{
		Texture2D[] textures = new Texture2D[sprites.Count];
		for (int i = 0; i < sprites.Count; ++i) textures[i] = sprites[i].tex;

		Rect[] rects = tex.PackTextures(textures, UISettings.atlasPadding, 4096);

		for (int i = 0; i < sprites.Count; ++i)
		{
			sprites[i].rect = NGUIMath.ConvertToPixels(rects[i], tex.width, tex.height, true);
		}
	}

	/// <summary>
	/// Helper function that creates a single sprite list from both the atlas's sprites as well as selected textures.
	/// Dictionary value meaning:
	/// 0 = No change
	/// 1 = Update
	/// 2 = Add
	/// </summary>

	Dictionary<string, int> GetSpriteList (List<Texture> textures)
	{
		Dictionary<string, int> spriteList = new Dictionary<string, int>();

		if (UISettings.atlas != null && UISettings.atlas.name == UISettings.atlasName)
		{
			List<string> spriteNames = UISettings.atlas.GetListOfSprites();
			foreach (string sp in spriteNames) spriteList.Add(sp, 0);
		}

		// If we have textures to work with, include them as well
		if (textures.Count > 0)
		{
			List<string> texNames = new List<string>();
			foreach (Texture tex in textures) texNames.Add(tex.name);
			texNames.Sort();

			foreach (string tex in texNames)
			{
				if (spriteList.ContainsKey(tex)) spriteList[tex] = 1;
				else spriteList.Add(tex, 2);
			}
		}
		return spriteList;
	}

	/// <summary>
	/// Add a new sprite to the atlas, given the texture it's coming from and the packed rect within the atlas.
	/// </summary>

	static UIAtlas.Sprite AddSprite (List<UIAtlas.Sprite> sprites, SpriteEntry se)
	{
		UIAtlas.Sprite sprite = null;

		// See if this sprite already exists
		foreach (UIAtlas.Sprite sp in sprites)
		{
			if (sp.name == se.tex.name)
			{
				sprite = sp;
				break;
			}
		}

		if (sprite != null)
		{
			float x0 = sprite.inner.xMin - sprite.outer.xMin;
			float y0 = sprite.inner.yMin - sprite.outer.yMin;
			float x1 = sprite.outer.xMax - sprite.inner.xMax;
			float y1 = sprite.outer.yMax - sprite.inner.yMax;

			sprite.outer = se.rect;
			sprite.inner = se.rect;

			sprite.inner.xMin = Mathf.Max(sprite.inner.xMin + x0, sprite.outer.xMin);
			sprite.inner.yMin = Mathf.Max(sprite.inner.yMin + y0, sprite.outer.yMin);
			sprite.inner.xMax = Mathf.Min(sprite.inner.xMax - x1, sprite.outer.xMax);
			sprite.inner.yMax = Mathf.Min(sprite.inner.yMax - y1, sprite.outer.yMax);
		}
		else
		{
			sprite = new UIAtlas.Sprite();
			sprite.name = se.tex.name;
			sprite.outer = se.rect;
			sprite.inner = se.rect;
			sprites.Add(sprite);
		}

		float width  = Mathf.Max(1f, sprite.outer.width);
		float height = Mathf.Max(1f, sprite.outer.height);

		// Sprite's padding values are relative to width and height
		sprite.paddingLeft	 = se.minX / width;
		sprite.paddingRight  = se.maxX / width;
		sprite.paddingTop	 = se.maxY / height;
		sprite.paddingBottom = se.minY / height;
		return sprite;
	}

	/// <summary>
	/// Create a list of sprites using the specified list of textures.
	/// </summary>

	static List<SpriteEntry> CreateSprites (List<Texture> textures)
	{
		List<SpriteEntry> list = new List<SpriteEntry>();

		foreach (Texture tex in textures)
		{
			Texture2D oldTex = NGUIEditorTools.ImportTexture(tex, true, false);
			if (oldTex == null) continue;

			// If we aren't doing trimming, just use the texture as-is
			if (!UISettings.atlasTrimming)
			{
				SpriteEntry sprite = new SpriteEntry();
				sprite.rect = new Rect(0f, 0f, oldTex.width, oldTex.height);
				sprite.tex = oldTex;
				sprite.temporaryTexture = false;
				list.Add(sprite);
				continue;
			}

			// If we want to trim transparent pixels, there is more work to be done
			Color32[] pixels = oldTex.GetPixels32();

			int xmin = oldTex.width;
			int xmax = 0;
			int ymin = oldTex.height;
			int ymax = 0;
			int oldWidth = oldTex.width;
			int oldHeight = oldTex.height;

			// Find solid pixels
			for (int y = 0, yw = oldHeight; y < yw; ++y)
			{
				for (int x = 0, xw = oldWidth; x < xw; ++x)
				{
					Color32 c = pixels[y * xw + x];
					
					if (c.a != 0)
					{
						if (y < ymin) ymin = y;
						if (y > ymax) ymax = y;
						if (x < xmin) xmin = x;
						if (x > xmax) xmax = x;
					}
				}
			}

			int newWidth  = (xmax - xmin) + 1;
			int newHeight = (ymax - ymin) + 1;

			// If the sprite is empty, don't do anything with it
			if (newWidth > 0 && newHeight > 0)
			{
				SpriteEntry sprite = new SpriteEntry();
				sprite.rect = new Rect(0f, 0f, oldTex.width, oldTex.height);

				// If the dimensions match, then nothing was actually trimmed
				if (newWidth == oldWidth && newHeight == oldHeight)
				{
					sprite.tex = oldTex;
					sprite.temporaryTexture = false;
				}
				else
				{
					// Copy the non-trimmed texture data into a temporary buffer
					Color32[] newPixels = new Color32[newWidth * newHeight];

					for (int y = 0; y < newHeight; ++y)
					{
						for (int x = 0; x < newWidth; ++x)
						{
							int newIndex = y * newWidth + x;
							int oldIndex = (ymin + y) * oldWidth + (xmin + x);
							newPixels[newIndex] = pixels[oldIndex];
						}
					}

					// Create a new texture
					sprite.temporaryTexture = true;
					sprite.tex = new Texture2D(newWidth, newHeight);
					sprite.tex.name = oldTex.name;
					sprite.tex.SetPixels32(newPixels);
					sprite.tex.Apply();

					// Remember the padding offset
					sprite.minX = xmin;
					sprite.maxX = oldWidth - newWidth - xmin;
					sprite.minY = ymin;
					sprite.maxY = oldHeight - newHeight - ymin;
				}
				list.Add(sprite);
			}
		}
		return list;
	}

	/// <summary>
	/// Release all temporary textures created for the sprites.
	/// </summary>

	static void ReleaseSprites (List<SpriteEntry> sprites)
	{
		foreach (SpriteEntry se in sprites)
		{
			if (se.temporaryTexture)
			{
				NGUITools.Destroy(se.tex);
				se.tex = null;
			}
		}
		Resources.UnloadUnusedAssets();
	}

	/// <summary>
	/// Replace the sprites within the atlas.
	/// </summary>

	static void ReplaceSprites (UIAtlas atlas, List<SpriteEntry> sprites)
	{
		// Get the list of sprites we'll be updating
		List<UIAtlas.Sprite> spriteList = atlas.spriteList;
		List<UIAtlas.Sprite> kept = new List<UIAtlas.Sprite>();

		// The atlas must be in pixels
		atlas.coordinates = UIAtlas.Coordinates.Pixels;

		// Run through all the textures we added and add them as sprites to the atlas
		for (int i = 0; i < sprites.Count; ++i)
		{
			SpriteEntry se = sprites[i];
			UIAtlas.Sprite sprite = AddSprite(spriteList, se);
			kept.Add(sprite);
		}

		// Remove unused sprites
		for (int i = spriteList.Count; i > 0; )
		{
			UIAtlas.Sprite sp = spriteList[--i];
			if (!kept.Contains(sp)) spriteList.RemoveAt(i);
		}
		atlas.MarkAsDirty();
	}

	/// <summary>
	/// Extract sprites from the atlas, adding them to the list.
	/// </summary>

	static void ExtractSprites (UIAtlas atlas, List<SpriteEntry> sprites)
	{
		// Make the atlas texture readable
		Texture2D atlasTex = NGUIEditorTools.ImportTexture(atlas.texture, true, false);

		if (atlasTex != null)
		{
			atlas.coordinates = UIAtlas.Coordinates.Pixels;

			Color32[] oldPixels = null;
			int oldWidth = atlasTex.width;
			int oldHeight = atlasTex.height;
			List<UIAtlas.Sprite> list = atlas.spriteList;

			foreach (UIAtlas.Sprite asp in list)
			{
				bool found = false;

				foreach (SpriteEntry se in sprites)
				{
					if (asp.name == se.tex.name)
					{
						found = true;
						break;
					}
				}

				if (!found)
				{
					// Read the atlas
					if (oldPixels == null) oldPixels = atlasTex.GetPixels32();

					Rect rect = asp.outer;
					rect.xMin = Mathf.Clamp(rect.xMin, 0f, oldWidth);
					rect.yMin = Mathf.Clamp(rect.yMin, 0f, oldHeight);
					rect.xMax = Mathf.Clamp(rect.xMax, 0f, oldWidth);
					rect.yMax = Mathf.Clamp(rect.yMax, 0f, oldHeight);

					int newWidth = Mathf.RoundToInt(rect.width);
					int newHeight = Mathf.RoundToInt(rect.height);
					if (newWidth == 0 || newHeight == 0) continue;

					Color32[] newPixels = new Color32[newWidth * newHeight];
					int xmin = Mathf.RoundToInt(rect.x);
					int ymin = Mathf.RoundToInt(oldHeight - rect.yMax);

					for (int y = 0; y < newHeight; ++y)
					{
						for (int x = 0; x < newWidth; ++x)
						{
							int newIndex = y * newWidth + x;
							int oldIndex = (ymin + y) * oldWidth + (xmin + x);
							newPixels[newIndex] = oldPixels[oldIndex];
						}
					}

					// Create a new sprite
					SpriteEntry sprite = new SpriteEntry();
					sprite.temporaryTexture = true;
					sprite.tex = new Texture2D(newWidth, newHeight);
					sprite.tex.name = asp.name;
					sprite.rect = new Rect(0f, 0f, newWidth, newHeight);
					sprite.tex.SetPixels32(newPixels);
					sprite.tex.Apply();

					// Min/max coordinates are in pixels
					sprite.minX = Mathf.RoundToInt(asp.paddingLeft * newWidth);
					sprite.maxX = Mathf.RoundToInt(asp.paddingRight * newWidth);
					sprite.minY = Mathf.RoundToInt(asp.paddingBottom * newHeight);
					sprite.maxY = Mathf.RoundToInt(asp.paddingTop * newHeight);

					sprites.Add(sprite);
				}
			}
		}

		// The atlas no longer needs to be readable
		NGUIEditorTools.ImportTexture(atlas.texture, false, false);
	}

	/// <summary>
	/// Combine all sprites into a single texture and save it to disk.
	/// </summary>

	static Texture2D UpdateTexture (UIAtlas atlas, List<SpriteEntry> sprites)
	{
		// Get the texture for the atlas
		Texture2D tex = atlas.texture as Texture2D;
		string oldPath = (tex != null) ? AssetDatabase.GetAssetPath(tex.GetInstanceID()) : "";
		string newPath = NGUIEditorTools.GetSaveableTexturePath(atlas);

		if (tex == null || oldPath != newPath)
		{
			// Create a new texture for the atlas
			tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);

			// Pack the sprites into this texture
			PackTextures(tex, sprites);
			byte[] bytes = tex.EncodeToPNG();
			System.IO.File.WriteAllBytes(newPath, bytes);
			bytes = null;

			// Load the texture we just saved as a Texture2D
			AssetDatabase.Refresh();
			tex = NGUIEditorTools.ImportTexture(newPath, false, true);

			// Update the atlas texture
			if (tex == null) Debug.LogError("Failed to load the created atlas saved as " + newPath);
			else atlas.spriteMaterial.mainTexture = tex;
		}
		else
		{
			// Make the atlas readable so we can save it
			tex = NGUIEditorTools.ImportTexture(oldPath, true, false);

			// Pack all sprites into atlas texture
			PackTextures(tex, sprites);
			byte[] bytes = tex.EncodeToPNG();
			System.IO.File.WriteAllBytes(newPath, bytes);
			bytes = null;

			// Re-import the newly created texture, turning off the 'readable' flag
			AssetDatabase.Refresh();
			tex = NGUIEditorTools.ImportTexture(newPath, false, false);
		}
		return tex;
	}

	/// <summary>
	/// Update the sprite atlas, keeping only the sprites that are on the specified list.
	/// </summary>

	static void UpdateAtlas (UIAtlas atlas, List<SpriteEntry> sprites)
	{
		if (sprites.Count > 0)
		{
			// Combine all sprites into a single texture and save it
			UpdateTexture(atlas, sprites);

			// Replace the sprites within the atlas
			ReplaceSprites(atlas, sprites);

			// Release the temporary textures
			ReleaseSprites(sprites);
		}
		else
		{
			atlas.spriteList.Clear();
			string path = NGUIEditorTools.GetSaveableTexturePath(atlas);
			atlas.spriteMaterial.mainTexture = null;
			if (!string.IsNullOrEmpty(path)) AssetDatabase.DeleteAsset(path);
		}
		EditorUtility.SetDirty(atlas.gameObject);
	}

	/// <summary>
	/// Add the specified texture to the atlas, or update an existing one.
	/// </summary>

	static public void AddOrUpdate (UIAtlas atlas, Texture2D tex)
	{
		if (atlas != null && tex != null)
		{
			List<Texture> textures = new List<Texture>();
			textures.Add(tex);
			List<SpriteEntry> sprites = CreateSprites(textures);
			ExtractSprites(atlas, sprites);
			UpdateAtlas(atlas, sprites);
		}
	}

	/// <summary>
	/// Update the sprites within the texture atlas, preserving the sprites that have not been selected.
	/// </summary>

	void UpdateAtlas (List<Texture> textures, bool keepSprites)
	{
		// Create a list of sprites using the collected textures
		List<SpriteEntry> sprites = CreateSprites(textures);

		if (sprites.Count > 0)
		{
			// Extract sprites from the atlas, filling in the missing pieces
			if (keepSprites) ExtractSprites(UISettings.atlas, sprites);

			// NOTE: It doesn't seem to be possible to undo writing to disk, and there also seems to be no way of
			// detecting an Undo event. Without either of these it's not possible to restore the texture saved to disk,
			// so the undo process doesn't work right. Because of this I'd rather disable it altogether until a solution is found.

			// The ability to undo this action is always useful
			//NGUIEditorTools.RegisterUndo("Update Atlas", UISettings.atlas, UISettings.atlas.texture, UISettings.atlas.material);

			// Update the atlas
			UpdateAtlas(UISettings.atlas, sprites);
		}
		else if (!keepSprites)
		{
			UpdateAtlas(UISettings.atlas, sprites);
		}
	}

	/// <summary>
	/// Draw the UI for this tool.
	/// </summary>

	void OnGUI ()
	{
		bool create = false;
		bool update = false;
		bool replace = false;

		string prefabPath = "";
		string matPath = "";

		// If we have an atlas to work with, see if we can figure out the path for it and its material
		if (UISettings.atlas != null && UISettings.atlas.name == UISettings.atlasName)
		{
			prefabPath = AssetDatabase.GetAssetPath(UISettings.atlas.gameObject.GetInstanceID());
			if (UISettings.atlas.spriteMaterial != null) matPath = AssetDatabase.GetAssetPath(UISettings.atlas.spriteMaterial.GetInstanceID());
		}

		// Assume default values if needed
		if (string.IsNullOrEmpty(UISettings.atlasName)) UISettings.atlasName = "New Atlas";
		if (string.IsNullOrEmpty(prefabPath)) prefabPath = NGUIEditorTools.GetSelectionFolder() + UISettings.atlasName + ".prefab";
		if (string.IsNullOrEmpty(matPath)) matPath = NGUIEditorTools.GetSelectionFolder() + UISettings.atlasName + ".mat";

		// Try to load the prefab
		GameObject go = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
		if (UISettings.atlas == null && go != null) UISettings.atlas = go.GetComponent<UIAtlas>();

		EditorGUIUtility.LookLikeControls(80f);

		GUILayout.Space(6f);
		GUILayout.BeginHorizontal();

		if (go == null)
		{
			GUI.backgroundColor = Color.green;
			create = GUILayout.Button("Create", GUILayout.Width(76f));
		}
		else
		{
			GUI.backgroundColor = Color.red;
			create = GUILayout.Button("Replace", GUILayout.Width(76f));
		}

		GUI.backgroundColor = Color.white;
		UISettings.atlasName = GUILayout.TextField(UISettings.atlasName);
		GUILayout.EndHorizontal();

		if (create)
		{
			// If the prefab already exists, confirm that we want to overwrite it
			if (go == null || EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to replace the contents of the " +
				UISettings.atlasName + " atlas with the textures currently selected in the Project View? All other sprites will be deleted.", "Yes", "No"))
			{
				replace = true;

				// Try to load the material
				Material mat = AssetDatabase.LoadAssetAtPath(matPath, typeof(Material)) as Material;

				// If the material doesn't exist, create it
				if (mat == null)
				{
					Shader shader = Shader.Find("Unlit/Transparent Colored");
					mat = new Material(shader);

					// Save the material
					AssetDatabase.CreateAsset(mat, matPath);
					AssetDatabase.Refresh();

					// Load the material so it's usable
					mat = AssetDatabase.LoadAssetAtPath(matPath, typeof(Material)) as Material;
				}

				if (UISettings.atlas == null || UISettings.atlas.name != UISettings.atlasName)
				{
					// Create a new prefab for the atlas
#if UNITY_3_4
					Object prefab = (go != null) ? go : EditorUtility.CreateEmptyPrefab(prefabPath); 
#else
					Object prefab = (go != null) ? go :	PrefabUtility.CreateEmptyPrefab(prefabPath);
#endif
					// Create a new game object for the atlas
					go = new GameObject(UISettings.atlasName);
					go.AddComponent<UIAtlas>().spriteMaterial = mat;

					// Update the prefab
#if UNITY_3_4
					EditorUtility.ReplacePrefab(go, prefab);
#else
					PrefabUtility.ReplacePrefab(go, prefab);
#endif
					DestroyImmediate(go);
					AssetDatabase.Refresh();

					// Select the atlas
					go = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
					UISettings.atlas = go.GetComponent<UIAtlas>();
				}
			}
		}

		ComponentSelector.Draw<UIAtlas>("...or select", UISettings.atlas, OnSelectAtlas);

		List<Texture> textures = GetSelectedTextures();

		if (UISettings.atlas != null && UISettings.atlas.name == UISettings.atlasName)
		{
			Material mat = UISettings.atlas.spriteMaterial;
			Texture tex = UISettings.atlas.texture;

			// Material information
			GUILayout.BeginHorizontal();
			{
				if (mat != null)
				{
					if (GUILayout.Button("Material", GUILayout.Width(76f))) Selection.activeObject = mat;
					GUILayout.Label(" " + mat.name);
				}
				else
				{
					GUI.color = Color.grey;
					GUILayout.Button("Material", GUILayout.Width(76f));
					GUI.color = Color.white;
					GUILayout.Label(" N/A");
				}
			}
			GUILayout.EndHorizontal();

			// Texture atlas information
			GUILayout.BeginHorizontal();
			{
				if (tex != null)
				{
					if (GUILayout.Button("Texture", GUILayout.Width(76f))) Selection.activeObject = tex;
					GUILayout.Label(" " + tex.width + "x" + tex.height);
				}
				else
				{
					GUI.color = Color.grey;
					GUILayout.Button("Texture", GUILayout.Width(76f));
					GUI.color = Color.white;
					GUILayout.Label(" N/A");
				}
			}
			GUILayout.EndHorizontal();

			// Padding and trimming
			GUILayout.BeginHorizontal();
			UISettings.atlasPadding = Mathf.Clamp(EditorGUILayout.IntField("Padding", UISettings.atlasPadding, GUILayout.Width(100f)), 0, 8);
			UISettings.atlasTrimming = EditorGUILayout.Toggle("Trim Alpha", UISettings.atlasTrimming);
			GUILayout.EndHorizontal();

			if (textures.Count > 0)
			{
				GUI.backgroundColor = Color.green;
				update = GUILayout.Button("Add/Update All");
				GUI.backgroundColor = Color.white;
			}
			else
			{
				NGUIEditorTools.DrawSeparator();
				GUILayout.Label("You can reveal more options by selecting\none or more textures in the Project View\nwindow.");
			}
		}
		else
		{
			NGUIEditorTools.DrawSeparator();
			GUILayout.Label("You can create a new atlas by selecting\none or more textures in the Project View\nwindow, then clicking \"Create\".");
		}

		Dictionary<string, int> spriteList = GetSpriteList(textures);

		if (spriteList.Count > 0)
		{
			NGUIEditorTools.DrawHeader("Sprites");
			GUILayout.Space(-7f);

			mScroll = GUILayout.BeginScrollView(mScroll);

			string delSprite = null;
			int index = 0;
			foreach (KeyValuePair<string, int> iter in spriteList)
			{
				++index;
				NGUIEditorTools.HighlightLine(new Color(0.6f, 0.6f, 0.6f));
				GUILayout.BeginHorizontal();
				GUILayout.Label(index.ToString(), GUILayout.Width(24f));
				GUILayout.Label(iter.Key);

				if (iter.Value == 2)
				{
					GUI.color = Color.green;
					GUILayout.Label("Add", GUILayout.Width(27f));
					GUI.color = Color.white;
				}
				else if (iter.Value == 1)
				{
					GUI.color = Color.cyan;
					GUILayout.Label("Update", GUILayout.Width(45f));
					GUI.color = Color.white;
				}
				else
				{
					if (string.IsNullOrEmpty(mDelName) || mDelName != iter.Key)
					{
						// If we have not yet selected a sprite for deletion, show a small "X" button
						if (GUILayout.Button("X", GUILayout.Width(22f))) mDelName = iter.Key;
					}
					else
					{
						GUI.backgroundColor = Color.red;

						if (GUILayout.Button("Delete", GUILayout.Width(60f)))
						{
							// Confirmation button clicked on -- delete this sprite
							delSprite = iter.Key;
							mDelName = null;
						}
						GUI.backgroundColor = Color.white;
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();

			// If this sprite was marked for deletion, remove it from the atlas
			if (!string.IsNullOrEmpty(delSprite))
			{
				List<UIAtlas.Sprite> list = UISettings.atlas.spriteList;

				foreach (UIAtlas.Sprite sp in list)
				{
					if (sp.name == delSprite)
					{
						list.Remove(sp);
						List<SpriteEntry> sprites = new List<SpriteEntry>();
						ExtractSprites(UISettings.atlas, sprites);
						UpdateAtlas(UISettings.atlas, sprites);
						mDelName = null;
						return;
					}
				}
			}
			else if (update) UpdateAtlas(textures, true);
			else if (replace) UpdateAtlas(textures, false);
			return;
		}
	}
}