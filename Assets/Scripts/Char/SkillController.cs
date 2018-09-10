using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : Photon.PunBehaviour
{
    public Button.ButtonClickedEvent qSkillStart;
    public Button.ButtonClickedEvent qSkillCancel;

    public Button.ButtonClickedEvent wSkillStart;
    public Button.ButtonClickedEvent wSkillCancel;

    public Button.ButtonClickedEvent eSkillStart;
    public Button.ButtonClickedEvent eSkillCancel;

    public Button.ButtonClickedEvent rSkillStart;
    public Button.ButtonClickedEvent rSkillCancel;

    private KeyCode castingSpell;
    private BasicAttackController basicAttackController;

    private void Start()
    {
        basicAttackController = GetComponent<BasicAttackController>();
    }

    void Update ()
    {
        if (photonView.isMine)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                basicAttackController.CancelAttack();
                CancelAll();
                qSkillStart.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                basicAttackController.CancelAttack();
                CancelAll();
                wSkillStart.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                basicAttackController.CancelAttack();
                CancelAll();
                eSkillStart.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                basicAttackController.CancelAttack();
                CancelAll();
                rSkillStart.Invoke();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                CancelAll();
            }
        }
    }

    private void CancelAll()
    {
        qSkillCancel.Invoke();
        wSkillCancel.Invoke();
        eSkillCancel.Invoke();
        rSkillCancel.Invoke();
    }
}
