using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PolygonCollider2D))]
public class NavMap : MonoBehaviour
{

    private PolygonCollider2D areaCaminable;
    private PuntoNavegable[][] puntosNavegables;

    public int puntosNavegablesX = 200;
    public int puntosNavegablesY = 20;
    public GameObject puntoNavegacion;

    [HideInInspector]
    public float minY;
    [HideInInspector]
    public float maxY;

    private float distX;
    private float distY;

    void Start()
    {
        areaCaminable = GetComponent<PolygonCollider2D>();
        minY = areaCaminable.bounds.min.y;
        maxY = areaCaminable.bounds.max.y;
        generarGrillaDeNavegacion();
    }

    public void generarGrillaDeNavegacion()
    {

        GameObject auxCol;
        GameObject auxObj;

        auxObj = new GameObject("Navegacion");
        auxObj.transform.parent = transform;

        puntosNavegables = new PuntoNavegable[puntosNavegablesY][];
        distX = areaCaminable.bounds.extents.x * 2 / puntosNavegablesX;
        distY = areaCaminable.bounds.extents.y * 2 / puntosNavegablesY;
        float auxX = areaCaminable.bounds.min.x;
        float auxY = areaCaminable.bounds.min.y;

        for (var i = 0; i < puntosNavegablesY; i++)
        {
            puntosNavegables[i] = new PuntoNavegable[puntosNavegablesX];
            for (var j = 0; j < puntosNavegablesX; j++)
            {
                if (areaCaminable.OverlapPoint(new Vector2(auxX, auxY)))
                { //Si el punto esta dentro del polygonCollider2D
                    //Creo el collider del punto de navegacion
                    auxCol = Instantiate(puntoNavegacion, auxObj.transform, true);
                    auxCol.transform.position = new Vector3(auxX, auxY, 0);
                    auxCol.GetComponent<BoxCollider2D>().size = new Vector2(distX, distY);
                    auxCol.GetComponent<NavPunto>().i = i;
                    auxCol.GetComponent<NavPunto>().j = j;
                    auxCol.gameObject.name = auxCol.gameObject.name + "-" + i.ToString() + "-" + j.ToString();

                    //Creo el punto navegable
                    puntosNavegables[i][j] = new PuntoNavegable(new Vector3(auxX, auxY, 0), j, i, auxCol);

                    //Armo las aristas
                    if ((j > 0) && (puntosNavegables[i][j - 1] != null))
                    {
                        puntosNavegables[i][j].hermanos.Add(puntosNavegables[i][j - 1]);
                        puntosNavegables[i][j - 1].hermanos.Add(puntosNavegables[i][j]);
                    }

                    if ((i > 0) && (puntosNavegables[i - 1][j] != null))
                    {
                        puntosNavegables[i][j].hermanos.Add(puntosNavegables[i - 1][j]);
                        puntosNavegables[i - 1][j].hermanos.Add(puntosNavegables[i][j]);
                    }
                    
                    if ((i > 0) && (j > 0) && (puntosNavegables[i - 1][j - 1] != null))
                    {
                        puntosNavegables[i][j].hermanos.Add(puntosNavegables[i - 1][j - 1]);
                        puntosNavegables[i - 1][j - 1].hermanos.Add(puntosNavegables[i][j]);
                    }

                    if ((j < puntosNavegables.Length - 2) && (i > 0) && (puntosNavegables[i - 1][j + 1] != null))
                    {
                        puntosNavegables[i][j].hermanos.Add(puntosNavegables[i - 1][j + 1]);
                        puntosNavegables[i - 1][j + 1].hermanos.Add(puntosNavegables[i][j]);
                    }

                }
                else
                {
                    puntosNavegables[i][j] = null;
                }
                auxX += distX;
            }
            auxX = areaCaminable.bounds.min.x;
            auxY += distY;
        }
        areaCaminable.enabled = false;
    }

    public List<PuntoNavegable> caminoMinimo(int oi, int oj, int di, int dj)
    {
        NavMinHeap abiertos = new NavMinHeap();
        NavMinHeap cerrados = new NavMinHeap();
        NodoAE auxNodo;
        NodoAE auxNodoHermano;
        NodoAE nodoEncontradoAbiertos;
        NodoAE nodoEncontradoCerrados;
        PuntoNavegable origen;
        PuntoNavegable destino;
        List<PuntoNavegable> minPath = new List<PuntoNavegable>(); ;

        origen = puntosNavegables[oi][oj];
        destino = puntosNavegables[di][dj];

        auxNodo = new NodoAE();
        auxNodo.punto = origen;
        auxNodo.costo = 0;
        abiertos.Add(auxNodo);
        minPath.Add(destino);
        while (abiertos.size() > 0)
        {
            auxNodo = abiertos.remove();

            foreach (PuntoNavegable hermano in auxNodo.punto.hermanos)
            {
                if (hermano != null && !hermano.objeto.GetComponent<NavPunto>().blocked)
                {
                    if ((hermano.x == destino.x) && (hermano.y == destino.y))
                    {
                        minPath.Add(auxNodo.punto);
                        while (auxNodo.padre != null)
                        {
                            auxNodo = auxNodo.padre;
                            minPath.Add(auxNodo.punto);
                        }
                        return minPath;
                    }
                    auxNodoHermano = new NodoAE();
                    auxNodoHermano.padre = auxNodo;
                    auxNodoHermano.punto = hermano;
                    auxNodoHermano.distanciaAlOrigen = auxNodo.costo + 1;
                    auxNodoHermano.distanciaAlFinal = distHeuristica(auxNodoHermano.punto, destino);
                    auxNodoHermano.costo = auxNodoHermano.distanciaAlOrigen + auxNodoHermano.distanciaAlFinal;
                    nodoEncontradoAbiertos = abiertos.find(auxNodoHermano);
                    nodoEncontradoCerrados = cerrados.find(auxNodoHermano);
                    if ((nodoEncontradoAbiertos != null) && (nodoEncontradoAbiertos.costo < auxNodoHermano.costo))
                    {
                        continue;
                    }
                    if ((nodoEncontradoCerrados != null) && (nodoEncontradoCerrados.costo < auxNodoHermano.costo))
                    {
                        continue;
                    }
                    abiertos.Add(auxNodoHermano);
                }
            }
            cerrados.Add(auxNodo);
        }
        return null;
    }

    //Obtiene la distancia heurística entre nodos
    public float distHeuristica(PuntoNavegable a, PuntoNavegable b)
    {
        return Vector3.Distance(a.objeto.transform.position, b.objeto.transform.position);
    }

}

//Usado por el algoritmo A*
public class NodoAE : IComparable<NodoAE>
{
    public NodoAE padre = null;
    public PuntoNavegable punto = null;
    public float costo = 0;
    public float distanciaAlOrigen = 0;
    public float distanciaAlFinal = 0;

    public int CompareTo(NodoAE otro)
    {
        if (this.costo > otro.costo) return 1;
        if (this.costo == otro.costo) return 0;
        return -1;
    }
}

public class PuntoNavegable
{
    public Vector3 punto;
    public List<PuntoNavegable> hermanos;
    public int x;
    public int y;
    public GameObject objeto;
    public bool optimizado;

    public PuntoNavegable(Vector3 p, int px, int py, GameObject o)
    {
        punto = p;
        hermanos = new List<PuntoNavegable>();
        x = px;
        y = py;
        objeto = o;
        optimizado = false;
    }
}