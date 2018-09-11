using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class NavChar : MonoBehaviour
{

    public float speed = 3f;

    private NavMap escenario;

    private Vector3 clickPos;
    private Vector3 puntoClickeado;
    private GameObject pies;

    private Stack<Vector3> caminoMinimoPuntos;
    private int pasoCamino;
    private Vector3 caminarHacia;

    private bool caminar = false;

    private SyncAnimator syncAnimator;

    private UnityAction callback;

    void Start()
    {
        caminarHacia = transform.position;
        pies = transform.Find("Pies").gameObject;
        syncAnimator = transform.GetComponent<SyncAnimator>();
        escenario = FindObjectOfType<NavMap>();
    }

    void Update()
    {
        if (caminar)
        {
            Movimiento();
        }
    }

    public void cancelarMovimiento()
    {
        caminoMinimoPuntos = new Stack<Vector3>();
        caminar = false;
        syncAnimator.SetBool("walk", caminar);
    }

    public void irHaciaClick(UnityAction cb = null)
    {
        if (cb != null)
        {
            callback = cb;
        }

        //Obtengo el punto de navegación más cercano al click
        clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = transform.position.z;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(clickPos, 10F, LayerMask.GetMask("Navigation"));
        float minDistance = float.MaxValue;
        Collider2D minHit = null;
        float distance;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            distance = Vector3.Distance(hitColliders[i].transform.position, clickPos);
            if (distance < minDistance && !hitColliders[i].GetComponent<NavPunto>().blocked)
            {
                minDistance = distance;
                minHit = hitColliders[i];
            }
        }
        if (minHit != null)
        {
            puntoClickeado = minHit.transform.position;
            armarCamino(minHit);
        }
    }

    private void armarCamino(Collider2D hitObjetivo)
    {
        caminarHacia = transform.position;
        
        //Obtengo el punto de navegación más cercano a los pies
        //RaycastHit2D hitPies = Physics2D.Raycast(pies.transform.position, Vector2.down, 10, LayerMask.GetMask("Navigation"));
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pies.transform.position, 10F, LayerMask.GetMask("Navigation"));
        float minDistance = float.MaxValue;
        Collider2D minHitPies = null;
        float distance;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            distance = Vector3.Distance(hitColliders[i].transform.position, pies.transform.position);
            if (distance < minDistance && !hitColliders[i].GetComponent<NavPunto>().blocked)
            {
                minDistance = distance;
                minHitPies = hitColliders[i];
            }
        }


        //Si encontre dos puntos de navegación, obtengo las posiciones de ambos y obtengo el camino mínimo
        if ((hitObjetivo != null) && (minHitPies != null))
        {
            var oi = minHitPies.gameObject.GetComponent<NavPunto>().i;
            var oj = minHitPies.gameObject.GetComponent<NavPunto>().j;
            var di = hitObjetivo.gameObject.GetComponent<NavPunto>().i;
            var dj = hitObjetivo.gameObject.GetComponent<NavPunto>().j;
            caminoMinimoPuntos = new Stack<Vector3>();
            if (!(oi == di && oj == dj))
            {
                generarPilaPasos(escenario.caminoMinimo(oi, oj, di, dj));
            }
            else
            {
                caminoMinimoPuntos.Push(new Vector3(puntoClickeado.x, puntoClickeado.y, transform.position.z));
            }
            if (!caminar)
            {
                caminar = true;
                syncAnimator.SetBool("walk", caminar);
            }
        }
    }

    private void generarPilaPasos(List<PuntoNavegable> caminoMinimo)
    {
        if (caminoMinimo != null && caminoMinimo.Count > 1)
        {
            Vector3[] caminoMinimoSuavizado = new Vector3[caminoMinimo.Count - 1];
            for (int i = 0; i < caminoMinimo.Count - 1; i++)
            {
                caminoMinimoSuavizado[i] = new Vector3(caminoMinimo[i].objeto.transform.position.x, caminoMinimo[i].objeto.transform.position.y, transform.position.z);
            }
            caminoMinimoSuavizado = Curver.MakeSmoothCurve(caminoMinimoSuavizado, 1.0F);
            for (int i = 0; i < caminoMinimoSuavizado.Length - 1; i++)
            {
                caminoMinimoPuntos.Push(caminoMinimoSuavizado[i]);
            }

            /*for (int i = 0; i < caminoMinimo.Count - 1; i++)
            {
                caminoMinimoPuntos.Push(caminoMinimo[i].punto);
            }*/
        }
    }

    private void Movimiento()
    {
        Vector3 dir = caminarHacia - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, caminarHacia) <= 0.2f)
        {
            if (caminoMinimoPuntos.Count > 0)
            {
                caminarHacia = caminoMinimoPuntos.Pop();
            }
            else
            {
                if (callback != null)
                {
                    callback();
                    callback = null;
                }
                caminar = false;
                syncAnimator.SetBool("walk", caminar);
            }
        }
    }

}

