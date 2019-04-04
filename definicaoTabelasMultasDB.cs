
// *******************************************************************
// Connection String

<connectionStrings >
      <add name ="MultasDBConnectionString"
           connectionString ="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MultasDB.mdf;Integrated Security=True;"
           providerName="System.Data.SqlClient"
      />
</connectionStrings>







// *******************************************************************
// Agentes

public int ID { get; set; }

public string Nome { get; set; }

public string Esquadra { get; set; }

public string Fotografia { get; set; }



// *******************************************************************
// Viaturas

public int ID { get; set; }

public string Matricula { get; set; }

public string Marca { get; set; }

public string Modelo { get; set; }

public string Cor { get; set; }

public string NomeDono { get; set; }

public string MoradaDono { get; set; }

public string CodPostalDono { get; set; }



// *******************************************************************
// Condutores

public int ID { get; set; }

public string Nome { get; set; }

public string BI { get; set; }

public string Telemovel { get; set; }

public DateTime DataNascimento { get; set; }

public string NumCartaConducao { get; set; }

public string LocalEmissao { get; set; }

public DateTime DataValidadeCarta { get; set; }


// *******************************************************************
// Multas

public int ID { get; set; }

public string Infracao { get; set; }

public string LocalDaMulta { get; set; }

public decimal ValorMulta { get; set; }

public DateTime DataDaMulta { get; set; }



// *******************************************************************
// MultasDB

 public MultasDB(): base("MultasDBConnectionString") { }

// vamos colocar, aqui, as instruções relativas às tabelas do 'negócio'
// descrever os nomes das tabelas na Base de Dados
public virtual DbSet<Multas> Multas { get; set; } // tabela Multas
public virtual DbSet<Condutores> Condutores { get; set; } // tabela Condutores
public virtual DbSet<Agentes> Agentes { get; set; } // tabela Agentes
public virtual DbSet<Viaturas> Viaturas { get; set; } // tabela Viaturas


protected override void OnModelCreating(DbModelBuilder modelBuilder) {
	modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
	modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
	base.OnModelCreating(modelBuilder);
}

