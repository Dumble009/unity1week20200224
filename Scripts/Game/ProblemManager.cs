using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public class ProblemManager : SingletonMonoBehaviour<ProblemManager>
{

}

[DataContract]
public class Problem
{
	[DataMember(Name = "bars")]
	public Bar[] bars;
}

[DataContract]
public class Bar
{
	[DataMember(Name = "nodes")]
	public Node[] nodes;
}

[DataContract]
public class Node
{
	[DataMember(Name = "timing")]
	public int Timing;
	[DataMember(Name = "pitch")]
	public int Pitch;
}
