using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using JamGame.GameObjects;
using JamGame.Maps;

namespace JamGame
{
    // 1280 * 720
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Vars
        // Singleton instanssi.
        private static Game instance;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<GameObject> allObjects;
        private List<DrawableGameObject> drawableObjects;
        #endregion

        #region Properties
        private static object padLock = new object();
        /// <summary>
        /// Palauttaa singleton instanssin pelistä.
        /// </summary>
        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game();
                }

                return instance;
            }
        }
        public int ScreenWidth
        {
            get
            {
                return graphics.GraphicsDevice.Viewport.Width;
            }
        }
        public int ScreenHeight
        {
            get
            {
                return graphics.GraphicsDevice.Viewport.Height;
            }
        }
        public Vector2 ScreenPosition
        {
            get
            {
                return new Vector2(graphics.GraphicsDevice.Viewport.X,
                                   graphics.GraphicsDevice.Viewport.Y);
            }
        }
        public Map Current
        {
            get;
            set;
        }
        #endregion

        private Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            allObjects = new List<GameObject>();
            drawableObjects = new List<DrawableGameObject>();

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        public void AddGameObject(GameObject gameObject)
        {
            allObjects.Add(gameObject);

            DrawableGameObject drawableGameObject = gameObject as DrawableGameObject;
            if (drawableGameObject != null)
            {
                drawableObjects.Add(drawableGameObject);
            }
        }
        public void RemoveGameObject(GameObject gameObject)
        {
            allObjects.Remove(gameObject);

            DrawableGameObject drawableGameObject = gameObject as DrawableGameObject;
            if (drawableGameObject != null)
            {
                drawableObjects.Remove(drawableGameObject);
            }
        }
        public GameObject GetGameObject(Predicate<GameObject> predicate)
        {
            return allObjects.Find(o => predicate(o));
        }
        public IEnumerable<GameObject> GetGameObjects(Predicate<GameObject> predicate)
        {
            return allObjects.Where(o => predicate(o));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Current = new Map();
            Current.Load(@"Maps\MapFiles\testmap.xml");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Current.Update(gameTime);
            allObjects.ForEach(
                o => o.Update(gameTime));
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            Current.Draw(spriteBatch);
            drawableObjects.ForEach(
                o => o.Draw(spriteBatch));

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
