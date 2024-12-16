
using UnityEngine;
using TMPro;

public class TMPColorManager : MonoBehaviour
{
    private TMP_Text m_Text;
    [SerializeField]
    private Color onColor, offColor;
    private void Awake()
    {
        m_Text = GetComponent<TMP_Text>();
    }
    public void ChangeColor(bool isOn)
    {
        if (isOn)
        {
            m_Text.color = onColor;
        }
        else
        {
            m_Text.color = offColor;
        }
    }
}
