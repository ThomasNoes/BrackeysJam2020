using UnityEngine;

public class FollowParent : MonoBehaviour
{
    public bool scaleColliderToParent, followCustomObj;
    public GameObject customObj;

    private GameObject _parentObject;

    void Start()
    {
        _parentObject = transform.parent.gameObject;

        if (followCustomObj && customObj != null)
            _parentObject = customObj;

        if (scaleColliderToParent)
        {
            if (GetComponent<BoxCollider>() != null)
                GetComponent<BoxCollider>().size = _parentObject.transform.localScale;
            else if (GetComponent<SphereCollider>() != null)
                GetComponent<SphereCollider>().radius = _parentObject.transform.localScale.x;
        }
    }

    void LateUpdate()
    {
        if (_parentObject != null)
            transform.position = _parentObject.transform.position;
    }
}
