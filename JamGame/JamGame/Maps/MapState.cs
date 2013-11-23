using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using JamGame.GameObjects.Monsters;

namespace JamGame.Maps
{
    public class MapState
    {
        #region Vars
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
        public Rectangle StateArea
        {
            get;
            private set;
        }
        public bool Finished
        {
            get;
            private set;
        }
        public bool HasWaves
        {
            get
            {
                return waves.Count > 0;
            }
        }
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

            releasedMonsters = new List<Monster>();
        }

        public void Start()
        {
            if (!Started)
            {
                elapsed = 0;
                Started = true;
            }
        }
        public void Update(GameTime gameTime)
        {
            if (Started)
            {
                elapsed += gameTime.TotalGameTime.Milliseconds;
                MonsterWave nextWave = waves.FirstOrDefault(w => w.ReleaseTime >= elapsed);
                
                if (nextWave != null)
                {
                    if (OnNextWave != null)
                    {
                        OnNextWave(this, new MapStateEventArgs(nextWave, elapsed));
                    }

                    waves.Remove(nextWave);

                    foreach (Monster monster in nextWave.ReleaseMonsters())
                    {
                        Game.Instance.AddGameObject(monster);
                        releasedMonsters.Add(monster);
                    }
                }

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
            spriteBatch.Draw(background, new Rectangle(StateArea.X, 0, Game.Instance.ScreenWidth, Game.Instance.ScreenHeight / 2), Color.White);
            spriteBatch.Draw(foreground, new Rectangle(StateArea.X, background.Height / 2, Game.Instance.ScreenWidth, Game.Instance.ScreenHeight), Color.White);
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
