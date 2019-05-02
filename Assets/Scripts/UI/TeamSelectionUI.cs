using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[DefaultExecutionOrder(200)]
public class TeamSelectionUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] HeroButton prefabHeroButton;
    [SerializeField] Button selectLevelButton;

    [SerializeField] List<HeroButton> teamASlots;
    [SerializeField] List<HeroButton> teamBSlots;
    
    HeroButton selectedHeroButton = null;
    HeroButton selectedSlot = null;

    List<HeroButton> heroButtons = new List<HeroButton>();

    // Start is called before the first frame update
    void Start()
    {
        // Create all hero buttons that wait to be selected
        foreach (HeroInfo info in DataManager.instance.heroes.heroes)
        {
            HeroButton b = Instantiate(prefabHeroButton, container);
            b.info = info;

            b.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Clicked on hero");
                selectedHeroButton = b;

            });

            heroButtons.Add(b);
        }



        foreach (HeroButton button in teamASlots)
        {
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectedSlot = button;
            });
        }

        foreach (HeroButton button in teamBSlots)
        {
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectedSlot = button;
            });
        }
    }

    public void ResetTeams()
    {
        selectedSlot = null;
        selectedHeroButton = null;
        for (int i = 0; i < container.childCount; i++)
        {
            HeroButton button = container.GetChild(i).GetComponent<HeroButton>();
            if(button != null)
            {
                button.team = -1;
                button.selected = false;
            }
        }

        foreach(HeroButton button in teamASlots)
        {
            button.info = null;
        }

        foreach (HeroButton button in teamBSlots)
        {
            button.info = null;
        }
    }

    private void Update()
    {
        if(selectedHeroButton != null && selectedSlot != null)
        {
            if(selectedSlot.info != null)
            {
                HeroButton button = GetButtonWithHero(selectedSlot.info);
                if(button != null)
                {
                    button.team = -1;
                    button.selected = false;
                }
            }


            selectedSlot.info = selectedHeroButton.info;
            selectedHeroButton.team = selectedSlot.team;
            selectedHeroButton.selected = true;


            // reset selection
            selectedSlot = null;
            selectedHeroButton = null;

            selectLevelButton.interactable = GetTeamA().Count > 0 && GetTeamB().Count > 0;
        }
    }

    HeroButton GetButtonWithHero(HeroInfo _info)
    {
        HeroButton button = null;

        foreach(HeroButton b in heroButtons)
        {
            if(b.info == _info)
            {
                button = b;
                break;
            }
        }

        return button;
    }

    public void SetHeroesToSpawn()
    {
        DataManager.instance.heroToSpawn.Clear();
        
        DataManager.instance.heroToSpawn.AddRange(GetTeamA().Select(
            x => new HeroTeam() {
                team = 0,
                info = x
            }));

        DataManager.instance.heroToSpawn.AddRange(GetTeamB().Select(
            x => new HeroTeam()
            {
                team = 1,
                info = x
            }));
    }

    public List<HeroInfo> GetTeamA()
    {
        List<HeroInfo> infos = new List<HeroInfo>();

        foreach (HeroButton button in teamASlots)
        {
            if (button != null && button.info != null)
            {
                infos.Add(button.info);
            }
        }
        return infos;
    }


    public List<HeroInfo> GetTeamB()
    {
        List<HeroInfo> infos = new List<HeroInfo>();

        foreach (HeroButton button in teamBSlots)
        {
            if (button != null && button.info != null)
            {
                infos.Add(button.info);
            }
        }
        return infos;
    }
}
