using UnityEngine;

public class PlayerTorchInteractor : MonoBehaviour
{
    public float interactionRange = 2f;

    public void CheckIfNearTorch()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange);
        foreach (var hit in hits)
        {
            var torch = hit.GetComponent<Torch>();
            if (torch != null && !torch.isLit)
                torch.LightUp();
        }
    }
}
