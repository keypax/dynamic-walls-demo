using Map.Domain;
using People.Domain;
using UnityEngine;

namespace People.Application
{
    public class PeopleManager : MonoBehaviour
    {
        private PersonList _personList;

        public void Init(PersonList personList)
        {
            _personList = personList;
        }
        
        public void Update()
        {
            foreach (Person person in _personList.People)
            {
                if (person.PersonMode == PersonMode.Idle)
                {
                    if (Random.Range(1, 10) == 1)
                    {
                        person.PersonMode = PersonMode.Walking;
                        person.Destination2D = person.Position2D + Random.insideUnitCircle * 25;
                    }
                }

                if (person.PersonMode == PersonMode.Walking)
                {
                    if (Vector3.Distance(person.Position2D, person.Destination2D) < 0.2f)
                    {
                        person.PersonMode = PersonMode.Idle;
                    }

                    person.Position = Vector3.MoveTowards(person.Position, new Vector3(person.Destination2D.x, person.Position.y, person.Destination2D.y), 5 * Time.deltaTime);
                    person.Position2D = new Vector2(person.Position.x, person.Position.z);
                }
            }
        }
    }
}