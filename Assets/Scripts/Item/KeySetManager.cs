using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySetManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Keyposition = new List<GameObject>();
    [SerializeField]
    private GameObject realKey = null;
    [SerializeField]
    private GameObject fakeKey = null;

    private int fakeKeyCount = 4;

    private void Start()
    {
        MakeKey();
    }

    private void MakeKey()
    {
        int realKeyIndex = Random.Range(0, Keyposition.Count); // 0 ~ 4 사이의 값 나옴. 최댓값 미 포함
        Instantiate(realKey, Keyposition[realKeyIndex].transform);
        Keyposition.RemoveAt(realKeyIndex);

        List<int> fakeKeyIndex = new List<int>();
        List<GameObject> remainingPositions = new List<GameObject>(Keyposition);

        while (fakeKeyIndex.Count < fakeKeyCount)
        {
            int index = Random.Range(0, remainingPositions.Count);
            int realIndex = Keyposition.IndexOf(remainingPositions[index]);
            if (realIndex == -1 || fakeKeyIndex.Contains(realIndex)) continue;

            fakeKeyIndex.Add(realIndex);
            remainingPositions.RemoveAt(index);
        }

        for (int i = 0; i < fakeKeyIndex.Count; ++i)
        {
            int index = fakeKeyIndex[i];
            Instantiate(fakeKey, Keyposition[index].transform);
        }

        gameObject.GetComponent<KeySetManager>().enabled = false;
    }

}