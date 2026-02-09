using System;
using System.IO;
using BepInEx;
using UnityEngine;
using TMPro;

[BepInPlugin("com.oms.fps", "Fps counter", "1.0.0")]
public class PixelFPSCounter : BaseUnityPlugin
{
	private GameObject canvasObj;
	private TextMeshProUGUI fpsText;
	private float deltaTime = 0.0f;
	private Camera mainCam;
	private TMP_FontAsset pixelFont;

	void Start()
	{
		// Load the font from your PixelFPS folder
		string fontPath = Path.Combine(Paths.PluginPath, "PixelFPS", "pixelify.ttf");
		if (File.Exists(fontPath))
		{
			Font osFont = new Font(fontPath);
			pixelFont = TMP_FontAsset.CreateFontAsset(osFont);
			pixelFont.name = "PixelifySans-Custom";
		}
	}

	// LateUpdate is better for head-locked VR UI to prevent jitter
	void LateUpdate()
	{
		if (mainCam == null)
		{
			mainCam = Camera.main;
			return;
		}

		if (canvasObj == null)
		{
			SetupHUD();
		}

		// FPS Calculation
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;

		if (fpsText != null)
		{
			fpsText.text = $"{Mathf.Ceil(fps)} FPS";
		}
	}

	void SetupHUD()
	{
		canvasObj = new GameObject("Pixel_FPS_Canvas");
		Canvas canvas = canvasObj.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.WorldSpace;
		canvasObj.transform.SetParent(mainCam.transform);

		// --- POSITIONING ---
		// Changed X from 0.6f to 0.85f to move it further right
		// Changed Y to 0.45f to tuck it higher into the corner
		canvasObj.transform.localPosition = new Vector3(-0.55f, -0.45f, 1.5f);

		canvasObj.transform.localRotation = Quaternion.identity;
		canvasObj.transform.localScale = new Vector3(0.002f, 0.002f, 0.002f);

		GameObject textObj = new GameObject("Pixel_FPS_Text");
		textObj.transform.SetParent(canvasObj.transform, false);

		fpsText = textObj.AddComponent<TextMeshProUGUI>();

		if (pixelFont != null)
		{
			fpsText.font = pixelFont;
		}

		fpsText.fontSize = 30;
		fpsText.color = Color.white;
		fpsText.alignment = TextAlignmentOptions.Right;

		// Shadow/Outline for visibility
		fpsText.outlineWidth = 0.25f;
		fpsText.outlineColor = Color.black;
	}

}
