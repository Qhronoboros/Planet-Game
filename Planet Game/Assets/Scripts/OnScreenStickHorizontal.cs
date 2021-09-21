using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UI;

namespace UnityEngine.InputSystem.OnScreen
{
    [AddComponentMenu("Input/On-Screen Stick")]
    public class OnScreenStickHorizontal : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public GameObject joystick;
        public GameObject darkBlob;
        public Vector3 joystickPos = Vector3.zero;
        public Vector3 originalPos = Vector3.zero;
        public float transparencyDrag = 0.6f;
        public float transparencyRelease = 0.3f;

        public void OnPointerDown(PointerEventData eventData)
        {
            changeTransparency(transparencyDrag);

            darkBlob.SetActive(true);

            //joystick.SetActive(true);

            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));

            joystickPos = eventData.position;
            joystick.transform.position = joystickPos + Vector3.zero;
            darkBlob.transform.position = joystick.transform.position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_PointerDownPos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
            var delta = position - m_PointerDownPos;

            delta = Vector2.ClampMagnitude(delta, movementRange);
            joystick.transform.position = joystickPos + new Vector3(delta.x, 0, 0);

            var newPos = new Vector2(delta.x / movementRange, 0);
            SendValueToControl(newPos);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            changeTransparency(transparencyRelease);
            //joystick.SetActive(false);
            joystick.transform.position = originalPos;
            darkBlob.transform.position = joystick.transform.position;
            darkBlob.SetActive(false);
            SendValueToControl(Vector2.zero);
        }

        private void Start()
        {
            originalPos = joystick.transform.position;
            darkBlob.transform.position = joystick.transform.position;
            darkBlob.SetActive(false);
            changeTransparency(transparencyRelease);
            //m_StartPos = ((RectTransform)transform).anchoredPosition;
        }

        public void changeTransparency(float transparency)
        {
            Color color = joystick.GetComponent<Image>().color;
            color.a = transparency;
            joystick.GetComponent<Image>().color = color;
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
