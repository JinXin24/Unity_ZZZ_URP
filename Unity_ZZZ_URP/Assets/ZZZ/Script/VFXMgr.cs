using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXMgr : SingletonMono<VFXMgr>
{
    private WaitForSeconds twoSeconds = new WaitForSeconds(2f);
    [SerializeField]private GameObject hitPrefab;
    [SerializeField] private GameObject soundPrefab;

    public void ReuseHitObj(Vector3 effectPosition)
    {
        GameObject hitObj = PoolManager.Instance.ReuseObject(hitPrefab, effectPosition, Quaternion.identity);
        hitObj.SetActive(true);
        StartCoroutine(DisableHarvestActionEffect(hitObj,twoSeconds));
    }

    public void ReuseHitSound(Vector3 audioPosition)
    {
        GameObject hitSound = PoolManager.Instance.ReuseObject(soundPrefab, audioPosition, Quaternion.identity);
        hitSound.SetActive(true);
        StartCoroutine(DisableHarvestActionEffect(hitSound,twoSeconds));
    }

    private IEnumerator DisableHarvestActionEffect(GameObject effectGameObject, WaitForSeconds secondsToWait)
    {
        yield return secondsToWait;
        effectGameObject.SetActive(false);
    }
}
