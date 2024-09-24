using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestão_de_uma_biblioteca
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string stringconexao = "Data Source=(localDb)\\MSSQLLocalDB;Initial Catalog=gestão_biblioteca;Integrated Security=True";
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("\n1 - Livros");
                Console.WriteLine("\n2 - Requisições");
                Console.WriteLine("\n\t0 - Sair");
                Console.Write("\n\n\tSeleciona uma opção: ");
                opcao = Convert.ToInt32(Console.ReadLine());


                switch (opcao)
                {
                    case 1:
                        Livros(stringconexao);
                        break;
                    case 2:
                        Requisicao(stringconexao);
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;

                }


            } while (opcao != 0);
        }
        static void Livros(string stringconexao)
        {
            Console.Clear();
            Console.Write("Adicione o nome do livro: ");
            string nomelivro = Console.ReadLine();

            using (SqlConnection conexao = new SqlConnection(stringconexao))
            {
                conexao.Open();

                string codigo = "INSERT INTO livros (nomelivro) VALUES (@nomelivro)";
                using (SqlCommand comando = new SqlCommand(codigo, conexao))
                {
                    comando.Parameters.AddWithValue("@nomelivro", nomelivro);

                    int linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                        Console.WriteLine("Livro registado com sucesso!");
                    else
                        Console.WriteLine("Livro não registado");
                }
            }

        }
        static void Requisicao(string stringconexao)
        {
            Console.Clear();
            DateTime dateTime = DateTime.Now;
            DateTime devolverLivro = dateTime.AddDays(15);

            Console.Clear();
            Console.WriteLine($"\n\t\t\t\t{dateTime}");

            using (SqlConnection conexao = new SqlConnection(stringconexao))
            {
                conexao.Open();

                string codigo = "SELECT id, nomelivro FROM livros";
                using (SqlCommand comando = new SqlCommand(codigo, conexao))
                {
                    using (SqlDataReader ler = comando.ExecuteReader())
                    {
                        Console.WriteLine("\n\n\tLivros registados:");

                        while (ler.Read())
                        {
                            Console.WriteLine($"\tID: {ler["id"]},\tNome do Livro: {ler["nomelivro"]}");
                        }
                    }
                }


                Console.Write("\n\tDigite o id do livro para fazer o empréstimo: ");
                int id_livro = Convert.ToInt32(Console.ReadLine());

                Console.Write("\n\tDigite o seu número telefone: ");
                int telefone = Convert.ToInt32(Console.ReadLine());

                Console.Write("\n\tDigite o seu nome de utilizador: ");
                string nome = Console.ReadLine();

                Console.WriteLine($"\n\tData de devolução: {devolverLivro}");
                Console.WriteLine($"{nome}, se não devolver no prazo correto, a multa será de 50centimos.");
                Console.ReadLine();

                string codigo1 = "INSERT INTO requisicao (id_livro, numeroTelefoneUtilizador, nomeUtilizador, dataEntrega, dataDevolucao) VALUES (@id_livro, @numeroTelefoneUtilizador, @nomeUtilizador, @dataEntrega, @dataDevolucao)";
                using (SqlCommand comando = new SqlCommand(codigo1, conexao))
                {
                    comando.Parameters.AddWithValue("@id_livro", id_livro);
                    comando.Parameters.AddWithValue("@NumeroTelefoneUtilizador", telefone);
                    comando.Parameters.AddWithValue("@nomeUtilizador", nome);
                    comando.Parameters.AddWithValue("@dataEntrega", dateTime);
                    comando.Parameters.AddWithValue("@dataDevolucao", devolverLivro);


                    int linhasafetadas = comando.ExecuteNonQuery();

                    if (linhasafetadas > 0)
                        Console.WriteLine("Requisição efetuada com sucesso!");
                    else
                        Console.WriteLine("Erro ao efetuar a requisição do livro.");
                    Console.ReadKey();
                }


            }
        }
    }
    
}
