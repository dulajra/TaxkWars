﻿using UnityEngine;
using System.Collections;

public class GameFrameHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameClient.GameDomain.GameWorld.Instance.FrameAdvanced += OnFrameAdvanced;
	}

	void updateTanks ()
	{
		GameClient.GameDomain.GameWorld world = GameClient.GameDomain.GameWorld.Instance;
		for (int i = 0; i < world.Players.Length; i++) {
			GameClient.GameDomain.PlayerDetails playerDetails = world.Players[i];
			if(i >= UIReferenceMap.Instance.Players.Count)
			{
				UITank.loadTank(i);
			}
			GameObject playerGameObject = UIReferenceMap.Instance.Players[i];

			//find new container
			string name = "World/Ground/R" + (playerDetails.Position.X + 1).ToString() + "/C" + (playerDetails.Position.Y + 1).ToString(); ;
			GameObject cell = GameObject.Find(name);

			//move by changing parent
			//TODO: setup animations here
			Transform cellTransform = cell.transform;
			playerGameObject.transform.SetParent(cellTransform, false);
			playerGameObject.transform.rotation = Quaternion.AngleAxis(UIHelper.DirectionToAngle(playerDetails.Direction),new Vector3(0,1,0));
			Debug.Log("Direction " + UIHelper.DirectionToAngle(playerDetails.Direction).ToString());


		}
	}

    void OnFrameAdvanced(object sender, System.EventArgs e)
    {
        updateBrickwalls();
		updateTanks ();

    }

    private void updateBrickwalls()
    {
        GameClient.GameDomain.GameWorld world = GameClient.GameDomain.GameWorld.Instance;
        foreach (GameClient.GameDomain.Brick brick in world.BrickState)
        {
			GameObject container = UIReferenceMap.Instance.BrickWallContainers["R" + (brick.Postition.X + 1).ToString() + "/C" + (brick.Postition.Y + 1).ToString()];
            //string name = "World/Ground/R" + (brick.Postition.X + 1).ToString() + "/C" + (brick.Postition.Y + 1).ToString() + "/BrickWall100"; ;
            //GameObject cell = GameObject.Find(name);

			foreach (Transform child in container.transform) {
				GameObject.Destroy(child.gameObject);
			}

			string resName = "BrickWall100";
			switch(brick.DamageLevel)
			{
			case 1:
				resName = "BrickWall75";
				break;
			case 2:
				resName = "BrickWall50";
				break;
			case 3:
				resName = "BrickWall25";
				break;
			case 4:
				resName = "BrickWall0";
				break;
			}

            GameObject BrickWall = GameObject.Instantiate(Resources.Load(resName)) as GameObject;

            Transform containerTransform = container.transform;
            BrickWall.transform.SetParent(container.transform, false);


        }
    }


	
	// Update is called once per frame
	void Update () {
	    
	}
}
