using System;
using System.Collections.Generic;
using System.Text;
public enum ElementalTypes { Normal, Fighting, Flying, Poison, Ground, Rock, Bug, Ghost, Fire, Water, Grass, Electric, Psychic, Ice, Dragon, Error }
public class Types
{
	private static Dictionary <string, ElementalTypes> typesDatabase = new Dictionary<string, ElementalTypes>();
	private static Types instance;

	public static Types GetInstance()
	{
		if (instance == null)
		{
			instance = new Types();
		}
		return instance;
	}
	private Types()
	{
		typesDatabase.Add("Normal", ElementalTypes.Normal);
		typesDatabase.Add("Fighting", ElementalTypes.Fighting);
		typesDatabase.Add("Flying", ElementalTypes.Flying);
		typesDatabase.Add("Poison", ElementalTypes.Poison);
		typesDatabase.Add("Ground", ElementalTypes.Ground);
		typesDatabase.Add("Rock", ElementalTypes.Rock);
		typesDatabase.Add("Bug", ElementalTypes.Bug);
		typesDatabase.Add("Ghost", ElementalTypes.Ghost);
		typesDatabase.Add("Fire", ElementalTypes.Fire);
		typesDatabase.Add("Water", ElementalTypes.Water);
		typesDatabase.Add("Grass", ElementalTypes.Grass);
		typesDatabase.Add("Electric", ElementalTypes.Electric);
		typesDatabase.Add("Psychic", ElementalTypes.Psychic);
		typesDatabase.Add("Ice", ElementalTypes.Ice);
		typesDatabase.Add("Dragon", ElementalTypes.Dragon);
	}

	public ElementalTypes GetType(string typeName)
	{
		if (typesDatabase.ContainsKey(typeName))
		{
			return typesDatabase[typeName];
		}
		else
		{
			Console.WriteLine("Error!");
			return ElementalTypes.Error;
		}
	}
}