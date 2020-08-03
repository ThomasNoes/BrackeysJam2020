// This script should be located at the head of the vacuum cleaner.
namespace Assets.Scripts.Interaction.Vacuum
{
    using UnityEngine;
    using Assets.Scripts.Test;

    [RequireComponent(typeof(SphereCollider))]
    public class VacuumSource : MonoBehaviour, IPower, ITester
    {
        // Public:
        public bool powered = false, isSucking = false; // NOTE: These are currently public for testing purposes
        [Range(0.5f, 20.0f)]public float suckPowerLevel = 10.0f;
        [Tooltip("In degrees")] public float effectiveAngle = 90.0f;
        [Tooltip("In meters")] public float maximumDistance = 7.0f;

        // Private:
        private SphereCollider _interactionSphere;
        private int _layerMask;

        private void Start()
        {
            _interactionSphere = GetComponent<SphereCollider>();
            _interactionSphere.isTrigger = true;
            _interactionSphere.center = transform.position;
            _interactionSphere.radius = maximumDistance;

            _layerMask = LayerMask.GetMask("UI");
            _layerMask |= LayerMask.GetMask("Ignore Raycast");
            _layerMask = ~_layerMask;
        }

        private void OnTriggerStay(Collider col)
        {
            if (isSucking)
                DoSuck(col.gameObject);
        }

        private void ToggleSuck(bool isOn)
        {
            isSucking = isOn;
        }

        private void DoSuck(GameObject obj)
        {
            ISuckable suckable = obj.GetComponent<ISuckable>();

            if (suckable != null)
            {
                if (AngleCheck(gameObject, obj))
                {
                    var tempDistance = Vector3.Distance(transform.position, obj.transform.position);
                    suckable.Suck(transform.position, tempDistance * suckPowerLevel);
                }
            }
        }

        private bool AngleCheck(GameObject from, GameObject to)
        {

            Vector3 dir = to.transform.position - from.transform.position;
            float vacuumToSuckableAngle = Vector3.Angle(from.transform.forward, dir);

            if (vacuumToSuckableAngle < effectiveAngle / 2)
            {
                RaycastHit hit;

                if (Physics.Raycast(from.transform.position, dir, out hit, maximumDistance + 2.0f, _layerMask))
                {
                    if (hit.collider.gameObject == to)
                        return true;
                }
            }

            return false;
        }

        public void PowerOn()
        {
            powered = true;
        }

        public void PowerOff()
        {
            powered = false;
        }

        public void StartTest()
        {
            ToggleSuck(true);
        }

        public void StopTest()
        {
            ToggleSuck(false);
        }
    }
}