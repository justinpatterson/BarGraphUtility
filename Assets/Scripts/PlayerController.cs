using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Note - basic event data setup was inspired by https://discussions.unity.com/t/how-to-raycast-onto-a-unity-canvas-ui-image/782699/4

namespace Prisms.Assignment
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] GraphicRaycaster _Raycaster;
        PointerEventData _PointerEventData;
        [SerializeField] EventSystem _EventSystem;
        [SerializeField] RectTransform _CanvasRect;

        IMoveable _activeMoveable;
        IMoveable _currMoveable;

        public enum ControlModes { Idle, Click, Drag, Release }
        public ControlModes currentMode = ControlModes.Idle;

        void Update()
        {
            _PointerEventData = new PointerEventData(_EventSystem);
            _PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            _Raycaster.Raycast(_PointerEventData, results);

            switch (currentMode)
            {
                case ControlModes.Idle:
                    

                    if (results.Count > 0)
                    {
                        _currMoveable  =  results[0].gameObject.GetComponent<IMoveable>();
                        //Debug.Log("Hovering Over: " + results[0].gameObject.name);
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0) && _currMoveable != null)
                    {
                        if(_activeMoveable != null) 
                        {
                            //need to do some release feature
                        }
                        currentMode = ControlModes.Click;
                    }
                    break;
                case ControlModes.Click:
                    if (_currMoveable != null)
                    {
                        _activeMoveable = _currMoveable;
                        _activeMoveable.Click(Input.mousePosition);
                        currentMode = ControlModes.Drag;
                    }
                    else
                        currentMode = ControlModes.Idle;
                    break;
                case ControlModes.Drag:
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        currentMode = ControlModes.Release;
                    }
                    else
                    {
                        if(results.Count>0)
                            _activeMoveable.Drag(results[0].worldPosition); //screen pos gets weird with world canvases
                    }
                    break;
                case ControlModes.Release:
                    _activeMoveable.Release(Input.mousePosition);
                    _activeMoveable = null;
                    currentMode = ControlModes.Idle;
                    break;
            }

            

            
        }
    }
}
