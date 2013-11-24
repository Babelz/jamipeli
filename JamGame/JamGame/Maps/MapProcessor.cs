using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using JamGame.Factories;
using JamGame.GameObjects.Monsters;
using Microsoft.Xna.Framework;

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

        // Yrittää lukea int arvoa atribuutista, jos atribuuttia ei ole, palauttaa defaultin (0)
        private int ReadAttribute(XElement xElement, string name)
        {
            int value = 0;
            XAttribute attribute = xElement.Attribute(name);

            if (attribute != null)
            {
                value = int.Parse(attribute.Value);
            }

            return value;
        }
        private Texture2D LoadForeground(XElement stateElement)
        {
            string foreground = stateElement.Attribute("Foreground").Value;
            return Game.Instance.Content.Load<Texture2D>(foreground);
        }
        private Texture2D LoadBackground(XElement stateElement)
        {
            string background = stateElement.Attribute("Background").Value;
            return Game.Instance.Content.Load<Texture2D>(background);
        }
        // Lukee kaikki statet tiedostosta.
        private IEnumerable<XElement> ReadStates()
        {
            return  from states in mapFile.Descendants("States")
                    from state in states.Descendants()
                    where state.Name == "State"
                    select state;
        }
        // Lukee kaikki wavet statesta.
        private IEnumerable<XElement> ReadWaveElements(XElement stateElement)
        {
            return from waveElements in stateElement.Descendants("Waves")
                   select waveElements;
        }
        // Parsii ja luo jokaisen waven.
        private List<MonsterWave> ParseWaves(XElement waveElements)
        {
            List<MonsterWave> waves = new List<MonsterWave>();
            MonsterFactory factory = new MonsterFactory("JamGame.GameObjects.Monsters");

            foreach (XElement waveElement in waveElements.Descendants("Wave"))
            {
                // Lukee waven releasetimen.
                int releaseTime = int.Parse(waveElement.Attribute("ReleaseTime").Value);


                // Hakee kaikki monsterit wavesta ja projektaa ne anonyymeiksi olioiksi.
                Dictionary<string, int> monsterDatasets = (from monsterElements in waveElement.Descendants("Monsters")
                                                           from monsterElement in monsterElements.Descendants()
                                                           select new KeyValuePair<string, int>(monsterElement.Attribute("Type").Value, int.Parse(monsterElement.Attribute("Count").Value)))
                                                           .ToDictionary(v => v.Key, v => v.Value);


                // Lukee position modifierin wavesta.
                Vector2 positionModifier = new Vector2(ReadAttribute(waveElement, "XModifier"), ReadAttribute(waveElement, "YModifier"));

                waves.Add(new MonsterWave(monsterDatasets, releaseTime, positionModifier));
            }

            return waves;
        }

        public List<MapState> LoadMapStates()
        {
            List<MapState> mapStates = new List<MapState>();
            IEnumerable<XElement> stateElements = ReadStates();

            foreach (XElement stateElement in stateElements)
            {
                Texture2D foreground = LoadForeground(stateElement);
                Texture2D background = LoadBackground(stateElement);

                List<MonsterWave> waves = null;
                IEnumerable<XElement> waveElements = ReadWaveElements(stateElement);
                
                foreach (XElement waveElement in waveElements)
                {
                    waves = ParseWaves(waveElement);
                }

                Rectangle stateArea = new Rectangle(
                    (mapStates.Count > 0 ? 1 : 0) * Game.Instance.ScreenWidth,
                    (mapStates.Count > 0 ? 1 : 0) * Game.Instance.ScreenHeight,
                    Game.Instance.ScreenWidth, 
                    Game.Instance.ScreenHeight);

                mapStates.Add(new MapState(foreground, background, waves, stateArea));
            }

            return mapStates;
        }
    }
}
