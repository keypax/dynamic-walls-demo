using System;
using Buildings.Domain;

namespace Buildings.Application
{
    public class BuildingAreaGetter
    {
        private BuildingsTypesList _buildingsTypesList;

        public BuildingAreaGetter(BuildingsTypesList buildingsTypesList)
        {
            _buildingsTypesList = buildingsTypesList;
        }
        
        public BuildingArea Get(IBuilding building)
        {
            return new BuildingArea
            (
                GetMinX((short) building.Position2D.x, building),
                GetMaxX((short) building.Position2D.x, building),
                GetMinY((short) building.Position2D.y, building),
                GetMaxY((short) building.Position2D.y, building)
            );
        }
        
        private short GetMinX(short positionX, IBuilding building)
        {
            switch (building.Rotation)
            {
                case 0:
                    return positionX;
                case 1:
                    return positionX;
                case 2:
                    return (short) (positionX - GetLength(building) + 1);
                case 3:
                    return (short) (positionX - GetHeight(building) + 1);
            }
            
            throw new Exception("No rotation: " + building.Rotation);
        }
        
        private short GetMaxX(short positionX, IBuilding building)
        {
            switch (building.Rotation)
            {
                case 0:
                    return (short) (positionX + GetLength(building));
                case 1:
                    return (short) (positionX + GetHeight(building));
                case 2:
                    return (short) (positionX + 1);
                case 3:
                    return (short) (positionX + 1);
            }
            
            throw new Exception("No rotation: " + building.Rotation);
        }

        private short GetMinY(short positionY, IBuilding building)
        {
            switch (building.Rotation)
            {
                case 0:
                    return positionY;
                case 1:
                    return (short) (positionY - GetLength(building) + 1);
                case 2:
                    return (short) (positionY - GetHeight(building) + 1);
                case 3:
                    return positionY;
            }
            
            throw new Exception("No rotation: " + building.Rotation);
        }
        
        private short GetMaxY(short positionY, IBuilding building)
        {
            switch (building.Rotation)
            {
                case 0:
                    return (short) (positionY + GetHeight(building));
                case 1:
                    return (short) (positionY + 1);
                case 2:
                    return (short) (positionY + 1);
                case 3:
                    return (short) (positionY + GetLength(building));
            }
            
            throw new Exception("No rotation: " + building.Rotation);
        }

        private byte GetLength(IBuilding building)
        {
            return _buildingsTypesList.GetByType(building.BuildingType).Length;
        }
        
        private byte GetHeight(IBuilding building)
        {
            return _buildingsTypesList.GetByType(building.BuildingType).Height;
        }
    }
}