using UnityEngine;
using System.Collections;

public class Room {

	public int x1;
	public int x2;

	public int y1;
	public int y2;

	public int width;
	public int height;

	public Vector2 center;
	public int connections = 0;

	public int difficulty;

	public int SetSize (int x, int y, int w, int h) {
		
		this.x1 = x;
		this.x2 = x + w;
		this.y1 = y;
		this.y2 = y + h;
		this.width = w;
		this.height = h;

		center = new Vector2 (x + w/2, y + h/2); 

		return 0;
	}
	

	// returns true if the box intersects a provided room
	public bool IsIntersecting (Room room) {
		return (x1 <= room.x2 && x2 >= room.x1 &&
			y1 <= room.y2 && y2 >= room.y1);
	}
}
