using BusinessLogic.Controllers;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticoAdo
{
    internal class Program
    {
        private static AlmacenController almacen;

        static void Main(string[] args)
        {
             almacen = new AlmacenController();

            short opcionUsuario = 0;
            do 
            {
                MostrarMenu();
                opcionUsuario = SolicitarOpcion();
                CargarOpcion(opcionUsuario);
            }
            while (opcionUsuario != 0);
        }

        private static void MostrarMenu() 
        {
            Console.Clear();
            Console.WriteLine("1. Ingresar Familia");
            Console.WriteLine("2. Ingresar Producto");
            Console.WriteLine("3. Ver las Familias de productos existentes");
            Console.WriteLine("4. Ver cantidad de productos");
            Console.WriteLine("0. Salir");
        }

        private static short SolicitarOpcion() 
        {
            short op = 0;
            Console.WriteLine("Ingrese una opcion: ");
            op = short.Parse(Console.ReadLine());
            return op;
        }

        private static void CargarOpcion(short opcion) 
        {
            switch (opcion)
            {
                case 1:
                    IngresarFamilia();
                    Console.ReadKey();
                    break;
                case 2:
                    IngresarProducto();
                    Console.ReadKey();
                    break;
            case 3:
                    ListarFamilias();
                    Console.ReadKey();
                    break;
            case 4:
                    cantidadProductosAlta();
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("Cerrando...");
                    Console.ReadKey();
                    break;
            }
        }

        private static void IngresarFamilia() 
        {
            FamiliaProducto nuevaFamilia = new FamiliaProducto();
            Console.WriteLine("Ingrese el codigo de familia");
            nuevaFamilia.CodigoFamilia = Console.ReadLine();

            Console.WriteLine("Ingrese la descripcion de familia");
            nuevaFamilia.Descripcion = Console.ReadLine();

            almacen.AgregarFamilia(nuevaFamilia);

            Console.WriteLine("Familia ingresada con exito");
        }

        private static void IngresarProducto()
        {
            Producto nuevoProducto = new Producto();
            do
            {
                Console.WriteLine("Ingrese el código de producto");
                nuevoProducto.CodigoProducto = Console.ReadLine();
            } while (ExisteProducto(nuevoProducto.CodigoProducto)); 

            Console.WriteLine("Ingrese la descripción");
            nuevoProducto.Descripcion = Console.ReadLine();

            Console.WriteLine("Ingrese el precio de venta");
            nuevoProducto.PrecioVenta = float.Parse(Console.ReadLine());

            Console.WriteLine("Ingrese el código de la familia");
            nuevoProducto.CodigoFamilia = Console.ReadLine();

            almacen.AgregarProducto(nuevoProducto);

        }

        private static void ListarFamilias()
        {
            List<FamiliaProducto> colFamilias = new List<FamiliaProducto>();
            colFamilias = almacen.GetFamilias();

            Console.WriteLine("Familias: ");
            foreach (FamiliaProducto item in colFamilias)
            {
                Console.WriteLine("Codigo Familia: " + item.CodigoFamilia + " Descripción: " + item.Descripcion);
            }
        }

        private static void cantidadProductosAlta()
        {
            int? cantidad = almacen.CantidadProductos();
            Console.WriteLine($"Cantidad de productos dados de alta: {cantidad}");
        }

        public static bool ExisteProducto(string id)
        {
            return almacen.ExisteProducto(id);
        }
    }
}
