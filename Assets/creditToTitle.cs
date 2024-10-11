using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditToTitle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource SE_dicision;

    [SerializeField] GameObject creditPanel;
    public void pushToTitle()
    {
        creditPanel.SetActive(false);
        SE_dicision.Play();
    }
}
