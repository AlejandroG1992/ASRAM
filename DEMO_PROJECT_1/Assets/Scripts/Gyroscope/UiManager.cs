using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiManager : MonoBehaviour
{
	/// <summary>
	/// The reference to the compass value label.
	/// </summary>
	[SerializeField]
	private Text compass;



	/// <summary>
	/// Writes the compass Heading value in the corresponding
	/// UI text label.
	/// </summary>
	/// <param name="value">The compass true heading value.</param>
	public void WriteCompassValue(string value)
	{
		compass.text = value;
	}



}
