using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialCanvasManager : MonoBehaviour
{
    [Header("Páginas del tutorial")]
    public GameObject[] paginas;

    [Header("Botones de navegación")]
    public Button flechaIzquierda;
    public Button flechaDerecha;
    public Button botonCerrar;

    [Header("Otra UI que se debe ocultar mientras el tutorial está activo")]
    public GameObject otraUI;

    private int paginaActual = 0;

    void Start()
    {
        MostrarTutorial(); // Se asegura de empezar desde la primera página

        // Listeners de botones
        flechaIzquierda.onClick.AddListener(PaginaAnterior);
        flechaDerecha.onClick.AddListener(PaginaSiguiente);
        botonCerrar.onClick.AddListener(CerrarTutorial);
    }

    void ActualizarPaginas()
    {
        for (int i = 0; i < paginas.Length; i++)
        {
            paginas[i].SetActive(i == paginaActual);
        }

        flechaIzquierda.gameObject.SetActive(paginaActual > 0);
        flechaDerecha.gameObject.SetActive(paginaActual < paginas.Length - 1);
        botonCerrar.gameObject.SetActive(paginaActual == paginas.Length - 1);
    }

    void PaginaAnterior()
    {
        if (paginaActual > 0)
        {
            paginaActual--;
            ActualizarPaginas();
        }
    }

    void PaginaSiguiente()
    {
        if (paginaActual < paginas.Length - 1)
        {
            paginaActual++;
            ActualizarPaginas();
        }
    }

    void CerrarTutorial()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (otraUI != null)
            otraUI.SetActive(true);

        gameObject.SetActive(false); // Desactiva el Canvas
    }

    public void MostrarTutorialExternamente()
    {
        MostrarTutorial(); // Para activación desde otro script
    }

    void MostrarTutorial()
    {
        paginaActual = 0;
        ActualizarPaginas();

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (otraUI != null)
            otraUI.SetActive(false);
    }

}
