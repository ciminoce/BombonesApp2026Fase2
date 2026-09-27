using Bombones2026.Servicios.Servicios;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Datos.Repositorios;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Servicios;
using CajaesApp2026.Datos.Interfaces;
using CajaesApp2026.Datos.Repositorios;
using CajaesApp2026.Servicios.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

            var options =
                new DbContextOptionsBuilder<BombonesDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;

            var context =
                new BombonesDbContext(options);
            //instanciar repositorios y servicios aquí si es necesario
            ITipoBombonRepositorio tipoBombonRepositorio = new TipoBombonRepositorio(context);
            IFormaDePagoRepositorio formaDePagoRepositorio = new FormaDePagoRepositorio(context);
            IRolRepositorio rolRepositorio = new RolRepositorio(context);
            IProvinciaRepositorio provinciaRepositorio = new ProvinciaRepositorio(context);
            ICiudadRepositorio ciudadRepositorio = new CiudadRepositorio(context);
            ITransporteRepositorio transporteRepositorio = new TransporteRepositorio(context);
            IBombonRepositorio bombonRepositorio = new BombonRepositorio(context);
            IClienteRepositorio clienteRepositorio = new ClienteRepositorio(context);
            ICajaRepositorio cajaRepositorio=new CajaRepositorio(context);

            ITipoBombonServicio tipoBombonServicio = new TipoBombonServicio(tipoBombonRepositorio);
            IFormaDePagoServicio formaDePagoServicio = new FormaDePagoServicio(formaDePagoRepositorio);
            IRolServicio rolServicio = new RolServicio(rolRepositorio);
            IProvinciaServicio provinciaServicio = new ProvinciaServicio(provinciaRepositorio);
            ICiudadServicio ciudadServicio = new CiudadServicio(ciudadRepositorio);
            ITransporteServicio transporteServicio = new TransporteServicio(transporteRepositorio);
            IBombonServicio bombonServicio = new BombonServicio(bombonRepositorio);
            IClienteServicio clienteServicio = new ClienteServicio(clienteRepositorio);
            ICajaServicio cajaServicio=new CajaServicio(cajaRepositorio);
            Application.Run(new frmPrincipal(
                tipoBombonServicio,
                formaDePagoServicio,
                rolServicio,
                provinciaServicio,
                ciudadServicio,
                transporteServicio,
                bombonServicio,
                clienteServicio,
                cajaServicio));
        }
    }
}