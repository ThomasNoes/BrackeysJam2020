// This script should be located at the head of the vacuum cleaner.
namespace Assets.Scripts.Interaction.Vacuum
{
    using UnityEngine;
    using Assets.Scripts.Test;
    using System.Collections;
    using System.Collections.Generic;

    [RequireComponent(typeof(SphereCollider))]
    public class VacuumSource : MonoBehaviour, IPower, ITester
    {
        // Public:
        public bool powered = false, isSucking = false; // NOTE: These are currently public for testing purposes
        [Range(0.5f, 25.0f)]public float suckPowerLevel = 15.0f;
        [Tooltip("In degrees")] public float effectiveAngle = 90.0f;
        [Tooltip("In meters")] public float maximumDistance = 15.0f;
        [Tooltip("In meters. The distance between the vacuum source and object to suck it into the machine")] public float eatDistance = 0.5f;
        public List<GameObject> eatenObjects; // Public for testing purposes, make private later

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

            eatenObjects = new List<GameObject>();
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
            IEatable eatable = obj.GetComponent<IEatable>();

            var tempDistance = Vector3.Distance(transform.position, obj.transform.position);

            if (eatable != null)
            {
                if (tempDistance < eatDistance)
                {
                    GameObject tempObj = eatable.Eat();

                    if (tempObj != null)
                    {
                        eatenObjects.Add(tempObj);
                        tempObj.SetActive(false);
                        return;
                    }
                }
            }

            if (suckable != null)
            {
                if (AngleCheck(gameObject, obj))
                {
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