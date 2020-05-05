using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum PLATFORM_MODE
{
    YOYO,
    CYCLIC
}

public class AutomatedPlatform : MonoBehaviour
{
    /****************** VISUALS *******************/
    //GameObjects
    [SerializeField]
    GameObject platformPrefab;
    [SerializeField]
    GameObject chainPrefab;
    [SerializeField]
    GameObject operatorSwitch;
    ToggleSwitch toggleSwitch;

    public float chainDistance;


    Transform platformInstance;
    Animator animator;

    /***************** PLATFORM *********************/

    //PLATFORM PARAMS
    public float speed;
    public bool activated;
    public bool switchOperated;
    public PLATFORM_MODE mode;

    public float snapAngle = 45;
    [SerializeField]
    protected bool backwards = false;

    //WAYPOINTS
    public List<PathInfo> pathPoints = new List<PathInfo>();

    public int currentIndex;
    public int nextIndex;
    bool back;
    //METHODS
    public virtual void SwitchMode(PLATFORM_MODE newMode)
    {
        StopCoroutine(currentCoroutine);
        mode = newMode;
        if(currentCoroutine!=null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = MovePlatform(); 
        StartCoroutine(currentCoroutine);
    }

    public virtual void NextIndex()
    {
        switch (mode)
        {
            case PLATFORM_MODE.YOYO:
                if(nextIndex==pathPoints.Count-1 && !back)
                {
                    back = true;
                }
                if(nextIndex==0 && back)
                {
                    back = false;
                }

                if(!back)
                {
                    nextIndex++;
                }
                else
                {
                    nextIndex--;
                }
                break;
            case PLATFORM_MODE.CYCLIC:
                if(backwards)
                {
                    nextIndex = (nextIndex - 1) % pathPoints.Count;
                    if(nextIndex<0)
                    {
                        nextIndex += pathPoints.Count;
                    }
                }
                else
                {
                    nextIndex = (nextIndex + 1) % pathPoints.Count;
                }
                break;
            default:
                break;
        }
    }

    public void CreateDefaultPath()
    {
        pathPoints.Clear();
        AddPoint(transform.position);
        AddPoint(transform.position + Vector3.up);
    }

    //DEBUG
    public float discSize = 0.3f;
    public float chainSize = 0.1f;


    //COROUTINES
    IEnumerator currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        platformInstance = Instantiate(platformPrefab,pathPoints[0].startPoint,Quaternion.identity,transform).transform;
        animator = platformInstance.GetComponent<Animator>();
        foreach(PathInfo p in pathPoints)
        {
            foreach (Vector2 chain in p.chainPoints)
            {
                GameObject obj = Instantiate(chainPrefab,chain, Quaternion.identity, transform);
            }
        }
        if (activated && !switchOperated)
        {
            currentCoroutine = MovePlatform();
            StartCoroutine(currentCoroutine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("activate", activated);
        if(switchOperated)
        {

        }
    }

    public IEnumerator MovePlatform()
    {
        Vector3 pos = new Vector3(pathPoints[nextIndex].startPoint.x, pathPoints[nextIndex].startPoint.y, transform.position.z);
        while ( platformInstance.position != pos)
        {
            platformInstance.position = Vector3.MoveTowards(platformInstance.position, pathPoints[nextIndex].startPoint, speed*Time.deltaTime);
            yield return null;
        }
        NextIndex();
        if(currentCoroutine!=null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = MovePlatform();
        StartCoroutine(currentCoroutine);
    }

    #region PATH_METHODS

    [System.Serializable]
    public class PathInfo
    {
        public Vector2 startPoint;
        public List<Vector2> chainPoints = new List<Vector2>();
        public Vector2 nextPoint;
        public float chainSeparation;
        public PathInfo(Vector2 startPoint,float chainSeparation = 0.5f)
        {
            this.startPoint = startPoint;
            nextPoint = Vector2.negativeInfinity;
            this.chainSeparation = chainSeparation>0?chainSeparation:0.5f;
        }

        public void ReconfigureChains()
        {
            chainPoints.Clear();
            if (nextPoint.Equals(Vector2.negativeInfinity))
            {
                Debug.Log("Infinite distance");
                return;
            }

            float dist = Vector2.Distance(startPoint, nextPoint);

            Vector2 dir = (nextPoint - startPoint).normalized;
            for(int i=0;i*chainSeparation <= dist;i++)
            {
                chainPoints.Add(startPoint + (i * chainSeparation) * dir);
            }

        }
    }

    public void AddPoint(Vector2 point)
    {
        if (pathPoints.Count > 0)
        {
            pathPoints[pathPoints.Count - 1].nextPoint = point;
            pathPoints[pathPoints.Count - 1].ReconfigureChains();

            Debug.Log("start: " + pathPoints[pathPoints.Count - 1].startPoint);
            Debug.Log("next: " + pathPoints[pathPoints.Count - 1].nextPoint);
        }
        pathPoints.Add(new PathInfo(point,chainDistance));
        if (mode == PLATFORM_MODE.CYCLIC && pathPoints.Count>2)
        {
            pathPoints[pathPoints.Count - 1].nextPoint = pathPoints[0].startPoint;
            pathPoints[pathPoints.Count - 1].ReconfigureChains();
        }

    }

    public void RemovePoint(Vector2 point)
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            if (pathPoints[i].startPoint == point)
            {
                RemovePointAt(i);
                break;
            }
        }
    }
    public void RemovePointAt(int index)
    {
        if (index == 0 || index == pathPoints.Count - 1)
        {
            if (index != 0)
            {
                if (mode == PLATFORM_MODE.YOYO)
                {
                    pathPoints[index- 1].nextPoint = Vector2.negativeInfinity;
                }
                else
                {
                    pathPoints[index - 1].nextPoint = pathPoints[0].startPoint;
                }
                pathPoints[index - 1].ReconfigureChains();
            }
        }
        else
        {
            pathPoints[index - 1].nextPoint = pathPoints[index + 1].startPoint;
            pathPoints[index - 1].ReconfigureChains();
        }
        pathPoints.RemoveAt(index);
        
    }
    #endregion
}
