using R3;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private ReactiveProperty<bool> _onDrag = new ReactiveProperty<bool>();
        
    public ReadOnlyReactiveProperty<bool> OnDrag => _onDrag;
    
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        _onDrag.Value = true;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        _onDrag.Value = false;
        base.OnPointerUp(eventData);
    }
}