using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prisms.Assignment {
    public class GraphPanel : MonoBehaviour
    {
        public GraphController controller;
        public Transform nodeContainer;
        public GameObject nodePrefab;
        
        public GameObject placeholderNode;
        Dictionary<int, LayoutNode> _layoutNodes = new Dictionary<int, LayoutNode>();

        int _maxVal = -1;
        public int MaxVal { get { return _maxVal; } }

        private void Awake()
        {
            InitializePanel();
        }
        public void InitializePanel() 
        {
            if (controller == null) return;
            RefreshLimits();
            InstantiateNodes();
        }
        public void ResetPanel() 
        {
            foreach(int nodeIndex in _layoutNodes.Keys) 
            {
                _layoutNodes[nodeIndex].transform.SetSiblingIndex(nodeIndex);
            }
        }

        void RefreshLimits() 
        {
            for (int i = 0; i < controller.graphDataModel.data.Length; i++)
            {
                if(controller.graphDataModel.data[i].value > _maxVal) 
                {
                    _maxVal = controller.graphDataModel.data[i].value;
                }
            }
        }

        void InstantiateNodes() 
        {
            //TODO: probably would want to deal with old layoutnodes if we actually saw any
            _layoutNodes.Clear();

            for(int i = 0; i < controller.graphDataModel.data.Length; i++) 
            {
                GameObject inst = Instantiate(nodePrefab, nodeContainer);
                inst.name = string.Format("Node_{0}_{1}", controller.graphDataModel.data[i].index, controller.graphDataModel.data[i].value);
                LayoutNode layoutInst = inst.GetComponent<LayoutNode>();
                if (layoutInst)
                {
                    layoutInst.AssignData(this, controller.graphDataModel.data[i]);
                    _layoutNodes.Add(i,layoutInst);
                }
            }
        }

        public void SetPlaceholderNode(bool isActive, LayoutNode node) 
        {
            if(isActive)
                placeholderNode.transform.SetSiblingIndex(node.transform.GetSiblingIndex());
            else 
                node.transform.SetSiblingIndex(placeholderNode.transform.GetSiblingIndex());
            placeholderNode.SetActive(isActive);
        }

        public void ReportNodeDrag(LayoutNode node) 
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
        int GetKeyForLayoutNode(LayoutNode node) 
        {
            foreach (int i in _layoutNodes.Keys)
            {
                if (node == _layoutNodes[i])
                    return i;
            }
            return -1;
        }
        LayoutNode GetClosestNodeToBar(LayoutNode node)
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
                            Debug.Log("Closest Sibling is " + currSiblingIndex);
                        }
                    }
                }
            }

            if (closestSiblingIndex == placeholderSiblingIndex)
                return node;
            else
                return placeholderNode.transform.parent.GetChild(closestSiblingIndex).GetComponent<LayoutNode>();

        }
        bool ShiftBars(LayoutNode from, LayoutNode to) 
        {
            placeholderNode.transform.SetSiblingIndex(to.transform.GetSiblingIndex());
            return true;
        }
    }
}