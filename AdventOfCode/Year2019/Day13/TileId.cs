namespace AdventOfCode.Year2019.Day13;

public enum TileId
{
    Empty = 0, // No game object appears in this tile
    Wall, // Walls are indestructible barriers.
    Block, // Blocks can be broken by the ball.
    HorizontalPaddle, // The paddle is indestructible.
    Ball // The ball moves diagonally and bounces off objects.
}