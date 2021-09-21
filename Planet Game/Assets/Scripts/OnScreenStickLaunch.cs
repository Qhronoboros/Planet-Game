using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.OnScreen
{
    [AddComponentMenu("Input/On-Screen Stick")]
    public class OnScreenStickLaunch : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public GameObject joystick;
        public Vector3 joystickPos = Vector3.zero;

        public void OnPointerDown(PointerEventData eventData)
        {
            joystick.SetActive(true);

            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));

            joystickPos = eventData.position;
            Debug.Log(joystickPos/2);
            joystick.transform.position = joystickPos + Vector3.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_PointerDownPos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
            var delta = position - m_PointerDownPos;

            delta = Vector2.ClampMagnitude(delta, movementRange);
            joystick.transform.position = joystickPos + new Vector3(delta.x, delta.y, 0);

            var newPos = delta/movementRange;
            SendValueToControl(newPos);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            joystick.SetActive(false);
            joystick.GetComponent<RectTransform>().anchoredPosition = joystickPos / 2;
            SendValueToControl(Vector2.zero);
        }

        private void Start()
        {
            //m_StartPos = ((RectTransform)transform).anchoredPosition;
        }

        public float movementRange
        {
            get => m_MovementRange;
            set => m_MovementRange = value;
        }

        [FormerlySerializedAs("movementRange")]
        [SerializeField]
        private float m_MovementRange = 50;

        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;

        private Vector3 m_StartPos;
        private Vector2 m_PointerDownPos;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }
    }
}
