using Oculus.Interaction;
using UnityEngine;

public class TestRayInteractor : MonoBehaviour
{
    public GameObject[] leftInteractors;
    public GameObject[] rightInteractors;
    public GameObject leftRayInteractor;
    public GameObject rightRayInteractor;
    public MeshRenderer leftReticle;
    public MeshRenderer rightReticle;

    private void Update()
    {
        //Left hand
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (leftReticle.enabled)
            {
                leftRayInteractor.SetActive(false);
                SetInteractors(leftInteractors, true);
            }
            else
            {
                leftRayInteractor.SetActive(true);
                SetInteractors(leftInteractors, false);
            }
        }
        else
        {
            leftRayInteractor.SetActive(true);
            SetInteractors(leftInteractors, false);
        }
        //Right hand
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (rightReticle.enabled)
            {
                rightRayInteractor.SetActive(true);
                SetInteractors(rightInteractors, false);
            }
            else
            {
                rightRayInteractor.SetActive(false);
                SetInteractors(rightInteractors, true);
            }
        }
        else
        {
            rightRayInteractor.SetActive(true);
            SetInteractors(rightInteractors, true);
        }
    }
    private void SetInteractors(GameObject[] interactors, bool isActive)
    {
        for (int i = 0; i < interactors.Length; i++)
        {
            interactors[i].SetActive(isActive);
        }
    }
}