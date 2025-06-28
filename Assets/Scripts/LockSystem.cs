using UnityEngine;
using UnityEngine.UI;

public class LockSystem : MonoBehaviour
{
    private int[] correctPassword = new int[4];
    private int[] inputPassword = new int[4];

    [SerializeField] private Text[] digitTexts; // 4자리 숫자 텍스트
    [SerializeField] private Button[] upButtons;
    [SerializeField] private Button[] downButtons;
    [SerializeField] private Button checkButton;
    public Image lockImage;             // 락 오브젝트의 Image 컴포넌트
    public Sprite lockedSprite;         // 자물쇠 잠김 이미지
    public Sprite unlockedSprite;       // 자물쇠 풀림 이미지

    void Start()
    {
        GeneratePassword();

        // 버튼 이벤트 연결
        for (int i = 0; i < 4; i++)
        {
            int index = i;
            upButtons[i].onClick.AddListener(() => ChangeDigit(index, +1));
            downButtons[i].onClick.AddListener(() => ChangeDigit(index, -1));
        }

        checkButton.onClick.AddListener(CheckPassword);
        UpdateDigitTexts();
    }

    void Update()
    {

    }
    void GeneratePassword()
    {
        for (int i = 0; i < 4; i++)
        {
            correctPassword[i] = Random.Range(0, 10);
        }

        Debug.Log($"정답 비밀번호: {string.Join("", correctPassword)}");
    }

    void ChangeDigit(int index, int delta)
    {
        inputPassword[index] = (inputPassword[index] + delta + 10) % 10;
        UpdateDigitTexts();
    }

    void UpdateDigitTexts()
    {
        for (int i = 0; i < 4; i++)
        {
            digitTexts[i].text = inputPassword[i].ToString();
        }
    }

    void CheckPassword()
    {
        for (int i = 0; i < 4; i++)
        {
            if (inputPassword[i] != correctPassword[i])
            {
                return;
            }
        }
        PasswordCorrect();
    }

    public void PasswordCorrect()
    {
        Debug.Log("비밀번호가 일치합니다!");
        if (lockImage != null && unlockedSprite != null)
        {
            lockImage.sprite = unlockedSprite;
        }
    }
    
}