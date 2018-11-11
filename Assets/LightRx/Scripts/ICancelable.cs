using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CancelState
{
	None,
	Cancelling,
}

public interface ICancelable
{
	CancelState State { get; }
	
	void RequestCancel();

}
