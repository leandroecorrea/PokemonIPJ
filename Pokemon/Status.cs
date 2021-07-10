using System;
public enum NonVolatileStatus { Burn, Freeze, Paralysis, Poison, BadPoison, Sleep, None }
[Flags]
public enum VolatileStatus { None = 0, Confusion = 1, Curse = 2, Charge = 4, Reflect = 8}