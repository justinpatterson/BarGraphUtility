using System.Collections;
using System.Collections.Generic;
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

        protected override void OpenPanel()
        {
            base.OpenPanel();
            InitializePanel();
        }

        public virtual void InitializePanel() 
        {
            if (controller == null) return;

            GraphDataModel.GraphDataElement[] transformedData = GetTransformedDataModel();

            RefreshLimits(transformedData);
            InstantiateNodes(transformedData);
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
    }
}