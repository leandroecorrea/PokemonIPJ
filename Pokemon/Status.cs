using System;
public enum NonVolatileStatus { Burn, Freeze, Paralysis, Poison, BadPoison, Sleep, Focus, None }
[Flags]
public enum VolatileStatus { None = 0, Confusion = 1, Curse = 2, Charge = 4, DefenseBoost = 8, Leer = 16, TailWhip = 32, Rage = 64}