using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CorazonUI : MonoBehaviour
{
    public List<GameObject> corazones = new List<GameObject>();

    public void PerderVida()
    {
        if (corazones.Count > 0)
        {
            GameObject ultimo = corazones[corazones.Count - 1];
            corazones.RemoveAt(corazones.Count - 1);

            StartCoroutine(ParpadearYDestruir(ultimo));
        }
    }

    private IEnumerator ParpadearYDestruir(GameObject corazon)
    {
        Image img = corazon.GetComponent<Image>();

        for (int i = 0; i < 3; i++)
        {
            img.color = Color.white;
            yield return new WaitForSeconds(0.1f);

            img.color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(corazon);
    }
}