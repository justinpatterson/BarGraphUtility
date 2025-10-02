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
            nodeLayout.ignoreLayout = true;
            _panel.SetPlaceholderNode(true,this);

        }

        public void Drag(Vector3 WorldPos)
        {
            //Oh, the joys of funky Transformations to auto-scaled World Space elements
            //Vector3 localPos = nodeBar.transform.InverseTransformPoint(WorldPos);
            //nodeBar.transform.localPosition = localPos;
            
            transform.position = WorldPos;
            transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z); //this is more consistent
            
            _panel.ReportNodeDrag(this);
        }

        public void Release(Vector3 ScreenPos)
        {
            //nodeBar.transform.localPosition = Vector3.zero;
            nodeLayout.ignoreLayout = false;
            this.raycastTarget = true;
            _panel.SetPlaceholderNode(false,this);
        }

        Coroutine _shiftRoutine;
        public void InitShift() 
        {
            if (_shiftRoutine != null)
                StopCoroutine(_shiftRoutine);

            nodeBar.transform.SetParent(_panel.transform, true);
            _shiftRoutine = StartCoroutine(  ShiftRoutine(this.transform.GetSiblingIndex()) );
        }
        IEnumerator ShiftRoutine(int startSiblingIndex) 
        {
            float timeOut = Time.unscaledTime + 0.5f; 
            while (startSiblingIndex == this.transform.GetSiblingIndex() && (Time.unscaledTime < timeOut)) yield return new WaitForEndOfFrame();
            nodeBar.transform.SetParent(this.transform, true);
            float progress = 0f;
            Vector3 startWorldPos = nodeBar.transform.position;
            float snapSpeed = 5f;

            while (progress < 1f) 
            {
                progress+= Time.deltaTime*snapSpeed;
                nodeBar.transform.position = Vector3.Lerp(startWorldPos, this.transform.position, progress);
                yield return new WaitForEndOfFrame();
            }
            nodeBar.transform.localPosition = Vector3.zero;
            _shiftRoutine = null;
        }
    }
}
