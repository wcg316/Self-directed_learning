using UnityEngine;
using System.Collections;
using static UnityEngine.Random;
using Unity.Collections;
using Unity.Burst.CompilerServices;

[System.Serializable]
public class EffectPlayer
{
	[SerializeField] GameObject effect;
	public GameObject Effect
	{
		get => effect;
	}
	AudioSource audioSource;
	[SerializeField] AudioClip sound;
	int horizontalDirectionMultiplier;
	[SerializeField] int offsetAngle;
	[SerializeField] float distanceX;
	[SerializeField] float distanceY;
	[SerializeField] float duration;
	public float Duration
	{
		get => duration;
	}
	[SerializeField] float cooldownDuration;
	public float CooldownDuration
	{
		get => cooldownDuration;
	}
	bool onCooldown;
	public bool OnCooldown
	{
		get => onCooldown;
	}

	public void Initialize()
	{
		if (effect != null)
		{
			audioSource = effect.GetComponent<AudioSource>();

			if (audioSource == null)
				audioSource = effect.AddComponent<AudioSource>();

			effect.SetActive(false);
		}
	}

	public IEnumerator PlayEffect(Transform transform)
	{
		horizontalDirectionMultiplier = transform.localScale.x < 0f ? -1 : 1;

		SetPositionFrom(transform);
		SetAngle();
		SetDirection();

		Activate();
		PlaySound();
		StartCooldown();

		yield return new WaitForSeconds(Duration);
		Hide();

		yield return new WaitForSeconds(CooldownDuration);
		EndCooldown();
	}

	void Activate()
	{
		effect.SetActive(true);
	}

	void Hide()
	{
		effect.SetActive(false);
	}

	void PlaySound()
	{
		audioSource.PlayOneShot(sound);
	}

	void StartCooldown()
	{
		onCooldown = true;
	}

	void EndCooldown()
	{
		onCooldown = false;
	}

	public void SetPositionFrom(Transform transform)
	{
		Vector3 offset =
			new Vector3(distanceX * horizontalDirectionMultiplier, distanceY, 0f);

		effect.transform.localPosition = transform.position + offset;
	}

	void SetAngle()
	{
		int randomAngle = Range(-offsetAngle, offsetAngle + 1);

		effect.transform.eulerAngles =
			new Vector3(0f, 0f, randomAngle);
	}

	void SetDirection()
	{
		Vector3 scale = effect.transform.localScale;

		scale.x = Mathf.Abs(scale.x) * horizontalDirectionMultiplier;
		effect.transform.localScale = scale;
	}

}