using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using Splat;

namespace CoWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Make sure Splat and ReactiveUI are already configured in the locator
            // so that our override runs last
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            // A helper method that will register all classes that derive off IViewFor 
            // into our dependency injection container. ReactiveUI uses Splat for it's 
            // dependency injection by default, but you can override this if you like.
            //Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            Locator.CurrentMutable.Register<IViewFor<CoEd.EditorViewModel>>(() => new CoEd.EditorView());
            Locator.CurrentMutable.Register<IViewFor<CoEd.TextViewModel>>(() => new CoEd.TextView());

            var locator = Locator.Current.GetService<IViewLocator>();
            //Locator.CurrentMutable.RegisterLazySingleton(() => new ConventionalViewLocator(), typeof(IViewLocator));
        }

        public class ConventionalViewLocator : IViewLocator
        {
            public IViewFor ResolveView<T>(T viewModel, string contract = null) where T : class
            {
                // Find view's by chopping of the 'Model' on the view model name
                // MyApp.ShellViewModel => MyApp.ShellView
                var viewModelName = viewModel.GetType().FullName;
                var viewTypeName = viewModelName.TrimEnd("Model".ToCharArray());

                try
                {
                    var viewType = Type.GetType(viewTypeName);
                    if (viewType == null)
                    {
                        this.Log().Error($"Could not find the view {viewTypeName} for view model {viewModelName}.");
                        return null;
                    }
                    return Activator.CreateInstance(viewType) as IViewFor;
                }
                catch (Exception)
                {
                    this.Log().Error($"Could not instantiate view {viewTypeName}.");
                    throw;
                }
            }
        }
    }
}
