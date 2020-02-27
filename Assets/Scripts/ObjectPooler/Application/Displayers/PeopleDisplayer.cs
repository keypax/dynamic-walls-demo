using CustomCamera.Domain;
using Map.Domain;
using People.Domain;
using UnityEngine;

namespace ObjectPooler.Application.Displayers
{
    /**
     * Display people within camera visibility
     *
     * People are moving, so even camera stands still, we need to update people positions.
     * Updating displayed people positions are done in "PersonObjectPoolingComponent"
     *
     * Adding component "PersonObjectPoolingComponent" to "Person" object inform script that person is already displayed.
     */
    public class PeopleDisplayer : AObjectPoolerDisplayer
    {
        private const string Prefix = "person_";

        private PersonList _personList;

        public PeopleDisplayer(ObjectPoolerManager objectPoolerManager, PersonList personList) : base(
            objectPoolerManager)
        {
            _personList = personList;
        }

        public override bool IsDynamic()
        {
            return true;
        }

        public override void Display(int minX, int maxX, int minY, int maxY, TerrainPositionsFromCameraBoundaries terrainPositionsFromCameraBoundaries)
        {
            GameObject go;

            foreach (Person person in _personList.People)
            {
                if (terrainPositionsFromCameraBoundaries.IsInsidePolygon(person.Position2D))
                {
                    //object is not displayed yet
                    if (!person.PersonObjectPoolingComponent)
                    {
                        go = _objectPoolerManager.SpawnFromPool(
                            Prefix + person.PersonType,
                            person.Position,
                            person.Rotation
                        );

                        person.PersonObjectPoolingComponent = go.GetComponent<PersonObjectPoolingComponent>();
                        person.PersonObjectPoolingComponent.SetPerson(person);
                    }
                }
                else
                {
                    //hide object
                    if (person.PersonObjectPoolingComponent)
                    {
                        _objectPoolerManager.ReleaseBackToPool(Prefix + person.PersonType, person.PersonObjectPoolingComponent.gameObject);

                        person.PersonObjectPoolingComponent.UnsetPerson();
                        person.PersonObjectPoolingComponent = null;
                    }
                }
            }
        }
    }
}