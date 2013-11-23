namespace JamGame.Input
{
    /// <summary>
    /// Ohjaa control setuppeja ja niiden bindaamista globaaliin input
    /// systeemiin
    /// </summary>
	public class InputController
    {
        #region Vars

	    private readonly InputManager input;

        #endregion

        #region Constructor

	    /// <summary>
	    /// Luo uuden ohjaimen
	    /// </summary>
	    /// <param name="keyInputStateManager"></param>
	    public InputController (InputManager input)
	    {
	        this.input = input;
	    }

        #endregion

        #region Properties
        /// <summary>
        /// Palauttaa nykyisen asetelman
        /// </summary>
        public InputControlSetup CurrentSetup
        {
            get;
            protected set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Vaihtaa controllerin asetelmaa
        /// </summary>
        /// <param name="newSetup">Uusi setuppi</param>
        public void ChangeSetup(InputControlSetup newSetup)
        {
            if (CurrentSetup != null)
            {
                input.Mapper.RemoveSetup(CurrentSetup);
            }
            
            CurrentSetup = newSetup;
            input.Mapper.AddSetup(CurrentSetup);
        }

        #endregion
    }
}

