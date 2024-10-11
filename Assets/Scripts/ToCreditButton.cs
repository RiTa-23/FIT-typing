using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToCreditButton : MonoBehaviour
{
    [SerializeField] AudioSource SE_dicision;
    // Start is called before the first frame update
    [SerializeField] GameObject creditPanel;
    public void ToCredit()
    {
        creditPanel.SetActive(true);
        SE_dicision.Play();
    }
}
