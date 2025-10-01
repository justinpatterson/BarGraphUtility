using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prisms.Assignment {
    public class GraphPanel : MonoBehaviour
    {
        public GraphController controller;
        public Transform nodeContainer;
        public GameObject nodePrefab;

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

        Dictionary<int,LayoutNode> _layoutNodes = new Dictionary<int,LayoutNode>();
        void InstantiateNodes() 
        {
            //TODO: probably would want to deal with old layoutnodes if we actually saw any
            _layoutNodes.Clear();

            for(int i = 0; i < controller.graphDataModel.data.Length; i++) 
            {
                GameObject inst = Instantiate(nodePrefab, nodeContainer);
                inst.name = string.Format("Node_{0}", i);
                LayoutNode layoutInst = inst.GetComponent<LayoutNode>();
                if (layoutInst)
                {
                    layoutInst.AssignData(this, controller.graphDataModel.data[i]);
                    _layoutNodes.Add(i,layoutInst);
                }
            }
        }

        public void ReportNodeDrag(LayoutNode node) 
        {
            int targetIndex = GetKeyForLayoutNode(node);
            if(targetIndex != -1) 
            {
                LayoutNode closestNode = GetClosestNodeToBar(targetIndex, node.nodeBar);
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
        LayoutNode GetClosestNodeToBar(int nodeIndex, NodeBarView nodeBarView) 
        {
            int closestIndex = nodeIndex;
            float closestDistance = Vector3.Distance(nodeBarView.transform.position, _layoutNodes[closestIndex].transform.position);
            int indexOffset = (nodeIndex) - _layoutNodes[nodeIndex].transform.GetSiblingIndex(); //Sibling index shifts as objects slide around
            /*
            for(int i = -1; i <= 1; i++) 
            {
                if(nodeIndex + i + indexOffset >= 0 && nodeIndex + i + indexOffset < _layoutNodes.Count) 
                {
                    float testDistance = Vector3.Distance(nodeBarView.transform.position, _layoutNodes[nodeIndex + i + indexOffset].transform.position);
                    if(testDistance < closestDistance) 
                    {
                        closestIndex = (nodeIndex + i + indexOffset);
                    }
                }
            }
            */

            foreach(int i in _layoutNodes.Keys) 
            {
                float testDistance = Vector3.Distance(nodeBarView.transform.position, _layoutNodes[i].transform.position);
                if (testDistance < closestDistance)
                {
                    closestIndex = i;
                }

            }

            return _layoutNodes[closestIndex];
        }
        bool ShiftBars(LayoutNode from, LayoutNode to) 
        {
            int fromIndex = GetKeyForLayoutNode(from);
            int toIndex = GetKeyForLayoutNode(to);

            int fromSibling = from.transform.GetSiblingIndex();
            int toSibling = to.transform.GetSiblingIndex();
            from.transform.SetSiblingIndex(toSibling);

            return true;
        }
        bool SwapNodeBars(LayoutNode from, LayoutNode to) 
        {

            /*
            var fromBar = from.nodeBar;
            var toBar = to.nodeBar;
            from.nodeBar = toBar;
            to.nodeBar = fromBar;
            */
            return true;
        }
    }
}