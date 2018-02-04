﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMessageScript : MonoBehaviour {

	private GameObject message;
	private int countPlayers;

	// Use this for initialization
	void Start ()
	{
			countPlayers = 0;
			message = transform.parent.Find("Message").gameObject;
	}

	void OnTriggerEnter(Collider other)
	{
			if(other.gameObject.CompareTag("Player"))
			{
					countPlayers++;
					message.SetActive(true);
			}
	}

	void OnTriggerExit(Collider other)
	{
			if(other.gameObject.CompareTag("Player"))
			{
					countPlayers--;
			}

			if(countPlayers == 0) {
					message.SetActive(false);
			}
	}
}
