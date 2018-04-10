[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(KRA.UI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(KRA.UI.App_Start.NinjectWebCommon), "Stop")]

namespace KRA.UI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
  
    using Data.Repository;
    using Domain.Contracts;
    using Domain.Service;

    using DataSql;
    using Domain.Contract;
    using Domain.Services;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)

        {
            //Data Layer Dependencies
            kernel.Bind<IAccountsDal>().To<AccountsDal>();
            kernel.Bind<IKraInputScoresDal>().To<KraInputScoresDal>();
            kernel.Bind<IAccountParameteresDal>().To<AccountParametersDal>();
            kernel.Bind<IKraParametersDal>().To<KraParametersDal>();
            kernel.Bind<IParameterBoundsDal>().To<ParameterBoundsDal>();
            kernel.Bind<IAccountTeamSizeDal>().To<AccountTeamSizeDal>();
             //Service Layer Dependencies   
            kernel.Bind<IAccountsService>().To<AccountsService>();
            kernel.Bind<IAccountParameterService>().To<AccountParamterService>();
            kernel.Bind<IKraInputScoresService>().To<KraInputScoresService>();
            kernel.Bind<IKraParameterService>().To<KraParameterService>();
            kernel.Bind<IParameterBoundsService>().To<ParameterBoundsService>();
            kernel.Bind<IKraScoreCalculator>().To<KraScoreCalculator>();
            kernel.Bind<IAccountTeamSizeService>().To<AccountTeamSizeService>();
            kernel.Bind<IExcelReportGenerator>().To<ExcelReportGenerator> ();
 

        }
    }
}
