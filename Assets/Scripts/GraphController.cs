using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Prisms.Assignment
{
    public class GraphController : MonoBehaviour
    {
        public GraphDataModel graphDataModel;
        
        public enum GraphModes { INIT, Standard, Mode, Median, Mean }
        public GraphModes graphMode = GraphModes.Standard;

        [System.Serializable]
        public struct GraphPanelInfo
        {
            public GraphModes graphMode;
            public UIPanel panel;
            public Button tabButton;
            public GameObject activeMarker;
        }
        [SerializeField]
        GraphPanelInfo[] panels;

        private void Awake()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].tabButton != null)
                {
                    panels[i].tabButton.onClick.RemoveAllListeners();
                    GraphModes target = panels[i].graphMode;
                    panels[i].tabButton.onClick.AddListener(() => { GraphTransition( target ); });
                }
            }
            GraphTransition(GraphModes.Standard);
        }

        public void GraphTransition(GraphModes nextMode) 
        {
            UIPanel pFrom = GetPanel(graphMode);
            UIPanel pTo = GetPanel(nextMode);
            graphMode = nextMode;
            if (pFrom != pTo)
            {
                pTo?.SetPanelActivity(true);
                pFrom?.SetPanelActivity(false);
            }
            else { 
                Debug.Log("Refreshing current active panel.");
                pTo?.SetPanelActivity(true);
            }
            RefreshMarkers();
        }

        void RefreshMarkers() 
        {
            foreach(GraphPanelInfo gpi in panels) 
            {
                bool markActive = gpi.graphMode == graphMode;
                if( gpi.graphMode == GraphModes.Standard) 
                {
                    markActive |= graphMode == GraphModes.Median;
                    markActive |= graphMode == GraphModes.Mean;
                }
                gpi.activeMarker.SetActive( markActive );
            }
        }

        UIPanel GetPanel(GraphModes mode) 
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].graphMode == mode) return panels[i].panel;
            }
            return null;
        }

        public float CalculateMean() 
        {
            int sum = 0;
            foreach(GraphDataModel.GraphDataElement gde in graphDataModel.data) 
            {
                sum += gde.value;
            }
            float avg = (float)sum / (float) graphDataModel.data.Length;
            return avg;
        }
        
        public float CalculateMedian()
        {
            List<GraphDataModel.GraphDataElement> sorted = graphDataModel.data.OrderBy(s => s.value).ToList();
            int count = sorted.Count;
            int middleIndex = count / 2;

            if (count % 2 == 0)
            {
                // Even number of elements, take the average of the two middle elements
                return (sorted[middleIndex - 1].value + sorted[middleIndex].value) / 2.0f;
            }
            else
            {
                // Odd number of elements, take the middle element
                return sorted[middleIndex].value;
            }
        }
    }



    
}
