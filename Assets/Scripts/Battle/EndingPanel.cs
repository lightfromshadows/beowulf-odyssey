using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class EndingPanel : MonoBehaviour {

    [SerializeField] string loseMessage = "You have been slain";
    [SerializeField] string winMessage = "You are victorious";

    [SerializeField] TownsPerson[] townsPeople;
    [SerializeField] Text titleText;
    [SerializeField] Text endingText;

    [SerializeField] CharacterStatsObj playerStats;
    [SerializeField] CharacterStatsObj wolfStats;

    private void OnEnable()
    {

        if (wolfStats.Health <= 0f)
        {
            PlayerWins();
        }
        else {
            WolfWins();
        }

    }

    void PlayerWins()
    {
        var livePeople = from person in townsPeople where !person.dead select person;

        switch (livePeople.Count())
        {
            case 10:
                endingText.text = "Congrats you saved the village! Zero casualties! Hold your head high, call yourself a hero, and hope the towns people don’t burn you at the stake for witchcraft.";
                break;
            case 9:
                endingText.text = "Only one person died! Give yourself a pat on the back and a prayer to the church, they were getting old anyway.";
                break;
            case 8:
                endingText.text = "Two people died – that’s 80% a passing grade! Run and tell your friends that you’re not mediocre – a true almost perfect victory.";
                break;
            case 7:
                endingText.text = "Three people are dead… maybe not your best work but better than some. Just think about it, if you hadn’t shown up the werewolf might have been killed by a competent hero.";
                break;
            case 6:
                endingText.text = "Congrats! You defeated the werewolf at the cost of four human lives@ Hope you’re proud of yourself.";
                break;
            case 5:
                endingText.text = "50%, no one should be happy about 50%.";
                break;
            case 4:
                endingText.text = "Well… at least you’re alive? Guess that means something.";
                break;
            case 3:
                endingText.text = "You defeated the werewolf but your grief weighs on you shoulders as you pass the empty houses in shame.";
                break;
            case 2:
                endingText.text = "After defeating the werewolf you cannot bring yourself to celebrate with the remaining towns people. You can hear their wails already.";
                break;
            case 1:
                endingText.text = "At least there’s one left to bury what’s left of the villagers. Their bones and flesh collected fit only in one coffin. You don’t help. The village has nothing more to give you.";
                break;
            case 0:
                endingText.text = "To be determined";
                break;
        }

        titleText.text = winMessage;
    }

    void WolfWins()
    {
        string[] endings = new string[]
        {
            "You died! Throw yourself a party!",
            "Dying is the only way you learn.",
            "Did you hear a fly when you died, or was that just me?",
            "The village grave yard is not for you. The ground you fall upon is where you stay."
        };

        endingText.text = endings[Random.Range(0, endings.Length)];
        titleText.text = loseMessage;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
