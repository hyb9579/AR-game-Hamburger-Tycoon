using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    //    하나쌓을때

    //ShockWave

    //일반 오더 성공

    //CFX2_PickupStar 2

    //마지막 잘못쌓았을때(시간 지나서 쌓았을떄도)

    //DustSmoke_A

    //스페셜오더 성공

    //CFX2_PickupStar 1

    //버릴때

    //DustSmoke_A

    [SerializeField]
    private GameObject stackEffect;

    [SerializeField]
    private GameObject wrongStackEffect;

    [SerializeField]
    private GameObject normalSuccessEffect;

    [SerializeField]
    private GameObject specialSuccessEffect;

    [SerializeField]
    private GameObject abandonEffect;

    [SerializeField]
    private GameObject onAirPos;

    [SerializeField]
    private GameObject onTrayPos;

    [SerializeField]
    private ParticleSystem dustEffectParticle;

    [SerializeField]
    private ParticleSystem shockWaveEffectParticle;

    public void playStackEffect()
    {
        StartCoroutine("playShockWaveEffect");
        //GameObject gameObject = Instantiate(stackEffect, onTrayPos.transform);
        //gameObject.transform.parent = onTrayPos.transform;
        //gameObject.transform.localEulerAngles = new Vector3(-90, 0, 0);
        //Destroy(gameObject, 1f);
    }

    public void playWrongStackEffect()
    {
        StartCoroutine("playDustEffect");
        //GameObject gameObject = Instantiate(wrongStackEffect, onTrayPos.transform);
        //gameObject.transform.parent = onTrayPos.transform;
        //gameObject.transform.localEulerAngles = new Vector3(-90, 0, 0);
        //Destroy(gameObject, 1f);
    }

    public void playNormalSuccessEffect()
    {
        GameObject gameObject = Instantiate(normalSuccessEffect, onAirPos.transform);
        gameObject.transform.parent = onAirPos.transform;
        Destroy(gameObject, 1f);
    }

    public void playSpecialSuccessEffect()
    {
        GameObject gameObject = Instantiate(specialSuccessEffect, onAirPos.transform);
        gameObject.transform.parent = onAirPos.transform;
        Destroy(gameObject, 1f);
    }

    public void playAbandonEffect()
    {
        StartCoroutine("playDustEffect");
        //GameObject gameObject = Instantiate(abandonEffect, onTrayPos.transform);
        //gameObject.transform.parent = onTrayPos.transform;
        //gameObject.transform.localEulerAngles = new Vector3(-90, 0, 0);
        //Destroy(gameObject, 1f);
    }

    IEnumerator playDustEffect()
    {
        dustEffectParticle.Play();
        yield return new WaitForSeconds(1f);

        dustEffectParticle.Stop();
        yield break;
    }

    IEnumerator playShockWaveEffect()
    {
        shockWaveEffectParticle.Play();
        yield return new WaitForSeconds(0.5f);

        shockWaveEffectParticle.Stop();
        yield break;
    }
}
