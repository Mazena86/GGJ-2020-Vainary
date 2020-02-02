using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public GameObject logo;
    public GameObject selection;

    private Drop logoDropper;
    private Drop selectionDropper;

    private void Awake() 
    {
        logoDropper = logo.GetComponent<Drop>();
        selectionDropper = selection.GetComponent<Drop>();

        if(logoDropper == null)
        {
            Debug.Log("Logo Missing a Drop component!");
        }
        else if(selectionDropper == null)
        {
            Debug.Log("Selection Missing a Drop component!");
        }
        else
        {
            logoDropper.Close(true, true);
            selectionDropper.Close(true, true);
        }
    }

    private void Start() 
    {
        StartCoroutine(IntroAnimation());
    }

    IEnumerator IntroAnimation()
    {
        yield return new WaitForSecondsRealtime(1);
        logoDropper.DropIn(true, true);
        yield return new WaitForSecondsRealtime(2);
        logoDropper.DropOut(true, false);
        selectionDropper.DropIn(true, false);

    }
}