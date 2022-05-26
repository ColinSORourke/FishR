using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Answer found: https://forum.unity.com/threads/scroll-elements-with-only-the-scrollbar.475617/ 
public class ScrollDerived : ScrollRect
{
    public override void OnDrag(PointerEventData eventData)
    {
        //  base.OnDrag(eventData);
        // print("Trying drag...but nothing happened.. Woah!? :)"); // definitely delete this when you get to using it :)
    }

    public override void OnScroll(PointerEventData eventData)
    {
        //  base.OnScroll(eventData);
        // print("Trying drag...but nothing happened.. Woah!? :)"); // definitely delete this when you get to using it :)
    }
}
