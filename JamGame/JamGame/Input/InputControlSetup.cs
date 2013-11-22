namespace JamGame.Input
{
	public class InputControlSetup
    {
        #region Properties
        public InputMapper Mapper
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        public InputControlSetup()
		{
            Mapper = new InputMapper();
            Mapper.AddInputBindProvider(typeof(KeyInputBindProvider), new KeyInputBindProvider());
            Mapper.AddInputBindProvider(typeof(PadInputBindProvider), new PadInputBindProvider());
		}
        #endregion 
    }
}

