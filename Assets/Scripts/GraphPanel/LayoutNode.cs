using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Prisms.Assignment
{
    public class LayoutNode : Graphic, IMoveable
    {
        public NodeBarView nodeBar;
        public LayoutElement nodeLayout;
        GraphPanel _panel;

        public void AssignData(GraphPanel panel, GraphDataModel.GraphDataElement data) 
        {
            _panel = panel;    
            nodeBar.dataElement = data;
            nodeBar.RefreshView(_panel.MaxVal );
        }

        public void Click(Vector3 ScreenPos)
        {
            this.raycastTarget = false;
            //nodeLayout.ignoreLayout = true;


        }

        public void Drag(Vector3 WorldPos)
        {
            //Oh, the joys of funky Transformations to auto-scaled World Space elements
            //Vector3 localPos = nodeBar.transform.InverseTransformPoint(WorldPos);
            //nodeBar.transform.localPosition = localPos;
            
            nodeBar.transform.position = WorldPos;
            nodeBar.transform.localPosition = new Vector3(nodeBar.transform.localPosition.x, 0f, nodeBar.transform.localPosition.z); //this is more consistent
            
            _panel.ReportNodeDrag(this);
        }

        public void Release(Vector3 ScreenPos)
        {
            nodeBar.transform.localPosition = Vector3.zero;
            //nodeLayout.ignoreLayout = false;
            this.raycastTarget = true;
        }
    }
}
