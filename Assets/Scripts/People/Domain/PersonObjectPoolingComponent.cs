using UnityEngine;

namespace People.Domain
{
    /**
     * If person is displayed, check if GameObject position or rotation are the same as in Person object.
     * If not, update position/rotation
     */
    public class PersonObjectPoolingComponent : MonoBehaviour
    {
        private Person _person;
        
        public void Update()
        {
            if (_person == null)
            {
                return;
            }

            if (transform.position != _person.Position)
            {
                transform.position = _person.Position;
            }
            
            if (transform.rotation != _person.Rotation)
            {
                transform.rotation = _person.Rotation;
            }
        }

        public void SetPerson(Person person)
        {
            _person = person;
        }

        public void UnsetPerson()
        {
            _person = null;
        }
    }
}