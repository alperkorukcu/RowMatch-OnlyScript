using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Yunus Alper KÖRÜKCÜ  
public class LerpHelper : MonoBehaviour
{
    public bool shouldLerp = false;

    public Vector3 endPosition;

    public string getDir;

    void Update()
    {
        if (shouldLerp)
        {
            if (getDir == "right")
            {
                if (transform.localPosition.x <= (endPosition.x - 2.5f))
                {
                    transform.localPosition += new Vector3(500 * Time.deltaTime, 0, 0);

                    if (transform.localPosition.x == (endPosition.x - 2.5f))
                    {
                        shouldLerp = false;
                    }

                }
            }
            if (getDir == "left")
            {
                if (transform.localPosition.x >= (endPosition.x + 2.5f))
                {
                    transform.localPosition -= new Vector3(500 * Time.deltaTime, 0, 0);

                    if (transform.localPosition.x == (endPosition.x + 2.5f))
                    {
                        shouldLerp = false;
                    }

                }
            }
            if (getDir == "up")
            {
                if (transform.localPosition.y <= (endPosition.y - 2.5f))
                {
                    transform.localPosition += new Vector3(0, 500 * Time.deltaTime, 0);

                    if (transform.localPosition.y == (endPosition.y - 2.5f))
                    {
                        shouldLerp = false;
                    }

                }
            }
            if (getDir == "down")
            {
                if (transform.localPosition.y >= (endPosition.y + 2.5f))
                {
                    transform.localPosition -= new Vector3(0, 500 * Time.deltaTime, 0);

                    if (transform.localPosition.y == (endPosition.y + 2.5f))
                    {
                        shouldLerp = false;
                    }

                }
            }


        }

    }

}
