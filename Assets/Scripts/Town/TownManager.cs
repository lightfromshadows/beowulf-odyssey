using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TownManager : MonoBehaviour {

    public Sun sun;
    public GameObject blackLayer;
    public GameObject textBacking;
    public GameObject description;
    public GameObject[] choiceTexts;
    private const float MAX_DAY_TIME = 10;
    private float dayTime = 0;
    private House currentHouse;

    private const int HOUSE_X_MIN = 100;
    private const int HOUSE_X_MAX = 1700;
    private const int HOUSE_X_WOBBLE = 15;
    private const int HOUSE_Y_MIN = 410;
    private const int HOUSE_Y_MAX = 680;


    [SerializeField] CharacterStatsObj playerStats;
    [SerializeField] TownsPerson[] people;

    List<House> houses = new List<House>();

    private Coroutine endDayCoroutine;
    bool day = true;
    bool madeChoice = false;
    private SpriteRenderer fadeSprite;
    Color fadeVal;

    // Use this for initialization
    void Start () {
        playerStats.Init();

        ClearTextElements();
        CreateHouses();
        AssignPeopleToHouses();
        fadeSprite = blackLayer.GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {

        if (!day)
            return;

        dayTime += Time.deltaTime;

        if(dayTime >= MAX_DAY_TIME)
        {
            if(!madeChoice)
            {
                endDayCoroutine = StartCoroutine(EndDay(-1));
                day = false;
            }
        }
        else
        {
            //Set light fading.
            float a = 0;
            if(dayTime/MAX_DAY_TIME < 0.5)
            {
                a = 0.75f - dayTime / MAX_DAY_TIME * 1.5f;
            }
            else
            {
                a = (dayTime-MAX_DAY_TIME/2f) / MAX_DAY_TIME*1.5f;
            }
            fadeVal = new Color(0, 0, 0, a);
            fadeSprite.color = fadeVal;
            sun.DayTimeTween = dayTime / MAX_DAY_TIME;
        }


    }

    public IEnumerator EndDay(int choice)
    {

        //Show result text. (Including failure)
        ClearTextElements();
        if(choice < 0)
        {
            //Show the text with no good result.
        }
        else
        {
            choiceTexts[0].GetComponent<Text>().text = currentHouse.person.choice[choice];
            choiceTexts[1].GetComponent<Text>().text = currentHouse.person.responses[choice];
            choiceTexts[1].GetComponent<Text>().color = new Color(1, 0, 0);
        }

        yield return new WaitForSeconds(5); //Time to read the result
        textBacking.SetActive(false); 
        day = false;
        ClearTextElements();
        fadeVal = new Color(0, 0, 0, 1f); 
        fadeSprite.color = fadeVal;
        //Sound
        //Text saying who was killed
        yield return new WaitForSeconds(5);

        dayTime = 0;
        day = true;
        madeChoice = false;
        textBacking.SetActive(true);

    }

    public void HouseClicked(House house)
    {

        description.GetComponent<Text>().text = house.person.description;

        for(int i=0; i< choiceTexts.Length; i++)
        {
            choiceTexts[i].GetComponent<Text>().text = house.person.choice[i];
        }

        currentHouse = house;

    }


    public void ChoiceClicked(int choice)
    {
        if (madeChoice)
            return;

        currentHouse.person.visited = true;

        if(choice == currentHouse.person.chooseWisely)
        {
            Debug.Log("You chose wisely");
            playerStats.AddBuff(currentHouse.person.buff);
        }
        else
        {
            Debug.Log("You did not choose wisely");
        }

        madeChoice = true;
        endDayCoroutine = StartCoroutine(EndDay(choice));

    }

    public void ClearTextElements()
    {
        description.GetComponent<Text>().text = "";

        for (int i = 0; i < choiceTexts.Length; i++)
        {
            Text t = choiceTexts[i].GetComponent<Text>();
            t.text = "";
            t.color = new Color(1, 1, 1);
        }

    }

    private void CreateHouses()
    {
        House housePreFab = Resources.Load<House>("House");
        House house;

        Vector2 pos = new Vector2();

        for (int personCount = 0; personCount < people.Length; personCount++)
        {
            house = Instantiate(housePreFab);
            if (Random.Range(0, 2) == 1)
                house.GetComponent<SpriteRenderer>().flipX = true;
            house.town = this;
            pos.x = (HOUSE_X_MAX - HOUSE_X_MIN) / people.Length * personCount + HOUSE_X_MIN;
            pos.x += Random.Range(-HOUSE_X_WOBBLE, HOUSE_X_WOBBLE);
            pos.y = Random.Range(HOUSE_Y_MIN, HOUSE_Y_MAX);
            house.transform.position = pos;
            houses.Add(house);
        }
    }

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
