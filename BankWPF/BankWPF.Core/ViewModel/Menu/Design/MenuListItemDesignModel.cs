namespace BankWPF.Core
{
    /// <summary>
    /// The design-time data for a <see cref="MenuListItemViewModel"/>
    /// </summary>
    public class MenuListItemDesignModel : MenuListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static MenuListItemDesignModel Instance => new MenuListItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuListItemDesignModel()
        {
            DWLetter = "D";
            Value = "500";
            Message = "This chat app is awesome! I bet it will be fast too";
            ColorStringRGB = "00d405";
            Date = "2017/11/20 09:06:18";
        }

        #endregion
    }
}
