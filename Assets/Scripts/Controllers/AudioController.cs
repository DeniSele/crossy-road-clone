using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private Sprite soundOn;

    [SerializeField]
    private Sprite soundOff;

    [SerializeField]
    private Image soundIcon;

    [SerializeField]
    private List<AudioClip> steps;

    [SerializeField]
    private AudioClip deathSound;

    [SerializeField]
    private AudioClip goldPickupSound;

    [SerializeField]
    private AudioClip clickSound;

    private bool isAudioEnable = true;

    private void Awake()
    {
        DeathZone.OnDead += PlayDeathSound;
        GoldPicker.OnGoldPick += PlayGoldSound;
        PlayerController.OnJump += PlayJumpSound;
    }

    private void PlayJumpSound(Vector3 targetPosition)
    {
        if(!isAudioEnable || steps.Count == 0)
        {
            return;
        }

        AudioSource.PlayClipAtPoint(steps[Random.Range(0, steps.Count)], transform.position);
    }

    private void PlayGoldSound(int amount)
    {
        if (isAudioEnable)
        {
            AudioSource.PlayClipAtPoint(goldPickupSound, transform.position);
        }
    }

    private void PlayDeathSound(string zoneTag)
    {
        if (isAudioEnable)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
    }

    public void OnOffAudio()
    {
        if (isAudioEnable)
        {
            isAudioEnable = false;
            soundIcon.sprite = soundOff;
        }
        else
        {
            isAudioEnable = true;
            soundIcon.sprite = soundOn;
        }
    }

    public void PlaySoundOnClick()
    {
        if (isAudioEnable)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position);
        }
    }
}
