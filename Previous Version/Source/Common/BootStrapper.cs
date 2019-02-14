using System.Windows;
using Interfaces;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Modifiers;
using ServiceClients;

namespace Common
{
    public class BootStrapper : UnityBootstrapper
    {
        #region Overrides of Bootstrapper

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>
        /// The shell of the application.
        /// </returns>
        /// <remarks>
        /// If the returned instance is a <see cref="T:System.Windows.DependencyObject"/>, the
        ///             <see cref="T:Microsoft.Practices.Prism.Bootstrapper"/> will attach the default <seealso cref="T:Microsoft.Practices.Prism.Regions.IRegionManager"/> of
        ///             the application in its <see cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionManagerProperty"/> attached property
        ///             in order to be able to add regions by using the <seealso cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionNameProperty"/>
        ///             attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            return null;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IShowInformationClient, OMDBClient>(new ContainerControlledLifetimeManager())
                     .RegisterType<ITorrentClient, UTorrentClient>(new ContainerControlledLifetimeManager())
                     .RegisterType<IFileNameParser, FileNameParser>(new ContainerControlledLifetimeManager())
                     .RegisterType<IModifier, Modifier>(new ContainerControlledLifetimeManager())
                     .RegisterType<IFileModifier, FileModifier>(new ContainerControlledLifetimeManager())
                     .RegisterType<IDirectoryModifier, DirectoryModifier>(new ContainerControlledLifetimeManager())
                     .RegisterType<ISettingsManager, SettingsManager>(new ContainerControlledLifetimeManager());
        }

        #endregion
    }
}
