using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using JamGame.GameObjects.Monsters;
using JamGame.Factories;
using JamGame.GameObjects.PowerUpItems;

// TODO: optimoi wavet
namespace JamGame.Maps
{
    public class MapState
    {
        #region Vars
        private readonly Random random;
        private readonly Texture2D foreground;
        private readonly Texture2D background;
        private readonly List<MonsterWave> waves;
        private readonly List<Monster> releasedMonsters;

        private int elapsed;
        #endregion

        #region Events
        public event MapStateEventHandler OnNextWave;
        #endregion

        #region Properties
        /// <summary>
        /// Staten sijainti.
        /// </summary>
        public Point Position
        {
            get
            {
                return new Point(StateArea.X, StateArea.Y);
            }
            set
            {
                StateArea = new Rectangle(value.X, value.Y, StateArea.Width, StateArea.Height);
            }
        }
        /// <summary>
        /// Staten area.
        /// </summary>
        public Rectangle StateArea
        {
            get;
            private set;
        }
        /// <summary>
        /// Onko state toistettu jo loppuun.
        /// </summary>
        public bool Finished
        {
            get;
            private set;
        }
        /// <summary>
        /// Onko statessa vielä waveja jäljellä.
        /// </summary>
        public bool HasWaves
        {
            get
            {
                return waves.Count > 0;
            }
        }
        /// <summary>
        /// Ollaanko aloitettu staten toisto.
        /// </summary>
        public bool Started
        {
            get;
            private set;
        }
        #endregion

        public MapState(Texture2D foreground, Texture2D background, List<MonsterWave> waves, Rectangle stateArea)
        {
            this.foreground = foreground;
            this.background = background;
            this.waves = waves;

            StateArea = stateArea;

            Started = false;
            Finished = false;

            random = new Random();
            releasedMonsters = new List<Monster>();
        }

        public void Start()
        {
            if (!Started)
            {
                elapsed = 0;
                Started = true;

                PowerUpFactory powerUpFactory = new PowerUpFactory("JamGame.GameObjects.PowerUpItems");
                for (int i = 0; i < random.Next(3, 3 * 2); i++)
                {
                    int value = random.Next(0, 100);
                    PowerUpItem powerUp = null;

                    if (Utils.InRange(0, 25, value))
                    {
                        Game.Instance.AddGameObject(powerUp = powerUpFactory.MakeNew("HealingPowerUp"));
                    }
                    else if (Utils.InRange(25, 50, value))
                    {
                        Game.Instance.AddGameObject(powerUp = powerUpFactory.MakeNew("IncreasedHpPowerUp"));
                    }
                    else if (Utils.InRange(50, 75, value))
                    {
                        Game.Instance.AddGameObject(powerUp = powerUpFactory.MakeNew("DoubleSpeedPowerUp"));
                    }
                    else if (Utils.InRange(75, 100, value))
                    {
                        Game.Instance.AddGameObject(powerUp = powerUpFactory.MakeNew("DoubleDamagePowerUp"));
                    }

                    powerUp.Position = new Vector2(
                        random.Next(0, Game.Instance.ScreenWidth - powerUp.Size.Width), 
                        random.Next(Game.Instance.ScreenHeight / 2, Game.Instance.ScreenHeight - powerUp.Size.Height));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Started)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;
                MonsterWave nextWave = waves.FirstOrDefault(w => elapsed > w.ReleaseTime);
                
                // Vapautetaan seuraava aalto hirviöitä.
                if (nextWave != null)
                {
                    if (OnNextWave != null)
                    {
                        OnNextWave(this, new MapStateEventArgs(nextWave, elapsed));
                    }

                    waves.Remove(nextWave);

                    Random random = new Random();
                    foreach (Monster monster in nextWave.ReleaseMonsters())
                    {
                        Game.Instance.AddGameObject(monster);
                        releasedMonsters.Add(monster);

                        int height = (int)(monster.Animation.Scale * 256);
                        int max_X = Game.Instance.ScreenWidth + 200;
                        int max_Y = Game.Instance.ScreenHeight;

                        // Vetää monsterin positionin randomilla jonkin verran ulos ruudusta ja kertoo sen pos modifierillä 
                        // jotta se saadaan tulemaan oikeasta paikasta (vasen, oikea, ylä ja ala)
                        monster.Position = new Vector2(
                            random.Next(Game.Instance.ScreenWidth, max_X) * nextWave.PositionModifier.X,
                            random.Next(Game.Instance.ScreenHeight / 2 + height, max_Y) * nextWave.PositionModifier.Y);

                    }
                }

                // Jos kaikki wavet on lähetetty, katsotaan onko hirviöitä vielä elossa.
                // Jos kaikki hirviöt on jo tapettu, state on suoritettu.
                if (!HasWaves)
                {
                    Finished = true;
                    foreach (Monster releasedMonster in releasedMonsters)
                    {
                        if (Game.Instance.ContainsGameObject(releasedMonster))
                        {
                            Finished = false;
                            break;
                        }
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(
                StateArea.X, 
                0, 
                Game.Instance.ScreenWidth, 
                Game.Instance.ScreenHeight / 2), Color.White);
            
            spriteBatch.Draw(foreground, new Rectangle(
                StateArea.X, 
                Game.Instance.ScreenHeight / 2, 
                Game.Instance.ScreenWidth, 
                Game.Instance.ScreenHeight / 2), Color.White);
        }
    }

    public delegate void MapStateEventHandler(object sender, MapStateEventArgs e);

    public class MapStateEventArgs : EventArgs
    {
        #region Vars
        public MonsterWave NextWave
        {
            get;
            private set;
        }
        public bool NewWave
        {
            get;
            private set;
        }
        public int Elapsed
        {
            get;
            private set;
        }
        #endregion

        public MapStateEventArgs(MonsterWave nextWave, int elapsed)
        {
            NextWave = nextWave;
            NewWave = nextWave != null;
            Elapsed = elapsed;
        }
    }
}
