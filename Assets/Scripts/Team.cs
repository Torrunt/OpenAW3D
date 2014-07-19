using System.Collections.Generic;

public class Team
{
	public int TeamNo = -1;

	public int Resources = 8000;
	public List<Unit> Units = new List<Unit>();
	public List<Building> Buildings = new List<Building>();

	private const int INCOME_PER_BUILDING = 3000;

	public void GainIncome()
	{
		Resources += INCOME_PER_BUILDING * Buildings.Count;
	}

	public void ResetUnits()
	{
		for (int i = 0; i < Units.Count; i++)
		{
			Units[i].Reset();
		}
		for (int i = 0; i < Buildings.Count; i++)
		{
			Buildings[i].Reset();
		}
	}

	public void HealUnitsInCities()
	{
		for (int i = 0; i < Units.Count; i++)
		{
			if (Units[i].GetHitPoints() != 10 && Units[i].BuildingOn != null && Units[i].BuildingOn.Team == Units[i].Team)
			{
				if (Units[i].BuildingOn.Type == Building.CITY)
					Units[i].Heal(1);
				else if (Units[i].BuildingOn.Type == Building.BASE)
					Units[i].Heal(2);
			}
		}
	}
	public void HealUncontestedBuildings()
	{
		for (int i = 0; i < Buildings.Count; i++)
		{
			if (Buildings[i].GetHitPoints() < Buildings[i].GetHitPointsMax() && (Buildings[i].UnitOnTop == null || Buildings[i].UnitOnTop.Team == TeamNo))
				Buildings[i].Heal(2);
		}
	}
}