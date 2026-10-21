using Bombones2026.Servicios.Servicios;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Datos.Repositorios;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BombonesApp2026.Windows
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            IConfiguration configuration =
                new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile(
                        "appsettings.json",
                        optional: false,
                        reloadOnChange: true)
                    .Build();

            string connectionString =
                configuration.GetConnectionString("Bombones")
                ?? throw new InvalidOperationException(
                    "No se encontró la cadena de conexión.");
            var servicios = new ServiceCollection();

            servicios
                .AddDbContext<BombonesDbContext>(
                    options => options.UseAzureSql(connectionString),
                    ServiceLifetime.Scoped);

            servicios.AddScoped<IUnitOfWork,  UnitOfWork>();

            servicios.AddScoped<ITipoBombonRepositorio, TipoBombonRepositorio>();
            servicios.AddScoped<IFormaDePagoRepositorio, FormaDePagoRepositorio>();
            servicios.AddScoped<IRolRepositorio, RolRepositorio>();
            servicios.AddScoped<IProvinciaRepositorio, ProvinciaRepositorio>();
            servicios.AddScoped<ICiudadRepositorio, CiudadRepositorio>();
            servicios.AddScoped<ITransporteRepositorio, TransporteRepositorio>();
            servicios.AddScoped<IBombonRepositorio, BombonRepositorio>();
            servicios.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            servicios.AddScoped<ICajaRepositorio, CajaRepositorio>();

            // Registrar Servicios
            servicios.AddScoped<ITipoBombonServicio, TipoBombonServicio>();
            servicios.AddScoped<IFormaDePagoServicio, FormaDePagoServicio>();
            servicios.AddScoped<IRolServicio, RolServicio>();
            servicios.AddScoped<IProvinciaServicio, ProvinciaServicio>();
            servicios.AddScoped<ICiudadServicio, CiudadServicio>();
            servicios.AddScoped<ITransporteServicio, TransporteServicio>();
            servicios.AddScoped<IBombonServicio, BombonServicio>();
            servicios.AddScoped<IClienteServicio, ClienteServicio>();
            servicios.AddScoped<ICajaServicio, CajaServicio>();

            // 🌟 4. REGISTRO AUTOMÁTICO DE FORMULARIOS POR REFLEXIÓN
            // Esto busca todas las clases que heredan de Form en el proyecto actual y las registra como Transient
            var assembly = Assembly.GetExecutingAssembly(); // O el assembly donde tengas tus formularios

            var tiposFormularios = assembly.GetTypes()
                .Where(t => typeof(Form).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var tipoForm in tiposFormularios)
            {
                servicios.AddTransient(tipoForm);
            }
            var serviceProvider = servicios.BuildServiceProvider();

            var frm = serviceProvider.GetRequiredService<frmPrincipal>();
            frm.Text = "Sistema de Bombonería";
            
            Application.Run(frm);
        }
    }
}