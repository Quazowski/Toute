namespace Toute
{
    public class ApplicationViewModel : BaseViewModel
    {
        #region Private properties

        private BasePage currentPage;

        #endregion

        #region Public properties

        /// <summary>
        /// Determines a visibility of side menu
        /// </summary>
        public bool SideMenuHidden { get; set; }

        public ApplicationPage CurrentApplicationPage { get; set; } = ApplicationPage.LoginPage;

        public BasePage CurrentPage
        {
            get => currentPage;
            set
            {
                if (CurrentPage == value)
                    return;

                currentPage = value;
            }
        }

        #endregion

        public void GoToPage(ApplicationPage page)
        {
            if ((page == ApplicationPage.LoginPage) || (page == ApplicationPage.RegisterPage))
            {
                SideMenuHidden = true;
            }
            else
            {
                SideMenuHidden = false;
            }

            CurrentApplicationPage = page;

            CurrentPage = ApplicationPageHelper.GoToBasePage(CurrentApplicationPage);
        }
        

    }
}
