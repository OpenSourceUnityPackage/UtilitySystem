using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilitySystem
{

    public class UtilitySystem : MonoBehaviour
    {
        [SerializeField]
        List<Desire> actions = new List<Desire>();
        public List<Desire> Desires
        {
            get { return actions; }
        }


        [SerializeField]
        Blackboard bb;

        Desire currentDesire = null;

        public Desire CurrentDesire
        {
            get { return currentDesire; }
        }



        private void Start()
        {
            if (currentDesire != null)
            {
                currentDesire.StartDesire();
            }
        }

        private void Update()
        {
            float highestUtility = 0f;
            Desire newDesire = null;
            foreach (Desire action in actions)
            {
                float utility = action.EvaluateUtility(bb);
                if (utility > highestUtility || (newDesire != null && utility == highestUtility && action.priority > newDesire.priority))
                {
                    newDesire = action;
                    highestUtility = utility;
                }
            }

            if (newDesire == null)
            {
                newDesire = actions[Random.Range(0, actions.Count)];
            }

            if (newDesire != currentDesire)
            {
                if (currentDesire != null)
                    currentDesire.EndDesire();

                currentDesire = newDesire;
                currentDesire.StartDesire();
            }

            // Call update atleast 1 time, even if StartDesire() has been called during the same frame
            currentDesire.UpdateDesire();
        }
    }

}