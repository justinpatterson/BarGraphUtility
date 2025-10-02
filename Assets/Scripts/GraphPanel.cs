using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prisms.Assignment {
    public class GraphPanel : UIPanel
    {
        public GraphController controller;
        public Transform nodeContainer;
        public GameObject nodePrefab;
        
        public GameObject placeholderNode;
        Dictionary<int, LayoutNode> _layoutNodes = new Dictionary<int, LayoutNode>();

        int _maxVal = -1;
        public int MaxVal { get { return _maxVal; } }

        [System.Serializable]
        public struct GraphSubPanel { public GraphController.GraphModes mode; public UIPanel panel; }
        public GraphSubPanel[] subpanels;

        protected override void OpenPanel()
        {
            base.OpenPanel();
            InitializePanel();
        }
        public override void SetPanelActivity(bool isActive, bool force = false)
        {
            
            base.SetPanelActivity(isActive, force);

            if (isActive && _isActive)
            {
                GraphModeHandler();
            }
        }
        public virtual void InitializePanel() 
        {
            if (controller == null) return;

            GraphDataModel.GraphDataElement[] transformedData = GetTransformedDataModel();

            RefreshLimits(transformedData);
            InstantiateNodes(transformedData);
            
            //GraphModeHandler();
        }

        protected virtual GraphDataModel.GraphDataElement[] GetTransformedDataModel() 
        {
            return controller.graphDataModel.data;
        }

        public virtual void ResetPanel() 
        {
            Debug.Log("Reset pressed.");
            foreach(int nodeIndex in _layoutNodes.Keys) 
            {
                _layoutNodes[nodeIndex].transform.SetSiblingIndex(nodeIndex);
            }
        }


        protected virtual void GraphModeHandler() 
        {
            switch (controller.graphMode)
            {
                case GraphController.GraphModes.Standard:
                    if (Time.unscaledTime < 1f) //basically a do-once; I'm hitting a classic Canvas edge case quirk if I do things too fast at startup, due to auto size behaviors 
                    {
                        ResetPanel();
                    }
                    else
                        ShiftBarSeries(_layoutNodes.Values.ToList());
                    break;
                case GraphController.GraphModes.Mode:
                    //I have a separate panel for this -- see GraphPanel_MODE
                    break;
                case GraphController.GraphModes.Median:
                    //if median, reorder in ascending
                    List<LayoutNode> sorted = _layoutNodes.Values.OrderBy(s => s.nodeBar.dataElement.value).ToList();
                    ShiftBarSeries(sorted);
                    break;
                case GraphController.GraphModes.Mean:
                    break;
                default:
                    //otherwise, don't change behavior
                    break;
            }

            foreach(GraphSubPanel gsp in subpanels) 
            {
                Debug.Log("Activate " + gsp.panel.name + "? " + (controller.graphMode == gsp.mode).ToString());
                gsp.panel.SetPanelActivity(controller.graphMode == gsp.mode, true);
            }
        }

        protected virtual void RefreshLimits(GraphDataModel.GraphDataElement[] inData) 
        {
            for (int i = 0; i < inData.Length; i++)
            {
                if(inData[i].value > _maxVal) 
                {
                    _maxVal = inData[i].value;
                }
            }
        }
        protected virtual void InstantiateNodes(GraphDataModel.GraphDataElement[] inData) 
        {
            if (_layoutNodes.Count > 0)
            {
                ResetPanel();
                return;
            }
            for(int i = 0; i < inData.Length; i++) 
            {
                GameObject inst = Instantiate(nodePrefab, nodeContainer);
                inst.name = string.Format("Node_{0}_{1}", inData[i].index, inData[i].value);
                LayoutNode layoutInst = inst.GetComponent<LayoutNode>();
                if (layoutInst)
                {
                    layoutInst.AssignData(this, inData[i]);
                    _layoutNodes.Add(i,layoutInst);
                }
            }
        }

        public virtual void SetPlaceholderNode(bool isActive, LayoutNode node) 
        {
            if(isActive)
                placeholderNode.transform.SetSiblingIndex(node.transform.GetSiblingIndex());
            else 
                node.transform.SetSiblingIndex(placeholderNode.transform.GetSiblingIndex());
            placeholderNode.SetActive(isActive);
        }

        public virtual void ReportNodeDrag(LayoutNode node) 
        {
            int targetIndex = GetKeyForLayoutNode(node);
            if(targetIndex != -1) 
            {
                LayoutNode closestNode = GetClosestNodeToBar(node);
                if(closestNode != node) 
                {
                    Debug.Log("Should shift to: " + closestNode.gameObject.name);
                    ShiftBars(node, closestNode);
                }
            }
            else { Debug.Log("Invalid Node Value."); }
        }
        protected virtual int GetKeyForLayoutNode(LayoutNode node) 
        {
            foreach (int i in _layoutNodes.Keys)
            {
                if (node == _layoutNodes[i])
                    return i;
            }
            return -1;
        }
        protected virtual LayoutNode GetClosestNodeToBar(LayoutNode node)
        {
            int placeholderSiblingIndex = placeholderNode.transform.GetSiblingIndex();

            int closestSiblingIndex = -1;
            float closestDistance = -1f;

            for (int i = -2; i <= 2; i++) //placeholder placed at a spot between curr node and left/right, so we can't rely on +/-1; probably a cuter way to do this
            {
                int currSiblingIndex = placeholderSiblingIndex + i;
                
                if(currSiblingIndex >= 0 && currSiblingIndex < placeholderNode.transform.parent.childCount) 
                { 
                    Transform currNode = placeholderNode.transform.parent.GetChild(currSiblingIndex);
                    if (currNode != node.transform)
                    {
                        float currDistance = Vector3.Distance(node.transform.position, currNode.transform.position);
                        if (currDistance < closestDistance || closestSiblingIndex == -1)
                        {
                            closestDistance = currDistance;
                            closestSiblingIndex = currSiblingIndex;
    
                        }
                    }
                }
            }

            if (closestSiblingIndex == placeholderSiblingIndex)
                return node;
            else
                return placeholderNode.transform.parent.GetChild(closestSiblingIndex).GetComponent<LayoutNode>();

        }
        protected virtual bool ShiftBars(LayoutNode from, LayoutNode to) 
        {
            to.InitShift(); //Decouple NodeBar
            placeholderNode.transform.SetSiblingIndex(to.transform.GetSiblingIndex());
            return true;
        }

        protected virtual bool ShiftBarSeries(List<LayoutNode> orderedNodes)
        {
            for (int i = 0; i < orderedNodes.Count; i++)
            {
                orderedNodes[i].InitShift();
            }
            for (int i = 0; i < orderedNodes.Count; i++)
                orderedNodes[i].transform.SetSiblingIndex(i);

            return true;
        }
    }
}