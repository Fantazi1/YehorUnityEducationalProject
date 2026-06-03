using UnityEngine;

public class AnimationActivationOnObject : MonoBehaviour
{
    void Start()
    {
        Animation animation = GetComponent<Animation>();
        animation.Play();
        animation.PlayQueued("Armature_Armature_SpikeTrap_HideAnimation_BaseLayer_LEGACY", QueueMode.CompleteOthers);
    }
}
