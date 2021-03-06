﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;


public class TownManager : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip upgradeSound;
    public AudioClip doorKnockSound;
    public AudioClip doorSlamSound;
    public Sun sun;
    public SkyGradient sky;
    public GameObject blackLayer;
    public GameObject textBacking;
    public GameObject graveyard;
    public GameObject tpName;
    public GameObject description;
    public GameObject[] choiceTexts;
    public GameObject overnightText;
    public GameObject rewardText;
    public GameObject townspersonPic;

    [SerializeField] CharacterStatsObj playerStats;
    [SerializeField] TownsPerson[] people;


    private const float MAX_DAY_TIME = 20;
    private float dayTime = 0;
    private House currentHouse;

    private const int HOUSE_X_MIN = 100;
    private const int HOUSE_X_MAX = 1700;
    private const int HOUSE_X_WOBBLE = 15;
    private const int HOUSE_Y_MIN = 410;
    private const int HOUSE_Y_MAX = 550;

    public List<House> houses;// = new List<House>();

    bool day = true;
    bool madeChoice = false;
    bool waitForNewHouse = false;
    private SpriteRenderer fadeSprite;
    Color fadeVal;

    private int nextDeadString=0;

    private string[] nightStrings =
    {
        "All good things must come to an end and so day passes into night.",
        "The sun is struck from the sky and the moon mourns her brother’s departure.",
        "MOON!!!",
        "This is when the inverse of a rooster crows to alert that night has fallen."
    };

    private string[] deadStrings =
    {
        "Nobody answers.",
        "The house is dark, the door is open, everything is destroyed inside. You can’t bring yourself to enter.",
        "Check the graveyard.",
        "The door hangs open, the smell of a successful hunt emenates from within."
    };

    private string[] endDayStrings =
    {
        "You’ve wasted the day. Night falls. You hear a wolf howl in the distance.",
        "The day has ended."
    };


    // Use this for initialization
    void Start ()
    {
        playerStats.Init();

        ClearTextElements();
        //CreateHouses(); //Now creating houses by hand.
        AssignPeopleToHouses();
        ResetTownsfolk();       //This reset is required because the assets are persistent between runs.
        fadeSprite = blackLayer.GetComponent<SpriteRenderer>();
        textBacking.SetActive(false);
        ClearTownspersonPic();
    }
	
	// Update is called once per frame
	void Update () {

        if (!day)
            return;

        dayTime += Time.deltaTime;

        //TODO: Check if all angry
        //If so, speed up time to 3x as fast?

        if(dayTime >= MAX_DAY_TIME)
        {
            if(!madeChoice)
            {
                StartCoroutine(EndDay(-1));
                day = false;
            }
        }
        else
        {
            //Set light fading.
            float a = 0;
            if(dayTime/MAX_DAY_TIME < 0.5)  //Caps it from 0.5 darkening to start, with none at noon.
            {
                a = 0.5f - dayTime / MAX_DAY_TIME;
            }
            else
            {
                a = (dayTime-MAX_DAY_TIME/2f) / MAX_DAY_TIME;
            }
            a = a * 0.5f; //Was still too dark, so quick hack to lower the darkness.
            fadeVal = new Color(0, 0, 0, a);
            fadeSprite.color = fadeVal;
            sun.DayTimeTween = dayTime / MAX_DAY_TIME;
            sky.SetSkyColor(dayTime / MAX_DAY_TIME);
        }


    }

    public IEnumerator EndDay(int choice)
    {
        
        if(choice < 0)
        {
            ClearTextElements();
            ClearTownspersonPic();
            description.GetComponent<Text>().text = ChooseRandomStringFromArray(endDayStrings);
        }

        var livePeople = from person in people where !person.dead select person; //Everyone dead but one, go to the fight.


        textBacking.SetActive(true);        //Needed when player timed out without having clicked on anything.
        yield return new WaitForSeconds(8); //Time to read the result
        if (livePeople.Count() == 1)
        {
            SceneManager.LoadScene("Battle");
            yield break;
        }
        graveyard.SetActive(false);
        ClearTownspersonPic();
        textBacking.SetActive(false); 
        day = false;
        ClearTextElements();
        fadeVal = new Color(0, 0, 0, 1f); 
        fadeSprite.color = fadeVal;
        Text ont = overnightText.GetComponent<Text>();
        ont.text = ChooseRandomStringFromArray(nightStrings);
        yield return new WaitForSeconds(4);                     //Let them read the night string.
        TownsPerson p = KillVillager();
        ont.text = ont.text + "\n\nThe werewolf has killed the " + p.name + ".";
        yield return new WaitForSeconds(4);                     //Let them read who died.

        //Reset for new day.
        dayTime = 0;
        day = true;
        madeChoice = false;
        ClearTextElements();
        ResetVisitedTownsfolk();
        graveyard.SetActive(true);

    }

    public void MonsterAttack()
    {
        if (madeChoice || !day)
            return;

        SceneManager.LoadScene("Battle");
    }

    public void HouseClicked(House house)
    {
        if (madeChoice || !day)
            return;

        ClearTextElements();
        textBacking.SetActive(true);

        SpriteRenderer tpRend = townspersonPic.GetComponent<SpriteRenderer>();
        tpRend.sprite = house.person.sprite;
 
        if(house.person.dead)
        {
            currentHouse = null;
            description.GetComponent<Text>().text = ChooseNextDeadString();
            tpRend.sprite = null;
            audioSource.PlayOneShot(doorKnockSound);
            return;
        }

        if (house.person.gaveBoon)
        {
            currentHouse = null;
            tpName.GetComponent<Text>().text = house.person.name;
            description.GetComponent<Text>().text = "There is nothing more this person can do for you.";
            audioSource.PlayOneShot(doorSlamSound);
            return;
        }

        if(house.person.visited)
        {
            currentHouse = null;
            tpName.GetComponent<Text>().text = house.person.name;
            description.GetComponent<Text>().text = "Your knocking is met by a yell. GO AWAY!";
            audioSource.PlayOneShot(doorSlamSound);
            return;
        }

        tpName.GetComponent<Text>().text = house.person.name;
        townspersonPic.GetComponent<SpriteRenderer>().sprite = house.person.sprite;
        description.GetComponent<Text>().text = house.person.description;
        waitForNewHouse = false;

        for(int i=0; i< choiceTexts.Length; i++)
        {
            choiceTexts[i].GetComponent<Text>().text = house.person.choice[i];
        }

        currentHouse = house;
        audioSource.PlayOneShot(doorKnockSound);
    }


    public void ChoiceClicked(int choice)
    {
        if (madeChoice || waitForNewHouse) //Prevent clicks mattering on result text.
            return;

        currentHouse.person.visited = true;

        ClearTextElements();
        tpName.GetComponent<Text>().text = currentHouse.person.name;
        description.GetComponent<Text>().text = currentHouse.person.responses[choice];

        if(choice == currentHouse.person.chooseWisely)
        {
            var deadPeople = from person in people where person.dead && !person.gaveBoon select person;

            if (currentHouse.person.name == "GraveRobber" && deadPeople.Count() > 0 && Random.Range(0, 1) == 0)
            {
                //Handle taking stuff from the dead person.
                TownsPerson p = deadPeople.ElementAt(Random.Range(0, deadPeople.Count()));
                playerStats.AddBuff(p.buff);
                p.gaveBoon = true;
                rewardText.GetComponent<Text>().text = "You received: " + p.reward + " (gross)";

                //But also handle the grave robber.
                currentHouse.person.gaveBoon = true;

            }
            else
            {
                playerStats.AddBuff(currentHouse.person.buff);
                currentHouse.person.gaveBoon = true;
                rewardText.GetComponent<Text>().text = "You received: " + currentHouse.person.reward;
            }
            audioSource.PlayOneShot(upgradeSound);
            madeChoice = true;
            StartCoroutine(EndDay(choice));
        }
        else
        {
            waitForNewHouse = true;
        }

    }

    public void Highlight(int choice)
    {
        if (madeChoice || waitForNewHouse)
            return;

        choiceTexts[choice].GetComponent<Text>().color = new Color(0, 1, 0);
    }

    public void UnHighlight(int choice)
    {
        if (madeChoice || waitForNewHouse)
            return;

        choiceTexts[choice].GetComponent<Text>().color = new Color(1, 1, 1);
    }

    public TownsPerson KillVillager()
    {
        int leftAlive = 0;
        foreach(House h in houses)
        {
            if (!h.person.dead)
                leftAlive++;
        }

        int killNum = Random.Range(0, leftAlive);

        foreach(House h in houses)
        {
            if(!h.person.dead)
            {
                if(killNum == 0)
                {
                    h.person.dead = true;
                    return h.person;
                }
                killNum--;
            }
        }

        return null; //This is an error; shouldn't happen.
    }

    string ChooseRandomStringFromArray(string[] choices)
    {
        return choices[Random.Range(0, choices.Length)];
    }

    string ChooseNextDeadString()
    {
        nextDeadString++;
        if (nextDeadString == deadStrings.Count())
            nextDeadString = 0;

        return deadStrings[nextDeadString];
    }

    public void ResetVisitedTownsfolk()
    {
        foreach(House h in houses)
        {
            h.person.visited = false;
        }
    }

    public void ResetTownsfolk()
    { 
        foreach(House h in houses)
        {
            h.person.dead = false;
            h.person.visited = false;
            h.person.gaveBoon = false;
        }
    }
    
    public void ClearTownspersonPic()
    {
        SpriteRenderer tpRend = townspersonPic.GetComponent<SpriteRenderer>();
        tpRend.sprite = null;
    }

    public void ClearTextElements() //Resets contents and color.
    {
        tpName.GetComponent<Text>().text = "";
        description.GetComponent<Text>().text = "";
        rewardText.GetComponent<Text>().text = "";
        overnightText.GetComponent<Text>().text = "";

        for (int i = 0; i < choiceTexts.Length; i++)
        {
            Text t = choiceTexts[i].GetComponent<Text>();
            t.text = "";
            t.color = new Color(1, 1, 1);
        }

    }

    //Now creating houses by hand.
    //......................................................
    //private void CreateHouses()
    //{
    //    House housePreFab = Resources.Load<House>("House");
    //    House house;

    //    Vector2 pos = new Vector2();

    //    for (int personCount = 0; personCount < people.Length; personCount++)
    //    {
    //        house = Instantiate(housePreFab);
    //        if (Random.Range(0, 2) == 1)
    //            house.GetComponent<SpriteRenderer>().flipX = true;
    //        house.town = this;
    //        pos.x = (HOUSE_X_MAX - HOUSE_X_MIN) / people.Length * personCount + HOUSE_X_MIN;
    //        pos.x += Random.Range(-HOUSE_X_WOBBLE, HOUSE_X_WOBBLE);
    //        pos.y = Random.Range(HOUSE_Y_MIN, HOUSE_Y_MAX);
    //        house.transform.position = pos;
    //        houses.Add(house);
    //    }
    //}

    private void AssignPeopleToHouses()
    {
        //Fill list with numbers, used to index into the TownsPeople array.
        //Each time one is chosen, it will be removed from the list.
        List<int> choiceIndexes = new List<int>(); 
        for (int i = 0; i < people.Length; i++)
        {
            choiceIndexes.Add(i);
        }

        for (int personCount = 0; personCount < people.Length; personCount++)
        {
            int choiceIndex = choiceIndexes[Random.Range(0, choiceIndexes.Count)];
            houses[personCount].person = people[choiceIndex];
            choiceIndexes.Remove(choiceIndex);
        }
    }
}
