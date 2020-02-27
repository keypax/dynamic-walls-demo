using System.Collections.Generic;
using People.Domain;

namespace Map.Domain
{
    /**
     * List of all people on the map
     */
    public class PersonList
    {
        public List<Person> People { get; }

        public PersonList()
        {
            People = new List<Person>();
        }
    }
}