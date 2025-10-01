using System.Collections;
using System.Collections.Generic;
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
            
            if (pFrom != pTo)
            {
                pTo?.SetPanelActivity(true);
                pFrom?.SetPanelActivity(false);
            }

            else { Debug.Log("Cant transition to same panel."); }
            graphMode = nextMode;
        }

        UIPanel GetPanel(GraphModes mode) 
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].graphMode == mode) return panels[i].panel;
            }
            return null;
        }

    }



    
}
