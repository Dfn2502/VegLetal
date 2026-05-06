using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CorazonUI : MonoBehaviour
{
    public GameObject prefabCorazonEnemigo;
    public GameObject prefabCorazon;

    public Transform contenedorEnemigo;
    public Transform contenedorJugador;

    List<GameObject> corazonesEnemigo = new List<GameObject>();
    List<GameObject> corazonesJugador = new List<GameObject>();

    float separacion = 115f;


    public void MostrarCorazonesJugador(int cantidad)
    {
        LimpiarJugador();

        for (int i = 0; i < cantidad; i++)
        {
            GameObject c = Instantiate(prefabCorazon, contenedorJugador, false);
            corazonesJugador.Add(c);

            RectTransform rt = c.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.anchoredPosition = new Vector2(i * separacion, 0);
        }
    }

    public void MostrarCorazonesEnemigo(int cantidad)
    {
        LimpiarEnemigo();

        for (int i = 0; i < cantidad; i++)
        {
            GameObject c = Instantiate(prefabCorazonEnemigo, contenedorEnemigo, false);
            corazonesEnemigo.Add(c);

            RectTransform rt = c.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.anchoredPosition = new Vector2(-i * separacion, 0);
        }
    }

    public void QuitarVidaJugador()
    {
        if (corazonesJugador.Count == 0) return;

        GameObject ultimo = corazonesJugador[^1];
        corazonesJugador.RemoveAt(corazonesJugador.Count - 1);

        StartCoroutine(ParpadearYDestruir(ultimo));
    }

    public void QuitarVidaEnemigo()
    {
        if (corazonesEnemigo.Count == 0) return;

        GameObject ultimo = corazonesEnemigo[^1];
        corazonesEnemigo.RemoveAt(corazonesEnemigo.Count - 1);

        StartCoroutine(ParpadearYDestruir(ultimo));
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

    private void LimpiarJugador()
    {
        foreach (var c in corazonesJugador)
            Destroy(c);

        corazonesJugador.Clear();
    }

    private void LimpiarEnemigo()
    {
        foreach (var c in corazonesEnemigo)
            Destroy(c);

        corazonesEnemigo.Clear();
    }
}