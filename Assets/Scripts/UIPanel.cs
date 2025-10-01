using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Prisms.Assignment
{
    public class UIPanel : MonoBehaviour
    {
        bool _isActive = false;
        public bool IsActive { get { return _isActive; } }
        public CanvasGroup canvasGroup;

        public void SetPanelActivity(bool isActive, bool force = false) 
        {
            if (_isActive == isActive && !force) return;

            if (isActive) 
                OpenPanel();
            else 
                ClosePanel();
        }
        protected virtual void OpenPanel() 
        {
            _isActive = true;
            Debug.Log("Opening " + this.gameObject.name);
            if (canvasGroup!= null)
            {
                canvasGroup.interactable = true;
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            }

        }
        protected virtual void ClosePanel() 
        {
            _isActive = false;
            if (canvasGroup!= null)
            {
                canvasGroup.interactable = false;
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }
}