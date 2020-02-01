public enum Direction {
	//Can get the opposite direction with: (direction + 2) % 4
	Up,
	Right,
	Down,
	Left
}

static class DirectionMethods {
	public static Direction OppositeDirection(this Direction direction) {
		return (Direction)(((int)direction + 2) % 4);
	}
}