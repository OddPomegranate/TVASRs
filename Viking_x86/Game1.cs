using System;
using System.Threading;
using IMAK3Z0MB1EGAEM;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.director;
using IMAK3Z0MB1EGAEM.menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Viking_x86.character;
using Viking_x86.director;
using Viking_x86.loader;
using ZP2K9.store;
using Microsoft.Xna.Framework.Content;

namespace Viking_x86;

public class Game1 : Game
{
	// The game's whole rendering pipeline draws in this fixed virtual
	// resolution (see VScroll.screenSize below) - camera scroll bounds,
	// UI text centering, etc. all assume 1280x720. Rather than rewrite that,
	// we let the window/backbuffer be any size and just scale+letterbox the
	// 1280x720 output to fit it (see RecalculateScale/SpriteTools.transform).
	private const int VirtualWidth = 1280;
	private const int VirtualHeight = 720;

	private GraphicsDeviceManager graphics;

	private bool isFullscreen;
	private int windowedWidth = VirtualWidth;
	private int windowedHeight = VirtualHeight;
	private bool handlingResize;
	private KeyboardState prevKeyboard;

	public static float frameTime;

	public static Texture2D nullTex;

	public static VikingGame vgame;

	public static TimeMgr tMgr;

	public static Loader loader;

	private bool loadcomplete;

	public static Store store;

	public Game1()
	{
		graphics = new GraphicsDeviceManager(this);
		base.Content = new ColorFixContentManager(Services, "Content");
	}

	protected override void Initialize()
	{
		Rand.rand = new Random();
		Sound.Init(this.Content);
		loader = new Loader();
		store = new Store();
		VScroll.screenSize = new Vector2(1280f, 720f);
		graphics.PreferMultiSampling = false;
		graphics.PreferredBackBufferWidth = VirtualWidth;
		graphics.PreferredBackBufferHeight = VirtualHeight;
		graphics.SynchronizeWithVerticalRetrace = true;
		graphics.HardwareModeSwitch = false;
		graphics.ApplyChanges();

		Window.AllowUserResizing = true;
		Window.ClientSizeChanged += OnClientSizeChanged;
		RecalculateScale();

		base.Initialize();
	}

	private void OnClientSizeChanged(object sender, EventArgs e)
	{
		if (handlingResize || isFullscreen)
		{
			return;
		}
		int w = Window.ClientBounds.Width;
		int h = Window.ClientBounds.Height;
		if (w <= 0 || h <= 0)
		{
			return;
		}
		handlingResize = true;
		graphics.PreferredBackBufferWidth = w;
		graphics.PreferredBackBufferHeight = h;
		graphics.ApplyChanges();
		RecalculateScale();
		handlingResize = false;
	}

	private void RecalculateScale()
	{
		float w = graphics.PreferredBackBufferWidth;
		float h = graphics.PreferredBackBufferHeight;
		float scale = Math.Min(w / VirtualWidth, h / VirtualHeight);
		if (scale <= 0f)
		{
			scale = 1f;
		}
		float offsetX = (w - VirtualWidth * scale) / 2f;
		float offsetY = (h - VirtualHeight * scale) / 2f;
		SpriteTools.transform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(offsetX, offsetY, 0f);
	}

	private void ToggleFullscreen()
	{
		isFullscreen = !isFullscreen;
		if (isFullscreen)
		{
			windowedWidth = graphics.PreferredBackBufferWidth;
			windowedHeight = graphics.PreferredBackBufferHeight;
			DisplayMode display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
			graphics.PreferredBackBufferWidth = display.Width;
			graphics.PreferredBackBufferHeight = display.Height;
		}
		else
		{
			graphics.PreferredBackBufferWidth = windowedWidth;
			graphics.PreferredBackBufferHeight = windowedHeight;
		}
		graphics.IsFullScreen = isFullscreen;
		graphics.ApplyChanges();
		RecalculateScale();
	}

	protected override void LoadContent()
	{
		SpriteTools.sprite = new SpriteBatch(base.GraphicsDevice);
		nullTex = base.Content.Load<Texture2D>("gfx/1x1");

		Thread thread = new Thread(ThreadedMainLoad);
		thread.Start();
	}

	public void ThreadedMainLoad()
	{
		Text.Init(nullTex);
		HighScores.Init();
		CharDefMgr.Initialize();
		vgame = new VikingGame(base.Content);
		loadcomplete = true;
	}

	protected override void UnloadContent()
	{
	}

	protected override void Update(GameTime gameTime)
	{
		frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		if (Menu.needsQuit)
		{
			Exit();
		}

		KeyboardState keyboard = Keyboard.GetState();
		bool altHeld = keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt);
		bool altHeldPrev = prevKeyboard.IsKeyDown(Keys.LeftAlt) || prevKeyboard.IsKeyDown(Keys.RightAlt);
		bool altEnterPressed = altHeld && keyboard.IsKeyDown(Keys.Enter) && !(altHeldPrev && prevKeyboard.IsKeyDown(Keys.Enter));
		bool f11Pressed = keyboard.IsKeyDown(Keys.F11) && !prevKeyboard.IsKeyDown(Keys.F11);
		if (altEnterPressed || f11Pressed)
		{
			ToggleFullscreen();
		}
		prevKeyboard = keyboard;

		switch (GameState.state)
		{
		case GameState.State.Loading:
			loader.Update();
			if (loadcomplete)
			{
				GameState.state = GameState.State.VikingMenu;
				vgame.Init();
			}
			break;
		case GameState.State.VikingMenu:
		case GameState.State.VikingPlaying:
			vgame.Update(gameTime);
			break;
		}
		store.Update();
		Sound.Update();
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		base.GraphicsDevice.Clear(Color.Black);
		switch (GameState.state)
		{
		case GameState.State.Loading:
			if (!loadcomplete)
			{
				loader.Draw();
			}
			break;
		case GameState.State.VikingMenu:
		case GameState.State.VikingPlaying:
			vgame.Draw(graphics.GraphicsDevice);
			break;
		}
		base.Draw(gameTime);
	}
}
