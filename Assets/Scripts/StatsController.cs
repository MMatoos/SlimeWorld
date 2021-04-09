using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{
    private GameObject player;
    public int attack = 20;
    public int hp = 100;
    public float speed = 40f;
    public float jump = 600f;
    public Text attackContainer;
    public Text hpContainer;
    public Text speedContainer;
    public Text jumpContainer;
    public int savedAttack;
    public int savedHp;
    public float savedSpeed;
    public float savedJump;

    private bool isActive = false;
    public GameObject statsUI;
    private EditorController _editorController;

    private void Start()
    {
        savedAttack = attack;
        savedHp = hp;
        savedSpeed = speed;
        savedJump = jump;
        _editorController = GetComponent<EditorController>();
    }

    public void StatsButton()
    {
        statsUI.SetActive(!isActive);
        isActive = !isActive;
        attackContainer.text = savedAttack.ToString();
        hpContainer.text = savedHp.ToString();
        speedContainer.text = savedSpeed.ToString();
        jumpContainer.text = savedJump.ToString();
    }

    public void QuitStatsButton()
    {
        statsUI.SetActive(false);
        isActive = false;
    }

    public void RightAttackButton()
    {
        savedAttack++;
        attackContainer.text = savedAttack.ToString();
    }

    public void LeftAttackButton()
    {
        if (savedAttack != 0)
            savedAttack--;
        attackContainer.text = savedAttack.ToString();
    }
    
    public void RightHpButton()
    {
        savedHp += 10;
        hpContainer.text = savedHp.ToString();
    }

    public void LeftHpButton()
    {
        if (savedHp != 10)
            savedHp -= 10;
        hpContainer.text = savedHp.ToString();
    }
    
    public void RightSpeedButton()
    {
        savedSpeed++;
        speedContainer.text = savedSpeed.ToString();
    }

    public void LeftSpeedButton()
    {
        if (savedSpeed != 0)
            savedSpeed--;
        speedContainer.text = savedSpeed.ToString();
    }
    
    public void RightJumpButton()
    {
        savedJump += 10;
        jumpContainer.text = savedJump.ToString();
    }

    public void LeftJumpButton()
    {
        if (savedJump != 0)
            savedJump -= 10;
        jumpContainer.text = savedJump.ToString();
    }

    public void SaveStats()
    {
        _editorController.GetComponent<EditorController>().savedGame.attack = savedAttack;
        _editorController.GetComponent<EditorController>().savedGame.hp = savedHp;
        _editorController.GetComponent<EditorController>().savedGame.jump = savedJump;
        _editorController.GetComponent<EditorController>().savedGame.speed = savedSpeed;
    }

    public void LoadStats()
    {
        savedAttack = _editorController.savedGame.attack;
        savedHp = _editorController.savedGame.hp;
        savedSpeed = _editorController.savedGame.speed;
        savedJump = _editorController.savedGame.jump;
        attackContainer.text = savedAttack.ToString();
        hpContainer.text = savedHp.ToString();
        speedContainer.text = savedSpeed.ToString();
        jumpContainer.text = savedJump.ToString();
    }

    public void InsertStats()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject healthController = GameObject.FindGameObjectWithTag("Health Controller");
        player.GetComponent<CharacterAttack2D>().damage = _editorController.savedGame.attack;
        healthController.GetComponent<HealthController>().playerHealth = _editorController.savedGame.hp;
        player.GetComponent<CharacterMovement2D>().speed = _editorController.savedGame.speed;
        player.GetComponent<CharacterController2D>().m_JumpForce = _editorController.savedGame.jump;
    }
    
    
}
