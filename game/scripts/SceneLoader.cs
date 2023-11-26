using Godot;

public partial class SceneLoader : Node
{
    [Export]
    private Node mainNode;

    [Export]
    private PackedScene mainMenuPackedScene;

    [Export]
    private PackedScene gamePackedScene;

    [Export]
    private Node mainMenu;

    private Node game;

    public override void _Ready()
	{
	}

    public void LoadMainMenu()
    {
        Node instance = mainMenuPackedScene.Instantiate();
        mainNode.AddChild(instance);

        mainMenu = instance;
        ((MainMenu)mainMenu).Instantiate(this, true);

        game.QueueFree();
    }

    public void LoadLevel()
    {
        Node instance = gamePackedScene.Instantiate();
        mainNode.AddChild(instance);

        game = instance;
        ((Level)game).Instantiate(this);

        mainMenu.QueueFree();
    }
}
