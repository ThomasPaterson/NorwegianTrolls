using UnityEngine;
using System.Collections;

public class StatusCard : Card 
{
	public enum Status {Slow, Haste, Heal}

	public bool applyToSelf;
	public Status status;
	public float value;

}
