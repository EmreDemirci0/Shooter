using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SimplePlayerHealth : HealthManager
{
	public float health = 100f;

	[Header("Hurt Effect")]
	public Transform canvas;
	public GameObject hurtPrefab;
	public float decayFactor = 0.8f;

	private HurtHUD hurtUI;
	private bool dead = false;

	public Slider healthSlider;
	private TextMeshProUGUI healthText;

	private void Awake()
	{
		AudioListener.pause = false;

		// 🎯 Slider'ı sahneden "Slider" tag'i ile bul
		GameObject sliderObj = GameObject.FindWithTag("Slider");
		if (sliderObj != null)
		{
			healthSlider = sliderObj.GetComponent<Slider>();
			healthSlider.maxValue = health;
			// 🎯 Text, Slider'ın 2. çocuğunda (index 1)
			if (sliderObj.transform.childCount >= 3)
			{
				Transform textTransform = sliderObj.transform.GetChild(2);
				healthText = textTransform.GetComponent<TextMeshProUGUI>();
			}
		}
		else
		{
			Debug.LogWarning("Health Slider bulunamadı! Lütfen sahnede tag'i 'Slider' olan bir Slider objesi ekleyin.");
		}

		hurtUI = this.gameObject.AddComponent<HurtHUD>();
		hurtUI.Setup(canvas, hurtPrefab, decayFactor, this.transform);

		UpdateHealthUI();
	}

	public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin)
	{
		Debug.Log("Shoot");
		health -= damage;

		UpdateHealthUI();

		if (hurtPrefab && canvas)
			hurtUI.DrawHurtUI(origin.transform, origin.GetHashCode());

		if (health <= 0f && !dead)
		{
			dead = true;
			StartCoroutine(nameof(ReloadScene));
		}
	}

	private void UpdateHealthUI()
	{
		if (healthSlider != null)
			healthSlider.value = health;

		if (healthText != null)
			healthText.text = Mathf.RoundToInt(Mathf.Max(health, 0)).ToString();
	}

	private IEnumerator ReloadScene()
	{
		SendMessage("PlayerDead", SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds(0.5f);
		canvas.gameObject.SetActive(false);
		AudioListener.pause = true;
		Camera.main.clearFlags = CameraClearFlags.SolidColor;
		Camera.main.backgroundColor = Color.black;
		Camera.main.cullingMask = LayerMask.GetMask();

		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(0);
	}
}
