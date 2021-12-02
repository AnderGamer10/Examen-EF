using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static System.Console;


public class InstitutoContext : DbContext
{

    public DbSet<Alumno> Alumnos { get; set; }
    public DbSet<Modulo> Modulos { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }
    public string connString { get; private set; }

    public InstitutoContext()
    {

        var database = "EF11Ander"; // "EF{XX}Nombre" => EF00Santi
        connString = $"Server=185.60.40.210\\SQLEXPRESS,58015;Database={database};User Id=sa;Password=Pa88word;MultipleActiveResultSets=true";
    
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(connString);

}
public class Alumno
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int AlumnoId { get; set; }
    public string Nombre {get; set;}
    public int Edad {get; set;}
    public int Efectivo {get; set;}
    public string Pelo {get; set;}

    public List<Matricula> Matriculas { get; } = new List<Matricula>();
    public override string ToString() => $"{AlumnoId}: {Nombre}: {Efectivo}: {Pelo} ";

}
public class Modulo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ModuloId { get; set; }
    public string Titulo {get; set;}
    public int Creditos {get; set;}
    public int Curso {get; set;}

    public List<Matricula> Matriculas { get; } = new List<Matricula>();
    public override string ToString() => $"{ModuloId}: {Titulo}: {Creditos}: {Curso} ";
}
public class Matricula
{
    public int MatriculaId {get; set;}
    public int AlumnoId {get; set;}
    public int ModuloId {get; set;}
    
    public Alumno Alumnos {get; set;}
    public Modulo Modulos {get; set;}

    public override string ToString() => $"{AlumnoId}: {ModuloId}";
}

class Program
{
    static void GenerarDatos()
    {
        using (var db = new InstitutoContext())
        {

            // Borrar todo
            Console.WriteLine("Borrando los datos de las tablas");
            db.Alumnos.RemoveRange(db.Alumnos);
            db.Modulos.RemoveRange(db.Modulos);
            db.Matriculas.RemoveRange(db.Matriculas);
            db.SaveChanges();
            // Añadir Alumnos
            // Id de 1 a 7
            Console.WriteLine("Añadiendo los alumnos");
            db.Add(new Alumno { AlumnoId = 1, Nombre = "Antonio", Edad = 12, Efectivo = 5, Pelo = "Castaño" });
            db.Add(new Alumno { AlumnoId = 2, Nombre = "Maria", Edad = 15, Efectivo = 88, Pelo = "Rubio" });
            db.Add(new Alumno { AlumnoId = 3, Nombre = "Carlos", Edad = 20, Efectivo = 0, Pelo = "Moreno" });
            db.Add(new Alumno { AlumnoId = 4, Nombre = "Jon", Edad = 13, Efectivo = 3, Pelo = "Castaño" });
            db.Add(new Alumno { AlumnoId = 5, Nombre = "Iker", Edad = 10, Efectivo = 66, Pelo = "Castaño" });
            db.Add(new Alumno { AlumnoId = 6, Nombre = "Nerea", Edad = 22, Efectivo = 4, Pelo = "Moreno" });
            db.Add(new Alumno { AlumnoId = 7, Nombre = "Ane", Edad = 36, Efectivo = 16, Pelo = "Castaño" });
            db.SaveChanges();
            // Añadir Módulos
            // Id de 1 a 10
            Console.WriteLine("Añadiendo los modulos");
            db.Add(new Modulo { ModuloId = 1, Titulo = "Informatica", Creditos = 10, Curso = 1 });
            db.Add(new Modulo { ModuloId = 2, Titulo = "Matematica", Creditos = 22, Curso = 1 });
            db.Add(new Modulo { ModuloId = 3, Titulo = "Historia", Creditos = 15, Curso = 1 });
            db.Add(new Modulo { ModuloId = 4, Titulo = "Ingles", Creditos = 22, Curso = 1 });
            db.Add(new Modulo { ModuloId = 5, Titulo = "Redes", Creditos = 90, Curso = 2 });
            db.Add(new Modulo { ModuloId = 6, Titulo = "Euskera", Creditos = 10, Curso = 2 });
            db.Add(new Modulo { ModuloId = 7, Titulo = "Castellano", Creditos = 33, Curso = 2 });
            db.Add(new Modulo { ModuloId = 8, Titulo = "Biologia", Creditos = 14, Curso = 2 });
            db.Add(new Modulo { ModuloId = 9, Titulo = "Fisica", Creditos = 12, Curso = 3 });
            db.Add(new Modulo { ModuloId = 10, Titulo = "Quimica", Creditos = 10, Curso = 3 });
            db.SaveChanges();

            // Matricular Alumnos en Módulos
            Console.WriteLine("Matriculando los alumnos en los modulos");
            var alumno = db.Alumnos.OrderBy(b => b.AlumnoId);
            foreach(var x in alumno){
                foreach(var b in db.Modulos){
                    db.Add(new Matricula { AlumnoId = x.AlumnoId, ModuloId = b.ModuloId});
                }
            }
            db.SaveChanges();
        }
    }

    
    static void BorrarMatriculaciones()
    {
        using (var db = new InstitutoContext())
        {
            // Borrar las matriculas de
            

            Console.WriteLine("Borrando los AlumnoId multiplo de 3 y ModuloId Multiplo de 2");
            foreach(var x in db.Matriculas)
            {
                // AlumnoId multiplo de 3 y ModuloId Multiplo de 2;
                if(x.AlumnoId%3 == 0 && x.ModuloId%2 == 0){
                    db.Matriculas.Remove(x);
                }
                // AlumnoId multiplo de 2 y ModuloId Multiplo de 5; Creada pero no usada
                /*if(x.AlumnoId%3 == 0 && x.ModuloId%2 == 0){
                    db.Matriculas.Remove(x);
                }*/
            }
            db.SaveChanges();
        }    
    }
    static void RealizarQuery()
    {
        using (var db = new InstitutoContext())
        {
            // Las queries que se piden en el examen
            /*
            1 Filtering (cada 1)
            1 Anomnimous tupe (cada 1)
            3 Ordenring  (cada 1)

            2 Joining
            3 Grouping
            2 Paging  (cada 1)
            5 Elements Opertators (cada 1)

            Conversiones
            1 ToArray
            1 ToDicctionary
            1 ToList
            2 ILookup*/
            
            //1 Filtering (cada 1)
            var q1 = db.Matriculas.Where(b => b.AlumnoId%2 == 0);
            //1 Anomnimous tupe (cada 1)
            var q2 = db.Matriculas.Select(o => new{
                MatriculaId = o.MatriculaId,
                AlumnosId = o.AlumnoId
            });
            //3 Ordenring  (cada 1)
            var q3 = db.Alumnos.OrderBy(o => o.AlumnoId);
            var q4 = db.Alumnos.OrderByDescending(o => o.AlumnoId);
            var q5 = db.Matriculas.OrderBy(o => o.AlumnoId).ThenByDescending(o => o.ModuloId);
            //2 Joining
            var q6 = db.Matriculas.Join(db.Alumnos, c => c.AlumnoId, o => o.AlumnoId,
                (c,o) => new{
                    c.MatriculaId,
                    c.AlumnoId,
                    o.Edad,
                    o.Efectivo
                }
            );
            
            var q7 = db.Matriculas.Join(db.Modulos, c => c.ModuloId, o => o.ModuloId,(c,o) => new{c.MatriculaId,c.ModuloId,o.Titulo, o.Curso});
            //3 Grouping
            var q8 = db.Alumnos.GroupBy(
                o => o.AlumnoId).
                Select(g => new
                {
                    AlumnoId = g.Key,
                    AlumnosTotales = g.Count()
                });
            var q9 = db.Matriculas.GroupBy(
                o => o.MatriculaId).
                Select(g => new
                {
                    MatriculaId = g.Key,
                    MatriculasTotales = g.Count()
                });
            var q10 = db.Modulos.GroupBy(
                o => o.ModuloId).
                Select(g => new
                {
                    ModuloId = g.Key,
                    ModulosTotales = g.Count()
                });
            //2 Paging  (cada 1)
            var q11 = db.Alumnos.Where(o => o.AlumnoId == 1).Take(3);
            var q12 = (from o in db.Alumnos
                where o.AlumnoId == 1
                orderby o.Edad
                select o).Skip(2).Take(2);
            //5 Elements Opertators (cada 1)
            var q13 = db.Modulos.Single(
                c => c.ModuloId == 3);
            var q14 = db.Modulos.SingleOrDefault(
                c => c.ModuloId == 3); 
            //var q15 = db.Modulos.Where( c => c.ModuloId == 4).DefaultIfEmpty(new Modulo()).Single();
            var q16 = db.Alumnos.Where(
                o => o.AlumnoId == 4).
                OrderBy(o => o.Edad).Last();
            var q17 = db.Modulos.Where(
                c => c.ModuloId == 85).
                Select(o => o.ModuloId).SingleOrDefault();


            //Conversiones
            //1 ToArray
            string[] names = (from c in db.Modulos
                                select c.Titulo).ToArray();
            //1 ToDicctionary - No funciona
           /* Dictionary<int, Modulo> col = db.Modulos.ToDictionary(c => c.ModuloId);
            
            Dictionary<string, double> ModulosAlumnos = (from oc in
            
                    (from o in orders
                    join c in customers on o.CustomerID equals c.CustomerID
                    select new { c.Name, o.Cost })
            
                    group oc by oc.Name into g
                    select g).ToDictionary(g => g.Key, g => g.Max(oc => oc.Cost));
            //1 ToList - No funciona
            List<Alumno> edadSobre10 = (from o in db.Alumnos
                    where o.Edad > 10
                    orderby o.Edad).ToList();

            //2 ILookup*/
            ILookup<int, string> ModulosLookup =
                    db.Modulos.ToLookup(c => c.ModuloId, c => c.Titulo);
        }
    }

    static void Main(string[] args)
    {
        GenerarDatos();
        BorrarMatriculaciones();
        RealizarQuery();
    }

}