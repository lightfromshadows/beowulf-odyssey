using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TownManager : MonoBehaviour {

    public Sun sun;
    public GameObject blackLayer;
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
    private SpriteRenderer fadeSprite;

    // Use this for initialization
    void Start () {
        playerStats.Init();

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
            endDayCoroutine = StartCoroutine(EndDay());
            day = false;
        }
        else
        {

            sun.DayTimeTween = dayTime / MAX_DAY_TIME;
        }


    }

    public IEnumerator EndDay()
    {
        //Run as a coroutine?
        //Fade
        Debug.Log("Fading");
        yield return new WaitForSeconds(3);
        Color fadeVal = new Color(0, 0, 0, 0.8f);
        fadeSprite.color = fadeVal;
 
        //Show result text. (Including failure)
        //Sound
        //Text saying who was killed
        //Fade in
        dayTime = 0;
        day = true;
    }

    public void HouseClicked(House house)
    {
        //Draw sprite and three options.
        //Show buttons

        currentHouse = house;
    }

    public void ChoiceClicked(int choice)
    {
        playerStats.AddBuff(currentHouse.person.buff);
        currentHouse.person.visited = true;
        //Call EndDay?
        //Show result text on that screen
    }

    private void CreateHouses()
    {
        House housePreFab = Resources.Load<House>("House");
        House house;

        Vector2 pos = new Vector2();

        for (int personCount = 0; personCount < people.Length; personCount++)
        {
            house = Instantiate(housePreFab);
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
