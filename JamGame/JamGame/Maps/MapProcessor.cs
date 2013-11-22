using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using JamGame.Factories;
using JamGame.GameObjects.Monsters;

namespace JamGame.Maps
{
    public class MapProcessor
    {
        #region Vars
        private readonly XDocument mapFile;
        #endregion

        public MapProcessor(string mapName)
        {
            mapFile = XDocument.Load(mapName);
        }

        private Texture2D LoadForeground()
        {
            XElement styleElement = mapFile.Descendants("Style").First();
            return Game.Instance.Content.Load<Texture2D>(styleElement.Attribute("Foreground").Value);
        }
        private Texture2D LoadBackground()
        {
            XElement styleElement = mapFile.Descendants("Style").First();
            return Game.Instance.Content.Load<Texture2D>(styleElement.Attribute("Background").Value);
        }

        private IEnumerable<XElement> ReadStates()
        {
            return  from states in mapFile.Descendants("States")
                    from state in states.Descendants()
                    where state.Name == "State"
                    select state;
        }
        private IEnumerable<XElement> ReadWaveElements(XElement stateElement)
        {
            return from waveElements in stateElement.Descendants("Waves")
                   select waveElements;
        }
        private List<MonsterWave> ParseWaves(XElement waveElements)
        {
            List<MonsterWave> waves = new List<MonsterWave>();
            MonsterFactory factory = new MonsterFactory("JamGame.GameObjects.Monsters");

            foreach (XElement waveElement in waveElements.Descendants("Wave"))
            {
                int releaseTime = int.Parse(waveElement.Attribute("ReleaseTime").Value);


                var monsterDatasets = from monsterElements in waveElement.Descendants("Monsters")
                                      from monsterElement in monsterElements.Descendants()
                                      select new
                                      {
                                          Type = monsterElement.Attribute("Type").Value,
                                          Count = int.Parse(monsterElement.Attribute("Count").Value)
                                      };

                List<Monster> waveMonsters = new List<Monster>();
                foreach (var monsterDataset in monsterDatasets)
                {
                    waveMonsters.AddRange(factory.MakeNew(monsterDataset.Type, monsterDataset.Count));
                }

                waves.Add(new MonsterWave(waveMonsters, releaseTime));
            }

            return waves;
        }

        public List<MapState> LoadMapStates()
        {
            List<MapState> mapStates = new List<MapState>();
           
            Texture2D foreground = LoadForeground();
            Texture2D background = LoadBackground();

            IEnumerable<XElement> stateElements = ReadStates();

            foreach (XElement stateElement in stateElements)
            {
                List<MonsterWave> waves = null;
                IEnumerable<XElement> waveElements = ReadWaveElements(stateElement);
                
                foreach (XElement waveElement in waveElements)
                {
                    waves = ParseWaves(waveElement);
                }

                mapStates.Add(new MapState(foreground, background, waves));
            }

            return mapStates;
        }
    }
}
