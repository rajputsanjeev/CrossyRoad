using CrossyRoad;

using UnityEngine;

public class UIPanelComponent : View
{
    public Panel panelType;

    public void Show(bool on)
    {
       
            this.gameObject.SetActive(on);
        
    }

    public virtual void CloseCurrentPanel()
    {
        
    }
}


