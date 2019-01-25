
namespace PlaneAngles
{
    using System.Windows.Interop;

    using Ninject;

    using Mastercam.App;
    using Mastercam.App.Types;
    using Mastercam.Support.UI;

    using PlaneAngles.ViewModel;
    using PlaneAngles.Views;
    using PlaneAngles.Services;

    public class Main : NetHook3App
    {
        #region Public Override Methods

        /// <summary>
        /// The main entry point for your NETHook.
        /// </summary>
        /// <param name="param">System parameter.</param>
        /// <returns>A <c>MCamReturn</c> return type representing the outcome of your NetHook application.</returns>
        public override MCamReturn Run(int param)
        {
            using (var container = new StandardKernel())
            {
                container.Bind<IAngleService>().To<AngleService>();
                container.Bind<ISearchService>().To<SearchService>();

                var view = new MainView
                {
                    DataContext = container.Get<MainViewViewModel>()
                };

                var windowHelper = new WindowInteropHelper(view)
                {
                    Owner = MastercamWindow.GetHandle().Handle
                };

                view.Show();
            }

            return MCamReturn.NoErrors;
        }

        #endregion
    }
}
