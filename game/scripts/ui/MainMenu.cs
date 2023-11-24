using Godot;

public partial class MainMenu : Node
{
	[Export]
	private Node mainNode;

    [Export]
    private PackedScene gameScene;

    public void PlayGame()
	{
        Node gameNode = gameScene.Instantiate();
        mainNode.AddChild(gameNode);

        QueueFree();
    }
}
